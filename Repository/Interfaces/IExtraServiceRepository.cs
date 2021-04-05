using DataContracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Database.Interfaces
{
    public interface IExtraServiceRepository
    {
        Task<List<ExtraServices>> GetAllAsync();
        Task<List<ExtraServices>> GetAllServicesByParentId(Guid ParentServiceId);
        Task CreateExtraService(ExtraServices extraServices);
        Task UpdateExtraService(ExtraServices extraService);
        Task DeleteAsync(Guid id);
        Task UpdateStatus(Guid id, bool status);
        Task<ExtraServices> GetExtraServiceById(Guid id);
    }
}
