using Access2Justice.Api.Authorization;
using Access2Justice.Api.Interfaces;
using Access2Justice.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using static Access2Justice.Api.Authorization.Permissions;
using Pomelo.AntiXSS;

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
        [Permission(PermissionName.generatepermalink)]
        [HttpPost("permalink/generate")]
        public async Task<IActionResult> ShareAsync([FromBody] ShareInput shareInput)
        {
            if (shareInput != null)
            {
                var response = await shareBusinessLogic.ShareResourceDataAsync(shareInput);
                if (response == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
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
        [Permission(PermissionName.checkpermalink)]
        [HttpPost("permalink/check")]
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
        [Permission(PermissionName.removepermalink)]
        [HttpPost("permalink/remove")]
        public async Task<IActionResult> UnshareAsync([FromBody] ShareInput unShareInput)
        {
            if (unShareInput != null)
            {
                var response = await shareBusinessLogic.UnshareResourceDataAsync(unShareInput);
                if (response == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
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
        [HttpGet("permalink/resource")]
        public async Task<IActionResult> PermaLinkAsync([FromQuery] string permaLink)
        {
            permaLink = Instance.Sanitize(permaLink);
            if (permaLink != null)
            {
                var response = await shareBusinessLogic.GetPermaLinkDataAsync(permaLink);
                if (response == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
                return Ok(response);
            }
            return StatusCode(StatusCodes.Status412PreconditionFailed);
        }
    }
}