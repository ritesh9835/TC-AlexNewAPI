using System;
using System.Collections.Generic;
using System.Text;

namespace DataContracts.Entities
{
    public class Customer
    {
        public string Firstname { get; set; }
        public bool IsActive { get; set; }
        public string Lastname { get; set; }
        public string Fullname => Firstname + " " + Lastname;
        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public Guid? AddressId { get; set; }

        public Guid UserId { get; set; }

        public bool IsEmailVerified { get; set; }
        public bool IsPhoneVerified { get; set; }
        public string Suburb { get; set; }
        public string State { get; set; }
        public string HouseNumber { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string StreetName { get; set; }
        public DateTime Created { get; set; }
        public string Country { get; set; }
    }
}
