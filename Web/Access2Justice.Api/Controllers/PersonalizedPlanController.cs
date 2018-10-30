﻿using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.A2JAuthor;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Access2Justice.Api.Controllers
{
    [Route("api/personalized-plan")]
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
        public async Task<IActionResult> TestA2JAuthorLogicParser([FromBody] CuratedExperienceAnswers userAnswers)
        {
            // Todo:@Alaa remove this endpoint, added it just to test the parser duing development
            return Ok(new A2JAuthorLogicParser(new A2JAuthorLogicInterpreter()).Parse(userAnswers));
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
        public async Task<IActionResult> GeneratePersonalizedPlanAsync([FromQuery] Guid curatedExperienceId, [FromQuery] Guid answersDocId, [FromBody] Location location)
        {
            var personalizedPlan = await personalizedPlanBusinessLogic.GeneratePersonalizedPlanAsync(
               sessionManager.RetrieveCachedCuratedExperience(curatedExperienceId, HttpContext), answersDocId, location);

            if (personalizedPlan == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(personalizedPlan);
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
        [HttpGet("get-plan")]
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
        /// <param name="userPlan"></param>
        /// <response code="200">Returns the updated personalized plan </response>
        /// <response code="500">Failure</response>      
        // Todo:@Alaa check user is authorized
        // [Permission(PermissionName.)]
        [HttpPost("save")]
        public async Task<IActionResult> SavePersonalizedPlanAsync([FromBody] PersonalizedPlanViewModel personalizedPlan)
        {
            // Todo:@Alaa i need the user claims here so i could update the user profile of the logged in user.
            var newPlan = await personalizedPlanBusinessLogic.UpsertPersonalizedPlanAsync(personalizedPlan);

            if (newPlan == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(newPlan);
        }
    }
}
