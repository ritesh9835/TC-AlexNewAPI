using Dapper;
using DataAccess.Database.Interfaces;
using DataContracts.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Database.Repositories
{
    class ServiceRepository : IServiceRepository
    {
        private readonly IConfiguration _config;
        public ServiceRepository(IConfiguration config)
        {
            _config = config;
        }

        //async Task IServiceRepository.CreateService(Service service)
        //{
        //    var procedure = "uspService_Add";
        //    var reader = await _connection.QueryMultipleAsync(
        //        procedure,
        //        new { 
        //            service.CategoryId,
        //            service.Name,
        //            service.Description,
        //            service.Type,
        //            service.IsActive,
        //            service.Price,
        //            service.PriceUnit,
        //            service.Quantity,
        //            service.QuantityUnit,
        //            service.Duration,
        //            service.DurationUnit,
        //            service.Extra
        //        },
        //        commandType: CommandType.StoredProcedure,
        //        commandTimeout: 10);
        //}

        //async Task IServiceRepository.DeleteAsync(Guid id)
        //{
        //    var procedure = "uspService_Delete";
        //    await _connection.QueryMultipleAsync(
        //        procedure,
        //        new { id },
        //        commandType: CommandType.StoredProcedure,
        //        commandTimeout: 10);
        //}

        //async Task<List<Service>> IServiceRepository.GetAllAsync()
        //{
        //    var procedure = "uspService_GetAll";
        //    var reader = await _connection.QueryMultipleAsync(
        //        procedure,
        //        new { },
        //        commandType: CommandType.StoredProcedure,
        //        commandTimeout: 10);
        //    var services = (await reader.ReadAsync<Service>()).ToList();
        //    return services;
        //}

        //async Task IServiceRepository.UpdateAsync(Service service)
        //{
        //    var procedure = "uspService_Update";
        //    await _connection.QueryMultipleAsync(
        //        procedure,
        //        new
        //        {
        //            service.Id,
        //            service.CategoryId,
        //            service.Name,
        //            service.Description,
        //            service.Type,
        //            service.IsActive,
        //            service.Price,
        //            service.PriceUnit,
        //            service.Quantity,
        //            service.QuantityUnit,
        //            service.Duration,
        //            service.DurationUnit,
        //            service.Extra
        //        },
        //        commandType: CommandType.StoredProcedure,
        //        commandTimeout: 10);
        //}


        public async Task CreateService(Services service)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "uspServices_Add";

            var reader = await _connection.ExecuteAsync(
                            procedure,
                            new
                            {
                                service.Category,
                                service.Subcategory,
                                service.BillingUnit,
                                service.MinimumUnit,
                                service.Description,
                                service.CreatedBy,
                                service.UpdatedBy,
                                service.ServiceName,
                                service.Price,
                                service.ServiceImage
                            },
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 10);
        }
        public async Task DeleteAsync(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "uspServices_Delete";

            var reader = await _connection.ExecuteAsync(
                            procedure,
                            new
                            {
                                id

                            },
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 10);
        }

        public async Task<List<Services>> GetAllAsync()
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "uspServices_GetAll";

            var query = await _connection.QueryMultipleAsync(
                    procedure,
                    new { },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var services = (await query.ReadAsync<Services>()).ToList();

            return services;
        }

        public async Task<Services> GetServiceById(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "uspServices_GetServiceById";

            var query = await _connection.QueryMultipleAsync(
                    procedure,
                    new { id },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var services = (await query.ReadAsync<Services>()).FirstOrDefault();
            return services;
        }

        public async Task UpdateAsync(Services service)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "uspServices_Update";

            var reader = await _connection.ExecuteAsync(
                            procedure,
                            new
                            {
                                service.Id,
                                service.Category,
                                service.Subcategory,
                                service.BillingUnit,
                                service.MinimumUnit,
                                service.Description,
                                service.UpdatedBy,
                                service.ServiceName,
                                service.Price,
                                service.ServiceImage

                            },
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 10);
        }

        public async Task UpdateStatus(Guid id, bool status)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "uspServices_UpdateStatus";

            var reader = await _connection.ExecuteAsync(
                            procedure,
                            new
                            {
                                id,
                                status

                            },
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 10);
        }


    }
}
