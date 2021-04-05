using DataAccess.Database.Interfaces;
using DataContracts.ViewModels.ExtraServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.ExtraServices
{
    public class ExtraServiceManager : IExtraServiceManager
    {
        private readonly IExtraServiceRepository _extraServiceRepository;
        private readonly IServiceRepository _serviceRepository;
        public ExtraServiceManager(IExtraServiceRepository extraServiceRepository, IServiceRepository serviceRepository)
        {
            _extraServiceRepository = extraServiceRepository;
            _serviceRepository = serviceRepository;
        }
        public async Task<bool> Create(ExtraServiceModel service, Guid userId)
        {
            DataContracts.Entities.ExtraServices extraService = new DataContracts.Entities.ExtraServices
            {
                ServiceName = service.ServiceName,
                ParentServiceId = service.ParentServiceId,
                Price = service.Price,
                IconPath = service.IconPath,
                CreatedBy = userId,
                UpdatedBy = userId,
                IsActive = service.IsActive
            };

            await _extraServiceRepository.CreateExtraService(extraService);

            return true;
        }

        public async Task<bool> Delete(Guid serviceId)
        {
            await _extraServiceRepository.DeleteAsync(serviceId);

            return true;
        }

        public async Task<List<ExtraServiceDetailsModel>> GetAll()
        {
            var data = await _extraServiceRepository.GetAllAsync();
            var services = await _serviceRepository.GetAllAsync();

            List<ExtraServiceDetailsModel> models = new List<ExtraServiceDetailsModel>();

            if(data!=null)
            {
                foreach (var service in data)
                {
                    var parentServiceName = services.Where(s => s.Id == service.ParentServiceId).Select(s => s.ServiceName).FirstOrDefault();

                    models.Add(new ExtraServiceDetailsModel
                    {
                        ServiceName = service.ServiceName,
                        CreatedOn = service.CreatedOn,
                        Id = service.Id,
                        IconPath = service.IconPath,
                        ParentServiceId = service.ParentServiceId,
                        ParentServiceName = parentServiceName,
                        Price = service.Price,
                        IsActive = service.IsActive
                    });
                };
            }

            return models;
        }

        public async Task<List<ExtraServiceDetailsModel>> GetAllServicesByParentId(Guid id)
        {
            var data = await _extraServiceRepository.GetAllServicesByParentId(id);
            var services = await _serviceRepository.GetAllAsync();

            List<ExtraServiceDetailsModel> models = new List<ExtraServiceDetailsModel>();

            if (data != null)
            {
                foreach (var service in data)
                {
                    var parentServiceName = services.Where(s => s.Id == service.ParentServiceId).Select(s => s.ServiceName).FirstOrDefault();

                    models.Add(new ExtraServiceDetailsModel
                    {
                        ServiceName = service.ServiceName,
                        CreatedOn = service.CreatedOn,
                        Id = service.Id,
                        IconPath = service.IconPath,
                        ParentServiceId = service.ParentServiceId,
                        ParentServiceName = parentServiceName,
                        Price = service.Price
                    });
                };
            }

            return models;
        }

        public async Task<ExtraServiceDetailsModel> GetExtraServiceById(Guid serviceId)
        {
            var data = await _extraServiceRepository.GetExtraServiceById(serviceId);

            if (data == null)
                return new ExtraServiceDetailsModel();

            //get parent service details
            var services = await _serviceRepository.GetServiceById(data.ParentServiceId);

            ExtraServiceDetailsModel model = new ExtraServiceDetailsModel
            {
                ServiceName = data.ServiceName,
                CreatedOn = data.CreatedOn,
                Id = data.Id,
                IconPath = data.IconPath,
                ParentServiceId = data.ParentServiceId,
                ParentServiceName = services.ServiceName,
                Price = data.Price
            };

            return model;
        }

        public async Task<bool> Update(ExtraServiceModel model, Guid userId)
        {
            DataContracts.Entities.ExtraServices extraService = new DataContracts.Entities.ExtraServices
            {
                Id = model.Id,
                ServiceName = model.ServiceName,
                ParentServiceId = model.ParentServiceId,
                Price = model.Price,
                IconPath = model.IconPath,
                UpdatedBy = userId,
            };

            await _extraServiceRepository.UpdateExtraService(extraService);

            return true;
        }

        public async Task<bool> UpdateStatus(Guid serviceId)
        {
            var data = await _extraServiceRepository.GetExtraServiceById(serviceId);

            if (data == null)
                return false;

            await _extraServiceRepository.UpdateStatus(serviceId,!data.IsActive);

            return true;
        }
    }
}
