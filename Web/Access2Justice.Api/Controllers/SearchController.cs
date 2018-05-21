using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/search")]
    public class SearchController : Controller
    {
        private ILuisProxy _luisProxy;
        private IBackendDatabaseService _db;

        public SearchController(ILuisProxy luisProxy, IBackendDatabaseService backendDatabaseService)
        {
            _luisProxy = luisProxy;
            _db = backendDatabaseService;
        }

        [HttpGet("{query}")]
        public async Task<IActionResult> GetAsync(string query)
        {
            IntentWithScore intentWithScore = await _luisProxy.GetIntents(query);
            var spParams = _luisProxy.FilterLuisIntents(intentWithScore);

            if (spParams == null)
                return StatusCode(200, "can you please share your problem in more detail.");
            var response = await _db.ExecuteStoredProcedureAsyncWithParameters<string>(Constants.GetResourcesByKeywords, spParams);

            return StatusCode(200, response);
        }
    }
}