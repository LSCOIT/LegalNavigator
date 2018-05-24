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
        private readonly ILuisProxy _luisProxy;
        private readonly ILuisBusinessLogic _luisBusinessLogic;
        private readonly IWebSearchBusinessLogic _webSearchBusinessLogic;

        public SearchController(ILuisProxy luisProxy,ILuisBusinessLogic luisBusinessLogic, IWebSearchBusinessLogic webSearchBusinessLogic)
        {
            _luisProxy = luisProxy;
            _luisBusinessLogic = luisBusinessLogic;
            _webSearchBusinessLogic = webSearchBusinessLogic;
        }

        [HttpGet("{query}")]
        public async Task<IActionResult> GetAsync(string query)
        {
            var internalResources = _luisBusinessLogic.GetInternalResources(query);
            var webResources =  _webSearchBusinessLogic.GetWebResourcesAsync(query);           

            List<dynamic> resources =  new List<dynamic>() { await internalResources, await webResources };

            return StatusCode(200, resources);
        }
    }
}