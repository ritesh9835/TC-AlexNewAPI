using DataContracts.Entities;
using DataContracts.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Database.Interfaces
{
    public interface IOrderProcessRepository
    {
        Task<bool> ServiceRequest(ServiceRequested model);
        Task<bool> MakeOrder(Order model);
        Task<bool> AddBilling(Billing model);
        Task<List<OrderDetailsModel>> GetAllOrderDetails();
        Task<bool> AssignProfessional(Guid id , Guid profId);
        Task<ServiceRequested> ServiceRequestById(Guid id);
        Task<List<OrderDetailsModel>> GetAllOrderDetailsOfProfessional(Guid id);
    }
}
