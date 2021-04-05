using DataContracts.Common;
using DataContracts.Entities;
using DataContracts.ViewModels.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceContracts.ServiceManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TazzerClean.Util;

namespace TazzerClean.Api.Controllers
{
    [Route("api/services")]
    [ApiController]
    public class ServiceController : BaseController<IServiceManager>
    {
        private readonly ILogger<ServiceController> _logger;
        private readonly IServiceManager _serviceManager;
        public ServiceController(ILogger<ServiceController> logger, IServiceManager serviceManager) : base(serviceManager)
        {
            _logger = logger;
            _serviceManager = serviceManager;
        }

        // GET: api/services
        [HttpGet("getAllServices")]
        //[Route("")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _serviceManager.GetAll());
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in ServiceController:GetAsync");
                return BadRequest(ex);
            }
        }

        // POST: api/services
        [HttpPost("create-service")]
        //[Route("")]
        [Authorize]
        public async Task<ActionResult<List<Services>>> CreateAsync(ServiceModel service)
        {
            try
            {
                await _serviceManager.Create(service,currentUsers.Id);

                var units = await _serviceManager.GetAll();
                return units;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in UnitController:CreateAsync");
                return BadRequest(ex);
            }
        }

        // PUT: api/services/{serviceId}
        [HttpPut("update-service")]
        //[Route("{serviceId}")]
        [Authorize]
        public async Task<IActionResult> UpdateAsync(Services service, CancellationToken cancellationToken)
        {
            /*try
            {
                await _serviceManager.Update(service);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in UnitController:UpdateAsync");
                BadRequest(ex);
            }*/
            var res = await _serviceManager.Update(service,currentUsers.Id);

            return Ok(new ResponseViewModel<bool>
            {
                Data = res,
                Message = res ? TazzerCleanConstants.SavedSuccess : TazzerCleanConstants.GeneralError
            });
        }

        // DELETE: api/services/{serviceId}
        [HttpPut("delete-service")]
        //[Route("{serviceId}")]
        [Authorize]
        public async Task<IActionResult> DeleteAsync(Guid serviceId, CancellationToken cancellationToken)
        {
            /*try
            {
                await _serviceManager.Delete(serviceId);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in ServiceController:DeleteAsync");
                BadRequest(ex);
            }*/

            var res = await _serviceManager.Delete(serviceId);

            return Ok(new ResponseViewModel<bool>
            {
                Data = res,
                Message = res ? TazzerCleanConstants.SavedSuccess : TazzerCleanConstants.GeneralError
            });
        }


        [HttpPut("update-status")]
        //[Route("{serviceId}")]
        [Authorize]
        public async Task<IActionResult> UpdateStatus(Guid serviceId, CancellationToken cancellationToken)
        {
            /*try
            {
                await _serviceManager.Delete(serviceId);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in ServiceController:DeleteAsync");
                BadRequest(ex);
            }*/

            var res = await _serviceManager.UpdateStatus(serviceId);

            return Ok(new ResponseViewModel<bool>
            {
                Data = res,
                Message = res ? TazzerCleanConstants.SavedSuccess : TazzerCleanConstants.GeneralError
            });
        }

        [HttpGet("geServiceById")]
        //[Route("")]
        [AllowAnonymous]
        public async Task<IActionResult> GetServiceById(Guid serviceId, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _serviceManager.GetServiceById(serviceId));
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in ServiceController:GetAsync");
                return BadRequest(ex);
            }
        }

    }
}
