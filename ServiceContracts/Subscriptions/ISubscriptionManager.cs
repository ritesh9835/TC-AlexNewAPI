using DataContracts.ViewModels.Subscription;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceContracts.Subscriptions
{
    public interface ISubscriptionManager
    {
        Task<List<SubscriptionVM>> GetAll();
        Task<SubscriptionVM> GetSubscriptionById(Guid subscriptionId);
        Task<bool> Create(SubscriptionCreateRequest subscription, Guid userId);
        Task<bool> Update(SubscriptionUpdateRequest subscription, Guid userId);
        Task<bool> Delete(Guid subscriptionId);
        Task<bool> UpdateStatus(Guid subscriptionId);
    }
}
