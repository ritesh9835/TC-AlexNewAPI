using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace TazzerClean.Util
{
    public class EmailHelper
    {
        private readonly IConfiguration _configuration;
       
        public EmailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmail(string toEmail, string emailSubject, string htmlBody)
        {
            var apiKey = _configuration.GetSection("SendGrid").GetSection("ApiKey").Value;
            //var apiKey = "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";
            //var from = _configuration.GetSection("SendGrid").GetSection("From").Value;
            var client = new SendGridClient(apiKey);
            //var msg = new SendGridMessage()
            //{
            //    From = new EmailAddress(from),
            //    Subject = subject,
            //    PlainTextContent = body
            //};
            //msg.AddTo(new EmailAddress("ungureanualexbogdan@gmail.com", toName));
            //var response = await client.SendEmailAsync(msg).ConfigureAwait(false);

            var from = new EmailAddress(_configuration.GetValue<string>("SendGrid:From"), "Tazzer Clean");
            var tom = new EmailAddress(toEmail);

            var message = MailHelper.CreateSingleEmail(from, tom, emailSubject, htmlBody, htmlBody);
            var response = await client.SendEmailAsync(message);
        }

        public async Task SendSms (string to,string body)
        {
            //TwilioClient.Init("ACccd51a52b3653705d0dbf99b1fce1206", "a339fdd2c1804a1d601be0a74c515f39");
            TwilioClient.Init(_configuration.GetValue<string>("Twillio:AccountSid"), _configuration.GetValue<string>("Twillio:AuthToken"));
            var message = await MessageResource.CreateAsync(to : to,from : new Twilio.Types.PhoneNumber(_configuration.GetValue<string>("Twillio:FromMobileNo")), body : body);

            var gg = message.Sid;
        }
        
    }
}
