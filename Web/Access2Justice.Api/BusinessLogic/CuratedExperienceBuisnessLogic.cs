using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models.CuratedExperience;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Access2Justice.Api.BusinessLogic
{
    public class CuratedExperienceBuisnessLogic : ICuratedExperienceBusinessLogic
    {
        private readonly ICosmosDbSettings dbSettings;
        private readonly IBackendDatabaseService dbService;

        public CuratedExperienceBuisnessLogic(ICosmosDbSettings cosmosDbSettings, IBackendDatabaseService backendDatabaseService)
        {
            dbSettings = cosmosDbSettings;
            dbService = backendDatabaseService;
        }

        public CuratedExperienceComponentViewModel GetComponent(CuratedExperience curatedExperience, Guid componentId)
        {
            var dbComponent = new CuratedExperienceComponent();
            if (componentId == default(Guid))
            {
                dbComponent = curatedExperience.Components.First();
            }
            else
            {
                dbComponent = curatedExperience.Components.Where(x => x.ComponentId == componentId).FirstOrDefault();
            }
            return MapComponentToViewModelComponent(curatedExperience, dbComponent);
        }

        public async Task SaveAnswers(CuratedExperienceAnswersViewModel viewModelAnswer)
        {
            // Todo: we could store the answers doc in the session and persist it to the db when the user
            // answers the last question. This will save us a trip to the database each time the user moves to
            // the next step. The caveat for this is that the users will need to repeat the survey from the
            // beginning if the session expires which might be frustrating.
            var answersDbCollection = dbSettings.CuratedExperienceAnswersCollectionId;
            var dbAnswers = MapViewModelAnswerToCuratedExperienceAnswers(viewModelAnswer);

            var savedAnswersDoc = await dbService.GetItemAsync<CuratedExperienceAnswers>(viewModelAnswer.AnswersDocId.ToString(), answersDbCollection);
            if (savedAnswersDoc == null)
            {
                await dbService.CreateItemAsync(dbAnswers, answersDbCollection);
            }
            else
            {
                savedAnswersDoc.Answers.Add(dbAnswers.Answers.First());
                await dbService.UpdateItemAsync(viewModelAnswer.AnswersDocId.ToString(), savedAnswersDoc, answersDbCollection);
            }
        }
        
        public async Task<CuratedExperience> GetCuratedExperience(Guid id)
        {
            return await dbService.GetItemAsync<CuratedExperience>(id.ToString(), dbSettings.CuratedExperienceCollectionId);
        }

        private CuratedExperienceComponentViewModel MapComponentToViewModelComponent(CuratedExperience curatedExperience, CuratedExperienceComponent dbComponent)
        {
            int stepRemained = 0;  // Todo:@Alaa calculated remaining steps
            return new CuratedExperienceComponentViewModel
            {
                CuratedExperienceId = curatedExperience.CuratedExperienceId,
                AnswersDocId = Guid.NewGuid(),
                StepsRemained = stepRemained,
                ComponentId = dbComponent.ComponentId,
                Name = dbComponent.Name,
                Text = dbComponent.Text,
                Help = dbComponent.Help,
                Learn = dbComponent.Learn,
                Tags = dbComponent.Tags,
                Buttons = dbComponent.Buttons,
                Fields = dbComponent.Fields,
            };
        }

        private CuratedExperienceAnswers MapViewModelAnswerToCuratedExperienceAnswers(CuratedExperienceAnswersViewModel viewModelAnswer)
        {
            var selectedItemsIds = new List<Guid>();
            foreach (var selectedFieldId in viewModelAnswer.MultiSelectionFieldIds)
            {
                selectedItemsIds.Add(selectedFieldId);
            }

            var filledInTexts = new List<FilledInText>();
            foreach (var field in viewModelAnswer.Fields)
            {
                filledInTexts.Add(new FilledInText
                {
                    FieldId = field.FieldId,
                    Value = field.Value
                });
            }

            var collectAnswersList = new List<Answer>();
            collectAnswersList.Add(new Answer
            {
                ClickedButtonId = viewModelAnswer.ButtonId,
                FilledInTexts = filledInTexts,
                SelectedItemsIds = selectedItemsIds
            });

            return new CuratedExperienceAnswers
            {
                CuratedExperienceId = viewModelAnswer.CuratedExperienceId,
                AnswersDocId = viewModelAnswer.AnswersDocId,
                Answers = collectAnswersList,
            };
        }
    }
}
