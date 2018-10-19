using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Access2Justice.Api.Controllers
{
    [Route("api/curated-experience")]
    public class CuratedExperienceController : Controller
    {
        private readonly ICuratedExperienceConvertor a2jAuthorBuisnessLogic;
        private readonly ICuratedExperienceBusinessLogic curatedExperienceBusinessLogic;

        public CuratedExperienceController(ICuratedExperienceConvertor a2jAuthorBuisnessLogic, ICuratedExperienceBusinessLogic curatedExperienceBusinessLogic)
        {
            this.a2jAuthorBuisnessLogic = a2jAuthorBuisnessLogic;
            this.curatedExperienceBusinessLogic = curatedExperienceBusinessLogic;
        }

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

        [HttpGet("start")]
        public async Task<IActionResult> GetFirstComponent(Guid curatedExperienceId)
        {
            var component = curatedExperienceBusinessLogic.GetComponent(RetrieveCachedCuratedExperience(curatedExperienceId), Guid.Empty);
            if (component == null) return NotFound();

            return Ok(component);
        }

        [HttpPost("component/save-and-get-next")]
        public async Task<IActionResult> SaveAndGetNextComponent([FromBody] CuratedExperienceAnswersViewModel component)
        {
            var curatedExperience = RetrieveCachedCuratedExperience(component.CuratedExperienceId);
            var document = await curatedExperienceBusinessLogic.SaveAnswersAsync(component, curatedExperience);
            if (component == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(await curatedExperienceBusinessLogic.GetNextComponentAsync(curatedExperience, component));
        }

        [HttpGet("component")]
        public IActionResult GetSpecificComponent([FromQuery] Guid curatedExperienceId, [FromQuery] Guid componentId)
        {
            var component = curatedExperienceBusinessLogic.GetComponent(RetrieveCachedCuratedExperience(curatedExperienceId), componentId);
            if (component == null) return NotFound();

            return Ok(component);
        }

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