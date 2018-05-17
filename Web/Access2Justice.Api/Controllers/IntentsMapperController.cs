namespace Access2Justice.Api.Controllers
{
    using System;    
    using System.Threading.Tasks;    
    using Microsoft.AspNetCore.Mvc;    
    using Access2Justice.Shared;    
    using Access2Justice.Shared.Interfaces;    

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
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                IntentWithScore intentWithScore = await luisProxy.GetLuisIntent(luisInput);
                string input = intentWithScore.TopScoringIntent;
                if (!string.IsNullOrEmpty(input) && input.ToUpperInvariant() == "NONE")
                {
                    return StatusCode(200, "can you please share your problem in more detail.");
                }
                foreach (var item in intentWithScore.TopNIntents)
                {
                    input += ","+ item;
                }                
                string[] spParams= { Constants.keywords, input }; 
                var response = await backendDatabaseService.ExecuteStoredProcedureAsyncWithParameters<string>(Constants.GetResourcesByKeywords, spParams);               

                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                //need to implement error logging...
                return StatusCode(500, ex.Message);
            }
        }

    }
}