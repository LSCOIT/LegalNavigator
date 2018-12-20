using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.A2JAuthor;
using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Interfaces.A2JAuthor;
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
        private readonly IA2JAuthorLogicParser parser;

        public CuratedExperienceBuisnessLogic(ICosmosDbSettings cosmosDbSettings, IBackendDatabaseService backendDatabaseService,
            IA2JAuthorLogicParser parser)
        {
            dbSettings = cosmosDbSettings;
            dbService = backendDatabaseService;
            this.parser = parser;
        }

        public async Task<CuratedExperience> GetCuratedExperienceAsync(Guid id)
        {
            return await dbService.GetItemAsync<CuratedExperience>(id.ToString(), dbSettings.CuratedExperiencesCollectionId);
        }

        public async Task<CuratedExperienceComponentViewModel> GetComponent(CuratedExperience curatedExperience, Guid componentId)
        {
            var dbComponent = new CuratedExperienceComponent();
            var answers = new CuratedExperienceAnswers();
            if (componentId == default(Guid))
            {
                answers.AnswersDocId = Guid.NewGuid();
                answers.CuratedExperienceId = curatedExperience.CuratedExperienceId;
                await dbService.CreateItemAsync(answers, dbSettings.GuidedAssistantAnswersCollectionId);
                dbComponent = curatedExperience.Components.First();
            }
            else
            {
                dbComponent = curatedExperience.Components.Where(x => x.ComponentId == componentId).FirstOrDefault();
            }

            return MapComponentViewModel(curatedExperience, dbComponent, answers.AnswersDocId);
        }

        public async Task<CuratedExperienceComponentViewModel> GetNextComponentAsync(CuratedExperience curatedExperience, CuratedExperienceAnswersViewModel component)
        {
            return MapComponentViewModel(curatedExperience,
                await FindDestinationComponentAsync(curatedExperience, component.ButtonId, component.AnswersDocId), component.AnswersDocId);
        }

        public async Task<Document> SaveAnswersAsync(CuratedExperienceAnswersViewModel viewModelAnswer, CuratedExperience curatedExperience)
        {
            var dbAnswers = MapCuratedExperienceViewModel(viewModelAnswer, curatedExperience);
            var savedAnswersDoc = await dbService.GetItemAsync<CuratedExperienceAnswers>(viewModelAnswer.AnswersDocId.ToString(), dbSettings.GuidedAssistantAnswersCollectionId);
            if (savedAnswersDoc == null || savedAnswersDoc.AnswersDocId == default(Guid))
            {
                return await dbService.CreateItemAsync(dbAnswers, dbSettings.GuidedAssistantAnswersCollectionId);
            }

            savedAnswersDoc.ButtonComponents.AddRange(dbAnswers.ButtonComponents);
            savedAnswersDoc.FieldComponents.AddRange(dbAnswers.FieldComponents);

            return await dbService.UpdateItemAsync(viewModelAnswer.AnswersDocId.ToString(), savedAnswersDoc, dbSettings.GuidedAssistantAnswersCollectionId);
        }

        public async Task<CuratedExperienceComponent> FindDestinationComponentAsync(CuratedExperience curatedExperience, Guid buttonId, Guid answersDocId)
        {
            var allButtonsInCuratedExperience = curatedExperience.Components.Select(x => x.Buttons).ToList();
            var currentButton = new Button();
            foreach (var button in allButtonsInCuratedExperience)
            {
                if (button.Where(x => x.Id == buttonId).Any())
                {
                    //currentComponent = button.Where(x => x.Id == buttonId)
                    currentButton = button.Where(x => x.Id == buttonId).First();
                }
            }

            var destinationComponent = new CuratedExperienceComponent();
            CuratedExperienceAnswers answers = new CuratedExperienceAnswers();
            var currentComponent = curatedExperience.Components.Where(x => x.Buttons.Contains(currentButton)).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(currentComponent.Code.CodeAfter) && currentComponent.Code.CodeAfter.Contains(Tokens.GOTO))
            {
                answers = await dbService.GetItemAsync<CuratedExperienceAnswers>(answersDocId.ToString(), dbSettings.GuidedAssistantAnswersCollectionId);
                // get the answers so far - done
                // get all the code in all the curated experience - to be done
                var currentComponentLogic = ExtractLogic(currentComponent, answers);
                if (currentComponentLogic != null)
                {
                    var parsedCode = parser.Parse(currentComponentLogic);
                    if (parsedCode.Any())
                    {
                        destinationComponent = curatedExperience.Components.Where(x => x.Name.Contains(parsedCode.Values.LastOrDefault())).FirstOrDefault();
                    }
                }
            }
            else if (!string.IsNullOrWhiteSpace(currentButton.Destination))
            {
                if (curatedExperience.Components.Where(x => x.Name == currentButton.Destination).Any())
                {
                    destinationComponent = curatedExperience.Components.Where(x => x.Name == currentButton.Destination).FirstOrDefault();
                }
            }
            if (!string.IsNullOrWhiteSpace(destinationComponent.Code.CodeBefore) && destinationComponent.Code.CodeBefore.Contains(Tokens.GOTO))
            {
                if (answers.AnswersDocId == default(Guid))
                {
                    answers = await dbService.GetItemAsync<CuratedExperienceAnswers>(answersDocId.ToString(), dbSettings.GuidedAssistantAnswersCollectionId);
                }
                var currentComponentLogic = ExtractLogic(destinationComponent, answers);
                if (currentComponentLogic != null)
                {
                    var parsedCode = parser.Parse(currentComponentLogic);
                    if (parsedCode.Any())
                    {
                        destinationComponent = curatedExperience.Components.Where(x => x.Name.Contains(parsedCode.Values.LastOrDefault())).FirstOrDefault();
                    }
                }
            }
            return destinationComponent.ComponentId == default(Guid) ? null : destinationComponent;
        }

        private int CalculateRemainingQuestions(CuratedExperience curatedExperience, CuratedExperienceComponent currentComponent)
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

                    possibleRoutes[i].AddIfNotNull(RemainingQuestions(curatedExperience, component.Buttons[i].Id));
                }
            }

            // return the longest possible route
            return possibleRoutes.OrderByDescending(x => x.Count).First().Count;
        }

        private CuratedExperienceComponentViewModel MapComponentViewModel(CuratedExperience curatedExperience, CuratedExperienceComponent dbComponent, Guid answersDocId)
        {
            if (dbComponent == null || dbComponent.ComponentId == default(Guid))
            {
                return null;
            }
            var remainingQuestions = CalculateRemainingQuestions(curatedExperience, dbComponent);
            return new CuratedExperienceComponentViewModel
            {
                CuratedExperienceId = curatedExperience.CuratedExperienceId,
                AnswersDocId = answersDocId,
                QuestionsRemaining = remainingQuestions,
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

        public CuratedExperienceAnswers MapCuratedExperienceViewModel(CuratedExperienceAnswersViewModel viewModelAnswer, CuratedExperience curatedExperience)
        {
            var buttonComponent = new CuratedExperienceComponent();
            foreach (var component in curatedExperience.Components)
            {
                if (component.Buttons.Where(x => x.Id == viewModelAnswer.ButtonId).Any())
                {
                    var button = component.Buttons.Where(x => x.Id == viewModelAnswer.ButtonId).FirstOrDefault();
                    buttonComponent = curatedExperience.Components.Where(x => x.Buttons.Contains(button)).FirstOrDefault();
                }
            }

            var userSelectedButtons = new List<ButtonComponent>();
            userSelectedButtons.Add(new ButtonComponent
            {
                ButtonId = viewModelAnswer.ButtonId,
                Name = buttonComponent.Buttons.Where(x => x.Id == viewModelAnswer.ButtonId).FirstOrDefault().Name,
                Value = buttonComponent.Buttons.Where(x => x.Id == viewModelAnswer.ButtonId).FirstOrDefault().Value,

                CodeBefore = buttonComponent.Code.CodeBefore,
                CodeAfter = buttonComponent.Code.CodeAfter
            });

            var userSelectedFields = new List<FieldComponent>();
            foreach (var answerField in viewModelAnswer.Fields)
            {
                var fieldComponent = new CuratedExperienceComponent();
                foreach (var component in curatedExperience.Components)
                {
                    if (answerField != null && component.Fields.Where(x => x.Id == Guid.Parse(answerField.FieldId)).Any())
                    {
                        var selectedField = component.Fields.Where(x => x.Id == Guid.Parse(answerField.FieldId)).FirstOrDefault();
                        fieldComponent = curatedExperience.Components.Where(x => x.Fields.Contains(selectedField)).FirstOrDefault();
                        userSelectedFields.Add(new FieldComponent
                        {
                            CodeBefore = userSelectedButtons.Any(x => x.CodeBefore == fieldComponent.Code.CodeBefore) ? string.Empty : fieldComponent.Code.CodeBefore,
                            CodeAfter = userSelectedButtons.Any(x => x.CodeAfter == fieldComponent.Code.CodeAfter) ? string.Empty : fieldComponent.Code.CodeAfter,
                            Fields = new List<AnswerField>
                            {
                                new AnswerField
                                {
                                    FieldId = selectedField.Id,
                                    Text = answerField.Value,
                                    Name = selectedField.Name,
                                    Value = selectedField.Value
                                }
                            }
                        });
                    }
                }
            }

            return new CuratedExperienceAnswers
            {
                CuratedExperienceId = viewModelAnswer.CuratedExperienceId,
                AnswersDocId = viewModelAnswer.AnswersDocId,
                ButtonComponents = userSelectedButtons,
                FieldComponents = userSelectedFields
            };
        }

        private CuratedExperienceAnswers ExtractLogic(CuratedExperienceComponent component, CuratedExperienceAnswers answers)
        {
            // when dealing with finding next destination of the current component we need to remove all logic
            // except the specific GOTO statement that comes in the current component. That is why I'm setting all logic to string.Empty.
            foreach (var answer in answers.ButtonComponents)
            {
                answer.CodeBefore = string.Empty;
                answer.CodeAfter = string.Empty;
            }
            foreach (var answer in answers.FieldComponents)
            {
                answer.CodeBefore = string.Empty;
                answer.CodeAfter = string.Empty;
            }
            if (!string.IsNullOrWhiteSpace(component.Code.CodeBefore) || !string.IsNullOrWhiteSpace(component.Code.CodeAfter))
            {
                var button = new ButtonComponent();
                button.ButtonId = Guid.NewGuid();
                button.Name = string.Empty;
                button.Value = string.Empty;
                button.CodeBefore = component.Code.CodeBefore;
                button.CodeAfter = component.Code.CodeAfter;
                answers.ButtonComponents.Add(button);
            }

            return answers;
        }

        private CuratedExperienceComponent RemainingQuestions(CuratedExperience curatedExperience, Guid buttonId)
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
            CuratedExperienceAnswers answers = new CuratedExperienceAnswers();
            var currentComponent = curatedExperience.Components.Where(x => x.Buttons.Contains(currentButton)).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(currentButton.Destination))
            {
                if (curatedExperience.Components.Where(x => x.Name == currentButton.Destination).Any())
                {
                    destinationComponent = curatedExperience.Components.Where(x => x.Name == currentButton.Destination).FirstOrDefault();
                }
            }

            return destinationComponent.ComponentId == default(Guid) ? null : destinationComponent;
        }
    }
}
