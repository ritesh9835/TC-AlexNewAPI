using System;
using System.Collections.Generic;
using System.Text;

namespace DataContracts.Entities
{
    public class PromoCodes : CommonFields
    {
        public string PromocodeName { get; set; }
        public string Code { get; set; }
        public DateTime Validity { get; set; }
        public decimal Discount { get; set; }
        public bool IsDeleted { get; set; }
    }
}
