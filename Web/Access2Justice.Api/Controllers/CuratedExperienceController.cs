using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        /// <param name="curatedExperienceId"></param>
        /// <response code="200">Returns first component for curated experience </response>
        /// <response code="500">Failure</response>
        [HttpGet("start")]
        public async Task<IActionResult> GetFirstComponent(Guid curatedExperienceId)
        {            
                var component = await curatedExperienceBusinessLogic.GetComponent(sessionManager.RetrieveCachedCuratedExperience(curatedExperienceId, HttpContext), Guid.Empty);
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

                return Ok(await curatedExperienceBusinessLogic.GetNextComponentAsync(curatedExperience, component));
            }
            return StatusCode(400);
        }

        [HttpGet("components/restore-progress")]
        [Authorize]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "There is no matching answers batch")]
        public async Task<IActionResult> GetCurrentComponent()
        {
            var answers = await curatedExperienceBusinessLogic.GetAnswerProgress(userBusinessLogic.GetOId());
            if (answers == null || answers.CuratedExperienceId == Guid.Empty)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            var curatedExperience = sessionManager.RetrieveCachedCuratedExperience(answers.CuratedExperienceId, HttpContext);
            return Ok(await curatedExperienceBusinessLogic.GetNextComponentAsync(curatedExperience, answers));
        }

        /// <summary>
        /// Get specific component for curated experience
        /// </summary>
        /// <remarks>
        /// Helps to get specific component for curated experience 
        /// </remarks>
        /// <param name="curatedExperienceId"></param>
        /// <param name="componentId"></param>
        /// <response code="200">Returns specific component for curated experience </response>
        /// <response code="500">Failure</response>
        [HttpGet("component")]
        public async Task<IActionResult> GetSpecificComponent([FromQuery] Guid curatedExperienceId, [FromQuery] Guid componentId)
        {
                var component = await curatedExperienceBusinessLogic.GetComponent(sessionManager.RetrieveCachedCuratedExperience(curatedExperienceId, HttpContext), componentId);
                if (component == null) return NotFound();

                return Ok(component);
        }
    }
}