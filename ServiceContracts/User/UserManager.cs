using DataAccess.Database.Interfaces;
using DataContracts.Entities;
using DataContracts.ViewModels.User;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ServiceContracts.Common;
using ServiceContracts.Stripe;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TazzerClean.Util;
using TazzerClean.Util.Models;

namespace ServiceContracts.UserManager
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly IAuthRepository _authRepository;
        private readonly IPasswordHasher _hasher;
        private readonly IConfiguration Configuration;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IServiceRepository _serviceRepository;

        public UserManager(
            IUserRepository userRepository, 
            IMemoryCache memoryCache,
            IAuthRepository authRepository,
            IPasswordHasher hasher,
            IConfiguration configuration,
            ICategoryRepository categoryRepository,
            IServiceRepository serviceRepository)
        {
            _userRepository = userRepository;
            _memoryCache = memoryCache;
            _authRepository = authRepository;
            _hasher = hasher;
            Configuration = configuration;
            _categoryRepository = categoryRepository;
            _serviceRepository = serviceRepository;
            StripeConfiguration.ApiKey = Configuration.GetValue<string>("StripeCredentials:SecretKey");
        }

        public async Task<User> FindByName(string name)
        {
            var result = await _userRepository.FindByName(name);
            return result;
        }

        public async Task<bool> UpdateProfile(ProfileData user,Guid userId)
        {
            //user.UserProfile.UserId = userId;
            //user.UserProfile = new UserProfile();
            return await _userRepository.UpdateProfile(user,userId);
        }

        public async Task<bool> UpdateUserProfileImage(string url,Guid userId)
        {
            await _userRepository.UpdateUserProfileImage(userId,url);
            return true;
        }

        public async Task AddUserServices(Guid userId, Guid serviceId)
        {
            await _userRepository.AddUserServices(userId, serviceId);
        }

        public async Task<DataContracts.ViewModels.User.UserProfileModel> GetUserData(Guid userId)
        {
            var data =  await _userRepository.GetUserById(userId);
            var categoryData = await _categoryRepository.GetAllAsync();

            var professionalFormData = await _userRepository.GetProfessionalFormById(userId);
            //#region Steps Completed


            ////Logic to set steps complete
            //var stepsCompleted = 5;

            //if (data.Role.Name == RoleType.Professional.ToString())
            //{
            //    ProfessionalProfile professional = new ProfessionalProfile
            //    {
            //        WorkExperience = data.WorkExperience,
            //        Documents = new DataContracts.ViewModels.User.Documents
            //        {
            //            ProofOfAddress = professionalFormData.ProofOfAddress,
            //            BusinessInsurrance = professionalFormData.BusinessInsurrance,
            //            CarInsurance = professionalFormData.CarInsurance,
            //            DrivingLicence = professionalFormData.DrivingLicence,
            //            MOTCert = professionalFormData.MOTCert,
            //            QualificationDocument = professionalFormData.QualificationDocument
            //        },
            //        Activation = new Activation
            //        {
            //            Tools = professionalFormData.Tools,
            //            AreasAroundPostcode = professionalFormData.AreasAroundPostcode,
            //            OwnMaterialTools = professionalFormData.OwnMaterialTools
            //        }
            //    };
            //    //if any work experience not submitted yet
            //    if (professional.WorkExperience.GetType().GetProperties()
            //                                .Where(pi => pi.PropertyType == typeof(string))
            //                                .Select(pi => (string)pi.GetValue(professional.WorkExperience))
            //                                .Any(value => !string.IsNullOrEmpty(value)))
            //        stepsCompleted -= 1; //decrease steps completed
            //    else if (professional.Documents.GetType().GetProperties()
            //                                .Where(pi => pi.PropertyType == typeof(string))
            //                                .Select(pi => (string)pi.GetValue(professional.Documents))
            //                                .Any(value => !string.IsNullOrEmpty(value)))
            //        stepsCompleted -= 1; //If any documents not submitted yet.
            //    else if (professional.Activation.OwnMaterialTools == null)
            //        stepsCompleted -= 1;
            //}

            //if (string.IsNullOrEmpty(data.Firstname) && string.IsNullOrEmpty(data.Firstname))
            //    stepsCompleted -= -1;
            //else if (data.Address.GetType().GetProperties()
            //                            .Where(pi => pi.PropertyType == typeof(string))
            //                            .Select(pi => (string)pi.GetValue(data.Address))
            //                            .Any(value => !string.IsNullOrEmpty(value)))
            //    stepsCompleted -= -1;

            //#endregion

            #region Stripe check

            if (data.Role.Name == RoleType.Professional.ToString())
            {
                if (string.IsNullOrEmpty(professionalFormData.StripeAccountId))
                {
                    StripeService stripeService = new StripeService(_userRepository, Configuration);
                    await stripeService.AddStripeCustomer(data.Email);
                }
            }


            #endregion

            UserProfileModel model = new UserProfileModel
            {
                Firstname = data.Firstname,
                Lastname = data.Lastname,
                Email = data.Email,
                PhoneNumber = data.PhoneNumber,
                ProfileImage = data.ProfileImage,
                UserId = data.Id,
                UserProfile = data.UserProfile,
                Address = data.Address,
                WorkExperience = new WorkExperience
                {
                    WorkExperienceId = data.WorkExperience.WorkExperienceId,
                    Category = data.WorkExperience.Category,
                    CategoryName = categoryData.Where(c => c .Id == data.WorkExperience.Category).Select(c => c.Name).FirstOrDefault(),
                    SubCategory = data.WorkExperience.SubCategory,
                    SubCategoryName = categoryData.Where(c => c.Id == data.WorkExperience.SubCategory).Select(c => c.Name).FirstOrDefault(),
                    YearsOfExperience = data.WorkExperience.YearsOfExperience,
                    WorkEligibilityInUk = data.WorkExperience.WorkEligibilityInUk
                }
            };
            if(data.Role.Name == RoleType.Professional.ToString())
            {
                model.StripeAccountId = data.Role.Name == RoleType.Professional.ToString() ? professionalFormData.StripeAccountId : string.Empty;
                model.StripeOnBoarded = data.Role.Name == RoleType.Professional.ToString() ? professionalFormData.StripeOnBoarded : false;
                model.StripeConnectLink = data.Role.Name == RoleType.Professional.ToString() || !model.StripeOnBoarded ? GenerateStripeLink(professionalFormData.StripeAccountId) : string.Empty;
            }

            return model;
        }

        public async Task<bool> ChangePasswordAsync(string oldPassword,string newPassword,Guid UserId)
        {
            var user = await _userRepository.FindById(UserId);

            if (user == null)
                return false;

            //verify old password
            var verify = _hasher.Validate(user.Password,oldPassword,user.Salt);

            if (verify)
            {
                if (!string.IsNullOrEmpty(newPassword))
                {
                    var hashed = _hasher.Hash(user.Password);
                    user.Password = hashed.hashed;
                    user.Salt = hashed.salt;
                }
                await _authRepository.ChangePasswordAsync(user);

                return true;
            }
            else
                return false;            
        }

        //Update profile step2 (Documents upload)
        public async Task<List<DataContracts.ViewModels.User.Documents>> GetProfessionalDocuments(Guid userId)
        {
            var data =  await _userRepository.GetUserDocuments(userId);

            if (data == null)
                return new List<Documents>();            

            return data;
        }

        //Update profile step3 (general questions)
        public async Task<Activation> GetProfileS3(Guid userId)
        {
            var data = await _userRepository.GetuserForm(userId);

            Activation questions = new Activation
            {
                OwnMaterialTools = data.OwnMaterialTools,
                Tools = data.Tools,
                AreasAroundPostcode = data.AreasAroundPostcode
            };

            return questions;
        }

        public async Task<bool> UpdateDocuments(DataContracts.ViewModels.User.Documents documents, Guid userId)
        {

            return await _userRepository.UpdateUserDocuments(documents, userId); 
        }

        public async Task<bool> UpdateProfileS3(Activation model, Guid userId)
        {
            return await _userRepository.UpdateTools(model,userId);
        }

        public async Task<VerificationModel> GetUserEmailAndPhone(Guid userId)
        {
            var data = await _userRepository.GetUserById(userId);

            VerificationModel model = new VerificationModel
            {
                Email = data.Email,
                Phone = data.PhoneNumber,
                IsEmailVerified = data.IsEmailVerified,
                IsPhoneVarified = data.IsPhoneVerified
            };

            return model;
        }

        public async Task<bool> SetUserEmail(string emailId, Guid userId)
        {

            Helper helper = new Helper();

            //Generate 6 digit OTP
            var newToken = helper.CodeGenerator(6);
            DateTime expiryOn = DateTime.Now.AddMinutes(3);

            //Send Email
            using (var emailHelper = new CummunicationHelper(Configuration))
            {
                WelcomeEmail email = new WelcomeEmail
                {
                    Name = string.Empty
                };

                await emailHelper.SendEmailVerifyCode(email,newToken, emailId);
            }
            //Update to Db
            return await _userRepository.UpdateEmail(emailId,expiryOn,newToken, userId);
        }

        public Task<bool> GetUserPhone(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SetUserPhone(string phone, Guid userId)
        {
            Helper helper = new Helper();

            //Generate 6 digit OTP
            var newToken = helper.CodeGenerator(6);
            DateTime expiryOn = DateTime.UtcNow.AddMinutes(3);

            //Twillio service
            //using (var emailHelper = new CummunicationHelper(Configuration))
            //{

            //    await emailHelper.SendSms(string.Empty,string.Empty);
            //}

            return await _userRepository.UpdatePhone(phone, expiryOn, newToken, userId);
        }

        public async Task<bool> VerifyUserEmail(string code, Guid userId)
        {
            var data = await _userRepository.GetUserById(userId);

            if (code != data.EmailVerificationCode)
                return false;
            else if (code == data.EmailVerificationCode && data.EmailVerificationCodeExpiry <= DateTime.UtcNow)
                return false;

            return await _userRepository.VarifyEmail(userId);
        }

        public async Task<bool> VerifyUserPhone(string code, Guid userId)
        {
            var data = await _userRepository.GetUserById(userId);

            if (code != data.PhoneVerificationCode)
                return false;
            else if (code == data.PhoneVerificationCode && data.PhoneVerificationCodeExpiry<= DateTime.UtcNow)
                return false;

            return await _userRepository.VarifyPhone(userId); 
        }

        public async Task<bool> UpdateCustomerProfile(CustomerProfileModel user, Guid userId)
        {
            return await _userRepository.UpdateCustomerProfile(user,userId);
        }

        public async Task<ProfessionalProfile> GetProfessionalById(Guid guid)
        {
            var data = await _userRepository.GetProfessionalById(guid);

            //Get user services
            var services = await _userRepository.GetUserServices(guid);

            //get all categories 
            var categories = await _categoryRepository.GetAllAsync();


            //map to view models
            ProfessionalProfile professional = new ProfessionalProfile
            {
                Firstname = string.IsNullOrEmpty(data.Firstname) ? string.Empty : data.Firstname,
                Lastname = string.IsNullOrEmpty(data.Lastname) ? string.Empty : data.Lastname,
                Email = string.IsNullOrEmpty(data.Email) ? string.Empty : data.Email,
                PhoneNumber = string.IsNullOrEmpty(data.PhoneNumber) ? string.Empty : data.PhoneNumber,
                Documents = new List<Documents>(),
                WorkEligibilityInUk = data.WorkEligibilityInUk,
                Address = new DataContracts.Entities.Address
                {
                    HouseNumber = string.IsNullOrEmpty(data.HouseNumber) ? string.Empty : data.HouseNumber,
                    StreetName = string.IsNullOrEmpty(data.StreetName) ? string.Empty : data.StreetName,
                    City = string.IsNullOrEmpty(data.City) ? string.Empty : data.City,
                    State = string.IsNullOrEmpty(data.State) ? string.Empty : data.State,
                    Suburb = string.IsNullOrEmpty(data.Suburb) ? string.Empty : data.Suburb,
                    Country = string.IsNullOrEmpty(data.Country) ? string.Empty : data.Country,
                    ZipCode = string.IsNullOrEmpty(data.ZipCode) ? string.Empty : data.ZipCode
                },
                Role = string.Empty,
                StripeAccountId = data.StripeAccountId,
                StripeConnectLink = string.Empty,
                WorkExperience = new WorkExperience
                {
                    Category = data.Category,
                    CategoryName = categories.Where(c => c.Id == data.Category).Select(s => s.Name).FirstOrDefault(),
                    SubCategory = data.SubCategory,
                    SubCategoryName = categories.Where(c => c.Id == data.SubCategory).Select(s => s.Name).FirstOrDefault(),
                    YearsOfExperience = data.YearsOfExperience
                },
                UserId = data.UserId,
                IsEmailVerified = data.IsEmailVerified,
                IsPhoneVerified = data.IsPhoneVerified,
                IsVerified = data.IsApproved
                
            };

            //Map services to View model
            professional.OfferedServices = new System.Collections.Generic.List<string>();
            foreach(var service in services)
            {
                if(services!=null)
                {
                    professional.OfferedServices.Add(categories.Where(c => c.Id == service).Select(s => s.Name).FirstOrDefault());
                }
            }

            return professional;
        }

        public async Task<bool> VerifyDocuments(TazzerClean.Util.Document documents,Guid userId, bool status)
        {
            return await _userRepository.VerifyProfessionalDocument(userId,documents,status);
        }

        public async Task<ProfileData> GetUserProfileData(Guid id)
        {
            var data = await _userRepository.GetUserById(id);

            ProfileData profile = new ProfileData
            {
                Firstname = data.Firstname,
                Lastname = data.Lastname,
                Avatar = data.Avatar,
                UseCode = data.UserCode,
                Email = data.Email,
                Address = data.Address,
                PhoneNumber = data.PhoneNumber,
                IsEmailVerified = data.IsEmailVerified,
                IsPhoneVerified = data.IsPhoneVerified
                
            };

            return profile;
        }

        public async Task<ProfessionalQuestionsModel> GetProfessionalQuestions(Guid userId)
        {
            //Get user/professionals professional data for questions
            var data = await _userRepository.GetProfessionalFormById(userId);

            //Get selected services by user
            var userServices = await _userRepository.GetUserServices(userId);

            //Get list of services
            var categories = await _categoryRepository.GetAllAsync();

            //map with service model
            List<UserServices> serviceList = new List<UserServices>();
            foreach(var userService in userServices)
            {
                serviceList.Add(new UserServices
                {
                    Id = userService,
                    Name = categories.Where(s => s.Id == userService).Select(s => s.Name).FirstOrDefault()
                });
            }

            ProfessionalQuestionsModel questionsModel = new ProfessionalQuestionsModel
            {
                Questions = JsonConvert.DeserializeObject<List<DataContracts.ViewModels.User.Question>>(data.Questions),
                UserServices = serviceList

            };
            return questionsModel;
        }

        public async Task<bool> UpdateProfessionalQuestions(Guid userId, ProfessionalQuestionsModel model)
        {
            var question = JsonConvert.SerializeObject(model.Questions);

            foreach(var service in model.UserServices)
            {
                await _userRepository.AddUserServices(userId,service.Id);
            }

            return await _userRepository.UpdateProfessionalQuestions(userId, question);
        }


        #region Private Methods
        private string GenerateStripeLink(string accountId)
        {
            if (string.IsNullOrEmpty(accountId))
                return string.Empty;
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            string refreshUrl;
            string returnUrl;
            if (environment == "Production")
            {
                refreshUrl = Configuration.GetValue<string>("RefreshUrls:ForProduction");
                returnUrl = Configuration.GetValue<string>("RefreshUrls:ForProduction");
            }
            else
            {
                refreshUrl = Configuration.GetValue<string>("RefreshUrls:ForDevelopment"); 
                returnUrl = Configuration.GetValue<string>("RefreshUrls:ForDevelopment");
            }
            var options = new AccountLinkCreateOptions
            {
                Account = accountId,
                RefreshUrl = refreshUrl,
                ReturnUrl = returnUrl,
                Type = "account_onboarding",
            };
            var service = new AccountLinkService();
            var accountLink = service.Create(options);
            return accountLink.Url;
        }

        public async Task<bool> SendProfessionalInvitation(ProfessionalInvitationModel invitationModel)
        {
            var Token = Guid.NewGuid().ToString();

            //Send Email
            using (var emailHelper = new CummunicationHelper(Configuration))
            {
                WelcomeEmail email = new WelcomeEmail
                {
                    Name = invitationModel.Fname,
                    VerifyUrl = invitationModel.VerifyUrl+ "?invitationToken=" + Token
                };

                await emailHelper.SendEmailInvitation(email, invitationModel.Email);
            }

            return await _userRepository.SendInvitation(invitationModel,Token);
        }


        #endregion
    }
}
