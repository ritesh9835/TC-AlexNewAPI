using DataContracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Database.Interfaces
{
    public interface IPromoCodeRepository
    {
        Task<List<PromoCodes>> GetAllAsync();
        Task CreatePromoCode(PromoCodes promoCodes);
        Task UpdatePromoCode(PromoCodes promoCodes);
        Task DeleteAsync(Guid id);
        Task UpdateStatus(Guid id, bool status);
        Task<PromoCodes> GetPromoCodeById(Guid id);
        Task<PromoCodes> GetPromoCodeByCode(string code);
    }
}
