using Dapper;
using DataAccess.Database.Interfaces;
using DataContracts.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Database.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly IConfiguration _config;
        public SessionRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task AddSession(Session session)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spShoppingSession_AddOrUpdate";

            var query = await _connection.ExecuteAsync(
                procedure,
                new { },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10
                );

        }
    }
}
