using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models.CuratedExperience;
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
                return MapComponentToViewModelComponent(curatedExperience, dbComponent, answersDocId: Guid.Empty); // In future instead of sending an empty Guid
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
            var nextCompnent = FindDestinationComponent(curatedExperience, component.ButtonId);
            return MapComponentToViewModelComponent(curatedExperience, nextCompnent, component.AnswersDocId);
        }

        public async Task<Document> SaveAnswers(CuratedExperienceAnswersViewModel viewModelAnswer)
        {
            try
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

        private CuratedExperienceComponentViewModel MapComponentToViewModelComponent(CuratedExperience curatedExperience, CuratedExperienceComponent dbComponent, Guid answersDocId)
        {
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

        private int CalculateRemainingQuestions(CuratedExperience curatedExperience, CuratedExperienceComponent component)
        {
            var indexOfTheGivenQuestion = curatedExperience.Components.FindIndex(x => x.ComponentId == component.ComponentId);

            // In any given component max possible buttons is 3 as per the A2J Author system.
            var button1RemainingQuestions = new List<CuratedExperienceComponent>();
            var button2RemainingQuestions = new List<CuratedExperienceComponent>();
            var button3RemainingQuestions = new List<CuratedExperienceComponent>();

            foreach (var remainingComponent in curatedExperience.Components.Skip(indexOfTheGivenQuestion))
            {
                foreach (var button in remainingComponent.Buttons)
                {
                    switch (remainingComponent.Buttons.IndexOf(button))
                    {
                        case 0:
                            button1RemainingQuestions.Add(FindDestinationComponent(curatedExperience, button.Id));
                            break;
                        case 1:
                            button2RemainingQuestions.Add(FindDestinationComponent(curatedExperience, button.Id));
                            break;
                        case 2:
                            button3RemainingQuestions.Add(FindDestinationComponent(curatedExperience, button.Id));
                            break;
                        default:
                            return 0;
                    }
                }
            }

            var allRemainingQuestions = new[] { button1RemainingQuestions, button2RemainingQuestions, button3RemainingQuestions };
            allRemainingQuestions.ToList().OrderBy(x => x.Count);

            var breakpoint = string.Empty; // Todo:@Alaa - remove this temp code

            return allRemainingQuestions.First().Count; // return the longest possible route
        }

        private CuratedExperienceComponent FindDestinationComponent(CuratedExperience curatedExperience, Guid buttonId)
        {
            var allButtonsInCuratedExperience = curatedExperience.Components.Select(x => x.Buttons).ToList();
            var userClickedButton = new Button();
            foreach (var button in allButtonsInCuratedExperience)
            {
                if (button.Where(x => x.Id == buttonId).Any())
                {
                    userClickedButton = button.Where(x => x.Id == buttonId).First();
                }
            }

            var destinationComponent = new CuratedExperienceComponent();
            if (!string.IsNullOrWhiteSpace(userClickedButton.Destination))
            {
                if (curatedExperience.Components.Where(x => x.Name == userClickedButton.Destination).Any())
                {
                    destinationComponent = curatedExperience.Components.Where(x => x.Name == userClickedButton.Destination).First();
                }
            }

            return destinationComponent;
        }
    }
}
