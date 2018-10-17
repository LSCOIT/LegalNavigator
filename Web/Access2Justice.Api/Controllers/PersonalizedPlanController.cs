using Access2Justice.Api.Authorization;
using Access2Justice.Api.Interfaces;
using Access2Justice.Shared.A2JAuthor;
using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static Access2Justice.Api.Authorization.Permissions;

namespace Access2Justice.Api.Controllers
{
    [Route("api/personalized-plan")]
    public class PersonalizedPlanController : Controller
    {
        private readonly IPersonalizedPlanBusinessLogic personalizedPlan;
        private readonly ICuratedExperienceBusinessLogic curatedExperience;

        public PersonalizedPlanController(IPersonalizedPlanBusinessLogic personalizedPlan, ICuratedExperienceBusinessLogic curatedExperience)
        {
            this.personalizedPlan = personalizedPlan;
            this.curatedExperience = curatedExperience;
        }

        [HttpPost("parser-test")]
        public async Task<IActionResult> TestA2JAuthorLogicParser([FromBody] CuratedExperienceAnswers userAnswers)
        {
            // Todo:@Alaa remove this endpoint, added it just to test the parser duing development
            return Ok(new A2JAuthorLogicParser(new A2JAuthorLogicInterpreter()).Parse(userAnswers));
        }


        [HttpGet("generate")]
        public async Task<IActionResult> GeneratePersonalizedPlan([FromQuery] Guid curatedExperienceId, [FromQuery] Guid answersDocId)
        {
            var personalizedPlan = new object();
                //await personalizedPlanBusinessLogic.GeneratePersonalizedPlanAsync(
                // RetrieveCachedCuratedExperience(curatedExperienceId), answersDocId);
            if (personalizedPlan == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(personalizedPlan);
        }


        [Permission(PermissionName.updateplan)]
        [HttpPost("update-plan")]
        public async Task<IActionResult> UpdateUserProfileDocumentAsync([FromBody]UserPersonalizedPlan userPlan)
        {
            var personalizedPlan = await this.personalizedPlan.UpdatePersonalizedPlan(userPlan);
            return Ok(personalizedPlan);
        }

        private CuratedExperience RetrieveCachedCuratedExperience(Guid id)
        {
            // Todo:@Alaa remove this method or factor it out, i copied it from the CuratedExperience controller for now to finish an end-to-end personalized plan
            var cuExSession = HttpContext.Session.GetString(id.ToString());
            if (string.IsNullOrWhiteSpace(cuExSession))
            {
                var rawCuratedExperience = curatedExperience.GetCuratedExperienceAsync(id).Result;
                HttpContext.Session.SetObjectAsJson(id.ToString(), rawCuratedExperience);
            }

            return HttpContext.Session.GetObjectAsJson<CuratedExperience>(id.ToString());
        }
    }
}
