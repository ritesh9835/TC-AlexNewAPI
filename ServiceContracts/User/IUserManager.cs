using DataContracts.Entities;
using DataContracts.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceContracts.UserManager
{
    public interface IUserManager
    {
        Task<User> FindByName(string name);
        Task<bool> UpdateProfile(ProfileData user, Guid userId);
        Task<bool> UpdateCustomerProfile(CustomerProfileModel user, Guid userId);
        Task<bool> UpdateUserProfileImage(string url, Guid userId);
        Task AddUserServices(Guid userId, Guid serviceId);
        Task<DataContracts.ViewModels.User.UserProfileModel> GetUserData(Guid userId);
        Task<bool> ChangePasswordAsync(string oldPassword, string newPassword, Guid UserId);
        Task<List<DataContracts.ViewModels.User.Documents>> GetProfessionalDocuments(Guid userId);
        Task<Activation> GetProfileS3(Guid userId);
        Task<bool> UpdateDocuments(DataContracts.ViewModels.User.Documents documents, Guid userId);
        Task<bool> UpdateProfileS3(Activation model,Guid userId);
        Task<VerificationModel> GetUserEmailAndPhone(Guid userId);
        Task<bool> SetUserEmail(string email,Guid userId);
        Task<bool> SetUserPhone(string phone,Guid userId);
        Task<bool> VerifyUserEmail(string code, Guid userId);
        Task<bool> VerifyUserPhone(string code, Guid userId);
        //Task<bool> Addprofessional(ProfessionalInvitationModel model,Guid userId);
        Task<ProfessionalProfile> GetProfessionalById(Guid guid);
        Task<bool> SendProfessionalInvitation(ProfessionalInvitationModel invitationModel);
        Task<bool> VerifyDocuments(TazzerClean.Util.Document documents, Guid userId, bool status);
        Task<ProfileData> GetUserProfileData(Guid id);
        Task<ProfessionalQuestionsModel> GetProfessionalQuestions(Guid userId);
        Task<bool> UpdateProfessionalQuestions(Guid userId, ProfessionalQuestionsModel model);
    }
}
