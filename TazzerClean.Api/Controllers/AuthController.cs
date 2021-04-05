using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DataContracts.Common;
using DataContracts.Entities;
using DataContracts.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ServiceContracts.Auth;
using ServiceContracts.Common;
using ServiceContracts.Notification;
using ServiceContracts.UserManager;
using TazzerClean.Util;
using TazzerClean.Util.Models;

namespace TazzerClean.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthManager _authManager;
        private readonly IUserManager _userManager;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        public AuthController(ILogger<AuthController> logger, IAuthManager authManager, IUserManager userManager, IPasswordHasher passwordHasher, IConfiguration config, IEmailService emailService)
        {
            _logger = logger;
            _authManager = authManager;
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _config = config;
            _emailService = emailService;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginRepsponseModel>> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByName(request.Email);

                if (user != null && _passwordHasher.Validate(user.Password, request.Password, user.Salt))
                {
                    user.Token = GenerateJsonWebToken(user.Email,user.Role.Name,user.Id);
                    user.Password = null;
                    user.Salt = null;

                    LoginRepsponseModel repsponseModel = new LoginRepsponseModel
                    {
                        Token = user.Token,
                        Firstname = user.Firstname,
                        Lastname = user.Lastname,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Avatar = user.Avatar,
                        IsActive = user.IsActive
                    };

                    return repsponseModel;
                }

                return BadRequest(new { message = "Username or password is incorrect" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message,ex);
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromBody] CustomerRequest request)
        {
            try
            {
                var user = new User
                {
                    Password = request.Password,
                    Source = UserSource.Site,
                    PrimaryService = request.PrimaryService,
                    Firstname = request.FirstName,
                    Lastname = request.LastName,
                    Email = request.Email,
                    PhoneNumber = request.Phone,
                    UserProfile = new UserProfile
                    {
                        Id = Guid.NewGuid(),
                        PrimaryService = request.PrimaryService
                    }
                };

                var existingUser = await _userManager.FindByName(user.Email);

                if (existingUser != null) return BadRequest(new { message = "The user already exists!" });

                var userRole = 0;
                if (string.IsNullOrWhiteSpace(request.Role))
                {
                    userRole = string.IsNullOrWhiteSpace(request.Role) ? (int)RoleType.Customer : (int)RoleType.Admin;
                }
                else
                {
                    userRole = int.Parse(request.Role);
                }

                await _authManager.RegisterAsync(user,request.VerifyUrl, userRole);


                if(userRole == 2)
                {
                    var registeredUser = await _userManager.FindByName(user.Email);

                    request.UserId = registeredUser.Id;

                    await _authManager.RegisterProfessional(request);
                }

                if(request.Services != null)
                {
                    foreach (var u in request.Services)
                    {
                        await _userManager.AddUserServices(request.UserId, u);
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("google-login")]
        [AllowAnonymous]
        public async Task<ActionResult> GoogleLogin()
        {
            try
            {
                var user = await _authManager.GoogleLogin();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in AuthController:GoogleLogin");
                return BadRequest(ex);
            }
        }

        [HttpPost("verify-account")]
        [AllowAnonymous]
        public async Task<ActionResult> VerifyAccount(string token)
        {
            try
            {
                var res = await _authManager.VerifyAccount(token);
                return Ok( new ResponseViewModel<bool> {
                    Data = res,
                    Message = res ? EmailSubjectConstants.WelcomeEmail : TazzerCleanConstants.GeneralError
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in AuthController:GoogleLogin");
                return BadRequest(ex);
            }
        }


        [HttpPost]
        [Route("forgotpasswordlink")]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPasswordLink([FromBody] ForgotPasswordRequest user)
        {
            try
            {
                var existingUser = await _userManager.FindByName(user.Email);

                if (existingUser == null) return BadRequest(new { message = "There are no accounts linked to this email!" });

                Random r = new Random();
                int randNum = r.Next(1000000);
                string code = randNum.ToString("D6");

                var userWithCode = await _authManager.ForgotPasswordCode(existingUser.Id,code);

                //var body = $"Reset password code is: {userWithCode.ForgotPasswordCode} ";
                //var subject = "Reset password code";

                //await _emailService.SendEmail(existingUser.Username, existingUser.Username, body, subject);

                using (var emailHelper = new CummunicationHelper(_config))
                {
                    Common email = new Common
                    {
                        Name = existingUser.Email
                    };
                    await emailHelper.SendForgotPassword(email,code,existingUser.Email);
                }

                return Ok();

            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in AuthController:ForgotPassword");
                return BadRequest(ex);
            }
        }

        [HttpGet("get-invited-user")]
        [AllowAnonymous]
        public async Task<IActionResult> UserByInviteToken(string token)
        {
            try
            {
                var res = await _authManager.GetInvitedUser(token);
                return Ok(new ResponseViewModel<ProfessionalInvitationModel>
                {
                    Data = res,
                    Message = res != null ? EmailSubjectConstants.WelcomeEmail : TazzerCleanConstants.InvalidLink
                });
                
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in AuthController:GoogleLogin");
                return BadRequest(ex);
            }
        }




        private string GenerateJsonWebToken(string username,string role, Guid userId)
        {
            var claims = new List<Claim>
            {
                new Claim("username", username),
                new Claim(ClaimTypes.Role,role),
                new Claim("id", userId.ToString()),
            };


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddYears(1),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
