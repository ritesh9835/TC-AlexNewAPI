using DataContracts.ViewModels.ExtraServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.ExtraServices
{
    public interface IExtraServiceManager
    {
        Task<List<ExtraServiceDetailsModel>> GetAll();
        Task<List<ExtraServiceDetailsModel>> GetAllServicesByParentId(Guid id);
        Task<bool> Create(ExtraServiceModel service, Guid userId);
        Task<bool> Update(ExtraServiceModel model, Guid userId);
        Task<bool> Delete(Guid serviceId);
        Task<ExtraServiceDetailsModel> GetExtraServiceById(Guid serviceId);
        Task<bool> UpdateStatus(Guid serviceId);
    }
}
