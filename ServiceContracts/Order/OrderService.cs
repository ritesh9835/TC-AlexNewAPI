using DataAccess.Database.Interfaces;
using DataContracts.Entities;
using DataContracts.ViewModels.Order;
using ServiceContracts.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TazzerClean.Util;

namespace ServiceContracts.Order
{
    public class OrderService : IOrderService
    {
        private readonly IOrderProcessRepository _orderProcessRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICommonRepository _commonRepository;
        private readonly IAuthManager _authManager;
        public OrderService(
            IOrderProcessRepository orderProcessRepository, 
            IUserRepository userRepository, 
            ICommonRepository commonRepository,
            IAuthManager authManager)
        {
            _orderProcessRepository = orderProcessRepository;
            _userRepository = userRepository;
            _commonRepository = commonRepository;
            _authManager = authManager;
        }

        public async Task<List<OrderDetailsModel>> GetAllOrderDetails()
        {
            var res = await _orderProcessRepository.GetAllOrderDetails();

            return res == null ? new List<OrderDetailsModel>() : res.OrderByDescending(r => r.CreatedOn).ToList();
        }

        public async Task<List<OrderDetailsModel>> GetProfessionalOrders(Guid id)
        {
            var res = await _orderProcessRepository.GetAllOrderDetailsOfProfessional(id);

            return res == null ? new List<OrderDetailsModel>() : res.OrderByDescending(r => r.CreatedOn).ToList() ;
        }

        public async Task<bool> MakeOrder(OrderModel model)
        {
            DataContracts.Entities.Order order = new DataContracts.Entities.Order
            {
                Id = Guid.NewGuid(),
                ServiceRequestId = model.ServiceRequestId,
                SubscriptionId = model.SubscriptionId,
                ServiceFrequency = model.ServiceFrequency,
                ExtraServiceList = model.ExtraServices,
                FirstName = model.FirstName,
                LastName = model.LastName,
                City = model.City,
                Amount = model.Amount,
                Flat = model.Flat,
                Street = model.Street,
                Country = model.Country,
                GrandTotal = model.GrandTotal,
                Promocode = model.Promocode,
                TransactionDetails = model.TransactionDetails
            };

            var res = await _orderProcessRepository.MakeOrder(order);

            //Get service request for email
            var serviceRequestData = await _orderProcessRepository.ServiceRequestById(order.ServiceRequestId);

            var userData = await _userRepository.FindByName(serviceRequestData.Email);

            //If user not registred
            if(userData==null)
            {
                Helper helper = new Helper();

                User user = new User
                {
                    Email = serviceRequestData.Email,
                    PhoneNumber = serviceRequestData.Phone,
                    Firstname = order.FirstName,
                    Lastname = order.LastName,
                    Address = new Address {
                        City = order.City,
                        StreetName = order.Street,
                        Country = order.Country,
                        HouseNumber = order.Flat,
                        ZipCode = serviceRequestData.Postcode
                    },
                    IsEmailVerified = true,
                    IsPhoneVerified = true,
                    Password = helper.PasswordGenerator(8)
                };

                await _authManager.RegisterAsync(user,string.Empty, (int)RoleType.Customer);
            }

            return true;
        }

        public async Task<bool> ServiceRequest(ServiceRequested model)
        {
            return await _orderProcessRepository.ServiceRequest(model);
        }

        public async Task ServiceRequestById(Guid id)
        {
            var res = await _orderProcessRepository.ServiceRequestById(id);
        }
    }
}
