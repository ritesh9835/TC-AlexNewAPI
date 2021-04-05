using DataContracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Database.Interfaces
{
    public interface IServiceRepository
    {
        Task<List<Services>> GetAllAsync();
        Task CreateService(Services service);
        Task UpdateAsync(Services service);
        Task DeleteAsync(Guid id);
        Task UpdateStatus(Guid id,bool status);
        Task<Services> GetServiceById(Guid id);

    }
}
