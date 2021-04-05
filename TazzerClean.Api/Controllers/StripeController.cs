using DataAccess.Database.Interfaces;
using DataContracts.Common;
using DataContracts.ViewModels.Stripe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ServiceContracts.Stripe;
using Stripe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TazzerClean.Util;

namespace TazzerClean.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IOptions<StripeOptions> Options;
        private readonly IConfiguration Configuration;
        public StripeController(IOptions<StripeOptions> options, IUserRepository userRepository, IConfiguration configuration)
        {
            Options = options;
            _userRepository = userRepository;
            Configuration = configuration;
        }

        [HttpGet("config")]
        [Authorize]
        public async Task<IActionResult> GetConfig()
        {
            var obj = new ConfigResponse
            {
                PublishableKey = Options.Value.PublishableKey,
            };
            return Ok(obj);
        }

        [HttpPost("create-subscription")]
        [Authorize]
        public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscription sub)
        {
            var option = new PaymentMethodAttachOptions
            {
                Customer = sub.Customer,
            };

            var service = new PaymentMethodService();

            var paymentMethod = service.Attach(sub.PaymentMethod,option);

            // Update customer's default invoice payment method
            var customerOptions = new CustomerUpdateOptions
            {
                InvoiceSettings = new CustomerInvoiceSettingsOptions
                {
                    DefaultPaymentMethod = paymentMethod.Id,
                },
            };
            var customerService = new CustomerService();
            await customerService.UpdateAsync(sub.Customer, customerOptions);

            var subscriptionOptions = new SubscriptionCreateOptions
            {
                Customer = sub.Customer,
                Items = new List<SubscriptionItemOptions>
                {
                    new SubscriptionItemOptions
                    {
                        Price = Environment.GetEnvironmentVariable(sub.Price),
                    }
                },
            };

            subscriptionOptions.AddExpand("latest_invoice.payment_intent");

            var subscriptionService = new SubscriptionService();

            try
            {
                Subscription subscription = subscriptionService.Create(subscriptionOptions);
                return Ok(new ResponseViewModel<Subscription>
                {
                    Data = subscription,
                    Message = StripeConstants.SubscriptionAdded
                });
            }
            catch(StripeException e)
            {
                Console.WriteLine($"Failed to create subscription.{e}");
                return BadRequest();
            }
        }

        [HttpPost("cancel-subscription")]
        [Authorize]
        public async Task<IActionResult> CancelSubscription([FromBody] CancelSubscriptionRequest req)
        {
            var service = new SubscriptionService();
            var subscription = service.Cancel(req.Subscription, null);
            return Ok(new ResponseViewModel<Subscription> { Data = subscription, Message = StripeConstants.CancelSubscription });
        }


        //[HttpPost]
        //public async Task<IActionResult> Webhook()
        //{
        //    var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        //    Event stripeEvent;
        //    try
        //    {
        //        stripeEvent = EventUtility.ConstructEvent(
        //            json,
        //            Request.Headers["Stripe-Signature"],
        //            Options.Value.WebhookSecret
        //        );
        //        Console.WriteLine($"Webhook notification with type: {stripeEvent.Type} found for {stripeEvent.Id}");
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine($"Something failed {e}");
        //        return BadRequest();
        //    }

        //    if (stripeEvent.Type == Events.PaymentIntentSucceeded)
        //    {
        //        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
        //        //enroll to course once payment is success
        //        //update transaction status and order details
        //        await _teachersService.EnrollStudentOnPaid(new EnrollStudent
        //        {
        //            PaymentSource = PaymentSource.Stripe,
        //            TransactionDetails = JsonConvert.SerializeObject(paymentIntent),
        //            PaymentStatus = PaymentStatus.Success,
        //            RefId = paymentIntent.Id

        //        });
        //    }
        //    if (stripeEvent.Type == Events.PaymentIntentPaymentFailed || stripeEvent.Type == Events.PaymentIntentCanceled)
        //    {
        //        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
        //        await _teachersService.EnrollStudentOnPaid(new EnrollStudent
        //        {
        //            PaymentSource = PaymentSource.Stripe,
        //            TransactionDetails = JsonConvert.SerializeObject(paymentIntent),
        //            PaymentStatus = PaymentStatus.Cancelled,
        //            RefId = paymentIntent.Id
        //        });
        //        //update transaction status and order details as failed
        //    }
        //    if (stripeEvent.Type == Events.PaymentIntentProcessing)
        //    {
        //        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
        //        await _teachersService.EnrollStudentOnPaid(new EnrollStudent
        //        {
        //            PaymentSource = PaymentSource.Stripe,
        //            TransactionDetails = JsonConvert.SerializeObject(paymentIntent),
        //            PaymentStatus = PaymentStatus.Pending,
        //            RefId = paymentIntent.Id
        //        });
        //    }

        //    if (stripeEvent.Type == "invoice.paid")
        //    {
        //        Invoice invoice = (Invoice)stripeEvent.Data.Object;
        //        // Used to provision services after the trial has ended.
        //        // The status of the invoice will show up as paid. Store the status in your
        //        // database to reference when a user accesses your service to avoid hitting rate
        //        // limits.
        //    }
        //    if (stripeEvent.Type == "invoice.payment_failed")
        //    {
        //        // If the payment fails or the customer does not have a valid payment method,
        //        // an invoice.payment_failed event is sent, the subscription becomes past_due.
        //        // Use this webhook to notify your user that their payment has
        //        // failed and to retrieve new card details.
        //    }
        //    if (stripeEvent.Type == "invoice.finalized")
        //    {
        //        // If you want to manually send out invoices to your customers
        //        // or store them locally to reference to avoid hitting Stripe rate limits.
        //    }
        //    if (stripeEvent.Type == "customer.subscription.deleted")
        //    {
        //        // handle subscription cancelled automatically based
        //        // upon your subscription settings. Or if the user cancels it.
        //    }
        //    if (stripeEvent.Type == "customer.subscription.trial_will_end")
        //    {
        //        // Send notification to your user that the trial will end
        //    }

        //    return Ok();
        //}

        [HttpPost]
        [Route("connect-webhook")]
        public async Task<IActionResult> ConnectWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            Event stripeEvent;
            try
            {
                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    Configuration.GetValue<string>("StripeCredentials:WebhookSecret")
                );
                Console.WriteLine($"Webhook notification with type: {stripeEvent.Type} found for {stripeEvent.Id}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Something failed {e}");
                return BadRequest();
            }

            if (stripeEvent.Type == Events.AccountUpdated)
            {
                Account account = stripeEvent.Data.Object as Account;   
                StripeService service = new StripeService(_userRepository,Configuration);
                if (account.ChargesEnabled)
                    await service.UpdateStripeConnected(account.Id);

            }
            return Ok();
        }

        #region Get Current User
        private CurrentUser GetCurrentUser()
        {
            var identityClaims = HttpContext.User;
            var currentUsers = new CurrentUser();
            var claimValue = identityClaims.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            currentUsers.Id = new Guid(identityClaims.Claims.FirstOrDefault(c => c.Type == "id")?.Value);
            currentUsers.Name = identityClaims.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            return currentUsers;
        }
        #endregion
    }
}
