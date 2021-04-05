using DataContracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.UnitManager
{
    public interface IUnitManager
    {
        Task<List<Unit>> GetAll();
        Task Create(Unit unit);
        Task Update(Unit unit);
        Task Delete(Guid unitId);
    }
}
