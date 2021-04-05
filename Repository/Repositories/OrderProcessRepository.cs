using Dapper;
using DataAccess.Database.Interfaces;
using DataContracts.Entities;
using DataContracts.ViewModels.Order;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Database.Repositories
{
    public class OrderProcessRepository : IOrderProcessRepository
    {
        private readonly IConfiguration _config;
        private string _connectionString;

        public OrderProcessRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("TazzerCleanCs");
        }

        public async Task<bool> AddBilling(Billing model)
        {
            using IDbConnection _connection = new SqlConnection(_connectionString);
            _connection.Open();

            var procedure = "uspBilling_Create";

            var reader = await _connection.ExecuteAsync(
                            procedure,
                            new
                            {
                                model.Id,
                                model.ServiceRequestId,
                                model.OrderId

                            },
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 10);

            return true;
        }

        public async Task<bool> AssignProfessional(Guid id, Guid profId)
        {
            using IDbConnection _connection = new SqlConnection(_connectionString);
            _connection.Open();

            var procedure = "uspBilling_AssignProfessional";

            var reader = await _connection.ExecuteAsync(
                            procedure,
                            new
                            {
                                id,
                                profId

                            },
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 10);

            return true;
        }

        public async Task<List<OrderDetailsModel>> GetAllOrderDetails()
        {
            using IDbConnection _connection = new SqlConnection(_connectionString);
            _connection.Open();

            var procedure = "uspOrder_GetAllDetails";

            var query = await _connection.QueryMultipleAsync(
                    procedure,
                    new { },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var orderDetails = (await query.ReadAsync<OrderDetailsModel>()).ToList();

            foreach(var orderDetail in orderDetails)
            {
                if (string.IsNullOrEmpty(orderDetail.ExtraServices))
                    orderDetail.ExtraServiceList = JsonConvert.DeserializeObject<List<Guid>>(orderDetail.ExtraServices);
            }

            return orderDetails;
        }

        public async Task<List<OrderDetailsModel>> GetAllOrderDetailsOfProfessional(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_connectionString);
            _connection.Open();

            var procedure = "uspOrder_GetAllDetails";

            var query = await _connection.QueryMultipleAsync(
                    procedure,
                    new { id },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var orderDetails = (await query.ReadAsync<OrderDetailsModel>()).ToList();

            foreach (var orderDetail in orderDetails)
            {
                if (string.IsNullOrEmpty(orderDetail.ExtraServices))
                    orderDetail.ExtraServiceList = JsonConvert.DeserializeObject<List<Guid>>(orderDetail.ExtraServices);
            }

            return orderDetails;
        }

        public async Task<bool> MakeOrder(Order model)
        {
            using IDbConnection _connection = new SqlConnection(_connectionString);
            _connection.Open();

            model.Id = Guid.NewGuid();
            model.ServiceRequestId = Guid.Parse("B9A4929C-6215-4CD2-B37B-58032C812E1A");

            model.ExtraServices = JsonConvert.SerializeObject(model.ExtraServiceList);

            var procedure = "uspOrder_Create";

            var reader = await _connection.ExecuteAsync(
                            procedure,
                            new
                            {
                                model.Id,
                                model.ServiceRequestId,
                                model.SubscriptionId,
                                model.ServiceFrequency,
                                model.ExtraServices,
                                model.FirstName,
                                model.LastName,
                                model.City,
                                model.Country,
                                model.Flat,
                                model.Street,
                                model.Amount,
                                model.GrandTotal,
                                model.Promocode,
                                model.TransactionDetails

                            },
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 10);

            return true;
        }

        public async Task<bool> ServiceRequest(ServiceRequested model)
        {
            using IDbConnection _connection = new SqlConnection(_connectionString);
            _connection.Open();

            model.Id = Guid.NewGuid();

            var Time = TimeSpan.Parse(model.Time.ToString("hh:mm"));

            var procedure = "uspServiceRequest_Create";

            var reader = await _connection.ExecuteAsync(
                            procedure,
                            new
                            {
                                model.Id,
                                model.ServiceId,
                                model.TotalTime,
                                model.Date,
                                Time,
                                model.Email,
                                model.Phone,
                                model.Postcode

                            },
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 10);

            return true;
        }

        public async Task<ServiceRequested> ServiceRequestById(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_connectionString);
            _connection.Open();

            var procedure = "uspServices_GetServiceReqId";

            var query = await _connection.QueryMultipleAsync(
                    procedure,
                    new { },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var serviceRequest = (await query.ReadAsync<ServiceRequested>()).FirstOrDefault();

            return serviceRequest;
        }
    }
}
