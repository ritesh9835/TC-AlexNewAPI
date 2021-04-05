using DataAccess.Database.Interfaces;
using DataContracts.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.Pages
{
    public class PagesService : IPagesService
    {
        private readonly IPagesRepository _pagesRepository;
        private readonly IServiceRepository _serviceRepository;
        public PagesService(IPagesRepository pagesRepository, IServiceRepository serviceRepository)
        {
            _pagesRepository = pagesRepository;
            _serviceRepository = serviceRepository;
        }

        public async Task<bool> Create(PagesModel model, Guid userId)
        {

            var data = await _pagesRepository.GetByServiceId(model.ServiceId);

            if (data != null)
                return false;

            DataContracts.Entities.Pages page = new DataContracts.Entities.Pages
            {
                PageTitle = model.PageTitle,
                PageImage = model.PageImage,
                Template = model.Template,
                CreatedBy = userId,
                IsActive = model.IsActive,
                ServiceId = model.ServiceId
            };

            page.Sections = JsonConvert.SerializeObject(model.Sections);

            await _pagesRepository.CreatePage(page);

            return true;
        }

        public async Task<bool> DeletePage(Guid pageId)
        {
            await _pagesRepository.DeletePage(pageId);

            return true;
        }

        public async Task<List<DataContracts.Entities.Pages>> GetAll()
        {
            var data = await _pagesRepository.GetAllAsync();
            var services = await _serviceRepository.GetAllAsync();

            foreach(var page in data)
            {
                page.ServiceName = services.Where(s => s.Id == page.ServiceId).Select(s => s.ServiceName).FirstOrDefault();
            }

            return data;
        }

        public async Task<DataContracts.Entities.Pages> GetPageById(Guid pageId)
        {
            
            var data = await _pagesRepository.GetPageById(pageId);
            var serviceName = await _serviceRepository.GetServiceById(data.ServiceId);

            data.ServiceName = serviceName.ServiceName;

            return data;
        }

        public async Task<DataContracts.Entities.Pages> GetPageByServiceId(Guid ServiceId)
        {
            var data = await _pagesRepository.GetByServiceId(ServiceId);
            if (data != null)
            {
                var serviceName = await _serviceRepository.GetServiceById(data.ServiceId);
                data.ServiceName = serviceName?.ServiceName;
            }

            return data;
        }

        public async Task<bool> Update(PagesModel model, Guid userId)
        {
            await _pagesRepository.UpdatePage(model);

            return true;
        }

        public async Task<bool> UpdateStatus(Guid pageId)
        {
            var data = await _pagesRepository.GetPageById(pageId);

            await _pagesRepository.UpdateStatus(pageId,!data.IsActive);

            return true;
        }
    }
}
