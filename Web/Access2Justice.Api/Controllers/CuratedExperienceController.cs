using Access2Justice.Api.Models.CuratedExperience;
using Access2Justice.Api.ViewModels;
using Access2Justice.CosmosDb;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Access2Justice.Shared.A2JExtensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Access2Justice.Api.BusinessLogic;
using Microsoft.VisualBasic;
using System;

namespace Access2Justice.Api.Controllers
{
    [Route("api/[controller]")]
    public class CuratedExperienceController : Controller
    {
        private readonly IBackendDatabaseService _backendDatabaseService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CuratedExperienceController(IBackendDatabaseService backendDatabaseService, IHttpContextAccessor httpContextAccessor)
        {
            _backendDatabaseService = backendDatabaseService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<CuratedExperienceSurvay> Get([FromQuery] string survayId)
        {
            var curatedExperience = await GetCuratedExperience(survayId);
            return CuratedExperienceChoiceSetMapper.GetQuestions(curatedExperience, curatedExperience.SurvayTree.First().SurvayItemId);  // start with the first question
        }



        [HttpPost]
        public async Task<CuratedExperienceSurvay> Post([FromQuery] string survayId, string questionId, string answer)
        {
            var curatedExperience = await GetCuratedExperience(survayId);
            var questions = CuratedExperienceChoiceSetMapper.GetQuestions(curatedExperience, questionId);

            var curatedExperienceAnswers = new CuratedExperienceAnswers();
            if (HttpContext.Session.GetString("CuratedExperienceAnswers") == null)
            {
                curatedExperienceAnswers = await _backendDatabaseService.GetItemAsync<CuratedExperienceAnswers>("84d2f769-2eb8-411d-a93a-cb4bfb6a0e5a");
                _httpContextAccessor.HttpContext.Session.SetObjectAsJson("CuratedExperienceAnswers", curatedExperienceAnswers);
            }
            else
            {
                curatedExperience = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<CuratedExperience>("CuratedExperienceAnswers");
            }
            // todo:@alaa check if there are answers to return, if not store them.

            return questions;
        }


         // todo:@alaa make this generic
        private async Task<CuratedExperience> GetCuratedExperience(string id)
        {
            // todo:@alaa we should probably use some kind of caching here. Azure Radius?

            var curatedExperience = new CuratedExperience();

            if (HttpContext.Session.GetString("CuratedExperience") == null)
            {
                curatedExperience = await _backendDatabaseService.GetItemAsync<CuratedExperience>(id);
                _httpContextAccessor.HttpContext.Session.SetObjectAsJson("CuratedExperience", curatedExperience);
            }
            else
            {
                curatedExperience = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<CuratedExperience>("CuratedExperience");
            }

            return curatedExperience;
        }
    }
}