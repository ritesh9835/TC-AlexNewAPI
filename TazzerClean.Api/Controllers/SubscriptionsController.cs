using DataContracts.Common;
using DataContracts.ViewModels.Subscription;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceContracts.Subscriptions;
using System;
using System.Threading;
using System.Threading.Tasks;
using TazzerClean.Util;

namespace TazzerClean.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : BaseController<ISubscriptionManager>
    {
        private readonly ILogger<SubscriptionsController> _logger;
        public SubscriptionsController(ISubscriptionManager subscriptionService, ILogger<SubscriptionsController> logger) : base(subscriptionService)
        {
            _logger = logger;
        }

        [HttpGet("getSubscriptionById")]
        [AllowAnonymous]
        public async Task<IActionResult> GetServiceById(Guid subscriptionId, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await Service.GetSubscriptionById(subscriptionId));
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in ServiceController:GetAsync");
                return BadRequest(ex);
            }
        }

        [HttpGet("getAllSubscriptions")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllSubscriptions(CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await Service.GetAll());
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in ServiceController:GetAsync");
                return BadRequest(ex);
            }
        }

        [HttpPost("create-subscription")]
        [Authorize]
        public async Task<IActionResult> CreateAsync(SubscriptionCreateRequest subscription)
        {
            try
            {
                var res = await Service.Create(subscription, currentUsers.Id);
                return Ok(new ResponseViewModel<bool> {
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

        [HttpPut("update-subscription")]
        [Authorize]
        public async Task<IActionResult> UpdateAsync(SubscriptionUpdateRequest subscription)
        {
            try
            {
                var res = await Service.Update(subscription, currentUsers.Id);
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

        [HttpPut("update-status")]
        [Authorize]
        public async Task<IActionResult> UpdateStatus(Guid guid)
        {
            try
            {
                var res = await Service.UpdateStatus(guid);
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

        [HttpPut("delete-subscription")]
        [Authorize]
        public async Task<IActionResult> DeleteSubscriptions(Guid guid)
        {
            try
            {
                var res = await Service.Delete(guid);
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
    }
}
