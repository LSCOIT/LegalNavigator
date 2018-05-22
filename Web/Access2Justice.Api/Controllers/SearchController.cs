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
        private IHelper helper;

        public SearchController(ILuisProxy luisProxy, IBackendDatabaseService backendDatabaseService, IHelper helper)
        {
            _luisProxy = luisProxy;
            _db = backendDatabaseService;
            this.helper = helper;
        }

        [HttpGet("{query}")]
        public async Task<IActionResult> GetAsync(string query)
        {
            string LuisResponse = await _luisProxy.GetIntents(query);
            IntentWithScore intentWithScore = string.IsNullOrEmpty(LuisResponse) ? null : _luisProxy.ParseLuisIntent(LuisResponse);
            if(intentWithScore == null || intentWithScore?.TopScoringIntent?.ToUpperInvariant() == "NONE")
                return StatusCode(200, "can you please share your problem in more detail.");

            IEnumerable<string> keywords = _luisProxy.FilterLuisIntents(intentWithScore);
            string input = "";
            foreach (var keyword in keywords)
            {
                input = keyword;break;   
            }
            var response = await helper.GetTopicAsync(input);
            //var response = await _db.ExecuteStoredProcedureAsyncWithParameters<string>(Constants.GetResourcesByKeywords, spParams);

            return StatusCode(200, response);
        }
    }
}