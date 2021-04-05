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
    public class PromoCodeRepository : IPromoCodeRepository
    {
        private readonly IConfiguration _config;
        private string _connectionString;
        public PromoCodeRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("TazzerCleanCs");
        }
        public async Task CreatePromoCode(PromoCodes promoCodes)
        {
            using IDbConnection _connection = new SqlConnection(_connectionString);
            _connection.Open();

            var procedure = "uspPromoCode_Add";

            var reader = await _connection.ExecuteAsync(
                            procedure,
                            new
                            {
                                promoCodes.PromocodeName,
                                promoCodes.Code,
                                promoCodes.Validity,
                                promoCodes.Discount,
                                promoCodes.CreatedBy

                            },
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 10);
        }

        public async Task DeleteAsync(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_connectionString);
            _connection.Open();

            var procedure = "uspPromoCode_Delete";

            var reader = await _connection.ExecuteAsync(
                            procedure,
                            new
                            {
                                id

                            },
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 10);
        }

        public async Task<List<PromoCodes>> GetAllAsync()
        {
            using IDbConnection _connection = new SqlConnection(_connectionString);
            _connection.Open();

            var procedure = "uspPromoCodes_GetAll";

            var query = await _connection.QueryMultipleAsync(
                    procedure,
                    new { },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var promocodes = (await query.ReadAsync<PromoCodes>()).ToList();

            return promocodes;
        }

        public async Task<PromoCodes> GetPromoCodeById(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_connectionString);
            _connection.Open();

            var procedure = "uspPromoCodes_GetPromoCodeById";

            var query = await _connection.QueryMultipleAsync(
                    procedure,
                    new { id },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var promocode = (await query.ReadAsync<PromoCodes>()).FirstOrDefault();

            return promocode;
        }

        public async Task UpdateStatus(Guid id, bool status)
        {
            using IDbConnection _connection = new SqlConnection(_connectionString);
            _connection.Open();

            var procedure = "uspPromoCode_UpdateStatus";

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

        public async Task UpdatePromoCode(PromoCodes promoCodes)
        {
            using IDbConnection _connection = new SqlConnection(_connectionString);
            _connection.Open();

            var procedure = "uspPromoCode_Update";

            var reader = await _connection.ExecuteAsync(
                            procedure,
                            new
                            {
                                promoCodes.Id,
                                promoCodes.PromocodeName,
                                promoCodes.Code,
                                promoCodes.Validity,
                                promoCodes.Discount

                            },
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 10);
        }

        public  async Task<PromoCodes> GetPromoCodeByCode(string code)
        {
            using IDbConnection _connection = new SqlConnection(_connectionString);
            _connection.Open();

            var procedure = "uspPromoCodes_GetPromoCodeByCode";

            var query = await _connection.QueryMultipleAsync(
                    procedure,
                    new { code },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var promocode = (await query.ReadAsync<PromoCodes>()).FirstOrDefault();

            return promocode;
        }
    }
}
