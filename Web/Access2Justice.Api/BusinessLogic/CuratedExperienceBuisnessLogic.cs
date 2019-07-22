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
using System.Linq.Expressions;
using System.Threading.Tasks;
using Access2Justice.CosmosDb;
using NuGet.Frameworks;


namespace Access2Justice.Api.BusinessLogic
{
    public class CuratedExperienceBuisnessLogic : ICuratedExperienceBusinessLogic
    {
        private readonly ICosmosDbSettings dbSettings;
        private readonly IBackendDatabaseService dbService;
        private readonly IA2JAuthorLogicParser parser;
        private readonly IUserProfileBusinessLogic userProfileBusinessLogic;

        public CuratedExperienceBuisnessLogic(ICosmosDbSettings cosmosDbSettings, IBackendDatabaseService backendDatabaseService,
            IA2JAuthorLogicParser parser, IUserProfileBusinessLogic userProfileBusinessLogic)
        {
            dbSettings = cosmosDbSettings;
            dbService = backendDatabaseService;
            this.parser = parser;
            this.userProfileBusinessLogic = userProfileBusinessLogic;
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

            return MapComponentViewModel(curatedExperience, dbComponent, answers.AnswersDocId, answers);
        }

        public async Task<CuratedExperienceComponentViewModel> GetNextComponentAsync(CuratedExperience curatedExperience, CuratedExperienceAnswersViewModel component, Document answers)
        {
            return await GetNextComponentAsync(curatedExperience, component, answers.Convert<CuratedExperienceAnswers>());
        }

        public async Task<CuratedExperienceComponentViewModel> GetNextComponentAsync(CuratedExperience curatedExperience, CuratedExperienceAnswersViewModel component, CuratedExperienceAnswers answers)
        {
            return await getNextComponentAsync(curatedExperience, component.ButtonId, answers);
        }

        public async Task<CuratedExperienceComponentViewModel> GetNextComponentAsync(CuratedExperience curatedExperience, CuratedExperienceAnswers answers, bool fillAnswer = false)
        {
            var nextComponent = await getNextComponentAsync(curatedExperience, getLastAnswerButtonId(answers), answers);
            if (fillAnswer)
            {
                nextComponent.ButtonsSelected = answers.ButtonComponents.FindAll(x => x.AnswerNumber == answers.CurrentActualAnswer - 1);
                nextComponent.FieldsSelected = answers.FieldComponents.FindAll(x => x.AnswerNumber == answers.CurrentActualAnswer - 1);
            }

            return nextComponent;
        }

        private Guid getLastAnswerButtonId(CuratedExperienceAnswers answers)
        {
            return answers.ButtonComponents
                       .Where(x => x.AnswerNumber <= answers.CurrentActualAnswer)
                       .OrderBy(x => x.AnswerNumber)
                       .LastOrDefault()?.ButtonId ?? Guid.Empty;
        }

        private async Task<CuratedExperienceComponentViewModel> getNextComponentAsync(CuratedExperience curatedExperience,
            Guid buttonId, CuratedExperienceAnswers answers)
        {
            return MapComponentViewModel(curatedExperience,
                await FindDestinationComponentAsync(curatedExperience, buttonId, answers.AnswersDocId),
                answers.AnswersDocId, answers);
        }

        public async Task<Guid> GetUserAnswerId(string userId)
        {
            var userProfile = await userProfileBusinessLogic.GetUserProfileDataAsync<UserProfile>(userId);
            if (userProfile == null)
            {
                return Guid.Empty;
            }

            return userProfile.CuratedExperienceAnswersId;
        }

        public async Task<CuratedExperienceAnswers> GetLastAnswerProgress(Guid curatedExperienceId, params Guid[] answerIds)
        {
            if (answerIds == null || answerIds.Length == 0)
            {
                return null;
            }

            var notEmptyIds = answerIds.Where(x => x != Guid.Empty).ToList();
            if (notEmptyIds.Count == 0)
            {
                return null;
            }

            Expression<Func<CuratedExperienceAnswers, bool>> whereCondition;
            if (curatedExperienceId == Guid.Empty)
            {
                whereCondition = x => notEmptyIds.Contains(x.AnswersDocId);
            }
            else
            {
                whereCondition = x =>
                    notEmptyIds.Contains(x.AnswersDocId) && x.CuratedExperienceId == curatedExperienceId;
            }

            var answers = await dbService.GetItemsAsync<CuratedExperienceAnswers>(
                whereCondition,
                dbSettings.GuidedAssistantAnswersCollectionId);

            return answers?.OrderBy(x => x.LastModifiedTimeStamp).LastOrDefault();
        }

        public async Task<bool> AnswersStepNext(CuratedExperienceAnswers answers)
        {
            return await changeActualAnswer(answers, +1);
        }

        public async Task<bool> AnswersStepBack(CuratedExperienceAnswers answers)
        {
            return await changeActualAnswer(answers, -1);
        }

        public async Task<Document> SaveAnswersAsync(CuratedExperienceAnswersViewModel viewModelAnswer, CuratedExperience curatedExperience, string userId = null)
        {
            var dbAnswers = MapCuratedExperienceViewModel(viewModelAnswer, curatedExperience);
            var savedAnswersDoc = await dbService.GetItemAsync<CuratedExperienceAnswers>(viewModelAnswer.AnswersDocId.ToString(), dbSettings.GuidedAssistantAnswersCollectionId);
            if (savedAnswersDoc == null || savedAnswersDoc.AnswersDocId == default(Guid))
            {
                setAnswerNumber(dbAnswers, 1);
                return await dbService.CreateItemAsync(dbAnswers, dbSettings.GuidedAssistantAnswersCollectionId);
            }
            else
            {
                var maxNumber = savedAnswersDoc.CurrentActualAnswer;
                setAnswerNumber(dbAnswers, maxNumber + 1);
            }
            savedAnswersDoc.CurrentActualAnswer = dbAnswers.CurrentActualAnswer;

            savedAnswersDoc.ButtonComponents.RemoveAll(x => x.AnswerNumber >= dbAnswers.CurrentActualAnswer);
            savedAnswersDoc.FieldComponents.RemoveAll(x => x.AnswerNumber >= dbAnswers.CurrentActualAnswer);

            savedAnswersDoc.ButtonComponents.AddRange(dbAnswers.ButtonComponents);
            savedAnswersDoc.FieldComponents.AddRange(dbAnswers.FieldComponents);

            var document = await dbService.UpdateItemAsync(viewModelAnswer.AnswersDocId.ToString(), savedAnswersDoc, dbSettings.GuidedAssistantAnswersCollectionId);
            if (!string.IsNullOrWhiteSpace(userId))
            {
                await saveAnswersToProfile(userId, viewModelAnswer.AnswersDocId);
            }

            return document;
        }

        private void setAnswerNumber(CuratedExperienceAnswers dbAnswers, uint currentActualAnswer)
        {
            dbAnswers.CurrentActualAnswer = currentActualAnswer;
            dbAnswers.ButtonComponents.ForEach(x => x.AnswerNumber = currentActualAnswer);
            dbAnswers.FieldComponents.ForEach(x => x.AnswerNumber = currentActualAnswer);
        }

        private async Task saveAnswersToProfile(string userId, Guid answersDocId)
        {
            var userProfile = await userProfileBusinessLogic.GetUserProfileDataAsync<UserProfile>(userId);
            if (userProfile == null || userProfile.CuratedExperienceAnswersId == answersDocId)
            {
                return;
            }

            userProfile.CuratedExperienceAnswersId = answersDocId;
            await dbService.UpdateItemAsync(userProfile.Id, userProfile, dbSettings.ProfilesCollectionId);
        }

        public async Task<CuratedExperienceComponent> FindDestinationComponentAsync(CuratedExperience curatedExperience, Guid buttonId, Guid answersDocId)
        {
            if (buttonId == Guid.Empty)
            {
                return await GetComponent(curatedExperience, Guid.Empty);
            }
            var allButtonsInCuratedExperience = curatedExperience.Components.Select(x => x.Buttons).ToList();
            var currentButton = new Button();
            foreach (var button in allButtonsInCuratedExperience)
            {
                var desiredButton =  button.FirstOrDefault(x => x.Id == buttonId);
                if (desiredButton == null)
                {
                    continue;
                }
                currentButton = desiredButton;
                break;
            }

            var destinationComponent = new CuratedExperienceComponent();
            var answers = new CuratedExperienceAnswers();
            var currentComponent = curatedExperience.Components.First(x => x.Buttons.Contains(currentButton));
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
                        destinationComponent = curatedExperience.Components.Where(x => x.Name.Contains(parsedCode.Values.FirstOrDefault())).FirstOrDefault();
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

        private CuratedExperienceComponentViewModel MapComponentViewModel(
            CuratedExperience curatedExperience, CuratedExperienceComponent dbComponent, Guid answersDocId, CuratedExperienceAnswers answers = null)
        {
            if (dbComponent == null || dbComponent.ComponentId == default(Guid))
            {
                return null;
            }
            var remainingQuestions = CalculateRemainingQuestions(curatedExperience, dbComponent);
            var component = new CuratedExperienceComponentViewModel
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
                AnswersId = answers?.AnswersDocId ?? Guid.Empty
            };
            if(answers != null)
            {
                var maxAnswer = maxAnswerNumber(answers);
                component.HasNextAnswers = maxAnswer > answers.CurrentActualAnswer;
                component.HasPreviousAnswers = answers.CurrentActualAnswer > 0;

                if (component.HasNextAnswers)
                {
                    component.ButtonsSelected =
                        answers.ButtonComponents.FindAll(x => x.AnswerNumber == answers.CurrentActualAnswer + 1);
                    component.FieldsSelected =
                        answers.FieldComponents.FindAll(x => x.AnswerNumber == answers.CurrentActualAnswer + 1);
                }
            }

            return component;
        }

        public CuratedExperienceAnswers MapCuratedExperienceViewModel(CuratedExperienceAnswersViewModel viewModelAnswer, CuratedExperience curatedExperience)
        {
            var buttonComponent = new CuratedExperienceComponent();
            foreach (var component in curatedExperience.Components)
            {
                if (component.Buttons.Any(x => x.Id == viewModelAnswer.ButtonId))
                {
                    var button = component.Buttons.FirstOrDefault(x => x.Id == viewModelAnswer.ButtonId);
                    buttonComponent = curatedExperience.Components.First(x => x.Buttons.Contains(button));
                }
            }

            var userSelectedButtons = new List<ButtonComponent>
            {
                new ButtonComponent
                {
                    ButtonId = viewModelAnswer.ButtonId,
                    Name = buttonComponent.Buttons.First(x => x.Id == viewModelAnswer.ButtonId).Name,
                    Value = buttonComponent.Buttons.First(x => x.Id == viewModelAnswer.ButtonId).Value,
                    CodeBefore = buttonComponent.Code.CodeBefore,
                    CodeAfter = buttonComponent.Code.CodeAfter
                }
            };

            var userSelectedFields = new List<FieldComponent>();
            var fieldComponent = new CuratedExperienceComponent();
            foreach (var answerField in viewModelAnswer.Fields)
            {
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
                button.CodeBefore = component.Code.CodeBefore.RemoveHtmlTags();
                button.CodeAfter = component.Code.CodeAfter.RemoveHtmlTags();
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

        private async Task<bool> changeActualAnswer(CuratedExperienceAnswers answers, int delta)
        {
            if (answers == null)
            {
                return false;
            }

            var newActualAnswer = answers.CurrentActualAnswer + delta;
            var maxPossibleAnswer = maxAnswerNumber(answers);

            if (newActualAnswer < 0)
            {
                newActualAnswer = 0;
            }

            if (newActualAnswer > maxPossibleAnswer)
            {
                newActualAnswer = maxPossibleAnswer;
            }

            answers.CurrentActualAnswer = (uint)newActualAnswer;
            return await dbService.UpdateItemAsync(answers.AnswersDocId.ToString(), answers, dbSettings.GuidedAssistantAnswersCollectionId) != null;
        }

        private static uint maxAnswerNumber(CuratedExperienceAnswers answers)
        {
            return Math.Max(
                answers.ButtonComponents.Select(x => x.AnswerNumber).DefaultIfEmpty().Max(),
                answers.FieldComponents.Select(x => x.AnswerNumber).DefaultIfEmpty().Max()
            );
        }
    }
}
