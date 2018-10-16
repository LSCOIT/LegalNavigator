using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.A2JAuthor;
using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using static Access2Justice.Api.Authorization.Permissions;
using Access2Justice.Api.Authorization;
using System.Threading.Tasks;

namespace Access2Justice.Api.Controllers
{
    [Route("api/curated-experience")]
    public class CuratedExperienceController : Controller
    {
        private readonly ICuratedExperienceConvertor a2jAuthorBuisnessLogic;
        private readonly ICuratedExperienceBusinessLogic curatedExperienceBusinessLogic;
        private readonly IPersonalizedPlanBusinessLogic personalizedPlanBusinessLogic;

        public CuratedExperienceController(ICuratedExperienceConvertor a2jAuthorBuisnessLogic, ICuratedExperienceBusinessLogic curatedExperienceBusinessLogic,
            IPersonalizedPlanBusinessLogic personalizedPlanBusinessLogic)
        {
            this.a2jAuthorBuisnessLogic = a2jAuthorBuisnessLogic;
            this.curatedExperienceBusinessLogic = curatedExperienceBusinessLogic;
            this.personalizedPlanBusinessLogic = personalizedPlanBusinessLogic;
        }


        /// <summary>
        /// This endpoint is just to demo the Curated Experience Personalized Plan schema. Added to help building the UI.
        /// </summary>
        /// <returns></returns>
        [HttpGet("personalized-plan/demo")]
        public IActionResult GetDemoPersonalizedPlan()
        {
            //var locations = new List<PlanLocation>();
            //locations.Add(new PlanLocation
            //{
            //    City = "test city",
            //    State = "test state",
            //    ZipCode = "11332"
            //});
            
            var quicklinks = new List<PlanQuickLink>();
            quicklinks.Add(new PlanQuickLink
            {
                Text = "Quick link to topic1",
                Url = new Uri("http://localhost/topic1")
            });

            //var resources = new List<PlanResource>();
            //resources.Add(new PlanResource
            //{
            //    Description = "Lorem ipsum dolor sit amet, usu soluta aliquid recusabo in, eloquentiam adversarium in vel.",
            //    ExternalUrl = new Uri("http://localhost"),
            //    Icon = "",
            //    IsRecommended = false,
            //    Location = locations,
            //    Name = "Resource Name",
            //    Overview = "Sale oratio tractatos duo et, pri harum senserit mediocritatem an. No eum aliquip menandri.",
            //    ResourceId = Guid.NewGuid(),
            //    Tags = new List<string>() { "Test", "Demo" },
            //    Type = "Organization",
            //});

            var steps = new List<PlanStep>();
            steps.Add(new PlanStep
            {
                StepId = Guid.NewGuid(),
                Title = "Plan title",
                Description = "test plan for building the UI",
                Order = 1,
                IsComplete = false,
                //Resources = resources,
                Type = "PersonalizedPlan"
            });

            var topics = new List<PlanTopic>();
            topics.Add(new PlanTopic
            {
                TopicId = Guid.NewGuid(),
                QuickLinks = quicklinks,
                Steps = steps
            });

            return Ok(new PersonalizedPlanViewModel()
            {
                PersonalizedPlanId = Guid.NewGuid(),
                Topics = topics,
            });
        }

        [HttpPost("a2j-personalized-plan/parser-test")]
        public async Task<IActionResult> TestA2JPersonalizedPlanParser([FromBody] CuratedExperienceAnswers userAnswers)
        {
            // Todo:@Alaa remove this endpoint, added it just to test the parser duing development
            var des = curatedExperienceBusinessLogic.FindDestinationComponentAsync(
                RetrieveCachedCuratedExperience(Guid.Parse("93bcabe2-8afc-4044-b4da-59f7b94510c4")), Guid.Parse("49989209-f89a-4991-86c4-bba41ac2d2d3"), userAnswers.AnswersDocId);

            return Ok(new A2JLogicParser(new A2JLogicInterpreter()).Parse(userAnswers));
        }

        [HttpPost("import")]
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

        [HttpGet("start")]
        public async Task<IActionResult> GetFirstComponent(Guid curatedExperienceId)
        {
            var component = curatedExperienceBusinessLogic.GetComponent(RetrieveCachedCuratedExperience(curatedExperienceId), Guid.Empty);
            if (component == null) return NotFound();

            return Ok(component);
        }

        [HttpGet("component")]
        public IActionResult GetSpecificComponent([FromQuery] Guid curatedExperienceId, [FromQuery] Guid componentId)
        {
            var component = curatedExperienceBusinessLogic.GetComponent(RetrieveCachedCuratedExperience(curatedExperienceId), componentId);
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

        [HttpGet("personalized-plan")]
        public async Task<IActionResult> GeneratePersonalizedPlan([FromQuery] Guid curatedExperienceId, [FromQuery] Guid answersDocId)
        {
            //var personalizedPlan = await personalizedPlanBusinessLogic.GeneratePersonalizedPlanAsync(
            //    RetrieveCachedCuratedExperience(curatedExperienceId), answersDocId);
            //if (personalizedPlan == null)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError);
            //}

            return Ok();
        }

        [Permission(PermissionName.updateplan)]
        [HttpPost("update-plan")]
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
                var rawCuratedExperience = curatedExperienceBusinessLogic.GetCuratedExperienceAsync(id).Result;
                HttpContext.Session.SetObjectAsJson(id.ToString(), rawCuratedExperience);
            }

            return HttpContext.Session.GetObjectAsJson<CuratedExperience>(id.ToString());
        }

         // Todo:@Alaa remove
             //[HttpGet]
             //[Route("getplandetails/{id}")]
             //public async Task<IActionResult> GetPlanDetailsAsync(string id)
             //{
             //    var actionPlans = await personalizedPlanBusinessLogic.GetPlanDataAsync(id);
             //    return Ok(actionPlans);
             //}

        [HttpGet]
        [Route("get-plan/{id}")]
        public async Task<IActionResult> GetPlanAsync(string id)
        {
            var actionPlans = await personalizedPlanBusinessLogic.GetPersonalizedPlan(id);
            return Ok(actionPlans);
        }
    }
}