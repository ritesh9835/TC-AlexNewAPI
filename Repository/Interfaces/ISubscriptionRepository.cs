using DataContracts.Entities;
using DataContracts.ViewModels.Subscription;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Database.Interfaces
{
    public interface ISubscriptionRepository
    {
        Task<Subscription> GetSubscriptionById(Guid id);
        Task<List<Subscription>> GetAllAsync();
        Task<bool> CreateSubscription(Subscription subscription);
        Task<bool> UpdateSubscription(Subscription subscription);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> UpdateStatus(Guid id, bool status);
        Task<List<SubscriptionType>> GetAllTypes();
        Task<bool> CreateDiscount(SubscriptionDiscount planPercentage);
        public Task<bool> UpdateDiscount(SubscriptionDiscount planPercentage);
        public Task<List<SubscriptionDiscount>> GetDiscountsBySubscriptionId(Guid id);
        public Task<SubscriptionDiscount> GetDiscountById(Guid id);
        public Task<SubscriptionType> GetSubscriptionTypeById(Guid id);
    }
}
