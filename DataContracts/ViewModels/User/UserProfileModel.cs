using DataContracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataContracts.ViewModels.User
{
    //Step 1 data model
    public class CustomerProfileModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string FullName => Firstname + " " + Lastname;
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfileImage { get; set; }
        public Guid UserId { get; set; }
        public Entities.UserProfile UserProfile { get; set; }
        public bool IsVerified { get; set; }
        public string Role { get; set; }
        public Address Address { get; set; }
        public bool IsPhoneVerified { get; set; }
        public bool IsEmailVerified { get; set; }
        public string Avatar { get; set; }
        public string UserCode { get; set; }

    }

    public class UserProfileModel : CustomerProfileModel
    {
        public Guid PrimaryService { get; set; }
        public WorkExperience WorkExperience { get; set; }
        public string StripeAccountId { get; set; }
        public bool StripeOnBoarded { get; set; }
        public string StripeConnectLink { get; set; }
        //are you eligible to work in UK.
        public bool WorkEligibilityInUk { get; set; }
    }

    //A complete professional profile
    public class ProfessionalProfile : UserProfileModel
    {
        public List<Documents> Documents { get; set; }
        public Activation Activation { get; set; }
        public List<string> OfferedServices { get; set; }
    }

    //Verification Email and phone
    public class VerificationModel
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsPhoneVarified { get; set; }
    }
    public class ProfileData
    {
        public string UseCode {get; set;}
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string FullName => Firstname + " " + Lastname;
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsVerified { get; set; }
        public Address Address { get; set; }
        public bool IsPhoneVerified { get; set; }
        public bool IsEmailVerified { get; set; }
        public string Avatar { get; set; }
    }
    public class UserServices
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
