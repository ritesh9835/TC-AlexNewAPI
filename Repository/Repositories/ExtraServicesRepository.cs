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
    public class ExtraServicesRepository : IExtraServiceRepository
    {
        private readonly IConfiguration _config;
        private string _connectionString;
        public ExtraServicesRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("TazzerCleanCs");
        }
        public async Task CreateExtraService(ExtraServices extraServices)
        {
            using IDbConnection _connection = new SqlConnection(_connectionString);
            _connection.Open();

            var procedure = "uspExtraService_Add";

            var reader = await _connection.ExecuteAsync(
                            procedure,
                            new
                            {
                                extraServices.ServiceName,
                                extraServices.IconPath,
                                extraServices.ParentServiceId,
                                extraServices.Price,
                                extraServices.CreatedBy

                            },
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 10);
        }

        public async Task DeleteAsync(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_connectionString);
            _connection.Open();

            var procedure = "uspExtraService_Delete";

            var reader = await _connection.ExecuteAsync(
                            procedure,
                            new
                            {
                                id

                            },
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 10);
        }

        public async Task<List<ExtraServices>> GetAllAsync()
        {
            using IDbConnection _connection = new SqlConnection(_connectionString);
            _connection.Open();

            var procedure = "uspExtraServices_GetAll";

            var query = await _connection.QueryMultipleAsync(
                    procedure,
                    new { },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var extraServices = (await query.ReadAsync<ExtraServices>()).ToList();

            return extraServices;
        }

        public async Task<List<ExtraServices>> GetAllServicesByParentId(Guid ParentServiceId)
        {
            using IDbConnection _connection = new SqlConnection(_connectionString);
            _connection.Open();

            var procedure = "uspExtraServices_GetAllByParentServiceId";

            var query = await _connection.QueryMultipleAsync(
                    procedure,
                    new { ParentServiceId },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var extraServices = (await query.ReadAsync<ExtraServices>()).ToList();

            return extraServices;
        }

        public async Task<ExtraServices> GetExtraServiceById(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_connectionString);
            _connection.Open();

            var procedure = "uspExtraServices_GetExtraServiceById";

            var query = await _connection.QueryMultipleAsync(
                    procedure,
                    new { id },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var extraService = (await query.ReadAsync<ExtraServices>()).FirstOrDefault();

            return extraService;
        }

        public async Task UpdateExtraService(ExtraServices extraService)
        {
            using IDbConnection _connection = new SqlConnection(_connectionString);
            _connection.Open();

            var procedure = "uspExtraService_Update";

            var reader = await _connection.ExecuteAsync(
                            procedure,
                            new
                            {
                                extraService.Id,
                                extraService.ServiceName,
                                extraService.IconPath,
                                extraService.ParentServiceId,
                                extraService.Price,
                                extraService.UpdatedBy

                            },
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 10);
        }

        public async Task UpdateStatus(Guid id, bool status)
        {
            using IDbConnection _connection = new SqlConnection(_connectionString);
            _connection.Open();

            var procedure = "uspExtraService_UpdateStatus";

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
