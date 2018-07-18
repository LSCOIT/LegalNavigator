using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.Extensions;
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

        public async Task<CuratedExperienceTree> GetCuratedExperience(Guid id)
        {
            return await dbService.GetItemAsync<CuratedExperienceTree>(id.ToString(), dbSettings.CuratedExperienceCollectionId);
        }

        public CuratedExperienceComponentViewModel GetComponent(CuratedExperienceTree curatedExperience, Guid componentId)
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

        public CuratedExperienceComponentViewModel GetNextComponent(CuratedExperienceTree curatedExperience, CuratedExperienceAnswersViewModel component)
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

        public CuratedExperienceComponent FindDestinationComponent(CuratedExperienceTree curatedExperience, Guid buttonId)
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

        public int CalculateRemainingQuestions(CuratedExperienceTree curatedExperience, CuratedExperienceComponent component)
        {
            var indexOfTheGivenQuestion = curatedExperience.Components.FindIndex(x => x.ComponentId == component.ComponentId);

            // In any given component, max possible buttons is 3 as per the A2J Author system.
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
                            button1RemainingQuestions.AddIfNotNull(FindDestinationComponent(curatedExperience, button.Id));
                            break;
                        case 1:
                            button2RemainingQuestions.AddIfNotNull(FindDestinationComponent(curatedExperience, button.Id));
                            break;
                        case 2:
                            button3RemainingQuestions.AddIfNotNull(FindDestinationComponent(curatedExperience, button.Id));
                            break;
                        default:
                            return 0;
                    }
                }
            }

            // return the longest possible route
            var allRemainingQuestions = new[] { button1RemainingQuestions, button2RemainingQuestions, button3RemainingQuestions };
            return allRemainingQuestions.ToList().OrderByDescending(x => x.Count).First().Count;
        }



        public int CalculateRemainingQuestionsV2(CuratedExperienceTree curatedExperience, CuratedExperienceComponent component)
        {
            // every curated experience has one or more components; every component has one or more buttons; every button has one or more destinations.
            var indexOfTheGivenQuestion = curatedExperience.Components.FindIndex(x => x.ComponentId == component.ComponentId);
            var remainingQuestions = new List<List<CuratedExperienceComponent>>();

            foreach (var remainingComponent in curatedExperience.Components.Skip(indexOfTheGivenQuestion))
            {
                var buttons = remainingComponent.Buttons;
                //foreach (var button in remainingComponent.Buttons)
                //{
                for (int i = 0; i < buttons.Count; i++)
                {
                    //if (remainingComponent.Buttons.IndexOf(buttons[i]) == i)
                    //{
                    if (remainingQuestions.Count <= i)
                    {
                        remainingQuestions.Add(new List<CuratedExperienceComponent>());
                    }
                    //var tempList = ;
                    //var com = new List<CuratedExperienceComponent> { FindDestinationComponent(curatedExperience, buttons[i].Id) };

                    remainingQuestions[i].AddIfNotNull(FindDestinationComponent(curatedExperience, buttons[i].Id));
                    //}
                    //}
                }
            }


            var breakpoint = remainingQuestions; // Todo:@Alaa - remove this temp code

            //return remainingQuestions.Count;
            // return the longest possible route
            //var allRemainingQuestions = new[] { button1RemainingQuestions, button2RemainingQuestions, button3RemainingQuestions };
            return remainingQuestions.OrderByDescending(x => x.Count).First().Count;
        }


        private CuratedExperienceComponentViewModel MapComponentToViewModelComponent(CuratedExperienceTree curatedExperience, CuratedExperienceComponent dbComponent, Guid answersDocId)
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
