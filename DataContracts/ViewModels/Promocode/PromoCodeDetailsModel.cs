using System;
using System.Collections.Generic;
using System.Text;

namespace DataContracts.ViewModels.Promocode
{
    public class PromoCodeDetailsModel : PromoCodeModel
    {
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
