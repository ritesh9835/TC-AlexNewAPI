using DataAccess.Database.Interfaces;
using DataContracts.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.Stripe
{
    public class StripeService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration Configuration;
        public StripeService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            Configuration = configuration;
            StripeConfiguration.ApiKey = Configuration.GetValue<string>("StripeCredentials:SecretKey"); 
        }
        public async Task<bool> UpdateStripeConnected(string stripeId)
        {
            var professional = await _userRepository.GetProfessionalByStripeId(stripeId);

            if (professional == null)
                return false;
            else
            {
                return await _userRepository.StripeOnBoarding(professional.UserId); ;
            }
        }
        public async Task AddStripeCustomer(string email)
        {
            var userData = await _userRepository.FindByName(email);

            if (userData != null)
            {
                var professional = _userRepository.GetProfessionalFormById(userData.Id).Result;
                if (professional != null && string.IsNullOrEmpty(professional.StripeAccountId))
                {
                    var options = new AccountCreateOptions
                    {
                        Email = email,
                        Type = "express",
                        Capabilities = new AccountCapabilitiesOptions
                        {
                            CardPayments = new AccountCapabilitiesCardPaymentsOptions
                            {
                                Requested = true
                            },
                            Transfers = new AccountCapabilitiesTransfersOptions
                            {
                                Requested = true
                            },
                        }
                    };

                    var service = new AccountService();
                    
                    var customer = service.Create(options);

                    await _userRepository.AddStripAccountId(userData.Id, customer.Id);

                }
            }
        }
    }
}
