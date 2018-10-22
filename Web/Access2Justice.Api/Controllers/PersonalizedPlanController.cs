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
        private readonly IPersonalizedPlanBusinessLogic personalizedPlanBusinessLogic;
        private readonly ICuratedExperienceBusinessLogic curatedExperienceBusinessLogic;

        public PersonalizedPlanController(IPersonalizedPlanBusinessLogic personalizedPlan, ICuratedExperienceBusinessLogic curatedExperience)
        {
            this.personalizedPlanBusinessLogic = personalizedPlan;
            this.curatedExperienceBusinessLogic = curatedExperience;
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
            var personalizedPlan = await personalizedPlanBusinessLogic.GeneratePersonalizedPlanAsync(
                RetrieveCachedCuratedExperience(curatedExperienceId), answersDocId);

             // Todo:@Alaa save/add the unprocessed plan to the existing one if the user is logged in
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
            return Ok(await personalizedPlanBusinessLogic.UpdatePersonalizedPlan(userPlan));
        }

        // Todo:@Alaa must refactor this, i copied it from the CuratedExperience controller for now to finish an end-to-end personalized plan
        private CuratedExperience RetrieveCachedCuratedExperience(Guid id)
        {
            var cuExSession = HttpContext.Session.GetString(id.ToString());
            if (string.IsNullOrWhiteSpace(cuExSession))
            {
                var rawCuratedExperience = curatedExperienceBusinessLogic.GetCuratedExperienceAsync(id).Result;
                HttpContext.Session.SetObjectAsJson(id.ToString(), rawCuratedExperience);
            }

            return HttpContext.Session.GetObjectAsJson<CuratedExperience>(id.ToString());
        }
    }
}
