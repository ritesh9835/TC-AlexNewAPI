using DataContracts.Entities;
using DataContracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Database.Interfaces
{
    public interface IPagesRepository
    {
        Task<List<Pages>> GetAllAsync();
        Task<Pages> GetByServiceId(Guid id);
        Task CreatePage(Pages pages);
        Task UpdatePage(PagesModel pages);
        Task UpdateStatus(Guid id, bool status);
        Task<Pages> GetPageById(Guid id);
        Task DeletePage(Guid id);
    }
}
