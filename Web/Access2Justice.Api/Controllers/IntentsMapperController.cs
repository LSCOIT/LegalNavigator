using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/IntentsMapper")]
    public class IntentsMapperController : Controller
    {
        private ILuisProxy luisProxy;
        private IBackendDatabaseService backendDatabaseService;

        public IntentsMapperController(ILuisProxy luisProxy, IBackendDatabaseService backendDatabaseService)
        {
            this.luisProxy = luisProxy;
            this.backendDatabaseService = backendDatabaseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(LuisInput luisInput)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            IntentWithScore intentWithScore = await luisProxy.GetLuisIntent(luisInput);
            var spParams = luisProxy.FilterLuisIntents(intentWithScore);

            if (spParams == null)
                return StatusCode(200, "can you please share your problem in more detail.");
            var response = await backendDatabaseService.ExecuteStoredProcedureAsyncWithParameters<string>(Constants.GetResourcesByKeywords, spParams);

            return StatusCode(200, response);
        }

    }
}