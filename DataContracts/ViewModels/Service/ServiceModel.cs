using System;
using System.Collections.Generic;
using System.Text;

namespace DataContracts.ViewModels.Service
{
    public class ServiceModel
    {
        public Guid Category { get; set; }
        public Guid Subcategory { get; set; }
        public string BillingUnit { get; set; }
        public decimal MinimumUnit { get; set; }
        public string Description { get; set; }
        public string ServiceName { get; set; }
        public decimal Price { get; set; }
        public string ServiceImage { get; set; }
    }
}
