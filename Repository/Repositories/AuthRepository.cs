using Dapper;
using DataAccess.Database.Interfaces;
using DataContracts.Entities;
using DataContracts.ViewModels.User;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Database.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IConfiguration _config;
        public AuthRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<User> RegisterAsync(User user)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            //using IDbContextTransaction transaction = _context.Database.BeginTransaction();
            var procedure = "spUser_Register";
            var userProfileId = user.UserProfile.Id;
            var query = await _connection.QueryMultipleAsync(
                    procedure,
                    new { user.Id,user.Email,user.Password,user.Salt,user.RoleId,user.Avatar,user.Firstname,user.Lastname,user.PhoneNumber,userProfileId,user.PrimaryService,user.VerifyToken,user.TokenExpiry,user.UserCode },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var result = (await query.ReadAsync<User>()).FirstOrDefault();

            return result;
        }

        public async Task<User> ForgotPasswordCodeAsync(Guid id, string code)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spUser_ForgotPassword_Code";

            var query = await _connection.QueryMultipleAsync(
                    procedure,
                    new { id, code},
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var result = (await query.ReadAsync<User>()).FirstOrDefault();
            return result;
        }

        public async Task ChangePasswordAsync(User user)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spUser_ChangePassword";

            var query = await _connection.ExecuteAsync(
                    procedure,
                    new { user.Id, user.Password, user.Salt },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10

                );

        }

        public async Task RegisterProfessional(CustomerRequest user)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spProfessionalForm_Add";

            var query = await _connection.ExecuteAsync(
                procedure,
                new { user.UserId,user.ProfessionalType/*,user.PrimaryService,user.AreasAroundPostcode,user.OwnMaterialTools,user.OwnTransport,user.OtherEmployment,user.WillingToTrain,user.Uniform*/ },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10
                );
            _connection.Close();
        }

        public async Task<User> GoogleLoginAsync()
        {
            return new User();
        }

        public async Task<bool> VerifyAccount(Guid userId)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spUsers_VerifyAccount";

            var query = await _connection.ExecuteAsync(
                    procedure,
                    new { userId },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10

                );

            return true;
        }

        public async Task<User> GetUserByToken(string token)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spUsers_Get_ByToken";

            var query = await _connection.QueryMultipleAsync(
                    procedure,
                    new { token },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var result = (await query.ReadAsync<User>()).FirstOrDefault();
            return result;
        }

        public async Task<ProfessionalInvitationModel> GetInvitedUser(string token)
        {
            using IDbConnection _connection = new SqlConnection(_config.GetConnectionString("TazzerCleanCs"));
            _connection.Open();

            var procedure = "spUsers_Get_InvitedUser";

            var query = await _connection.QueryMultipleAsync(
                    procedure,
                    new { token },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 10
                );

            var result = (await query.ReadAsync<ProfessionalInvitationModel>()).FirstOrDefault();
            return result;
        }
    }
}
