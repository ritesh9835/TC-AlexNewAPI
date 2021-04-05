using DataAccess.Database.Interfaces;
using DataContracts.ViewModels.Subscription;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataContracts.Entities;

namespace ServiceContracts.Subscriptions
{
    public class SubscriptionManager : ISubscriptionManager
    {
        private readonly ISubscriptionRepository _subscriptionsRepository;
        public SubscriptionManager(ISubscriptionRepository subscriptionsRepository)
        {
            _subscriptionsRepository = subscriptionsRepository;
        }
        public async Task<SubscriptionVM> GetSubscriptionById(Guid subscriptionId)
        {
            var subscription = await _subscriptionsRepository.GetSubscriptionById(subscriptionId);
            var discounts = await _subscriptionsRepository.GetDiscountsBySubscriptionId(subscriptionId);

            var model = new SubscriptionVM
            {
                Id = subscription.Id,
                SubscriptionName = subscription.SubscriptionName,
            };
            model.Discounts=new List<SubscriptionDiscountVM>();
            foreach (var discount in discounts)
            {
                var subscriptionType =
                    await _subscriptionsRepository.GetSubscriptionTypeById(discount.SubscriptionTypeId);
                model.Discounts.Add(new SubscriptionDiscountVM()
                {
                    Id = discount.Id,
                    SubscriptionId = discount.SubscriptionId,
                    SubscriptionTypeId = discount.SubscriptionTypeId,
                    SubscriptionTypeName = subscriptionType.Type,
                    ThreeMonthsDiscount = discount.Month3,
                    SixMonthsDiscount = discount.Month6,
                    TwelveMonthDiscount = discount.Month12
                });
            }

            return model;
        }

        public async Task<List<SubscriptionVM>> GetAll()
        {
            var dbSubscriptions = await _subscriptionsRepository.GetAllAsync();
            var subscriptions = new List<SubscriptionVM>();

            foreach (var item in dbSubscriptions)
            {
                var discounts = await _subscriptionsRepository.GetDiscountsBySubscriptionId(item.Id);
                var subscription = new SubscriptionVM
                {
                    Id = item.Id,
                    SubscriptionName = item.SubscriptionName,
                    Discounts = new List<SubscriptionDiscountVM>(),
                };
                foreach (var dbDiscount in discounts)
                {
                    var subscriptionType =
                        await _subscriptionsRepository.GetSubscriptionTypeById(dbDiscount.SubscriptionTypeId);
                    var discount = new SubscriptionDiscountVM()
                    {
                        Id = dbDiscount.Id,
                        SubscriptionId = dbDiscount.SubscriptionId,
                        SubscriptionTypeId = dbDiscount.SubscriptionTypeId,
                        SubscriptionTypeName = subscriptionType.Type,
                        ThreeMonthsDiscount = dbDiscount.Month3,
                        SixMonthsDiscount = dbDiscount.Month6,
                        TwelveMonthDiscount = dbDiscount.Month12
                    };
                    subscription.Discounts.Add(discount);

                }
                subscriptions.Add(subscription);
            }

            return subscriptions;
        }
        public async Task<bool> Create(SubscriptionCreateRequest request, Guid userId)
        {
            var subscriptionId=Guid.NewGuid();
            var subscription = new Subscription
            {
                Id = subscriptionId,
                SubscriptionName = request.SubscriptionName,
                CreatedBy = userId,
            };
            await _subscriptionsRepository.CreateSubscription(subscription);
            var plans = await _subscriptionsRepository.GetAllTypes();
            foreach (var plan in plans)
            {
                SubscriptionDiscount discount;
                if (plan.Type=="Weekly")
                {
                    if (request.Weekly!=null)
                    {
                        discount = new SubscriptionDiscount
                        {
                            Id = Guid.NewGuid(),
                            SubscriptionId = subscriptionId,
                            Month3 = request.Weekly.Month3,
                            Month6 = request.Weekly.Month6,
                            Month12 = request.Weekly.Month12,
                            CreatedBy = userId,
                            SubscriptionTypeId = plan.Id
                        };
                        await _subscriptionsRepository.CreateDiscount(discount);
                    }
                }
                if (plan.Type == "BiWeekly")
                {
                    if (request.BiWeekly!=null)
                    {
                        discount = new SubscriptionDiscount
                        {
                            Id = Guid.NewGuid(),
                            SubscriptionId = subscriptionId,
                            Month3 = request.BiWeekly.Month3,
                            Month6 = request.BiWeekly.Month6,
                            Month12 = request.BiWeekly.Month12,
                            CreatedBy = userId,
                            SubscriptionTypeId = plan.Id

                        };
                        await _subscriptionsRepository.CreateDiscount(discount);
                    }
                }
                if (plan.Type == "Monthly")
                {
                    if (request.Monthly!=null)
                    {
                        discount = new SubscriptionDiscount
                        {
                            Id = Guid.NewGuid(),
                            SubscriptionId = subscriptionId,
                            Month3 = request.Monthly.Month3,
                            Month6 = request.Monthly.Month6,
                            Month12 = request.Monthly.Month12,
                            CreatedBy = userId,
                            SubscriptionTypeId = plan.Id

                        };
                        await _subscriptionsRepository.CreateDiscount(discount);
                    }
                }
            }
            return true;
        }
        public async Task<bool> Update(SubscriptionUpdateRequest request, Guid userId)
        {
            var subscription = new Subscription
            {
                Id = request.Id,
                SubscriptionName = request.SubscriptionName,
                CreatedBy = userId,
            };
            await _subscriptionsRepository.UpdateSubscription(subscription);
            var plans = await _subscriptionsRepository.GetAllTypes();
            foreach (var plan in plans)
            {
                SubscriptionDiscount discount;
                if (plan.Type == "Weekly")
                {
                    if (request.Weekly!=null)
                    {
                        discount = new SubscriptionDiscount
                        {
                            Id = request.Weekly.Id,
                            Month3 = request.Weekly.Month3,
                            Month6 = request.Weekly.Month6,
                            Month12 = request.Weekly.Month12
                        };
                        await _subscriptionsRepository.UpdateDiscount(discount);
                    }

                }
                if (plan.Type == "BiWeekly")
                {
                    if (request.BiWeekly!=null)
                    {
                        discount = new SubscriptionDiscount
                        {
                            Id = request.BiWeekly.Id,
                            Month3 = request.BiWeekly.Month3,
                            Month6 = request.BiWeekly.Month6,
                            Month12 = request.BiWeekly.Month12
                        };
                        await _subscriptionsRepository.UpdateDiscount(discount);
                    }
                }
                if (plan.Type == "Monthly")
                {
                    if (request.Monthly!=null)
                    {
                        discount = new SubscriptionDiscount
                        {
                            Id = request.Monthly.Id,
                            Month3 = request.Monthly.Month3,
                            Month6 = request.Monthly.Month6,
                            Month12 = request.Monthly.Month12,
                        };
                        await _subscriptionsRepository.UpdateDiscount(discount);
                    }
                }
            }
            return true;
        }

        public async Task<bool> Delete(Guid subscriptionId)
        {
            await _subscriptionsRepository.DeleteAsync(subscriptionId);
            return true;
        }

        public async Task<bool> UpdateStatus(Guid subscriptionId)
        {
            var data = await _subscriptionsRepository.GetSubscriptionById(subscriptionId);

            await _subscriptionsRepository.UpdateStatus(subscriptionId,!data.IsActive);

            return true;
        }
    }
}
