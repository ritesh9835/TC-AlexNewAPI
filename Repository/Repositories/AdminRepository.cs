using Dapper;
using DataAccess.Database.Interfaces;
using DataContracts.Entities;
using DataContracts.ViewModels.User;
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
    public class AdminRepository : IAdminRepository
    {
        private readonly IConfiguration _config;
        public AdminRepository(IConfiguration config)
        {
            _config = config;
        }

        #region zip codes
        public async Task<List<PostalLookup>> GetAllZipCodes ()
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spZipCodes_Get_All";
           
            var query = await _connection.QueryMultipleAsync(
                    procedure,
                    new { },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 0
                );

            var locations = (await query.ReadAsync<PostalLookup>()).ToList();
            _connection.Close();
            return locations;
        }

        public async Task DeleteZipCode(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spZipCodes_Delete";

            await _connection.ExecuteAsync(
                    procedure,
                    new { id },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );
        }

        public async Task AddOrUpdateZipCode(PostalLookup lookup)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spZipCodes_AddOrUpdate";

            await _connection.ExecuteAsync(
                    procedure,
                    new { lookup.Id,lookup.Suburb,lookup.Country,lookup.Postcode,lookup.Latitude,lookup.Longitude,lookup.Type },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );
        }
        #endregion

        #region categories type
        public async Task<List<CategoryType>> GetAllCategoryType()
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spCategoriesType_GetAll";
            //_connection.Open()
            var query = await _connection.QueryMultipleAsync(
                    procedure,
                    new { },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var types = (await query.ReadAsync<CategoryType>()).ToList();
           // _connection.Close();
            return types;
        }

        public async Task DeleteCategoryType(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spCategoriesType_Delete";
            //_connection.Open();
            await _connection.ExecuteAsync(
                    procedure,
                    new { id },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );
            //_connection.Close();
        }

        public async Task AddOrUpdateCategoryType(CategoryType type)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spCategoriesType_AddOrUpdate";
            //_connection.Open();
            await _connection.ExecuteAsync(
                    procedure,
                    new { type.Id, type.Name,type.Description,type.Deleted,type.IconPath, type.TypeOrder, type.CatCode },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );
            //_connection.Close();
        }
        #endregion

        #region categories
        public async Task<List<Category>> GetAllCategories()
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spCategory_GetAll";

            var query = await _connection.QueryMultipleAsync(
                    procedure,
                    new { },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var categories = (await query.ReadAsync<Category>()).ToList();
           // _connection.Close();
            return categories;
        }

        public async Task DeleteCategory(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spCategory_Delete";

            await _connection.ExecuteAsync(
                    procedure,
                    new { id },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );
        }

        public async Task AddOrUpdateCategory(Category cat)
        {
            var procedure = "spCategory_AddOrUpdate";

//             await _connection.ExecuteAsync(
//                     procedure,
//                     new { cat.Id,cat.Name,cat.Description,cat.WorkingHours,cat.Price,cat.Deleted,cat.TypeId, cat.SubType,cat.PayPerHour, cat.PayPerSize },
//                     commandType: CommandType.StoredProcedure,
//                     commandTimeout: 10
//                 );
        }
        #endregion
        public async Task<List<CategoryType>> GetAllCategorySubType()
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();


            var procedure = "spCategoriesSubType_GetAll";
            //_connection.Open();
            var query = await _connection.QueryMultipleAsync(
                    procedure,
                    new { },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var types = (await query.ReadAsync<CategoryType>()).ToList();
            //_connection.Close();
            return types;
        }

        public async Task DeleteCategorySubType(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spCategoriesSubType_Delete";
            //_connection.Open();
            await _connection.ExecuteAsync(
                    procedure,
                    new { id },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );
            //_connection.Close();
        }

        public async Task AddOrUpdateCategorySubType(CategoryType type)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spCategoriesSubType_AddOrUpdate";
            //_connection.Open();
            await _connection.ExecuteAsync(
                    procedure,
                    new { type.Id, type.Name, type.Description, type.Deleted, type.PrimaryType, type.SubcatCode },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );
            //_connection.Close();
        }

        public async Task<List<CategoryType>> GetByTypeId(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spCategoriesSubType_GetByTypeId";

            var query = await _connection.QueryMultipleAsync(
                    procedure,
                    new { id },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var types = (await query.ReadAsync<CategoryType>()).ToList();

            return types;
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spCustomer_GetAll";

            var query = await _connection.QueryMultipleAsync(
                    procedure,
                    new { },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var customer = (await query.ReadAsync<Customer>()).ToList();

            return customer;
        }

        public async Task<List<Customer>> GetRecentCustomers(int count)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spCustomer_GetRecent";

            var query = await _connection.QueryMultipleAsync(
                    procedure,
                    new { count },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var customer = (await query.ReadAsync<Customer>()).ToList();

            return customer;
        }

        public async Task<List<Partner>> GetAllPartners()
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spPartner_GetAll";

            var query = await _connection.QueryMultipleAsync(
                    procedure,
                    new { },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var partner = (await query.ReadAsync<Partner>()).ToList();

            return partner;
        }
        public async Task<List<Professional>> GetAllProfessionals()
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spProfessional_GetAll";

            var query = await _connection.QueryMultipleAsync(
                    procedure,
                    new { },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var proff = (await query.ReadAsync<Professional>()).ToList();


            return proff;
        }
        public async Task<List<Professional>> GetRecentProfessionals(int count)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spProfessional_GetRecent";

            var query = await _connection.QueryMultipleAsync(
                    procedure,
                    new { count },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var proff = (await query.ReadAsync<Professional>()).ToList();

            return proff;
        }
        public async Task DeleteProfessional(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spProfessional_Delete";
            await _connection.ExecuteAsync(
                    procedure,
                    new { id },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );
        }

        public async Task ActiveProfessional(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spProfessional_Active";
            await _connection.ExecuteAsync(
                    procedure,
                    new { id },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );
        }

        public async Task DectiveProfessional(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spProfessional_Deactive";
            await _connection.ExecuteAsync(
                    procedure,
                    new { id },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );
        }

        public async Task DeleteCustomer(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spCustomer_Delete";
            await _connection.ExecuteAsync(
                    procedure,
                    new { id },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );
        }

        public async Task ActiveCustomer(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spCustomer_Active";
            await _connection.ExecuteAsync(
                    procedure,
                    new { id },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );
        }

        public async Task DeactiveCustomer(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spCustomer_Deactive";
            await _connection.ExecuteAsync(
                    procedure,
                    new { id },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );
        }

        public async Task DeletePartner(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spPartner  _Delete";
            await _connection.ExecuteAsync(
                    procedure,
                    new { id },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );
        }

        public async Task ActivePartner(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spPartner_Active";
            await _connection.ExecuteAsync(
                    procedure,
                    new { id },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );
        }

        public async Task DeactivePartner(Guid id)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spPartner_Deactive";
            await _connection.ExecuteAsync(
                    procedure,
                    new { id },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );
        }
    }
}
