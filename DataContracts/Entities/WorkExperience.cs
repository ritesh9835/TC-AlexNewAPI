using System;
using System.Collections.Generic;
using System.Text;

namespace DataContracts.Entities
{
    public class WorkExperience
    {
        public Guid WorkExperienceId { get; set; }
        public Guid Category { get; set; }
        public string CategoryName { get; set; }
        public Guid SubCategory { get; set; }
        public string SubCategoryName { get; set; }
        public int YearsOfExperience { get; set; }
        public bool WorkEligibilityInUk { get; set; }
    }
}
