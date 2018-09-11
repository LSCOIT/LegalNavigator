using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.A2JAuthor;

namespace Access2Justice.Api.Controllers
{
    [Route("api/[controller]")]
    public class CuratedExperienceController : Controller
    {
        private readonly IA2JAuthorBusinessLogic a2jAuthorBuisnessLogic;
        private readonly ICuratedExperienceBusinessLogic curatedExperienceBusinessLogic;
        private readonly IPersonalizedPlanBusinessLogic personalizedPlanBusinessLogic;

        public CuratedExperienceController(IA2JAuthorBusinessLogic a2jAuthorBuisnessLogic, ICuratedExperienceBusinessLogic curatedExperienceBusinessLogic,
            IPersonalizedPlanBusinessLogic personalizedPlanBusinessLogic)
        {
            this.a2jAuthorBuisnessLogic = a2jAuthorBuisnessLogic;
            this.curatedExperienceBusinessLogic = curatedExperienceBusinessLogic;
            this.personalizedPlanBusinessLogic = personalizedPlanBusinessLogic;
        }

        #region DEMOS
        [HttpPost("A2JPersonalizedPlan/ParserTest")]
        public async Task<IActionResult> TestA2JPersonalizedPlanParser([FromBody] CuratedExperienceAnswers userAnswers)
        {
            // Todo:@Alaa remove this endpoint, added it just to test the parser duing development
            return Ok(new Parser(new Evaluator()).Parse(userAnswers));
        }
        #endregion

        [HttpPost("Import")]
        public IActionResult ConvertA2JAuthorToCuratedExperience([FromBody] JObject a2jSchema)
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

        [HttpGet("Start")]
        public IActionResult GetFirstComponent(Guid curatedExperienceId)
        {
            var component = curatedExperienceBusinessLogic.GetComponent(RetrieveCachedCuratedExperience(curatedExperienceId), Guid.Empty);
            if (component == null) return NotFound();

            return Ok(component);
        }

        [HttpGet("Component")]
        public IActionResult GetSpecificComponent([FromQuery] Guid curatedExperienceId, [FromQuery] Guid componentId)
        {
            var component = curatedExperienceBusinessLogic.GetComponent(RetrieveCachedCuratedExperience(curatedExperienceId), componentId);
            if (component == null) return NotFound();

            return Ok(component);
        }

        [HttpPost("Component/SaveAndGetNext")]
        public async Task<IActionResult> SaveAndGetNextComponent([FromBody] CuratedExperienceAnswersViewModel component)
        {
            var curatedExperience = RetrieveCachedCuratedExperience(component.CuratedExperienceId);
            var document = await curatedExperienceBusinessLogic.SaveAnswers(component, curatedExperience);
            if (component == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(curatedExperienceBusinessLogic.GetNextComponent(curatedExperience, component));
        }

        [HttpGet("PersonalizedPlan")]
        public async Task<IActionResult> GeneratePersonalizedPlan([FromQuery] Guid curatedExperienceId, [FromQuery] Guid answersDocId)
        {
            var personalizedPlan = await personalizedPlanBusinessLogic.GeneratePersonalizedPlan(
                RetrieveCachedCuratedExperience(curatedExperienceId), answersDocId);
            if (personalizedPlan == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(personalizedPlan);
        }

        [HttpPost("updateplan")]
        public async Task<IActionResult> UpdateUserProfileDocumentAsync([FromBody]UserPersonalizedPlan userPlan)
        {
            var personalizedPlan = await personalizedPlanBusinessLogic.UpdatePersonalizedPlan(userPlan);
            return Ok(personalizedPlan);
        }

        private CuratedExperience RetrieveCachedCuratedExperience(Guid id)
        {
            var cuExSession = HttpContext.Session.GetString(id.ToString());
            if (string.IsNullOrWhiteSpace(cuExSession))
            {
                var rawCuratedExperience = curatedExperienceBusinessLogic.GetCuratedExperience(id).Result;
                HttpContext.Session.SetObjectAsJson(id.ToString(), rawCuratedExperience);
            }

            return HttpContext.Session.GetObjectAsJson<CuratedExperience>(id.ToString());
        }

        [HttpGet]
        [Route("getplandetails/{id}")]
        public async Task<IActionResult> GetPlanDetailsAsync(string id)
        {
            var actionPlans = await personalizedPlanBusinessLogic.GetPlanDataAsync(id);
            return Ok(actionPlans);
        }

        [HttpGet]
        [Route("getplan/{id}")]
        public async Task<IActionResult> GetPlanAsync(string id)
        {
            var actionPlans = await personalizedPlanBusinessLogic.GetPersonalizedPlan(id);
            return Ok(actionPlans);
        }
    }
}