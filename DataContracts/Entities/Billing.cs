using System;
using System.Collections.Generic;
using System.Text;
using TazzerClean.Util;

namespace DataContracts.Entities
{
    public class Billing
    {
        public Guid Id { get; set; }
        public Guid ServiceRequestId { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProfessionalId { get; set; }
        public ServiceStatus ServiceStatus { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn{ get; set; }
        public DateTime UpdatedOn{ get; set; }
    }
}
