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
    public class CommonRepository : ICommonRepository
    {
        private readonly IConfiguration _config;
        public CommonRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<PostalLookup>> FindSuburbAsync(string code)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spSuburb_Get_ByCode";

            var result = await _connection.QueryMultipleAsync(
                    procedure,
                    new { code },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var suburbs = (await result.ReadAsync<PostalLookup>()).ToList();

            return suburbs;
        }

        public async Task<Role> FindRoleByName(string name)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spRole_Get_ByName";

            var result = await _connection.QueryMultipleAsync(
                    procedure,
                    new { name },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );
            var role = (await result.ReadAsync<Role>()).FirstOrDefault();

            return role;
        }

        public async Task<DashboardStats> GetDashboardStats()
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spDashboard_GetStats";

            var result = await _connection.QueryMultipleAsync(
                    procedure,
                    new {  },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var count = (await result.ReadAsync<DashboardStats>()).FirstOrDefault();

            return count;
        }
    }
}
