﻿using Access2Justice.Api.Authorization;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static Access2Justice.Api.Authorization.Permissions;

namespace Access2Justice.Api.Controllers
{
    //[Authorize(Policy = "AnonymousPolicy")]
    //[Permission("AnonymousUser")]
    [Produces("application/json")]
    [Route("api/search")]
    public class SearchController : Controller
    {
        private readonly ILuisBusinessLogic luisBusinessLogic;

        public SearchController(ILuisBusinessLogic luisBusinessLogic)
        {
            this.luisBusinessLogic = luisBusinessLogic;
        }

        //[Permission(PermissionType.Anonymous , PermissionName.Search)]
        //[Permission("GetAsync")]
        [HttpPost]
        public async Task<IActionResult> GetAsync([FromBody]LuisInput luisInput)
        {
            if (string.IsNullOrWhiteSpace(luisInput.Sentence))
            {
                return BadRequest("search term cannot be empty string.");
            }

            var resources = await luisBusinessLogic.GetResourceBasedOnThresholdAsync(luisInput);
            return Content(resources);
        }
    }
}