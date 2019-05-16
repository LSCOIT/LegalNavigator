using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.A2JAuthor;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Access2Justice.Api.Controllers
{
    [ExcludeFromCodeCoverage]
    [Route("api/personalized-plans")]
    public class PersonalizedPlanController : Controller
    {
        private readonly IPersonalizedPlanBusinessLogic personalizedPlanBusinessLogic;
        private readonly ICuratedExperienceBusinessLogic curatedExperienceBusinessLogic;
        private readonly ISessionManager sessionManager;
        private readonly IUserRoleBusinessLogic userRoleBusinessLogic;

        public PersonalizedPlanController(IPersonalizedPlanBusinessLogic personalizedPlan, ICuratedExperienceBusinessLogic curatedExperience,
            ISessionManager sessionManager, IUserRoleBusinessLogic userRoleBusinessLogic)
        {
            this.personalizedPlanBusinessLogic = personalizedPlan;
            this.curatedExperienceBusinessLogic = curatedExperience;
            this.sessionManager = sessionManager;
            this.userRoleBusinessLogic = userRoleBusinessLogic;
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
                // Clear curated experience answer progress for authenticated user when plan generated
                var userId = userRoleBusinessLogic.GetOId();
                var personalizedPlan = await personalizedPlanBusinessLogic.GeneratePersonalizedPlanAsync(
                       sessionManager.RetrieveCachedCuratedExperience(curatedExperienceId, HttpContext), answersDocId, 
                       userId);

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
