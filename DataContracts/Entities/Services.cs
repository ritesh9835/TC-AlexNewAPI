using System;
using System.Collections.Generic;
using System.Text;

namespace DataContracts.Entities
{
    public class Services
    {
        public Guid Id { get; set; }
        public Guid Category { get; set; }
        public Guid Subcategory { get; set; }
        public string CategoryName{ get; set; }
        public string SubcategoryName { get; set; }
        public string BillingUnit { get; set; }
        public decimal MinimumUnit { get; set; }
        public string Description { get; set; }
        public string ServiceName { get; set; }
        public decimal Price { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string ServiceImage { get; set; }
    }
}
