using DataAccess.Database.Interfaces;
using DataContracts.Entities;
using DataContracts.ViewModels.Promocode;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.PromoCode
{
    public class PromoCodeService : IPromoCodeService
    {
        private readonly IPromoCodeRepository _promoCodeRepository;
        public PromoCodeService(IPromoCodeRepository promoCodeRepository)
        {
            _promoCodeRepository = promoCodeRepository;
        }
        public async Task<bool> CreatePromoCode(PromoCodeModel promoCodes, Guid id)
        {
            PromoCodes code = new PromoCodes
            {
                PromocodeName = promoCodes.PromocodeName,
                Code = promoCodes.Code,
                Validity = promoCodes.Validity,
                Discount = promoCodes.Discount,
                CreatedBy = id,
                UpdatedBy = id
            };

            await _promoCodeRepository.CreatePromoCode(code);

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _promoCodeRepository.DeleteAsync(id);

            return true;
        }

        public async Task<List<PromoCodeDetailsModel>> GetAllAsync()
        {
            var data = await _promoCodeRepository.GetAllAsync();

            List<PromoCodeDetailsModel> models = new List<PromoCodeDetailsModel>();

            foreach(var promocode in data)
            {
                models.Add(new PromoCodeDetailsModel
                {
                    PromocodeName = promocode.PromocodeName,
                    Code = promocode.Code,
                    Validity = promocode.Validity,
                    Discount = promocode.Discount,
                    Id = promocode.Id,
                    CreatedOn = promocode.CreatedOn,
                    IsActive = promocode.IsActive
                });
            };

            return models;
        }

        public async Task<double> GetPromoCodeByCode(string code)
        {
            var data = await _promoCodeRepository.GetPromoCodeByCode(code);

            var discount = 0.0;

            if (data == null)
                return discount;
            else if (!data.IsActive)
                return discount;
            else if (data.Validity < DateTime.UtcNow)
                return discount;

            discount = Convert.ToDouble(data.Discount);

            return discount;
        }

        public async Task<PromoCodeDetailsModel> GetPromoCodeById(Guid id)
        {
            var data = await _promoCodeRepository.GetPromoCodeById(id);

            PromoCodeDetailsModel model = new PromoCodeDetailsModel
            {
                PromocodeName = data.PromocodeName,
                Code = data.Code,
                Validity = data.Validity,
                Discount = data.Discount,
                Id = data.Id,
                CreatedOn = data.CreatedOn,
                IsActive = data.IsActive
            };

            return model;
        }

        public async Task<bool> UpdatePromoCode(PromoCodeModel promoCodes)
        {
            PromoCodes code = new PromoCodes
            {
                Id = promoCodes.Id,
                PromocodeName = promoCodes.PromocodeName,
                Code = promoCodes.Code,
                Validity = promoCodes.Validity,
                Discount = promoCodes.Discount,
            };

            await _promoCodeRepository.UpdatePromoCode(code);

            return true;
        }

        public async Task<bool> UpdateStatus(Guid id)
        {
            var data = await _promoCodeRepository.GetPromoCodeById(id);

            await _promoCodeRepository.UpdateStatus(id,!data.IsActive);

            return true;
        }
    }
}
