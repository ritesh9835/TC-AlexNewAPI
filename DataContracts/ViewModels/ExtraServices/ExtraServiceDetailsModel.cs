using System;
using System.Collections.Generic;
using System.Text;

namespace DataContracts.ViewModels.ExtraServices
{
    public class ExtraServiceDetailsModel : ExtraServiceModel
    {
        public DateTime CreatedOn { get; set; }
        public string ParentServiceName { get; set; }
    }
}
