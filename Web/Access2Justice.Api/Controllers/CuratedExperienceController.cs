using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Access2Justice.Shared;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Access2Justice.Api.Controllers
{
    [ExcludeFromCodeCoverage]
    [Route("api/curated-experiences")]
    public class CuratedExperienceController : Controller
    {
        private readonly ICuratedExperienceConvertor a2jAuthorBuisnessLogic;
        private readonly ICuratedExperienceBusinessLogic curatedExperienceBusinessLogic;
        private readonly ISessionManager sessionManager;
        private readonly IUserRoleBusinessLogic userBusinessLogic;

        public CuratedExperienceController(ICuratedExperienceConvertor a2jAuthorBuisnessLogic, ICuratedExperienceBusinessLogic curatedExperienceBusinessLogic,
            ISessionManager sessionManager, IUserRoleBusinessLogic userBusinessLogic)
        {
            this.a2jAuthorBuisnessLogic = a2jAuthorBuisnessLogic;
            this.curatedExperienceBusinessLogic = curatedExperienceBusinessLogic;
            this.sessionManager = sessionManager;
            this.userBusinessLogic = userBusinessLogic;
        }

        /// <summary>
        /// returns list of curated experiences
        /// </summary>
        /// <remarks>
        /// Helps to get list of curated experiences
        /// </remarks>
        /// <param name="location"></param>
        /// <response code="200">Returns curated experiences </response>
        /// <response code="500">Failure</response>
        [HttpGet]
        public async Task<IActionResult> GetCuratedExperiences([FromQuery]string location)
        {
            return Ok(await curatedExperienceBusinessLogic.GetCuratedExperiencesAsync(location));
        }

        /// <summary>
        /// deletes curated experiences by id
        /// </summary>
        /// <remarks>
        /// Helps to delete curated experience
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="partitionKey"></param>
        /// <response code="200"></response>
        /// <response code="500">Failure</response>
        [HttpDelete("{id}/{partitionKey}")]
        public async Task<IActionResult> DeleteCuratedExperience(string id, string partitionKey = null)
        {
            await curatedExperienceBusinessLogic.DeleteCuratedExperienceAsync(id, partitionKey);
            return Ok();
        }

        /// <summary>
        /// Convert A2JAuthor to curated experience
        /// </summary>
        /// <remarks>
        /// Helps to get A2JAuthor to curated experience converted 
        /// </remarks>
        /// <param name="a2jSchema"></param>
        /// <response code="200">Returns converted JSON </response>
        /// <response code="500">Failure</response>
        [HttpPost("import")]
        public IActionResult ImportA2JAuthorGuidedInterview([FromBody] JObject a2jSchema)
        {
            try
            {
                JObject.Parse(a2jSchema.ToString());
                return Json(a2jAuthorBuisnessLogic.ConvertA2JAuthorToCuratedExperience(a2jSchema));
            }
            catch
            {
                return BadRequest("The schema you sent does not have a valid json.");
            }
        }

        /// <summary>
        /// Get first component for curated experience
        /// </summary>
        /// <remarks>
        /// Helps to get first component for curated experience 
        /// </remarks>
        /// <response code="200">Returns first component for curated experience </response>
        /// <response code="500">Failure</response>
        [HttpGet("start")]
        public async Task<IActionResult> GetFirstComponent(Guid curatedExperienceId, Guid? answerId = null, bool ignoreProgress = false)
        {
            if (!ignoreProgress)
            {
                var currentProgress = await GetCurrentComponent(curatedExperienceId, answerId ?? default(Guid));
                if (currentProgress != null)
                {
                    return currentProgress;
                }
            }

            var component = await curatedExperienceBusinessLogic.GetComponent(
                sessionManager.RetrieveCachedCuratedExperience(curatedExperienceId, HttpContext), Guid.Empty);
            if (component == null) return NotFound();

            return Ok(component);
        }

        /// <summary>
        /// Save component saved and get next for curated experience
        /// </summary>
        /// <remarks>
        /// Helps to Save component saved and get next for curated experience
        /// </remarks>
        /// <param name="component"></param>
        /// <response code="200">Returns next component for curated experience </response>
        /// <response code="500">Failure</response>
        [HttpPost("components/save-and-get-next")]
        public async Task<IActionResult> SaveAndGetNextComponent([FromBody] CuratedExperienceAnswersViewModel component)
        {
            if (component != null)
            {
                var curatedExperience = sessionManager.RetrieveCachedCuratedExperience(component.CuratedExperienceId, HttpContext);
                var document = await curatedExperienceBusinessLogic.SaveAnswersAsync(component, curatedExperience, userBusinessLogic.GetOId());
                if (document == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                Response.Cookies.Append(Constants.GuidedAssistanceAnswerIdCookies, document.Id, new CookieOptions
                {
                    HttpOnly = false,
                    SameSite = SameSiteMode.None,
                    Secure = false,
                    Expires = DateTime.Now.AddYears(1),
                    Domain = Request.GetUri().Host
                });
                return Ok(await curatedExperienceBusinessLogic.GetNextComponentAsync(curatedExperience, component,
                    document));
            }
            return StatusCode(400);
        }

        [HttpGet("components/restore-progress")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "There is no matching answers batch")]
        public async Task<IActionResult> GetCurrentComponent(Guid? answerId = null)
        {
            var result = await GetCurrentComponent(answerId: answerId ?? default(Guid));
            return result ?? StatusCode(StatusCodes.Status404NotFound);
        }

        [HttpGet("components/back")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "There is no matching answers batch")]
        public async Task<IActionResult> GetPreviousComponent(Guid? answerId = null)
        {
            await curatedExperienceBusinessLogic.AnswersStepBack(await GetCurrentUserAnswers(answerId: answerId ?? default(Guid)));
            return await GetCurrentComponent(answerId);
        }

        [HttpGet("components/next")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "There is no matching answers batch")]
        public async Task<IActionResult> GetNextComponent(Guid? answerId = null)
        {
            await curatedExperienceBusinessLogic.AnswersStepNext(await GetCurrentUserAnswers(answerId: answerId ?? default(Guid)));
            return await GetCurrentComponent(answerId);
        }

        private async Task<IActionResult> GetCurrentComponent(Guid curatedExperienceId = default(Guid), Guid answerId = default(Guid))
        {
            var answers = await GetCurrentUserAnswers(curatedExperienceId, answerId);
            if (answers == null)
            {
                return null;
            }
            if (answers.CuratedExperienceId == Guid.Empty)
            {
                return null;
            }

            if (curatedExperienceId != Guid.Empty && curatedExperienceId != answers.CuratedExperienceId)
            {
                return null;
            }

            Response.Cookies.Append(Constants.GuidedAssistanceAnswerIdCookies, answers.AnswersDocId.ToString());

            var curatedExperience = sessionManager.RetrieveCachedCuratedExperience(answers.CuratedExperienceId, HttpContext);
            return Ok(await curatedExperienceBusinessLogic.GetNextComponentAsync(curatedExperience, answers));
        }

        private async Task<CuratedExperienceAnswers> GetCurrentUserAnswers(Guid curatedExperienceId = default(Guid), Guid answerId = default(Guid))
        {
            var profileAnswers = Guid.Empty;
            var cookieAnswers = Guid.Empty;
            var currentUserId = userBusinessLogic.GetOId();
            if (!string.IsNullOrWhiteSpace(currentUserId))
            {
                profileAnswers = await curatedExperienceBusinessLogic.GetUserAnswerId(currentUserId);
            }

            if (Request.Cookies.TryGetValue(Constants.GuidedAssistanceAnswerIdCookies, out var cookieValue))
            {
                Guid.TryParse(cookieValue, out cookieAnswers);
                // return await curatedExperienceBusinessLogic.GetAnswerProgress(answerId);
            }

            return await curatedExperienceBusinessLogic.GetLastAnswerProgress(curatedExperienceId, answerId, cookieAnswers, profileAnswers);
        }
    }
}