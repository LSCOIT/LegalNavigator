using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.Azure.Documents;
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

        public async Task<CuratedExperience> GetCuratedExperience(Guid id)
        {
            return await dbService.GetItemAsync<CuratedExperience>(id.ToString(), dbSettings.CuratedExperienceCollectionId);
        }

        public CuratedExperienceComponentViewModel GetComponent(CuratedExperience curatedExperience, Guid componentId)
        {
            try
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
                return MapComponentToViewModelComponent(curatedExperience, dbComponent, answersDocId: Guid.Empty); // In future, instead of sending an empty Guid
                // for the answersDocId, we could pass the actual Id of the answers document (if any) so that we could return not only the component but also the
                // answers associated with it.
            }
            catch
            {
                // log exception
                return null;
            }
        }

        public CuratedExperienceComponentViewModel GetNextComponent(CuratedExperience curatedExperience, CuratedExperienceAnswersViewModel component)
        {
            return MapComponentToViewModelComponent(curatedExperience,
                FindDestinationComponent(curatedExperience, component.ButtonId), component.AnswersDocId);
        }

        public async Task<Document> SaveAnswers(CuratedExperienceAnswersViewModel viewModelAnswer)
        {
            try
            {
                // We could store the answers doc in the session and persist it to the db when the user
                // answers the last question. This will save us a trip to the database each time the user moves to
                // the next step. The caveat for this is that the users will need to repeat the survey from the
                // beginning if the session expires which might be frustrating.
                var answersDbCollection = dbSettings.CuratedExperienceAnswersCollectionId;
                var dbAnswers = MapViewModelAnswerToCuratedExperienceAnswer(viewModelAnswer);

                var savedAnswersDoc = await dbService.GetItemAsync<CuratedExperienceAnswers>(viewModelAnswer.AnswersDocId.ToString(), answersDbCollection);
                if (savedAnswersDoc == null)
                {
                    return await dbService.CreateItemAsync(dbAnswers, answersDbCollection);
                }
                else
                {
                    savedAnswersDoc.Answers.Add(dbAnswers.Answers.First());
                    return await dbService.UpdateItemAsync(viewModelAnswer.AnswersDocId.ToString(), savedAnswersDoc, answersDbCollection);
                }
            }
            catch
            {
                // log exception
                return null;
            }
        }

        public CuratedExperienceComponent FindDestinationComponent(CuratedExperience curatedExperience, Guid buttonId)
        {
            var allButtonsInCuratedExperience = curatedExperience.Components.Select(x => x.Buttons).ToList();
            var currentButton = new Button();
            foreach (var button in allButtonsInCuratedExperience)
            {
                if (button.Where(x => x.Id == buttonId).Any())
                {
                    currentButton = button.Where(x => x.Id == buttonId).First();
                }
            }

            var destinationComponent = new CuratedExperienceComponent();
            if (!string.IsNullOrWhiteSpace(currentButton.Destination))
            {
                if (curatedExperience.Components.Where(x => x.Name == currentButton.Destination).Any())
                {
                    destinationComponent = curatedExperience.Components.Where(x => x.Name == currentButton.Destination).First();
                }
            }

            return destinationComponent.ComponentId == default(Guid) ? null : destinationComponent;
        }

        public int CalculateRemainingQuestions(CuratedExperience curatedExperience, CuratedExperienceComponent currentComponent)
        {
            // start calculating routes based on the current component location in the json tree.
            var indexOfCurrentComponent = curatedExperience.Components.FindIndex(x => x.ComponentId == currentComponent.ComponentId);

            // every curated experience has one or more components; every component has one or more buttons; every button has one or more destinations.
            var possibleRoutes = new List<List<CuratedExperienceComponent>>();

            foreach (var component in curatedExperience.Components.Skip(indexOfCurrentComponent))
            {
                for (int i = 0; i < component.Buttons.Count; i++)
                {
                    if (possibleRoutes.Count <= i)
                    {
                        possibleRoutes.Add(new List<CuratedExperienceComponent>());
                    }

                    possibleRoutes[i].AddIfNotNull(FindDestinationComponent(curatedExperience, component.Buttons[i].Id));
                }
            }

            // return the longest possible route
            return possibleRoutes.OrderByDescending(x => x.Count).First().Count;
        }

        private CuratedExperienceComponentViewModel MapComponentToViewModelComponent(CuratedExperience curatedExperience, CuratedExperienceComponent dbComponent, Guid answersDocId)
        {
            if (dbComponent == null || dbComponent.ComponentId == default(Guid))
            {
                return null;
            }

            return new CuratedExperienceComponentViewModel
            {
                CuratedExperienceId = curatedExperience.CuratedExperienceId,
                AnswersDocId = answersDocId == default(Guid) ? Guid.NewGuid() : answersDocId,
                QuestionsRemaining = CalculateRemainingQuestions(curatedExperience, dbComponent),
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

        private CuratedExperienceAnswers MapViewModelAnswerToCuratedExperienceAnswer(CuratedExperienceAnswersViewModel viewModelAnswer)
        {
            var filledInTexts = new List<AnswerField>();
            foreach (var field in viewModelAnswer.Fields)
            {
                filledInTexts.Add(new AnswerField
                {
                    FieldId = field.FieldId,
                    Value = field.Value
                });
            }

            var collectAnswersList = new List<Answer>();
            collectAnswersList.Add(new Answer
            {
                AnswerButtonId = viewModelAnswer.ButtonId,
                AnswerFields = filledInTexts,
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
