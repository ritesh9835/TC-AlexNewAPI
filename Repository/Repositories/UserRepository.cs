using Dapper;
using DataAccess.Database.Interfaces;
using DataContracts.Entities;
using DataContracts.ViewModels.User;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TazzerClean.Util;

namespace DataAccess.Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _config;
        public UserRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<User> FindByName(string email)
        {
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            dbConnection.Open();
            var procedure = "spUsers_Get_ByEmail";

            var result = await dbConnection.QueryMultipleAsync(
                    procedure,
                    new { email },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );


            var user = (await result.ReadAsync<User>()).FirstOrDefault();
            var role = (await result.ReadAsync<Role>()).FirstOrDefault();
            var userProfile = (await result.ReadAsync<DataContracts.Entities.UserProfile>()).FirstOrDefault();
            var address = (await result.ReadAsync<Address>()).FirstOrDefault();

            if (user != null)
            {
                user.Role = new Role
                {
                    Name = role.Name
                };

                user.UserProfile = new DataContracts.Entities.UserProfile();
                user.UserProfile = userProfile;
                user.Address = new Address();
                user.Address = address ?? new Address();
            }
            return user;
        }

        public async Task<bool> UpdateProfile(ProfileData user, Guid userId)
        {
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            dbConnection.Open();

            var procedure = "spUsers_UpdateUserProfile";

            try
            {
                await dbConnection.ExecuteAsync(
                               procedure,
                               new
                               {
                                   userId,
                                   user.Firstname,
                                   user.Lastname,
                                   user.Avatar,
                                   /*user.UserProfile.Facebook,
                                   user.UserProfile.Website,
                                   user.UserProfile.Linkedin,*/
                                   user.Address.Suburb,
                                   user.Address.City,
                                   user.Address.State,
                                   user.Address.Country,
                                   user.Address.StreetName,
                                   user.Address.HouseNumber,
                                   user.Address.ZipCode
                               },
                               commandType: CommandType.StoredProcedure,
                               commandTimeout: 10
                           );
            }
            catch(Exception ex)
            {

            }
            return true;
        }

        public async Task UpdateUserProfileImage(Guid Id, string url)
        {
            
            var procedure = "spUsers_UpdateProfileImage";
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            dbConnection.Open();
            await dbConnection.ExecuteAsync(
                                procedure,
                                new { url, Id },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 10
                            );
        }

        public async Task AddUserServices(Guid userId, Guid serviceId)
        {
            var procedure = "spUser_Add_Services";
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            dbConnection.Open();
            await dbConnection.ExecuteAsync(
                                procedure,
                                new { userId, serviceId },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 10
                            );
        }
        public async Task<User> GetUserById(Guid userId)
        {
            var procedure = "sp_GetUserProfileById";
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            dbConnection.Open();
            var result = await dbConnection.QueryMultipleAsync(
                                procedure,
                                new { userId },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 10
                                );



            User user = (await result.ReadAsync<User>()).FirstOrDefault();
            var role = (await result.ReadAsync<Role>()).FirstOrDefault();
            var customer = (await result.ReadAsync<DataContracts.Entities.UserProfile>()).FirstOrDefault();
            var address = (await result.ReadAsync<Address>()).FirstOrDefault();
            var workExperience = (await result.ReadAsync<WorkExperience>()).FirstOrDefault();
            var formDetails = (await result.ReadAsync<ProfessionalProfileFormModel>()).FirstOrDefault();
            //var category = (await result.ReadAsync<Category>()).FirstOrDefault();

            user.Role = role;
            user.UserProfile = new DataContracts.Entities.UserProfile
            {
                IsActive = customer.IsActive,
                UserId = customer.UserId,
                Id = customer.Id,
                Facebook = customer.Facebook,
                PrimaryService = customer.PrimaryService,
                
            };

            if (workExperience != null)
            {
                user.WorkExperience = new WorkExperience
                {
                    WorkExperienceId = workExperience.WorkExperienceId,
                    Category = workExperience.Category,
                    //CategoryName = category.Name,
                    SubCategory = workExperience.SubCategory,
                    YearsOfExperience = workExperience.YearsOfExperience,
                    WorkEligibilityInUk = workExperience.WorkEligibilityInUk
                };
            }
            else
            {
                user.WorkExperience = new WorkExperience();
            }

            if (address != null)
            {
                user.Address = new Address
                {
                    State = address.State,
                    City = address.City,
                    HouseNumber = address.HouseNumber,
                    StreetName = address.StreetName,
                    Suburb = address.Suburb,
                    Country = address.Country,
                    ZipCode = address.ZipCode,
                };
            }
            else
            {
                user.Address = new Address();
            }

            return user;
        }

        public async Task<User> FindById(Guid UserId)
        {
            var procedure = "sp_GetUserById";
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            dbConnection.Open();
            var result = await dbConnection.QueryMultipleAsync(
                    procedure,
                    new { UserId },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var user = (await result.ReadAsync<User>()).FirstOrDefault();
            return user;
        }

        public async Task<ProfessionalProfileFormModel> GetuserForm(Guid userId)
        {
            var procedure = "spProfessionalForm_GetFormById";
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            dbConnection.Open();
            var result = await dbConnection.QueryMultipleAsync(
                                procedure,
                                new { userId },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 10
                                );

            var formDetails = (await result.ReadAsync<ProfessionalProfileFormModel>()).FirstOrDefault();
            return formDetails;
        }

        public async Task<bool> UpdateUserDocuments(DocumentModel documents, Guid userId)
        {
            var procedure = "spDocuments_UpdateDocuments";
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            dbConnection.Open();
            await dbConnection.ExecuteAsync(
                                procedure,
                                new
                                {
                                    userId,
                                    documents.Document,
                                    documents.Url
                                },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 10
                            );
            return true;
        }

        //Update professional tools details
        public async Task<bool> UpdateTools(Activation model, Guid userId)
        {
            var procedure = "spProfessionalForm_UpdateUserTools";
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            dbConnection.Open();
            await dbConnection.ExecuteAsync(
                                procedure,
                                new
                                {
                                    userId,
                                    model.OwnMaterialTools,
                                    model.Tools,
                                    model.AreasAroundPostcode
                                },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 10
                            );
            return true;
        }

        public async Task<bool> UpdateEmail(string email, DateTime expiry, string token, Guid userId)
        {
            var procedure = "spUsers_UpdateEmail";
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            dbConnection.Open();
            await dbConnection.ExecuteAsync(
                                procedure,
                                new
                                {
                                    userId,
                                    email,
                                    token,
                                    expiry
                                },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 10
                            );

            return true;
        }

        public async Task<bool> VarifyEmail(Guid userId)
        {
            var procedure = "spUsers_VerifyEmail";
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            dbConnection.Open();
            await dbConnection.ExecuteAsync(
                                procedure,
                                new
                                {
                                    userId
                                },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 10
                            );

            return true;
        }

        public async Task<bool> UpdatePhone(string phone, DateTime expiry, string code, Guid userId)
        {
            var procedure = "spUsers_UpdatePhone";
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            dbConnection.Open();
            await dbConnection.ExecuteAsync(
                                procedure,
                                new
                                {
                                    userId,
                                    phone,
                                    code,
                                    expiry
                                },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 10
                            );

            return true;
        }

        public async Task<bool> VarifyPhone(Guid userId)
        {
            var procedure = "spUsers_VerifyPhone";
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            dbConnection.Open();
            await dbConnection.ExecuteAsync(
                                procedure,
                                new
                                {
                                    userId
                                },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 10
                            );
            return true;
        }

        public async Task<ProfessionalProfileFormModel> GetProfessionalFormById(Guid userId)
        {
            var procedure = "spProfessional_GetFormById";
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            dbConnection.Open();
            var result = await dbConnection.QueryMultipleAsync(
                                procedure,
                                new { userId },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 10
                                );



            ProfessionalProfileFormModel formData = (await result.ReadAsync<ProfessionalProfileFormModel>()).FirstOrDefault();

            return formData;
        }

        public async Task<bool> AddStripAccountId(Guid userId, string stripeId)
        {
            var procedure = "spProfessionalForm_UpdateStripeAccountId";
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            dbConnection.Open();
            await dbConnection.ExecuteAsync(
                                procedure,
                                new
                                {
                                    userId,
                                    stripeId
                                },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 10
                            );
            return true;
        }

        public async Task<ProfessionalProfileFormModel> GetProfessionalByStripeId(string accountId)
        {
            var procedure = "spProfessionalForm_GetStripeId";
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            dbConnection.Open();
            var result = await dbConnection.QueryMultipleAsync(
                                procedure,
                                new { accountId },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 10
                                );



            ProfessionalProfileFormModel formData = (await result.ReadAsync<ProfessionalProfileFormModel>()).FirstOrDefault();

            return formData;
        }

        public async Task<bool> UpdateCustomerProfile(CustomerProfileModel user, Guid userId)
        {
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            dbConnection.Open();

            var procedure = "spUsers_UpdateCustomerProfile";

            await dbConnection.ExecuteAsync(
                               procedure,
                               new
                               {
                                   userId,
                                   user.Firstname,
                                   user.Lastname,
                                   user.ProfileImage,
                                   /*user.UserProfile.Facebook,
                                   user.UserProfile.Website,
                                   user.UserProfile.Linkedin,*/
                                   user.Address.Suburb,
                                   user.Address.City,
                                   user.Address.State,
                                   user.Address.Country,
                                   user.Address.StreetName,
                                   user.Address.HouseNumber,
                                   user.Address.ZipCode
                               },
                               commandType: CommandType.StoredProcedure,
                               commandTimeout: 10
                           );

            return true;
        }

        public async Task<bool> StripeOnBoarding(Guid userId)
        {
            var procedure = "spProfessionalForm_UpdateStripeOnBoarded";
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            dbConnection.Open();
            await dbConnection.ExecuteAsync(
                                procedure,
                                new
                                {
                                    userId
                                },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 10
                            );
            return true;
        }

        public async Task<List<Guid>> GetUserServices(Guid id)
        {
            var procedure = "uspuserServices_GetByUserId";
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            dbConnection.Open();
            var result = await dbConnection.QueryMultipleAsync(
                                procedure,
                                new { id },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 10
                                );

            return result.Read<Guid>().ToList();
        }

        public async Task<Professional> GetProfessionalById(Guid guid)
        {
            var procedure = "spProfessional_GetById";
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            dbConnection.Open();
            var result = await dbConnection.QueryMultipleAsync(
                                procedure,
                                new { guid },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 10
                                );


            //get professional data
            var prof = (await result.ReadAsync<Professional>()).FirstOrDefault();

            return prof;
        }

        public async Task<bool> SendInvitation(ProfessionalInvitationModel user, string Token)
        {
            var procedure = "spUser_SendInvitation";
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            dbConnection.Open();

            await dbConnection.ExecuteAsync(
                                procedure,
                                new
                                {
                                    user.Email,
                                    user.Fname,
                                    user.Lname,
                                    user.Phone,
                                    Token
                                },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 10
                            );
            return true;
        }

        public async Task<bool> VerifyBusinessInsurance(Guid userId, bool status)
        {
            var procedure = "spDocuments_VerifyBusinessInsurance";
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            dbConnection.Open();

            await dbConnection.ExecuteAsync(
                                procedure,
                                new
                                {
                                    userId,
                                    status
                                },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 10
                            );
            return true;
        }

        public async Task<bool> VerifyCarInsurance(Guid userId, bool status)
        {
            var procedure = "spDocuments_VerifyCarInsurance";
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            dbConnection.Open();

            await dbConnection.ExecuteAsync(
                                procedure,
                                new
                                {
                                    userId,
                                    status
                                },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 10
                            );
            return true;
        }

        public async Task<bool> VerifyDrivingLicence(Guid userId, bool status)
        {
            var procedure = "spDocuments_VerifyDrivingLicence";
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            dbConnection.Open();

            await dbConnection.ExecuteAsync(
                                procedure,
                                new
                                {
                                    userId,
                                    status
                                },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 10
                            );
            return true;
        }

        public async Task<bool> VerifyMOTCert(Guid userId, bool status)
        {
            var procedure = "spDocuments_VerifyMOTCert";
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            dbConnection.Open();

            await dbConnection.ExecuteAsync(
                                procedure,
                                new
                                {
                                    userId,
                                    status
                                },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 10
                            );
            return true;
        }

        public async Task<bool> VerifyProfessionalDocument(Guid userId,Document Document, bool status)
        {
            var procedure = "spDocuments_VerifyMOTCert";
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            dbConnection.Open();

            await dbConnection.ExecuteAsync(
                                procedure,
                                new
                                {
                                    userId,
                                    Document,
                                    status
                                },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 10
                            );
            return true;
        }

        public async Task<bool> VerifyProofofAddress(Guid userId, bool status)
        {
            var procedure = "spDocuments_VerifyProofOfAddress";
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            dbConnection.Open();

            await dbConnection.ExecuteAsync(
                                procedure,
                                new
                                {
                                    userId,
                                    status
                                },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 10
                            );
            return true;
        }

        public async Task<bool> VerifyQualificationDoc(Guid userId, bool status)
        {
            var procedure = "spDocuments_VerifyQualificationDoc";
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            dbConnection.Open();

            await dbConnection.ExecuteAsync(
                                procedure,
                                new
                                {
                                    userId,
                                    status
                                },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 10
                            );
            return true;
        }

        public async Task<List<Documents>> GetUserDocuments(Guid userId)
        {
            var procedure = "spDocuments_GetDocumentsByUserId";
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            dbConnection.Open();
            var result = await dbConnection.QueryMultipleAsync(
                                procedure,
                                new { userId },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 10
                                );


            //get professional data
            var document = (await result.ReadAsync<Documents>()).ToList();

            return document;
        }

        public async Task<bool> UpdateProfessionalQuestions(Guid userId, string Questions)
        {
            var procedure = "spUsers_UpdateProfessionalQuestions";
            using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            dbConnection.Open();

            await dbConnection.ExecuteAsync(
                                procedure,
                                new
                                {
                                    userId,
                                    Questions
                                },
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: 10
                            );
            return true;
        }
    }
}
