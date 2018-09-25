using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Access2Justice.Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/UserRole")]
    public class UserRoleController : Controller
    {
        private readonly IUserRoleBusinessLogic userRoleBusinessLogic;

        public UserRoleController(IUserRoleBusinessLogic userRoleBusinessLogic)
        {
            this.userRoleBusinessLogic = userRoleBusinessLogic;
        }
        
        /// <summary>
        /// Get the user role by a user OId
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getUserPermissions")]
        public async Task<IActionResult> GetPermissionDataAsyn(string oId)
        {
            var users = await userRoleBusinessLogic.GetPermissionDataAsyn(oId);
            return Ok(users);
        }
    }
}