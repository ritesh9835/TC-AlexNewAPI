using DataContracts.Common;
using DataContracts.ViewModels.ExtraServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceContracts.ExtraServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TazzerClean.Util;

namespace TazzerClean.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExtraServiceController : BaseController<IExtraServiceManager>
    {
        private readonly ILogger<ExtraServiceController> _logger;
        public ExtraServiceController(IExtraServiceManager serviceManager, ILogger<ExtraServiceController> logger) : base(serviceManager)
        {
            _logger = logger;
        }

        [HttpGet("getExtraServiceById")]
        [AllowAnonymous]
        public async Task<IActionResult> GetServiceById(Guid serviceId, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await Service.GetExtraServiceById(serviceId));
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in ServiceController:GetAsync");
                return BadRequest(ex);
            }
        }

        [HttpGet("getAllExtraServices")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllServices(CancellationToken cancellationToken)
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

        [HttpGet("services-by-parentService")]
        [AllowAnonymous]
        public async Task<IActionResult> ChildServices(Guid serviceId, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await Service.GetAllServicesByParentId(serviceId));
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in ServiceController:GetAsync");
                return BadRequest(ex);
            }
        }

        [HttpPost("create-extra-service")]
        [Authorize]
        public async Task<IActionResult> CreateAsync(ExtraServiceModel model)
        {
            try
            {
                var res = await Service.Create(model, currentUsers.Id);
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

        [HttpPut("update-extra-service")]
        [Authorize]
        public async Task<IActionResult> UpdateAsync(ExtraServiceModel model)
        {
            try
            {
                var res = await Service.Update(model,currentUsers.Id);
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

        [HttpPut("delete-extra-service")]
        [Authorize]
        public async Task<IActionResult> DeleteExtraService(Guid guid)
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
