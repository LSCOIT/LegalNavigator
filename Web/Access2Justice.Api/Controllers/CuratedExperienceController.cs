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
        public async Task<CuratedExperienceSurvey> Get([FromQuery] string surveyId)
        {
            var curatedExperience = await _backendDatabaseService.GetItemAsync<CuratedExperience>(surveyId);
            return CuratedExperienceChoiceSetMapper.GetQuestions(curatedExperience, curatedExperience.SurveyTree.First().SurveyItemId);
        }



        [HttpPost]
        public async Task<CuratedExperienceSurvey> Post([FromQuery] string surveyId, string questionId, string answer)
        {
             // todo:@alaa in reality we wouldn't retrieve this a second time, we should use some kind of caching. Azure radius cache?
            var curatedExperience = await _backendDatabaseService.GetItemAsync<CuratedExperience>(surveyId);
            var questions = CuratedExperienceChoiceSetMapper.GetQuestions(curatedExperience, questionId);

            // todo:@alaa we need some additional identifier, 'x.CuratedExperienceId == surveyId' is good for now
            var savedUserInputDocument = await _backendDatabaseService.GetItemsAsync<CuratedExperienceAnswers>(x => x.CuratedExperienceId == surveyId); //.ToAsyncEnumerable().FirstOrDefault();
            
            //if (default(CuratedExperienceAnswers) != savedUserInputDocument)
            if(savedUserInputDocument.Any())
            {
                var answersIds = new List<Guid>(savedUserInputDocument.First().Answers.Keys);
                foreach (var answerId in answersIds)
                {
                    if(answerId.ToString() == questionId)
                    {
                        savedUserInputDocument.First().Answers[Guid.Parse(questionId)] = answer;
                    }
                }
                await _backendDatabaseService.UpdateItemAsync(savedUserInputDocument.First().Id, savedUserInputDocument.First());

                
                return CuratedExperienceChoiceSetMapper.MapAnswersToQuestions(questions, savedUserInputDocument.First().Answers);
            }
            else
            {
                // create a new answers document
            }

            return questions;
        }
    }
}