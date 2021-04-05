using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using TazzerClean.Util;

namespace DataContracts.Entities
{
    public class Professional : Customer
    {        
        //public Guid Id { get; set; }
        public VerificationStatus QualificationDocumentVerify { get; set; }
        public VerificationStatus CarInsuranceVerify { get; set; }
        public VerificationStatus MOTCertVerify { get; set; }
        public VerificationStatus DrivingLicenceVerify { get; set; }
        public VerificationStatus ProofOfAddressVerify { get; set; }
        public VerificationStatus BusinessInsuranceVerify { get; set; }
        public string QualificationDocument { get; set; }
        public string CarInsurance { get; set; }
        public string MOTCert { get; set; }
        public string DrivingLicence { get; set; }
        public string ProofOfAddress { get; set; }
        public string BusinessInsurrance { get; set; }
        public bool IsApproved { get; set; }
        public Guid WorkExperienceId { get; set; }
        public Guid Category { get; set; }
        public string CategoryName { get; set; }
        public Guid SubCategory { get; set; }
        public string SubCategoryName { get; set; }
        public int YearsOfExperience { get; set; }
        public bool WorkEligibilityInUk { get; set; }
        public List<string> OfferedServices { get; set; }
        public string StripeAccountId { get; set; }
        public bool IsInvited { get; set; }
        public EmployeeType ProfessionalTypeId { get; set; }
        public string ProfessionalType => ProfessionalTypeId.ToString();
    }
}
