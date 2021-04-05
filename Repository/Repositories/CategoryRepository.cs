using Dapper;
using DataAccess.Database.Interfaces;
using DataContracts.Entities;
using DataContracts.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Database.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IConfiguration _config;
        public CategoryRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "uspCategory_GetAll";
            var reader = await _connection.QueryMultipleAsync(
                procedure,
                new { },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10);
            var categories = (await reader.ReadAsync<Category>()).ToList();
            return categories;
        }

        public async Task CreateAsync(Category category)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "uspCategory_Add";
            var reader = await _connection.QueryMultipleAsync(
                procedure,
                new { category.ParentId, category.Name, category.Description, category.IconUrl, category.Code },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10);
        }

        public async Task<Category> GetByIdAsync(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "uspCategory_GetById";
            var reader = await _connection.QueryMultipleAsync(
                procedure,
                new { id },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10);
            var category = (await reader.ReadAsync<Category>()).FirstOrDefault();
            return category;
        }

        public async Task UpdateAsync(Category category)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "uspCategory_Update";
            await _connection.QueryMultipleAsync(
                procedure,
                new { category.Id, category.ParentId, category.Name, category.Description, category.IconUrl, category.Sequence, category.Code },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10);
        }

        public async Task DeleteAsync(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "uspCategory_Delete";
            await _connection.QueryMultipleAsync(
                procedure,
                new { id },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10);
        }

        public async Task UpAsync(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "uspCategory_UpSequence";
            await _connection.QueryMultipleAsync(
                procedure,
                new { id },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10);
        }

        public async Task DownAsync(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();
            var procedure = "uspCategory_DownSequence";
            await _connection.QueryMultipleAsync(
                procedure,
                new { id },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10);
        }

        public async Task<List<CategoryNavigationVM>> GeAllForMenuAsync()
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spCategoryType_Get_ForNavigation";

            var query = await _connection.QueryMultipleAsync(
                    procedure,
                    new { },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var types = (await query.ReadAsync<CategoryType>()).ToList();
            var categories = (await query.ReadAsync<Category>()).ToList();
            var subTypes = (await query.ReadAsync<CategoryType>()).ToList();

            var result = new List<CategoryNavigationVM>();

            foreach (var c in types)
            {
                var cnv = new CategoryNavigationVM()
                {
                    Name = c.Name,
                    Id = c.Id,
                    IconPath = c.IconPath,
                    Type = Enum.GetName(typeof(MainCategoryType), c.Type),
                    HasSubTypes = c.HasSubTypes
                };
                cnv.Categories = new List<Category>();
//                 cnv.Categories = categories.Where(x => x.TypeId == cnv.Id).ToList();
                cnv.SubTypes = subTypes.Where(x => x.PrimaryType == cnv.Id).ToList();

                result.Add(cnv);
            }
            return result;
        }

        public async Task<List<CategoryType>> FindByNameAsync(string name)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spCategoryType_Get_ByName";

            var result = await _connection.QueryMultipleAsync(
                    procedure,
                    new { name },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var categories = (await result.ReadAsync<CategoryType>()).ToList();

            return categories;
        }

        public async Task<Category> AddAsync(Category category)
        {
            return new Category();
        }

//         public async Task<List<Category>> GetByIdAsync(Guid id)
//         {
//             var procedure = "spCategory_GetBy_Id";
// 
//             var query = await _connection.QueryMultipleAsync(
//                     procedure,
//                     new { id },
//                     commandType: CommandType.StoredProcedure,
//                     commandTimeout: 10
//                 );
// 
//             var categories = (await query.ReadAsync<Category>()).ToList();
// 
// 
//             return categories;
//         }

        public async Task<List<Category>> GetByNameAsync(string name)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spCategory_GetBy_Name";

            var query = await _connection.QueryMultipleAsync(
                    procedure,
                    new { name },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var categories = (await query.ReadAsync<Category>()).ToList();

            return categories;
        }


        public async Task<Category> DeleteCategoryType(string id)
        {
            return new Category();
        }

        public async Task<bool> UpdateStatus(bool status, Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "uspCategory_UpdateStatus";
            await _connection.QueryMultipleAsync(
                procedure,
                new {   
                        id,
                        status  
                    },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10);

            return true;
        }
    }
}
