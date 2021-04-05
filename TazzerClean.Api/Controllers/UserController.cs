using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DataAccess.Database.Interfaces;
using DataContracts.Common;
using DataContracts.Entities;
using DataContracts.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceContracts.UserManager;
using TazzerClean.Util;

namespace TazzerClean.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : BaseController<IUserManager>
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserManager _userManager;
        private readonly IAzureRepository _azureRepository;


        public UserController(ILogger<UserController> logger,IUserManager userManager,IAzureRepository azureRepository) : base(userManager)
        {
            _logger = logger;
            _userManager = userManager;
            _azureRepository = azureRepository;
        }

        [HttpGet]
        [Route("getbyusername")]
        public async Task<ActionResult<User>> GetUserByUsername()
        {
            try
            {

                var user = User.Claims?.FirstOrDefault(x => x.Type.Equals("UserName", StringComparison.OrdinalIgnoreCase))?.Value;

                var result = await _userManager.FindByName(user);



                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in AuthController:Login");
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("updateProfileImage")]
        [Authorize]
        public async Task<IActionResult> UpdateUserProfileImage(string url)
        {
            try
            {
                var res = await _userManager.UpdateUserProfileImage(url, currentUsers.Id);
                return Ok( new ResponseViewModel<bool> {
                    Data = res,
                    Message = res ? TazzerCleanConstants.SavedSuccess : TazzerCleanConstants.GeneralError
                }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in AuthController:Login");
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// API to update step1 professsional profile
        /// </summary>
        /// <param name="user"/>
        /// <returns></returns>
        [HttpPut]
        [Route("update-profile")]
        [Authorize]
        public async Task<ActionResult> UpdateUser([FromBody] ProfileData user)
        {
            try
            {
                var res = await _userManager.UpdateProfile(user, currentUsers.Id);

                return Ok(new ResponseViewModel<bool>
                {
                    Data = res,
                    Message = res ? TazzerCleanConstants.ProfileUpdated : TazzerCleanConstants.GeneralError
                });
            }
            catch(Exception ex)
            {
                _logger.LogError("An error has been thrown in UserController:UpdateUser", ex);
                return BadRequest();
            }
        }

        /// <summary>
        /// API to update professsional questions
        /// </summary>
        /// <param name="documents"/>
        /// <returns></returns>
        [HttpPut]
        [Route("update-prof-questions")]
        [Authorize]
        public async Task<ActionResult> UpdateQuestions([FromBody] ProfessionalQuestionsModel model)
        {
            try
            {
                var res = await _userManager.UpdateProfessionalQuestions(currentUsers.Id, model);

                return Ok(new ResponseViewModel<bool>
                {
                    Data = res,
                    Message = res ? TazzerCleanConstants.SavedSuccess : TazzerCleanConstants.GeneralError
                });
            }
            catch(Exception ex)
            {
                _logger.LogError("An error has been thrown in UserController:UpdateQuestions", ex);
                return BadRequest();
            }
        }

        /// <summary>
        /// API to update step2 professsional profile
        /// </summary>
        /// <param name="documents"/>
        /// <returns></returns>
        [HttpPut]
        [Route("update-documents")]
        [Authorize]
        public async Task<ActionResult> UpdateDocuments(DataContracts.ViewModels.User.Documents documents)
        {
            var res = await _userManager.UpdateDocuments(documents, currentUsers.Id);

            return Ok(new ResponseViewModel<bool>
            {
                Data = res,
                Message = res ? TazzerCleanConstants.SavedSuccess : TazzerCleanConstants.GeneralError
            });
        }

        /// <summary>
        /// API to update step3 professsional profile
        /// </summary>
        /// <param name="model"/>
        /// <returns></returns>
        [HttpPut]
        [Route("updateProfile(step3)")]
        [Authorize]
        public async Task<ActionResult> UpdateUsers3([FromBody] Activation model)
        {
            var res = await _userManager.UpdateProfileS3(model, currentUsers.Id);

            return Ok(new ResponseViewModel<bool>
            {
                Data = res,
                Message = res ? TazzerCleanConstants.SavedSuccess : TazzerCleanConstants.GeneralError
            });
        }

        /// <summary>
        /// API to get user Data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("my-profile")]
        [Authorize]
        public async Task<IActionResult> GetuserData()
        {
            try
            {
                return Ok(await _userManager.GetUserProfileData(currentUsers.Id));
            }
            catch(Exception ex)
            {
                _logger.LogError("An error has been thrown in UserController:GetuserData", ex);
                return BadRequest();
            }
        }

        /// <summary>
        /// API to get user Data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-user-documents")]
        [Authorize]
        public async Task<IActionResult> GetuserDocuments()
        {
            try
            {
                return Ok(await _userManager.GetProfessionalDocuments(currentUsers.Id));
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in UserController:GetuserDocuments", ex);
                return BadRequest();
            }
        }

        /// <summary>
        /// API to get user Data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("userprofile(step3)")]
        [Authorize]
        public async Task<IActionResult> GetuserDataS3()
        {
            return Ok(await _userManager.GetProfileS3(currentUsers.Id));
        }

        /// <summary>
        /// API to change password
        /// </summary>
        /// <param name="oldPassword"/>
        /// <param name="newPassword"/>
        /// <returns></returns>
        [HttpPost]
        [Route("changepassword")]
        [Authorize]
        public async Task<ActionResult> ChangePassword(string oldPassword, string newPassword)
        {   
            var res = await _userManager.ChangePasswordAsync(oldPassword,newPassword,currentUsers.Id);

            return Ok(new ResponseViewModel<bool> 
            {
                Data = res,
                Message = res ? TazzerCleanConstants.PasswordChanged : TazzerCleanConstants.GeneralError
            });
        }

        /// <summary>
        /// API to update email
        /// </summary>
        /// <param name="Email"/>
        /// <returns></returns>
        [HttpPut]
        [Route("update-email")]
        [Authorize]
        public async Task<ActionResult> UpdateUserEmail(string Email)
        {
            var res = await _userManager.SetUserEmail(Email,currentUsers.Id);

            return Ok(new ResponseViewModel<bool>
            {
                Data = res,
                Message = res ? TazzerCleanConstants.SavedSuccess : TazzerCleanConstants.GeneralError
            });
        }

        /// <summary>
        /// API to update mobile number
        /// </summary>
        /// <param name="phone"/>
        /// <returns></returns>
        [HttpPut]
        [Route("update-phone")]
        [Authorize]
        public async Task<ActionResult> UpdateUserMobile(string phone)
        {
            var res = await _userManager.SetUserPhone(phone, currentUsers.Id);

            return Ok(new ResponseViewModel<bool>
            {
                Data = res,
                Message = res ? TazzerCleanConstants.SavedSuccess : TazzerCleanConstants.GeneralError
            });
        }

        /// <summary>
        /// API to varify email code.
        /// </summary>
        /// <param name="code"/>
        /// <returns></returns>
        [HttpPost]
        [Route("verify-email")]
        [Authorize]
        public async Task<ActionResult> VerifyEmail(string code)
        {
            var res = await _userManager.VerifyUserEmail(code, currentUsers.Id);

            return Ok(new ResponseViewModel<bool>
            {
                Data = res,
                Message = res ? TazzerCleanConstants.SavedSuccess : TazzerCleanConstants.GeneralError
            });
        }

        /// <summary>
        /// API to update mobile number
        /// </summary>
        /// <param name="code"/>
        /// <returns></returns>
        [HttpPost]
        [Route("verify-phone")]
        [Authorize]
        public async Task<ActionResult> VerifyPhone(string code)
        {
            var res = await _userManager.VerifyUserPhone(code, currentUsers.Id);

            return Ok(new ResponseViewModel<bool>
            {
                Data = res,
                Message = res ? TazzerCleanConstants.SavedSuccess : TazzerCleanConstants.GeneralError
            });
        }

        /// <summary>
        /// API to get mobile and email details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("phone-email")]
        [Authorize]
        public async Task<ActionResult> EmailAndMobile()
        {
            return Ok(await _userManager.GetUserEmailAndPhone(currentUsers.Id));
        }

        /// <summary>
        /// API to update step1 professsional profile
        /// </summary>
        /// <param name="user"/>
        /// <returns></returns>
        [HttpPut]
        [Route("updatecustomerprofile")]
        [Authorize]
        public async Task<ActionResult> UpdateCustomerProfile([FromBody] CustomerProfileModel user)
        {

            var res = await _userManager.UpdateCustomerProfile(user, currentUsers.Id);

            return Ok(new ResponseViewModel<bool>
            {
                Data = res,
                Message = res ? TazzerCleanConstants.ProfileUpdated : TazzerCleanConstants.GeneralError
            });
        }

        /// <summary>
        /// API to get professional details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-professional")]
        [Authorize]
        public async Task<ActionResult> ProfessionalById(Guid professionalId)
        {
            return Ok(await _userManager.GetProfessionalById(professionalId));
        }

        /// <summary>
        /// API to send invitation to professionals
        /// </summary>
        /// <param name="invitationModel"/>
        /// <returns></returns>
        [HttpPost]
        [Route("send-prof-invite")]
        [Authorize(Policy = nameof(Policies.Admin))]
        public async Task<ActionResult> SendProfesssionalInvite([FromBody] ProfessionalInvitationModel invitationModel)
        {

            var res = await _userManager.SendProfessionalInvitation(invitationModel);

            return Ok(new ResponseViewModel<bool>
            {
                Data = res,
                Message = res ? TazzerCleanConstants.InviteSent : TazzerCleanConstants.GeneralError
            });
        }

        [HttpPut("verify-professional-document")]
        [Authorize]
        public async Task<ActionResult> VerifyDocument(Util.Document documents, bool status,Guid userId)
        {
            try
            {
                var res = await _userManager.VerifyDocuments(documents, userId, status);

                return Ok(new ResponseViewModel<bool>
                {
                    Data = res,
                    Message = res ? TazzerCleanConstants.SavedSuccess : TazzerCleanConstants.GeneralError
                });
            }
            catch(Exception ex)
            {
                _logger.LogError("An error has been thrown in UserController:VerifyDocument", ex);
                return BadRequest();
            }
        }

        /// <summary>
        /// API to get professional questions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("professional-questions")]
        [Authorize]
        public async Task<IActionResult> ProfileQuestions()
        {
            try
            {
                return Ok(await _userManager.GetProfessionalQuestions(currentUsers.Id));
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in UserController:ProfileQuestions", ex);
                return BadRequest();
            }
        }
    }
}
