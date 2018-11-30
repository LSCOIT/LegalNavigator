using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Access2Justice.Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}