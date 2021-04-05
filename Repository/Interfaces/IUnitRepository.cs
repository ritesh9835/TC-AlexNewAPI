using DataContracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Database.Interfaces
{
    public interface IUnitRepository
    {
        Task<List<Unit>> GetAllAsync();
        Task CreateUnit(Unit unit);
        Task UpdateAsync(Unit unit);
        Task DeleteAsync(Guid unitId);
    }
}
