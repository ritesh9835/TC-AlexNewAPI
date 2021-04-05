using DataContracts.Entities;
using DataContracts.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TazzerClean.Util;

namespace DataAccess.Database.Interfaces
{
    public interface IUserRepository
    {
        Task<User> FindByName(string username);
        Task<bool> UpdateProfile(ProfileData user, Guid userId);
        Task<bool> UpdateCustomerProfile(CustomerProfileModel user, Guid userId);
        Task UpdateUserProfileImage(Guid userId, string url);
        Task<List<Documents>> GetUserDocuments(Guid userId);
        Task AddUserServices(Guid userId, Guid serviceId);
        Task<User> GetUserById(Guid userId);
        Task<User> FindById(Guid UserId);
        Task<ProfessionalProfileFormModel> GetuserForm(Guid userId);
        Task<bool> UpdateUserDocuments(DocumentModel documents, Guid userId);
        Task<bool> UpdateTools(Activation model, Guid userId);
        Task<bool> UpdateEmail(string email, DateTime expiry, string token, Guid userId);
        Task<bool> VarifyEmail(Guid userId);
        Task<bool> UpdatePhone(string phone, DateTime expiry, string code, Guid userId);
        Task<bool> VarifyPhone(Guid userId);
        Task<bool> AddStripAccountId(Guid userId,string stripeId);
        Task<ProfessionalProfileFormModel> GetProfessionalFormById(Guid userId);
        Task<ProfessionalProfileFormModel> GetProfessionalByStripeId(string accountId);
        Task<bool> StripeOnBoarding(Guid userId);
        Task<List<Guid>> GetUserServices(Guid id);
        Task<Professional> GetProfessionalById(Guid guid);
        Task<bool> SendInvitation(ProfessionalInvitationModel user, string Token);
        Task<bool> VerifyBusinessInsurance(Guid userId,bool status);
        Task<bool> VerifyCarInsurance(Guid userId, bool status);
        Task<bool> VerifyDrivingLicence(Guid userId, bool status);
        Task<bool> VerifyMOTCert(Guid userId, bool status);
        Task<bool> VerifyProofofAddress(Guid userId, bool status);
        Task<bool> VerifyQualificationDoc(Guid userId, bool status);
        Task<bool> UpdateProfessionalQuestions(Guid userId,string Questions);
        Task<bool> VerifyProfessionalDocument(Guid userId, Document Document, bool status);
    }
}
