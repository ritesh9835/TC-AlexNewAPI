using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataContracts.Entities;
using System.Threading;
using Microsoft.Extensions.Logging;
using ServiceContracts.UnitManager;

namespace TazzerClean.Api.Controllers
{
    [Route("api/units")]
    [ApiController]
    [AllowAnonymous]
    public class UnitController : Controller
    {
        private readonly ILogger<UnitController> _logger;
        private readonly IUnitManager _unitManager;
        public UnitController(ILogger<UnitController> logger, IUnitManager unitManager)
        {
            _logger = logger;
            _unitManager = unitManager;
        }

        // GET: api/units
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Unit>>> GetAsync(CancellationToken cancellationToken)
        {
            try
            {
                var units = await _unitManager.GetAll();
                return units;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in UnitController:GetAsync");
                return BadRequest(ex);
            }
        }

        // POST: api/units
        [HttpPost]
        [Route("")]
        [Authorize]
        public async Task<ActionResult<List<Unit>>> CreateAsync(Unit unit)
        {
            try
            {
                await _unitManager.Create(unit);

                var units = await _unitManager.GetAll();
                return units;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in UnitController:CreateAsync");
                return BadRequest(ex);
            }
        }

        // PUT: api/units/{unitId}
        [HttpPut]
        [Route("{unitId}")]
        [Authorize]
        public async Task UpdateAsync(Unit unit, CancellationToken cancellationToken)
        {
            try
            {
                await _unitManager.Update(unit);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in UnitController:UpdateAsync");
                BadRequest(ex);
            }
        }

        // DELETE: api/units/{unitId}
        [HttpDelete]
        [Route("{unitId}")]
        [Authorize]
        public async Task DeleteAsync(Guid unitId, CancellationToken cancellationToken)
        {
            try
            {
                await _unitManager.Delete(unitId);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error has been thrown in UnitController:DeleteAsync");
                BadRequest(ex);
            }
        }

    }
}
