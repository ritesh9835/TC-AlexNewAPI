using System;
using System.Collections.Generic;
using System.Text;

namespace DataContracts.Entities
{
    public class ExtraServices : CommonFields
    {
        public string ServiceName { get; set; }
        public string IconPath { get; set; }
        public Guid ParentServiceId { get; set; }
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; }
    }
}
