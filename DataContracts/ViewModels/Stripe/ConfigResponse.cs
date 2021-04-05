using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataContracts.ViewModels.Stripe
{
    public class ConfigResponse
    {
        [JsonProperty("publishableKey")]
        public string PublishableKey { get; set; }
    }
}
