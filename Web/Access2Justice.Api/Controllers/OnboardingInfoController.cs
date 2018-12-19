using Access2Justice.Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Integration = Access2Justice.Shared.Models.Integration;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/OnboardingInfo")]
    public class OnboardingInfoController : Controller
    {
        private readonly IOnboardingInfoBusinessLogic onboardingInfoBusinessLogic;

        public OnboardingInfoController(IOnboardingInfoBusinessLogic onboardingInfoBusinessLogic)
        {
            this.onboardingInfoBusinessLogic = onboardingInfoBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAsync(string organizationId)
        {
            if (string.IsNullOrWhiteSpace(organizationId))
            {
                return BadRequest("please provide organization name");
            }

            var response = await onboardingInfoBusinessLogic.GetOrganizationOnboardingInfo(organizationId);
            return Ok(response);
        }

        [HttpPost("send")]
        public async Task<IActionResult> PostAsync([FromBody] Integration.OnboardingInfo onboardingInfo)
        {
            if(onboardingInfo == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            var response = await onboardingInfoBusinessLogic.PostOnboardingInfo(onboardingInfo);
            return Ok(response);
        }

        [HttpPost("submission")]
        public IActionResult PostOrganizationAsync([FromBody] string json)
        {
            return Ok(json);
        }

    }
}