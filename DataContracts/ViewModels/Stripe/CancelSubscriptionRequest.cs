using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataContracts.ViewModels.Stripe
{
    public class CancelSubscriptionRequest
    {
        [JsonProperty("subscriptionId")]
        public string Subscription { get; set; }
    }
}
