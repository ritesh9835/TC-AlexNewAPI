using DataContracts.Common;
using DataContracts.Entities;
using DataContracts.ViewModels.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceContracts.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TazzerClean.Util;

namespace TazzerClean.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseController<IOrderService>
    {
        private readonly ILogger<OrderController> _logger;
        public OrderController(IOrderService orderService, ILogger<OrderController> logger) : base(orderService)
        {
            _logger = logger;
        }

        [HttpPost("service-request")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAsync(ServiceRequested model)
        {
            try
            {
                var res = await Service.ServiceRequest(model);
                return Ok(new ResponseViewModel<bool>
                {
                    Data = res,
                    Message = res ? TazzerCleanConstants.SavedSuccess : TazzerCleanConstants.GeneralError
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in UnitController:CreateAsync");
                return BadRequest(ex);
            }
        }

        [HttpPost("make-order")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateOrder(OrderModel model)
        {
            try
            {
                var res = await Service.MakeOrder(model);
                return Ok(new ResponseViewModel<bool>
                {
                    Data = res,
                    Message = res ? TazzerCleanConstants.SavedSuccess : TazzerCleanConstants.GeneralError
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in UnitController:CreateAsync");
                return BadRequest(ex);
            }
        }

        [HttpGet("all-order-details")]
        [AllowAnonymous]
        public async Task<IActionResult> AllOrderDetails()
        {
            try
            {
                return Ok(await Service.GetAllOrderDetails());
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in UnitController:CreateAsync");
                return BadRequest(ex);
            }
        }

        [HttpGet("professional-orders")]
        [Authorize(Policy = nameof(Policies.Professional))]
        public async Task<IActionResult> ProfessionalOrders()
        {
            try
            {
                return Ok(await Service.GetProfessionalOrders(currentUsers.Id));
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in UnitController:CreateAsync");
                return BadRequest(ex);
            }
        }

        [HttpGet("service-requestBy-id")]
        [AllowAnonymous]
        public async Task<IActionResult> ServiceRequestById(Guid id)
        {
            try
            {
                await Service.ServiceRequestById(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in UnitController:CreateAsync");
                return BadRequest(ex);
            }
        }

    }
}
