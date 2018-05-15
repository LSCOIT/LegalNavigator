namespace Access2Justice.Api.Controllers
{
    using System;    
    using System.Threading.Tasks;
    using Access2Justice.CognitiveServices;    
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Access2Justice.Shared;
    using Access2Justice.Shared.Interfaces;
    using Newtonsoft.Json;

    [Produces("application/json")]
    [Route("api/IntentsMapper")]
    public class IntentsMapperController : Controller
    {

        private IOptions<App> appSettings;
        private ILuisHelper luisHelper;
        private IBackendDatabaseService backendDatabaseService;

        public IntentsMapperController(IOptions<App> appSettings, ILuisHelper luisHelper,IBackendDatabaseService backendDatabaseService)
        {
            this.appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            this.luisHelper = luisHelper ?? throw new ArgumentNullException(nameof(luisHelper));
            this.backendDatabaseService = backendDatabaseService ?? throw new ArgumentNullException(nameof(backendDatabaseService));
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(LuisInput luisInput)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                IntentWithScore intentWithScore = await luisHelper.GetLuisIntent(luisInput);
                string input = intentWithScore.TopScoringIntent;
                string[] spParams = { "keywords", input };
                var response = await backendDatabaseService.ExecuteStoredProcedureAsyncWithParameters<string>("GetResourceByKeyword", spParams);
                var jsonResponse = JsonConvert.DeserializeObject(response);
                return null;
            }
            catch (Exception ex)
            {
                //need to implement error logging...
                return StatusCode(500, ex.Message);
            }
        }

    }
}