using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataContracts.ViewModels.Stripe
{
    public class CreateSubscription
    {
        [JsonProperty("paymentMethodId")]
        public string PaymentMethod { get; set; }

        [JsonProperty("customerId")]
        public string Customer { get; set; }

        [JsonProperty("priceId")]
        public string Price { get; set; }
    }
}
