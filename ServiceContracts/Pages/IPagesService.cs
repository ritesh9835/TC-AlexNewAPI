using DataContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.Pages
{
    public interface IPagesService
    {
        Task<List<DataContracts.Entities.Pages>> GetAll();
        Task<bool> Create(PagesModel model, Guid userId);
        Task<bool> Update(PagesModel model, Guid userId);
        Task<DataContracts.Entities.Pages> GetPageById(Guid pageIdId);
        Task<DataContracts.Entities.Pages> GetPageByServiceId(Guid ServiceId);
        Task<bool> UpdateStatus(Guid pageIdId);
        Task<bool> DeletePage(Guid pageId);
    }
}
