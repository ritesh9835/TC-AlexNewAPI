using System;
using System.Collections.Generic;
using System.Text;
using TazzerClean.Util;

namespace DataContracts.ViewModels.Subscription
{
    public class SubscriptionVM
    {
        public Guid Id { get; set; }
        public string SubscriptionName { get; set; }
        public List<SubscriptionDiscountVM> Discounts { get; set; }
    }
}
