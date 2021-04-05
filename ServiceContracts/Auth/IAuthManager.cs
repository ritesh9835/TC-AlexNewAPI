using DataContracts.Entities;
using DataContracts.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.Auth
{
    public interface IAuthManager
    {
        Task<Guid> RegisterAsync(User user, string verificationurl, int role);
        Task<User> ForgotPasswordCode(Guid id, string code);
        Task<User> GoogleLogin();
        Task ChangePasswordAsync(User user);
        Task RegisterProfessional(CustomerRequest user);
        Task<bool> VerifyAccount(string token);
        Task<ProfessionalInvitationModel> GetInvitedUser(string token);
    }
}
