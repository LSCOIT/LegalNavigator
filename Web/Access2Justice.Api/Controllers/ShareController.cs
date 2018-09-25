using Access2Justice.Api.Interfaces;
using Access2Justice.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/share")]
    public class ShareController : Controller
    {
        private readonly IShareBusinessLogic shareBusinessLogic;

        public ShareController(IShareBusinessLogic shareBusinessLogic)
        {
            this.shareBusinessLogic = shareBusinessLogic;
        }

        [HttpPost("generatepermalink")]
        public async Task<IActionResult> ShareAsync([FromBody] ShareInput shareInput)
        {
            if (shareInput != null)
            {
                var response = await shareBusinessLogic.ShareResourceDataAsync(shareInput);
                return Ok(response);
            }
            return StatusCode(StatusCodes.Status412PreconditionFailed);
        }

        [HttpPost("checkpermalink")]
        public async Task<IActionResult> CheckDataAsync([FromBody] ShareInput shareInput)
        {
            if (shareInput != null)
            {
                var response = await shareBusinessLogic.CheckPermaLinkDataAsync(shareInput);
                return Ok(response);
            }
            return StatusCode(StatusCodes.Status412PreconditionFailed);
        }

        [HttpPost("removepermalink")]
        public async Task<IActionResult> UnshareAsync([FromBody] ShareInput unShareInput)
        {
            if (unShareInput != null)
            {
                var response = await shareBusinessLogic.UnshareResourceDataAsync(unShareInput);
                return Ok(response);
            }
            return StatusCode(StatusCodes.Status412PreconditionFailed);
        }

        [HttpGet("getpermalinkresource")]
        public async Task<IActionResult> PermaLinkAsync([FromQuery] string permaLink)
        {
            if (permaLink != null)
            {
                var response = await shareBusinessLogic.GetPermaLinkDataAsync(permaLink);
                return Ok(response);
            }
            return StatusCode(StatusCodes.Status412PreconditionFailed);
        }


    }
}