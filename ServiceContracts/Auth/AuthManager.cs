using DataAccess.Database.Interfaces;
using DataContracts.Entities;
using DataContracts.ViewModels.User;
using Microsoft.Extensions.Configuration;
using ServiceContracts.Common;
using System;
using System.Threading.Tasks;
using TazzerClean.Util;
using TazzerClean.Util.Models;

namespace ServiceContracts.Auth
{
    public class AuthManager : IAuthManager
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICommonRepository _commonRepository;
        private readonly IPasswordHasher _hasher;
        private readonly IConfiguration Configuration;
        private static Helper helper = new Helper();
        public AuthManager(
            IAuthRepository authRepository,
            IUserRepository userRepository,
            ICommonRepository commonRepository,
            IPasswordHasher hasher,
            IConfiguration configuration)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _commonRepository = commonRepository;
            _hasher = hasher;
            Configuration = configuration;
        }

        public async Task<Guid> RegisterAsync(User user,string verificationurl, int role)
        { 
            var desiredRole = await _commonRepository.FindRoleByName(Enum.GetName(typeof(RoleType), role));

            user.Id = Guid.NewGuid();
            user.UserProfile.Id = Guid.NewGuid();
            user.RoleId = desiredRole.Id;

            if (!string.IsNullOrEmpty(user.Password))
            {
                var hashed = _hasher.Hash(user.Password);
                user.Password = hashed.hashed;
                user.Salt = hashed.salt;
            }

            user.UserCode = role == 1 ? "TCC" + helper.GenerateUserCode() : "TCP" + helper.GenerateUserCode();

            var token = Guid.NewGuid().ToString();

            //Send Email
            using (var emailHelper = new CummunicationHelper(Configuration))
            {      

                WelcomeEmail email = new WelcomeEmail
                {
                    Name = string.Empty,
                    VerifyUrl = verificationurl + token
                };

                await emailHelper.SendEmailVerifyEmail(email, user.Email);
            }

            user.VerifyToken = token;
            user.TokenExpiry = DateTime.UtcNow.AddDays(1);

            var result = await _authRepository.RegisterAsync(user);

            return result.Id;
        }

        public async Task ChangePasswordAsync(User user)
        {
            if (!string.IsNullOrEmpty(user.Password))
            {
                var hashed = _hasher.Hash(user.Password);
                user.Password = hashed.hashed;
                user.Salt = hashed.salt;
            }

            await _authRepository.ChangePasswordAsync(user);

        }

        public async Task<User> ForgotPasswordCode(Guid id,string code)
        {
            return await _authRepository.ForgotPasswordCodeAsync(id,code);
        }

        public async Task<User> GoogleLogin()
        {
            return await _authRepository.GoogleLoginAsync();
        }

        public async Task RegisterProfessional(CustomerRequest user)
        {
            await _authRepository.RegisterProfessional(user);
        }

        public async Task<bool> VerifyAccount(string token)
        {
            var data = await _authRepository.GetUserByToken(token);

            if (data.VerifyToken != token)
                return false;
            else if (data.VerifyToken == token && data.TokenExpiry <= DateTime.UtcNow)
                return false;

            return await _authRepository.VerifyAccount(data.Id);
        }

        public async Task<ProfessionalInvitationModel> GetInvitedUser(string token)
        {
            return await _authRepository.GetInvitedUser(token);
        }
    }
}
