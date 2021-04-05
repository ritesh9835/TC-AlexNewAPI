using System;
using System.Collections.Generic;
using System.Text;

namespace DataContracts.ViewModels.Promocode
{
    public class PromoCodeModel
    {
        public Guid Id { get; set; }
        public string PromocodeName { get; set; }
        public string Code { get; set; }
        public DateTime Validity { get; set; }
        public decimal Discount { get; set; }
    }
}
