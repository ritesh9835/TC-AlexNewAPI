using DataContracts.Common;
using DataContracts.Entities;
using DataContracts.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts.Pages;
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
    public class PagesController : BaseController<IPagesService>
    {
        public PagesController(IPagesService pagesService) : base(pagesService)
        {
        }

        [HttpGet("getAllPages")]
        [Authorize(Policy = nameof(Policies.Admin))]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                return Ok(await Service.GetAll());
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet("getPageById")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid guid)
        {
            try
            {
                return Ok(await Service.GetPageById(guid));
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet("getPageByServiceId")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByServiceId(Guid ServiceId)
        {
            try
            {
                return Ok(await Service.GetPageByServiceId(ServiceId));
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPost("create-page")]
        [Authorize(Policy = nameof(Policies.Admin))]
        public async Task<IActionResult> CreatePage([FromBody] PagesModel model)
        {
            try
            {
                var pageWithService=await Service.GetPageByServiceId(model.ServiceId);
                if (pageWithService!=null)
                {
                    return BadRequest(new ResponseViewModel<bool>
                    {
                        Data = false,
                        Message = TazzerCleanConstants.ServicePageExists
                    });
                }
                var res = await Service.Create(model,currentUsers.Id);
                return Ok( new ResponseViewModel<bool> {
                    Data = res,
                    Message = res ? TazzerCleanConstants.SavedSuccess : TazzerCleanConstants.GeneralError
                });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPut("update-page")]
        [Authorize(Policy = nameof(Policies.Admin))]
        public async Task<IActionResult> UpdatePage([FromBody] PagesModel model)
        {
            try
            {
                var res = await Service.Update(model, currentUsers.Id);
                return Ok(new ResponseViewModel<bool>
                {
                    Data = res,
                    Message = res ? TazzerCleanConstants.SavedSuccess : TazzerCleanConstants.GeneralError
                });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPut("update-status")]
        [Authorize(Policy = nameof(Policies.Admin))]
        public async Task<IActionResult> UpdateStatus(Guid pageId)
        {
            try
            {
                var res = await Service.UpdateStatus(pageId);
                return Ok(new ResponseViewModel<bool>
                {
                    Data = res,
                    Message = res ? TazzerCleanConstants.SavedSuccess : TazzerCleanConstants.GeneralError
                });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpPut("delete-page")]
        [Authorize(Policy = nameof(Policies.Admin))]
        public async Task<IActionResult> DeletePage(Guid pageId)
        {
            try
            {
                var res = await Service.DeletePage(pageId);
                return Ok(new ResponseViewModel<bool>
                {
                    Data = res,
                    Message = res ? TazzerCleanConstants.SavedSuccess : TazzerCleanConstants.GeneralError
                });
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
    }
}
