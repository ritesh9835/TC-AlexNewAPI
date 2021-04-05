using System;
using System.Collections.Generic;
using System.Text;

namespace DataContracts.ViewModels.Subscription
{
    public class SubscriptionDiscountVM
    {
        public Guid Id { get; set; }
        public Guid SubscriptionTypeId { get; set; }
        public string SubscriptionTypeName { get; set; }
        public Guid SubscriptionId { get; set; }
        public int ThreeMonthsDiscount { get; set; }
        public int SixMonthsDiscount { get; set; }
        public int TwelveMonthDiscount { get; set; }
    }
}
