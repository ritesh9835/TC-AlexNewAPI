using DataContracts.Entities;
using DataContracts.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.Order
{
    public interface IOrderService
    {
        Task<bool> ServiceRequest(ServiceRequested model);
        Task<bool> MakeOrder(OrderModel model);
        Task<List<OrderDetailsModel>> GetAllOrderDetails();
        Task<List<OrderDetailsModel>> GetProfessionalOrders(Guid id);
        Task ServiceRequestById(Guid id);
    }
}
