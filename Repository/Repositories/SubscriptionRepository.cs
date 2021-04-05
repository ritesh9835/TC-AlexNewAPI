using Dapper;
using DataAccess.Database.Interfaces;
using DataContracts.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Database.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly IConfiguration _config;

        public SubscriptionRepository(IConfiguration config)
        {
            _config = config;
        }
        public async Task<Subscription> GetSubscriptionById(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            if (_connection.State == ConnectionState.Closed) { _connection.Open(); }

            var procedure = "spSubscriptions";

            var query = await _connection.QueryMultipleAsync(
                procedure,
                new { id,Action="BYID" },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10
            );

            var subscription = (await query.ReadAsync<Subscription>()).FirstOrDefault();
            

            return subscription;
        }
        public async Task<List<Subscription>> GetAllAsync()
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            if (_connection.State == ConnectionState.Closed) { _connection.Open(); }

            var procedure = "spSubscriptions";

            var query = await _connection.QueryMultipleAsync(
                procedure,
                new {Action="SELECTALL" },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10
            );

            var subscriptions = (await query.ReadAsync<Subscription>()).ToList();
            
            return subscriptions;
        }
        public async Task<bool> CreateSubscription(Subscription subscription)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            if (_connection.State == ConnectionState.Closed){_connection.Open();}
            var procedure = "spSubscriptions";

            var rowsAffected = await _connection.ExecuteAsync(
                            procedure,
                            new
                            {
                                subscription.Id,
                                subscription.SubscriptionName,
                                subscription.CreatedBy,
                                Action= "Add"

                            },
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 10);
            
            return rowsAffected > 0;
        }
        public async Task<bool> UpdateSubscription(Subscription subscriptions)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            if (_connection.State == ConnectionState.Closed) { _connection.Open(); }

            var procedure = "spSubscriptions";

            var rowsAffected = await _connection.ExecuteAsync(
                procedure,
                new
                {
                    subscriptions.Id,
                    subscriptions.SubscriptionName,
                    Action="UPDATE"

                },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10);
            
            return rowsAffected > 0;
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            if (_connection.State == ConnectionState.Closed){_connection.Open();}

            var procedure = "spSubscriptions";

            var rowsAffected = await _connection.ExecuteAsync(
                            procedure,
                            new
                            {
                                id,
                                Action="DELETE"

                            },
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 10);
            
            return rowsAffected > 0;
        }
        public async Task<bool> UpdateStatus(Guid id, bool status)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            if (_connection.State == ConnectionState.Closed){_connection.Open();}

            var procedure = "spSubscriptions";

            var rowAffected = await _connection.ExecuteAsync(
                            procedure,
                            new
                            {
                                id,
                                IsActive=status,
                                Action= "UPDATESTATUS"

                            },
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 10);
            
            return rowAffected > 0;
        }

        public async Task<List<SubscriptionType>> GetAllTypes()
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            if (_connection.State == ConnectionState.Closed) { _connection.Open(); }

            var procedure = "spSubscriptionDiscounts";

            var query = await _connection.QueryMultipleAsync(
                procedure,
                new {Action= "SELECTALLTYPES" },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10
            );

            var subscriptionTypes = (await query.ReadAsync<SubscriptionType>()).ToList();
            
            return subscriptionTypes;
        }
        public async Task<SubscriptionDiscount> GetDiscountById(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            if (_connection.State == ConnectionState.Closed) { _connection.Open(); }

            var procedure = "spSubscriptionDiscounts";

            var query = await _connection.QueryMultipleAsync(
                procedure,
                new { id,Action = "BYID" },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10
            );

            var subscriptionType = await query.ReadSingleAsync<SubscriptionDiscount>();
            
            return subscriptionType;
        }
        public async Task<bool> CreateDiscount(SubscriptionDiscount discount)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            if (_connection.State == ConnectionState.Closed) { _connection.Open(); }

            var procedure = "spSubscriptionDiscounts";

            var reader = await _connection.ExecuteAsync(
                procedure,
                new
                {
                    discount.Id,
                    discount.SubscriptionTypeId,
                    discount.SubscriptionId,
                    discount.Month3,
                    discount.Month6,
                    discount.Month12,
                    discount.CreatedBy,
                    Action="ADD"

                },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10);
            
            return reader > 0;
        }
        public async Task<bool> UpdateDiscount(SubscriptionDiscount discount)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            if (_connection.State == ConnectionState.Closed) { _connection.Open(); }

            var procedure = "spSubscriptionDiscounts";

            var reader = await _connection.ExecuteAsync(
                procedure,
                new
                {
                    discount.Id,
                    discount.Month3,
                    discount.Month6,
                    discount.Month12,
                    Action="UPDATE"

                },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10);
            
            return reader > 0;
        }
        public async Task<List<SubscriptionDiscount>> GetDiscountsBySubscriptionId(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            if (_connection.State == ConnectionState.Closed) { _connection.Open(); }

            var procedure = "spSubscriptionDiscounts";

            var reader = await _connection.QueryMultipleAsync(
                procedure,
                new
                {
                   SubscriptionId=id,
                   Action= "BYSUBSCRIPTIONID"

                },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10);
            var result = (await reader.ReadAsync<SubscriptionDiscount>()).ToList();
            
            return result;
        }
        public async Task<SubscriptionType> GetSubscriptionTypeById(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            if (_connection.State == ConnectionState.Closed) { _connection.Open(); }

            var procedure = "spSubscriptions";

            var reader = await _connection.QueryMultipleAsync(
                procedure,
                new
                {
                    id,
                    Action = "TYPEBYID"

                },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10);
            var result = await reader.ReadSingleAsync<SubscriptionType>();
            
            return result;
        }
    }
}
