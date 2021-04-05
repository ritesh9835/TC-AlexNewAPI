using Dapper;
using DataAccess.Database.Interfaces;
using DataContracts.Entities;
using DataContracts.ViewModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Database.Repositories
{
    public class PagesRepository : IPagesRepository
    {
        private readonly IConfiguration _config;
        public PagesRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task CreatePage(Pages pages)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            //var Sections = JsonConvert.SerializeObject(pages.Sections);

            var procedure = "uspPages_Add";

            var reader = await _connection.ExecuteAsync(
                            procedure,
                            new
                            {
                                pages.PageTitle,
                                pages.Sections,
                                pages.PageImage,
                                pages.Template,
                                pages.IsActive,
                                pages.CreatedBy,
                                pages.ServiceId

                            },
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 10);
        }

        public async Task DeletePage(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "uspPages_Delete";

            var reader = await _connection.ExecuteAsync(
                            procedure,
                            new
                            {
                                id

                            },
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 10);
        }

        public async Task<List<Pages>> GetAllAsync()
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "uspPages_GetAll";

            var query = await _connection.QueryMultipleAsync(
                    procedure,
                    new { },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var pages = (await query.ReadAsync<Pages>()).ToList();

            foreach(var page in pages)
            {
                if (!string.IsNullOrEmpty(page.Sections))
                    page.SectionsList = JsonConvert.DeserializeObject<List<Sections>>(page.Sections);
            }

            return pages;
        }

        public async Task<Pages> GetByServiceId(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "uspPages_GetByServiceId";

            var query = await _connection.QueryMultipleAsync(
                    procedure,
                    new { id },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var page = (await query.ReadAsync<Pages>()).FirstOrDefault();

            if (page!=null && !string.IsNullOrEmpty(page.Sections))
                page.SectionsList = JsonConvert.DeserializeObject<List<Sections>>(page.Sections);

            return page;
        }

        public async Task<Pages> GetPageById(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "uspPages_GetById";

            var query = await _connection.QueryMultipleAsync(
                    procedure,
                    new { id },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );  

            var page = (await query.ReadAsync<Pages>()).FirstOrDefault();

            if (!string.IsNullOrEmpty(page.Sections))
                page.SectionsList = JsonConvert.DeserializeObject<List<Sections>>(page.Sections);

            return page;
        }

        public async Task UpdatePage(PagesModel pages)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var Sections = JsonConvert.SerializeObject(pages.Sections);

            var procedure = "uspPages_Update";

            var reader = await _connection.ExecuteAsync(
                            procedure,
                            new
                            {
                                pages.Id,
                                pages.PageTitle,
                                Sections,
                                pages.PageImage,
                                pages.Template,
                                pages.IsActive,
                                pages.ServiceId
                            },
                            commandType: CommandType.StoredProcedure,
                            commandTimeout: 10);
        }

        public async Task UpdateStatus(Guid id, bool status)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "uspPages_UpdateStatus";

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
