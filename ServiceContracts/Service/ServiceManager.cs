using DataAccess.Database.Interfaces;
using DataContracts.Entities;
using DataContracts.ViewModels.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.ServiceManager
{
    public class ServiceManager : IServiceManager
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly ICategoryRepository _categoryRepository;
        public ServiceManager(IServiceRepository serviceRepository, ICategoryRepository categoryRepository)
        {
            _serviceRepository = serviceRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<Services> GetServiceById(Guid serviceId)
        {
            var data = await _serviceRepository.GetServiceById(serviceId);
            var categories = await _categoryRepository.GetAllAsync();

            data.CategoryName = categories.Where(c => c.Id == data.Category).Select(c => c.Name).FirstOrDefault();
            data.SubcategoryName = categories.Where(c => c.Id == data.Subcategory).Select(c => c.Name).FirstOrDefault();

            return data;
        }

        async Task<bool> IServiceManager.Create(ServiceModel model,Guid userId)
        {
            Services service = new Services
            {
                Category = model.Category,
                Subcategory = model.Subcategory,
                CreatedBy = userId,
                UpdatedBy = userId,
                BillingUnit = model.BillingUnit,
                MinimumUnit = model.MinimumUnit,
                ServiceName = model.ServiceName,
                Description = model.Description,
                Price = model.Price,
                ServiceImage = model.ServiceImage
            };
            await _serviceRepository.CreateService(service);
            return true;
        }

        async Task<bool> IServiceManager.Delete(Guid serviceId)
        {
            await _serviceRepository.DeleteAsync(serviceId);
            return true;
        }

        async Task<List<Services>> IServiceManager.GetAll()
        {
            var data = await _serviceRepository.GetAllAsync();

            var categories = await _categoryRepository.GetAllAsync();

            foreach (var service in data)
            {
                service.CategoryName = categories.Where(c => c.Id == service.Category).Select(c => c.Name).FirstOrDefault();
                service.SubcategoryName = categories.Where(c => c.Id == service.Subcategory).Select(c => c.Name).FirstOrDefault();
            }

            return data.OrderByDescending(d => d.CreatedOn).ToList();
        }

        async Task<bool> IServiceManager.Update(Services model,Guid userId)
        {
            model.UpdatedBy = userId;
            await _serviceRepository.UpdateAsync(model);
            return true;
        }

        public async Task<bool> UpdateStatus(Guid serviceId)
        {
            var data = await _serviceRepository.GetServiceById(serviceId);

            if (data == null)
                return false;

            await _serviceRepository.UpdateStatus(serviceId,!data.IsActive);

            return true;
        }
    }
}
