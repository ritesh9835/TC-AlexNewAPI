using DataAccess.Database.Interfaces;
using DataContracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.UnitManager
{
    public class UnitManager : IUnitManager
    {
        private readonly IUnitRepository _unitRepository;
        public UnitManager(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
        }

        async Task IUnitManager.Create(Unit unit)
        {
            await _unitRepository.CreateUnit(unit);
        }

        async Task IUnitManager.Delete(Guid unitId)
        {
            await _unitRepository.DeleteAsync(unitId);
        }

        async Task<List<Unit>> IUnitManager.GetAll()
        {
            return await _unitRepository.GetAllAsync();
        }

        async Task IUnitManager.Update(Unit unit)
        {
            await _unitRepository.UpdateAsync(unit);
        }
    }
}
