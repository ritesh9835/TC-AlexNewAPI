using DataContracts.Common;
using DataContracts.ViewModels.Promocode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceContracts.PromoCode;
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
    public class PromoCodeController : BaseController<IPromoCodeService>
    {
        private readonly ILogger<PromoCodeController> _logger;
        public PromoCodeController(IPromoCodeService promoCodeService, ILogger<PromoCodeController> logger) : base(promoCodeService)
        {
            _logger = logger;
        }

        [HttpGet("getPromocodeById")]
        [AllowAnonymous]
        public async Task<IActionResult> GetServiceById(Guid promocodeId, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await Service.GetPromoCodeById(promocodeId));
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in ServiceController:GetAsync");
                return BadRequest(ex);
            }
        }

        [HttpGet("getPromocodeBy-code")]
        [AllowAnonymous]
        public async Task<IActionResult> GetServiceByCode(string code, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await Service.GetPromoCodeByCode(code));
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in ServiceController:GetAsync");
                return BadRequest(ex);
            }
        }

        [HttpGet("getAllPromoCodes")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllSubscriptions(CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await Service.GetAllAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in ServiceController:GetAsync");
                return BadRequest(ex);
            }
        }

        [HttpPost("create-promocode")]
        [Authorize]
        public async Task<IActionResult> CreateAsync(PromoCodeModel model)
        {
            try
            {
                var res = await Service.CreatePromoCode(model, currentUsers.Id);
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

        [HttpPut("update-promocode")]
        [Authorize]
        public async Task<IActionResult> UpdateAsync(PromoCodeModel model)
        {
            try
            {
                var res = await Service.UpdatePromoCode(model);
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

        [HttpPut("delete-promocode")]
        [Authorize]
        public async Task<IActionResult> DeletePromocode(Guid guid)
        {
            try
            {
                var res = await Service.DeleteAsync(guid);
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
