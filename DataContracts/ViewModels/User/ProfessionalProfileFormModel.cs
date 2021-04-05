using System;
using System.Collections.Generic;
using System.Text;
using TazzerClean.Util;

namespace DataContracts.ViewModels.User
{
    public class ProfessionalProfileFormModel
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int ProfessionalTypeId { get; set; }
        public Guid Department { get; set; }
        public int AreasAroundPostcode { get; set; }
        public bool OwnMaterialTools { get; set; }
        public bool OwnTransport { get; set; }
        public bool OtherEmployment { get; set; }
        public bool WillingToTrain { get; set; }
        public bool Uniform { get; set; }
        public bool RightToWorkInCountry { get; set; }
        public string DBS { get; set; }
        public string QualificationDocument { get; set; }
        public string CarInsurance { get; set; }
        public string MOTCert { get; set; }
        public string DrivingLicence { get; set; }
        public string ProofOfAddress { get; set; }
        public string BusinessInsurrance { get; set; }
        public string BankInformation { get; set; }
        public string Tools { get; set; }
        public int StepsCompleted { get; set; }
        public string StripeAccountId { get; set; }
        public bool StripeOnBoarded { get; set; }
        public bool IsApproved { get; set; }
        public string StripeConnectLink { get; set; }
        public bool IsInvited { get; set; }
        public string Questions { get; set; }
    }

    //update profile step2
    public class Documents : DocumentModel
    {
        public string DocumentName => Document.ToString();
        public VerificationStatus VerificationStatus { get; set; }
    }

    //update profile step 3
    public class Activation
    {
        public Guid UserId { get; set; }
        public bool OwnMaterialTools { get; set; }
        public string Tools { get; set; }
        public int AreasAroundPostcode { get; set; }
    };

    public class DocumentModel
    {        
        public Document Document { get; set; }
        public string Url { get; set; }
    }
}
