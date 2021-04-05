using System;
using System.Collections.Generic;
using System.Text;

namespace DataContracts.Common
{
    public class StripeOptions
    {
        public string PublishableKey { get; set; }
        public string SecretKey { get; set; }
        public string WebhookSecret { get; set; }
        public string ConnectWebhookSecret { get; set; }
    }
}
