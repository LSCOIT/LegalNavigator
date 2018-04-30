using Access2Justice.Api.BusinessLogic;
using Access2Justice.Api.Models.CuratedExperience;
using Access2Justice.Api.ViewModels;
using Access2Justice.CosmosDb;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.Controllers
{
    [Route("api/[controller]")]
    public class CuratedExperienceController : Controller
    {
        private readonly IBackendDatabaseService _backendDatabaseService;

        public CuratedExperienceController(IBackendDatabaseService backendDatabaseService)
        {
            _backendDatabaseService = backendDatabaseService;
        }

        [HttpGet]
        public async Task<CuratedExperienceSurveyViewModel> Get([FromQuery] string surveyId, string choiceId)
        {
            return await GetUserSurvay(surveyId, choiceId);
        }


        [HttpPost]
        public async Task<CuratedExperienceSurveyViewModel> Post([FromQuery] string surveyId, string choiceId, string answer)
        {
            var savedUserInputDocument = await _backendDatabaseService.GetItemsAsync<CuratedExperienceAnswers>(x => x.CuratedExperienceId == surveyId); 
            if (savedUserInputDocument.Any() && answer != null)
            {
                var answersIds = new List<Guid>(savedUserInputDocument.First().Answers.Keys);
                foreach (var answerId in answersIds)
                {
                    if (answerId.ToString() == choiceId)
                    {
                        savedUserInputDocument.First().Answers[Guid.Parse(choiceId)] = answer;
                    }
                }
                await _backendDatabaseService.UpdateItemAsync(savedUserInputDocument.First().Id, savedUserInputDocument.First());
            }
            else
            {
                // todo:@alaa  create a new answers document
            }

            return await GetUserSurvay(surveyId, choiceId);
        }


         // todo:@alaa move this method to the business logic
        private async Task<CuratedExperienceSurveyViewModel> GetUserSurvay(string surveyId, string choiceId = null)
        {
            // todo:@alaa in reality we wouldn't retrieve this a second time, we should use some kind of caching. Azure radius cache?
            var curatedExperience = await _backendDatabaseService.GetItemAsync<CuratedExperienceSurvey>(surveyId);

            var questions = new CuratedExperienceSurveyViewModel();
            if (choiceId != null)
            {
                questions = CuratedExperienceChoiceSetMapper.GetQuestions(curatedExperience, choiceId);
            }
            else
            {
                questions = CuratedExperienceChoiceSetMapper.GetQuestions(curatedExperience, curatedExperience.SurveyTree.First().QuestionId);
            }

            // todo:@alaa we need some additional identifier, 'x.CuratedExperienceId == surveyId' is good for now
            var savedUserInputDocument = await _backendDatabaseService.GetItemsAsync<CuratedExperienceAnswers>(x => x.CuratedExperienceId == surveyId);
            if (savedUserInputDocument.Any())
            {
                return CuratedExperienceChoiceSetMapper.MapAnswersToQuestions(questions, savedUserInputDocument.First().Answers);
            }

            return questions;
        }
    }
}