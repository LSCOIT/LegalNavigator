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
        private readonly IPersonalizedPlanParse parser;

        public CuratedExperienceBuisnessLogic(ICosmosDbSettings cosmosDbSettings, IBackendDatabaseService backendDatabaseService,
            IPersonalizedPlanParse parser)
        {
            dbSettings = cosmosDbSettings;
            dbService = backendDatabaseService;
            this.parser = parser;
        }

        public async Task<CuratedExperience> GetCuratedExperienceAsync(Guid id)
        {
            return await dbService.GetItemAsync<CuratedExperience>(id.ToString(), dbSettings.CuratedExperienceCollectionId);
        }

        public async Task<CuratedExperienceComponentViewModel> GetComponentAsync(CuratedExperience curatedExperience, Guid componentId)
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
                return await MapComponentToViewModelComponentAsync(curatedExperience, dbComponent, answersDocId: Guid.Empty); // In future, instead of sending an empty Guid
                // for the answersDocId, we could pass the actual Id of the answers document (if any) so that we could return not only the component but also the
                // answers associated with it.
            }
            catch
            {
                // log exception
                return null;
            }
        }

        public async Task<CuratedExperienceComponentViewModel> GetNextComponentAsync(CuratedExperience curatedExperience, CuratedExperienceAnswersViewModel component)
        {
            return await MapComponentToViewModelComponentAsync(curatedExperience,
                await FindDestinationComponentAsync(curatedExperience, component.ButtonId, component.AnswersDocId), component.AnswersDocId);
        }

        public async Task<Document> SaveAnswersAsync(CuratedExperienceAnswersViewModel viewModelAnswer, CuratedExperience curatedExperience)
        {
            try
            {
                // We could store the answers doc in the session and persist it to the db when the user
                // answers the last question. This will save us a trip to the database each time the user moves to
                // the next step. The caveat for this is that the users will need to repeat the survey from the
                // beginning if the session expires which might be frustrating.
                var answersDbCollection = dbSettings.CuratedExperienceAnswersCollectionId;
                var dbAnswers = await MapViewModelAsync(viewModelAnswer, curatedExperience);

                var savedAnswersDoc = await dbService.GetItemAsync<CuratedExperienceAnswers>(viewModelAnswer.AnswersDocId.ToString(), answersDbCollection);
                if (savedAnswersDoc == null)
                {
                    return await dbService.CreateItemAsync(dbAnswers, answersDbCollection);
                }
                else
                {
                    savedAnswersDoc.ButtonComponents.AddRange(dbAnswers.ButtonComponents);
                    savedAnswersDoc.FieldComponents.AddRange(dbAnswers.FieldComponents);
                    return await dbService.UpdateItemAsync(viewModelAnswer.AnswersDocId.ToString(), savedAnswersDoc, answersDbCollection);
                }
            }
            catch
            {
                // log exception
                return null;
            }
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
            var currentComponent = curatedExperience.Components.Where(x => x.Buttons.Contains(currentButton)).FirstOrDefault();
            if ((!string.IsNullOrWhiteSpace(currentComponent.Code.CodeBefore) && currentComponent.Code.CodeBefore.Contains(Tokens.GOTO)) ||
                (!string.IsNullOrWhiteSpace(currentComponent.Code.CodeAfter) && currentComponent.Code.CodeAfter.Contains(Tokens.GOTO)))
            {
                // get the answers so far - done
                // get all the code in all the curated experience - to be done
                var personalizedPlanEvaluator = ExtractCode(curatedExperience);
                var answers = await dbService.GetItemAsync<CuratedExperienceAnswers>(answersDocId.ToString(), dbSettings.CuratedExperienceAnswersCollectionId);
                if (answers != null)
                {
                    var tempParser = new Dictionary<string, string>();
                    foreach (var button in currentComponent.Buttons)
                    {
                        if (!string.IsNullOrWhiteSpace(button.Name) && !string.IsNullOrWhiteSpace(button.Value))
                            answers.ButtonComponents.Add(new ButtonComponent
                            {
                                CodeBefore = currentComponent.Code.CodeBefore,
                                CodeAfter = currentComponent.Code.CodeAfter,
                                Name = button.Name,
                                Value = button.Value,
                            });

                        var parsedCode = parser.Parse(answers);
                        tempParser = parser.Parse(answers);
                        var temp = parsedCode.Values.LastOrDefault();
                        destinationComponent = curatedExperience.Components.Where(x => x.Name.Contains(parsedCode.Values.LastOrDefault())).FirstOrDefault();
                    }

                    // todo: we need to find a better way to match var name with component (child) name. A2J Author childern names are appended with 
                    // numbers so this must be taken into account.
                    destinationComponent = curatedExperience.Components.Where(x => x.Name.Contains(parsedCode.Values.LastOrDefault())).FirstOrDefault();
                }
            }
            else if (!string.IsNullOrWhiteSpace(currentButton.Destination))
            {
                if (curatedExperience.Components.Where(x => x.Name == currentButton.Destination).Any())
                {
                    destinationComponent = curatedExperience.Components.Where(x => x.Name == currentButton.Destination).FirstOrDefault();
                }
            }

            return destinationComponent.ComponentId == default(Guid) ? null : destinationComponent;
        }

        public async Task<int> CalculateRemainingQuestionsAsync(CuratedExperience curatedExperience, CuratedExperienceComponent currentComponent, Guid answersDocId)
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

                    possibleRoutes[i].AddIfNotNull(await FindDestinationComponentAsync(curatedExperience, component.Buttons[i].Id, answersDocId));
                }
            }

            // return the longest possible route
            return possibleRoutes.OrderByDescending(x => x.Count).First().Count;
        }

        private async Task<CuratedExperienceComponentViewModel> MapComponentToViewModelComponentAsync(CuratedExperience curatedExperience, CuratedExperienceComponent dbComponent, Guid answersDocId)
        {
            if (dbComponent == null || dbComponent.ComponentId == default(Guid))
            {
                return null;
            }

            var answerDocId = answersDocId == default(Guid) ? Guid.NewGuid() : answersDocId;
            return new CuratedExperienceComponentViewModel
            {
                CuratedExperienceId = curatedExperience.CuratedExperienceId,
                AnswersDocId = answerDocId,
                QuestionsRemaining = await CalculateRemainingQuestionsAsync(curatedExperience, dbComponent, answerDocId),
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

        public async Task<CuratedExperienceAnswers> MapViewModelAsync(CuratedExperienceAnswersViewModel viewModelAnswer, CuratedExperience curatedExperience)
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

                CodeBefore =  buttonComponent.Code.CodeBefore,
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

        private CuratedExperienceAnswers ExtractCode(CuratedExperience curatedExperience)
        {
            var answers = new CuratedExperienceAnswers();
            foreach (var component in curatedExperience.Components)
            {
                if (!string.IsNullOrWhiteSpace(component.Code.CodeBefore) || !string.IsNullOrWhiteSpace(component.Code.CodeAfter))
                {
                    var button = new ButtonComponent();
                    button.CodeBefore = component.Code.CodeBefore;
                    button.CodeAfter = component.Code.CodeAfter;
                    answers.ButtonComponents.Add(button);
                }
            }

            return answers;
        }
    }
}
