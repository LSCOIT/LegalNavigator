using Access2Justice.Api.Interfaces;
using Integration = Access2Justice.Shared.Models.Integration;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

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
        /// <param name="organizationType"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAsync(string organizationType)
        {
            if (string.IsNullOrWhiteSpace(organizationType))
            {
                return BadRequest("please provide organization name");
            }

            var response = onboardingInfoBusinessLogic.GetOnboardingInfo(organizationType);
            return Ok(response);
        }

        [HttpPost("send")]
        public async Task<IActionResult> PostAsync([FromBody] Integration.OnboardingInfo onboardingInfo)
        {
            //Organization Name may not be unique hence need to identify the unique id else need to consider the location of the organization while checking
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