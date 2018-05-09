namespace Access2Justice.Api.Controllers
{
    using System;    
    using System.Threading.Tasks;
    using Access2Justice.CognitiveServices;    
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Access2Justice.Shared;


    [Produces("application/json")]
    [Route("api/IntentsMapper")]
    public class IntentsMapperController : Controller
    {

        private IOptions<App> appSettings;
        private ILuisHelper luisHelper;        

        public IntentsMapperController(IOptions<App> appSettings, ILuisHelper luisHelper)
        {
            this.appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            this.luisHelper = luisHelper ?? throw new ArgumentNullException(nameof(luisHelper));
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(LuisInput luisInput)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Json(await luisHelper.GetLuisIntent(luisInput));
            }
            catch (Exception ex)
            {
                //need to implement error logging...
                return StatusCode(500, ex.Message);
            }
        }

    }
}