using DataContracts.Entities;
using DataContracts.ViewModels.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.ServiceManager
{
    public interface IServiceManager
    {
        Task<List<Services>> GetAll();
        Task<bool> Create(ServiceModel service,Guid userId);
        Task<bool> Update(Services model, Guid userId);
        Task<bool> Delete(Guid serviceId);
        Task<Services> GetServiceById(Guid serviceId);
        Task<bool> UpdateStatus(Guid serviceId);
    }
}
