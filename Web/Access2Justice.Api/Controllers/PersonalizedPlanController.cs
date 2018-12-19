using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.A2JAuthor;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Access2Justice.Api.Controllers
{
    [Route("api/personalized-plans")]
    public class PersonalizedPlanController : Controller
    {
        private readonly IPersonalizedPlanBusinessLogic personalizedPlanBusinessLogic;
        private readonly ICuratedExperienceBusinessLogic curatedExperienceBusinessLogic;
        private readonly ISessionManager sessionManager;

        public PersonalizedPlanController(IPersonalizedPlanBusinessLogic personalizedPlan, ICuratedExperienceBusinessLogic curatedExperience,
            ISessionManager sessionManager)
        {
            this.personalizedPlanBusinessLogic = personalizedPlan;
            this.curatedExperienceBusinessLogic = curatedExperience;
            this.sessionManager = sessionManager;
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
        public IActionResult TestA2JAuthorLogicParser([FromBody] CuratedExperienceAnswers userAnswers)
        {
            // Todo:@Alaa remove this endpoint, added it just to test the parser duing development
            return Ok(new LogicParser(new LogicInterpreter()).Parse(userAnswers));
        }

        /// <summary>
        /// Generate personalized plan
        /// </summary>
        /// <remarks>
        /// Use to generate personalized plan for a curated experience
        /// </remarks>
        /// <param name="curatedExperienceId"></param>
        /// <param name="answersDocId"></param>
        /// <response code="200">Returns personalized plan for curated experience </response>
        /// <response code="500">Failure</response>
        [HttpGet("generate")]
        public async Task<IActionResult> GeneratePersonalizedPlanAsync([FromQuery] Guid curatedExperienceId, [FromQuery] Guid answersDocId)
        {
            if (curatedExperienceId != null && answersDocId != null)
            {
                var personalizedPlan = await personalizedPlanBusinessLogic.GeneratePersonalizedPlanAsync(
                       sessionManager.RetrieveCachedCuratedExperience(curatedExperienceId, HttpContext), answersDocId);

                if (personalizedPlan == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                return Ok(personalizedPlan); 
            }
            return StatusCode(400);
        }

        /// <summary>
        /// Get personalized plan
        /// </summary>
        /// <remarks>
        /// Use to get the personalized plan by personalized plan Id
        /// </remarks>
        /// <param name="personalizedPlanId"></param>
        /// <response code="200">Returns personalized plan for curated experience </response>
        /// <response code="500">Failure</response>
        [HttpGet("")]
        public async Task<IActionResult> GetPersonalizedPlanAsync([FromQuery] Guid personalizedPlanId)
        {
            var personalizedPlan = await personalizedPlanBusinessLogic.GetPersonalizedPlanAsync(personalizedPlanId);

            if (personalizedPlan.PersonalizedPlanId == default(Guid))
            {
                return StatusCode(StatusCodes.Status404NotFound, $"Could not find a plan with this Id {personalizedPlanId}");
            }

            if (personalizedPlan == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(personalizedPlan);
        }

        /// <summary>
        /// Insert or update a personalized plan
        /// </summary>
        /// <remarks>
        /// Use to insert/update a personalized plan
        /// </remarks>
        ///<param name="userPlan"></param>
        /// <response code="200">Returns the updated personalized plan </response>
        /// <response code="500">Failure</response>      
        //[Permission(PermissionName.updateplan)]
        [HttpPost("save")]
        public async Task<IActionResult> SavePersonalizedPlanAsync([FromBody] UserPlan userPlan)
        {
            if (userPlan != null)
            {
                var newPlan = await personalizedPlanBusinessLogic.UpsertPersonalizedPlanAsync(userPlan);
                if (newPlan == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                return Ok(newPlan); 
            }
            return StatusCode(400);
        }
    }
}
