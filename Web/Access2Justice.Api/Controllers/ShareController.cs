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

        /// <summary>
        /// Get permalink created
        /// </summary>
        /// <remarks>
        /// Helps to create permalink for given input
        /// </remarks>
        /// <param name="shareInput"></param>
        /// <response code="200">Get permalink created for given input</response>
        /// <response code="500">Failure</response>
        /// <response code="412">Precondtion fails</response>
        [HttpPost("generate-permalink")]
        public async Task<IActionResult> ShareAsync([FromBody] ShareInput shareInput)
        {
            if (shareInput != null)
            {
                var response = await shareBusinessLogic.ShareResourceDataAsync(shareInput);
                return Ok(response);
            }
            return StatusCode(StatusCodes.Status412PreconditionFailed);
        }

        /// <summary>
        /// Check permalink
        /// </summary>
        /// <remarks>
        /// Helps to check permalink for given input
        /// </remarks>
        /// <param name="shareInput"></param>
        /// <response code="200">Get permalink for given input</response>
        /// <response code="500">Failure</response>
        /// <response code="412">Precondtion fails</response>
        [HttpPost("check-permalink")]
        public async Task<IActionResult> CheckDataAsync([FromBody] ShareInput shareInput)
        {
            if (shareInput != null)
            {
                var response = await shareBusinessLogic.CheckPermaLinkDataAsync(shareInput);
                return Ok(response);
            }
            return StatusCode(StatusCodes.Status412PreconditionFailed);
        }

        /// <summary>
        /// Get permalink removed
        /// </summary>
        /// <remarks>
        /// Helps to remove permalink for given input
        /// </remarks>
        /// <param name="unShareInput"></param>
        /// <response code="200">Get permalink removed for given input</response>
        /// <response code="500">Failure</response>
        /// <response code="412">Precondtion fails</response>
        [HttpPost("remove-permalink")]
        public async Task<IActionResult> UnshareAsync([FromBody] ShareInput unShareInput)
        {
            if (unShareInput != null)
            {
                var response = await shareBusinessLogic.UnshareResourceDataAsync(unShareInput);
                return Ok(response);
            }
            return StatusCode(StatusCodes.Status412PreconditionFailed);
        }

        /// <summary>
        /// Get permalink data
        /// </summary>
        /// <remarks>
        /// Helps to get permalink for given input
        /// </remarks>
        /// <param name="permaLink"></param>
        /// <response code="200">Get permalink for given input</response>
        /// <response code="500">Failure</response>
        /// <response code="412">Precondtion fails</response>
        [HttpGet("get-permalink-resource")]
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