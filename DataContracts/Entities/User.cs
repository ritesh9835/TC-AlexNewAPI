using System;
using System.Collections.Generic;
using System.Text;

namespace DataContracts.Entities
{
    public class User
    {
        public Guid RoleId { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }

        public string Salt { get; set; }

        public string Avatar { get; set; }

        public bool IsVerified { get; set; }

        public string ExternalId { get; set; }

        public Role Role { get; set; }

        public string FullName => Firstname + " " + Lastname;


        public UserProfile UserProfile { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        public bool ForgotPasswrodEnabled { get; set; }
        public Guid Id { get; set; }
        public UserSource Source { get; set; }
        public string ForgotPasswordCode { get; set; }
        public Address Address { get; set; }
        public Guid PrimaryService { get; set; }
        public WorkExperience WorkExperience { get; set; }
        public string ProfileImage { get; set; }
        public string PhoneVerificationCode { get; set; }
        public DateTime PhoneVerificationCodeExpiry { get; set; }
        public DateTime EmailVerificationCodeExpiry { get; set; }
        public string EmailVerificationCode { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Guid? AddressId { get; set; }
        public bool IsPhoneVerified { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsActive { get; set; }
        public string VerifyToken { get; set; }
        public DateTime TokenExpiry { get; set; }
        public string UserCode { get; set; }
    }
}
