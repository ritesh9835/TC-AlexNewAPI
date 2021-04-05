using DataContracts.Entities;
using DataContracts.ViewModels.Promocode;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.PromoCode
{
    public interface IPromoCodeService
    {
        Task<List<PromoCodeDetailsModel>> GetAllAsync();
        Task<bool> CreatePromoCode(PromoCodeModel promoCodes,Guid id);
        Task<bool> UpdatePromoCode(PromoCodeModel promoCodes);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> UpdateStatus(Guid id);
        Task<PromoCodeDetailsModel> GetPromoCodeById(Guid id);
        Task<double> GetPromoCodeByCode(string code);
    }
}
