using System;
using System.Collections.Generic;
using System.Text;

namespace DataContracts.ViewModels.ExtraServices
{
    public class ExtraServiceModel
    {
        public Guid Id { get; set; }
        public string ServiceName { get; set; }
        public string IconPath { get; set; }
        public Guid ParentServiceId { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}
