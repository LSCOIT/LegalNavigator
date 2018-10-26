using Access2Justice.Api.Authorization;
using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
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

        /// <summary>
        /// Parser test
        /// </summary>
        /// <remarks>
        /// Helps to parse user answers
        /// </remarks>
        /// <param name="userAnswers"></param>
        /// <response code="200">Returns parsed user answers</response>
        /// <response code="500">Failure</response>
        [HttpPost("parser-test")]
        public async Task<IActionResult> TestA2JAuthorLogicParser([FromBody] CuratedExperienceAnswers userAnswers)
        {
            // Todo:@Alaa remove this endpoint, added it just to test the parser duing development
            return Ok(new A2JAuthorLogicParser(new A2JAuthorLogicInterpreter()).Parse(userAnswers));
        }


        /// <summary>
        /// Generate personalized plan for curated experience
        /// </summary>
        /// <remarks>
        /// Helps to generate personalized plan for curated experience
        /// </remarks>
        /// <param name="curatedExperienceId"></param>
        /// <param name="answersDocId"></param>
        /// <response code="200">Returns personalized plan for curated experience </response>
        /// <response code="500">Failure</response>
        [HttpGet("generate")]
        public async Task<IActionResult> GeneratePersonalizedPlan([FromQuery] Guid curatedExperienceId, [FromQuery] Guid answersDocId, [FromBody] Location location)
        {
            var personalizedPlan = await personalizedPlanBusinessLogic.GeneratePersonalizedPlanAsync(
                RetrieveCachedCuratedExperience(curatedExperienceId), answersDocId, location);

            if (personalizedPlan == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(personalizedPlan);
        }

        /// <summary>
        /// Update user profile document
        /// </summary>
        /// <remarks>
        /// Helps to updatae user profile document
        /// </remarks>
        /// <param name="userPlan"></param>
        /// <response code="200">Returns updated personalized plan for curated experience </response>
        /// <response code="500">Failure</response> 
        // Todo:@Alaa check user is logged in
        // [Permission(PermissionName.)]
        [HttpPost("update")]
        public async Task<IActionResult> UpdateUserProfileDocumentAsync([FromBody] PersonalizedPlanViewModel personalizedPlan)
        {
            var newPlan = await personalizedPlanBusinessLogic.UpsertPersonalizedPlan(personalizedPlan);

            if (newPlan == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(newPlan);
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
