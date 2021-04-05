using System;
using System.Collections.Generic;
using System.Text;

namespace DataContracts.Entities
{
    public class SubscriptionDiscount:CommonFields
    {
        public Guid SubscriptionId { get; set; }
        public Guid SubscriptionTypeId { get; set; }
        public int Month3 { get; set; }
        public int Month6 { get; set; }
        public int Month12 { get; set; }
    }
}
