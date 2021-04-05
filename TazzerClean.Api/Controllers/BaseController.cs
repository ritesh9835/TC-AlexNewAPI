using DataContracts.Common;
using DataContracts.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TazzerClean.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<TService> : Controller
    {
        protected TService Service { get; }
        protected CurrentUser currentUsers;
        public BaseController(TService service)
        {
            Service = service;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var identityClaims = context.HttpContext.User;
            currentUsers = new CurrentUser();
            var claimValue = identityClaims.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var parseResult = Enum.TryParse<RoleType>(claimValue, true, out var role);
            currentUsers.Id = identityClaims.Claims.FirstOrDefault(c => c.Type == "id")?.Value == null ? Guid.Empty :  Guid.Parse(identityClaims.Claims.FirstOrDefault(c => c.Type == "id")?.Value);

            currentUsers.Role = parseResult ? role.ToString() : RoleType.Customer.ToString();
            currentUsers.Name = identityClaims.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        }
    }
}
