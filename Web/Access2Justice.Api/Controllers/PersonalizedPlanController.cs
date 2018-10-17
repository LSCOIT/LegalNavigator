using Access2Justice.Api.Authorization;
using Access2Justice.Api.Interfaces;
using Access2Justice.Shared.A2JAuthor;
using Access2Justice.Shared.Interfaces;
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
        private readonly ICosmosDbSettings cosmosDbSettings;
        private readonly IBackendDatabaseService backendDatabaseService;
        private readonly IPersonalizedPlanViewModelMapper personalizedPlanViewModelMapper;
        private readonly IPersonalizedPlanBusinessLogic personalizedPlanBusinessLogic;

        public PersonalizedPlanController(ICosmosDbSettings cosmosDbSettings, IBackendDatabaseService backendDatabaseService, 
            IPersonalizedPlanViewModelMapper personalizedPlanViewModelMapper, IPersonalizedPlanBusinessLogic personalizedPlanBusinessLogic)
        {
            this.cosmosDbSettings = cosmosDbSettings;
            this.backendDatabaseService = backendDatabaseService;
            this.personalizedPlanViewModelMapper = personalizedPlanViewModelMapper;
            this.personalizedPlanBusinessLogic = personalizedPlanBusinessLogic;
        }

        [HttpGet("demo")]
        public async Task<IActionResult> GetPersonalizedPlan()
        {
            var unprocessedPlan = await backendDatabaseService.GetItemAsync<UnprocessedPersonalizedPlan>("ee9a0e48-1855-e236-9f45-485f388a53ae",
                cosmosDbSettings.PersonalizedActionPlanCollectionId);
			return Ok(await personalizedPlanViewModelMapper.MapViewModel(unprocessedPlan));
        }

        // Todo:@Alaa remove this endpoint, added it just to test the parser duing development
        [HttpPost("parser-test")]
        public async Task<IActionResult> TestA2JAuthorLogicParser([FromBody] CuratedExperienceAnswers userAnswers)
        {
            //var des = curatedExperienceBusinessLogic.FindDestinationComponentAsync(
            //    RetrieveCachedCuratedExperience(Guid.Parse("93bcabe2-8afc-4044-b4da-59f7b94510c4")), Guid.Parse("49989209-f89a-4991-86c4-bba41ac2d2d3"), userAnswers.AnswersDocId);

            return Ok(new A2JAuthorLogicParser(new A2JAuthorLogicInterpreter()).Parse(userAnswers));
        }

        // Todo:@Alaa remove
        [HttpGet]
        [Route("get-plan-details/{id}")]
        public async Task<IActionResult> GetPlanDetailsAsync([FromQuery] Guid curatedExperienceId, [FromQuery] Guid answersDocId)
        {
            var personalizedPlan = new object();// await personalizedPlanBusinessLogic.GeneratePersonalizedPlanAsync(
                // RetrieveCachedCuratedExperience(curatedExperienceId), answersDocId);
            if (personalizedPlan == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(personalizedPlan);
        }

        [HttpGet]
        [Route("get-plan/{id}")]
        public async Task<IActionResult> GetPlanAsync(string id)
        {
            var actionPlans = await personalizedPlanBusinessLogic.GetPersonalizedPlan(id);
            return Ok(actionPlans);
        }


        [HttpGet("personalized-plan")]
        public async Task<IActionResult> GeneratePersonalizedPlan([FromQuery] Guid curatedExperienceId, [FromQuery] Guid answersDocId)
        {
            var personalizedPlan = new object();
                //await personalizedPlanBusinessLogic.GeneratePersonalizedPlanAsync(
                // RetrieveCachedCuratedExperience(curatedExperienceId), answersDocId);
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
            var personalizedPlan = await personalizedPlanBusinessLogic.UpdatePersonalizedPlan(userPlan);
            return Ok(personalizedPlan);
        }

        // // Todo:@Alaa factor this out, i copied it from the CuratedExperience controller for now to finish an end-to-end personalized plan
        //private CuratedExperience RetrieveCachedCuratedExperience(Guid id)
        //{
        //    var cuExSession = HttpContext.Session.GetString(id.ToString());
        //    if (string.IsNullOrWhiteSpace(cuExSession))
        //    {
        //        var rawCuratedExperience = curatedExperienceBusinessLogic.GetCuratedExperienceAsync(id).Result;
        //        HttpContext.Session.SetObjectAsJson(id.ToString(), rawCuratedExperience);
        //    }

        //    return HttpContext.Session.GetObjectAsJson<CuratedExperience>(id.ToString());
        //}
    }
}
