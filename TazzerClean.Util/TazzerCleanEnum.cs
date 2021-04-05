using System;
using System.Collections.Generic;
using System.Text;

namespace TazzerClean.Util
{
    public enum Policies
    {
        Customer = 1,
        Professional = 2,
        Admin = 3,
        CustomerAndHigher = 4,
        ProfessionalAndHigher = 5
    }
    public enum BucketFolder
    {
        ProfileImage = 1,
        Documents = 2,
        CategorieIcons = 3,
        ServiceImage = 4
    }
    public enum Document
    {
        QualificationDocument = 1,
        ProofOfAddress = 2,
        MOTCert = 3,
        DrivingLicense = 4,
        CarInsurance = 5,
        BusinessInsurance = 6
    }
    public enum PagesTemplate
    {
        Services = 1
    }
    public enum SubscriptionValidity
    {
        Three_Month = 1,
        Six_Month = 2,
        Nine_Months = 3,
        Twelve_Months = 4
    }
    public enum ServiceFrequency
    {
        OneTime = 1,
        Weekly = 2,
        Monthly = 3
    }
    public enum ProfessionalType
    {
        OneTime = 1,
        Weekly = 2,
        Monthly = 3
    }
    public enum ServiceStatus
    {
        NotAlloted = 1,
        Pending = 2,
        Completed = 3,
        Cancelled = 4
    }
    public enum EmployeeType
    {
        Employee = 1,
        Subcontractor = 2,
        SelfEmployed = 3,
        Business = 4
    }
    public enum VerificationStatus
    {
        Approve = 1,
        Reject = 2,
        Processing = 3
    }
}
