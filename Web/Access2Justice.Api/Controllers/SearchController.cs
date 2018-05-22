using Access2Justice.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/search")]
    public class SearchController : Controller
    {
        private readonly ILuisBusinessLogic _luisBusinessLogic;

        public SearchController(ILuisBusinessLogic luisBusinessLogic)
        {
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