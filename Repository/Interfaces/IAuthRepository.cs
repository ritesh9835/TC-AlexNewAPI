﻿using DataContracts.Entities;
using DataContracts.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Database.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> RegisterAsync(User user);
        Task<User> ForgotPasswordCodeAsync(Guid id, string code);
        Task<User> GoogleLoginAsync();
        Task ChangePasswordAsync(User user);
        Task RegisterProfessional(CustomerRequest user);
        Task<bool> VerifyAccount(Guid userId);
        Task<User> GetUserByToken(string token);
        Task<ProfessionalInvitationModel> GetInvitedUser(string token);
    }
}
