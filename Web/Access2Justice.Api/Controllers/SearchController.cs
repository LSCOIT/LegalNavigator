using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/search")]
    public class SearchController : Controller
    {
        private ILuisProxy _luisProxy;
        private IBackendDatabaseService _db;
        private ILuisBusinessLogic _luisBusinessLogic;

        public SearchController(ILuisProxy luisProxy, IBackendDatabaseService backendDatabaseService, ILuisBusinessLogic luisBusinessLogic)
        {
            _luisProxy = luisProxy;
            _db = backendDatabaseService;
            _luisBusinessLogic = luisBusinessLogic;
        }

        [HttpGet("{query}")]
        public async Task<IActionResult> GetAsync(string query)
        {
            var internalResources = await _luisBusinessLogic.GetInternalResources(query);
            //var webResources = await _webSearchBusinessLogic.GetWebResources(query);

            return Content(internalResources, "application/json");
        }
    }
}