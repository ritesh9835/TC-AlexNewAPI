using System;
using System.Collections.Generic;
using System.Text;

namespace DataContracts.ViewModels.User
{
    public class LoginRepsponseModel
    {
        public string FullName => Firstname + " " + Lastname;
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
        public string Avatar { get; set; }
        public bool IsActive { get; set; }
    }
}
