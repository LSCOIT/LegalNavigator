using Access2Justice.Api.Authorization;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using static Access2Justice.Api.Authorization.Permissions;

namespace Access2Justice.Api.Controllers
{
    [ExcludeFromCodeCoverage]
    [Produces("application/json")]
    [Route("api/search")]
    public class SearchController : Controller
    {
        private readonly ILuisBusinessLogic luisBusinessLogic;

        public SearchController(ILuisBusinessLogic luisBusinessLogic)
        {
            this.luisBusinessLogic = luisBusinessLogic;
        }

        /// <summary>
        /// Get resource based on threshold
        /// </summary>
        /// <remarks>
        /// Helps to get resource based on threshold
        /// </remarks>
        /// <param name="luisInput"></param>
        /// <response code="200">Get resource based on threshold</response>
        /// <response code="500">Failure</response>
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