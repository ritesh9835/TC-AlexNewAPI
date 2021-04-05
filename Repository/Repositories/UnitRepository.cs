using Dapper;
using DataAccess.Database.Interfaces;
using DataContracts.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Database.Repositories
{
    public class UnitRepository : IUnitRepository
    {
        private readonly IDbConnection _connection;
        public UnitRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        async Task IUnitRepository.CreateUnit(Unit unit)
        {
            var procedure = "uspUnit_Add";
            var reader = await _connection.QueryMultipleAsync(
                procedure,
                new { unit.Name, unit.Display, unit.Amount },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10);
        }

        async Task IUnitRepository.DeleteAsync(Guid id)
        {
            var procedure = "uspUnit_Delete";
            await _connection.QueryMultipleAsync(
                procedure,
                new { id },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10);
        }

        async Task<List<Unit>> IUnitRepository.GetAllAsync()
        {
            var procedure = "uspUnit_GetAll";
            var reader = await _connection.QueryMultipleAsync(
                procedure,
                new { },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10);
            var units = (await reader.ReadAsync<Unit>()).ToList();
            return units;
        }

        async Task IUnitRepository.UpdateAsync(Unit unit)
        {
            var procedure = "uspUnit_Update";
            await _connection.QueryMultipleAsync(
                procedure,
                new { unit.Id, unit.Name, unit.Display, unit.Amount },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 10);
        }
    }
}
