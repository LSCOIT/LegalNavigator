using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Access2Justice.Api.BusinessLogic
{
    public class A2JAuthorBusinessLogic : ICuratedExperienceConvertor
    {
        private readonly ICosmosDbSettings dbSettings;
        private readonly IBackendDatabaseService dbService;
        private readonly IPersonalizedPlanEngine personalizedPlanEngine;

        public A2JAuthorBusinessLogic(ICosmosDbSettings cosmosDbSettings, IBackendDatabaseService backendDatabaseService,
            IPersonalizedPlanEngine a2JAuthorParserBusinessLogic)
        {
            dbSettings = cosmosDbSettings;
            dbService = backendDatabaseService;
            personalizedPlanEngine = a2JAuthorParserBusinessLogic;
        }

        public CuratedExperience ConvertA2JAuthorToCuratedExperience(JObject a2jSchema)
        {
            var cx = new CuratedExperience();
            var a2jProperties = a2jSchema.Properties();

            cx.CuratedExperienceId = Guid.NewGuid();
            cx.Title = a2jProperties.GetValue("title");
            var resource = MapResourceProperties(a2jProperties, cx.CuratedExperienceId);

            var pages = ((JObject)a2jProperties.Where(x => x.Name == "pages").FirstOrDefault()?.Value).Properties();
            foreach (var page in pages)
            {
                var pageProperties = ((JObject)page.FirstOrDefault()).Properties();
                var componentFields = GetFields(pageProperties);
                var componentButtons = GetButtons(pageProperties);
                var componentCodes = GetCodes(pageProperties);

                cx.Components.Add(new CuratedExperienceComponent
                {
                    ComponentId = Guid.NewGuid(),
                    Name = pageProperties.GetValue("name"),
                    Help = pageProperties.GetValue("help").RemoveHtmlTags(),
                    Learn = pageProperties.GetValue("learn").RemoveHtmlTags(),
                    Text = pageProperties.GetValue("text").RemoveHtmlTags(keepTextFormatingTags: true, keepHyperlinkTags: true).RemoveCustomA2JFunctions(),
                    Fields = componentFields,
                    Buttons = componentButtons,
                    Code = componentCodes
                });
            }

            dbService.CreateItemAsync(cx, dbSettings.CuratedExperiencesCollectionId);
            dbService.CreateItemAsync(resource, dbSettings.ResourcesCollectionId);

            return cx;
        }

        private GuidedAssistant MapResourceProperties(IEnumerable<JProperty> a2jProperties, Guid curatedExperienceId)
        {
            return new GuidedAssistant
            {
                ResourceId = Guid.NewGuid(),
                CuratedExperienceId = curatedExperienceId.ToString(),
                Name = a2jProperties.GetValue("subjectarea"),
                Description = a2jProperties.GetValue("description"),
                CreatedTimeStamp = a2jProperties.GetDateOrNull("createdate"),
                ModifiedTimeStamp = a2jProperties.GetDateOrNull("modifydate")
            };
        }

        private List<Field> GetFields(IEnumerable<JProperty> pageProperties)
        {
            var componentFields = new List<Field>();
            var fieldsProperties = pageProperties.GetArrayValue("fields").FirstOrDefault().ToList();

            foreach (var buttonProperty in fieldsProperties)
            {
                var field = ((JObject)buttonProperty).Properties();
                var type = MapA2JFieldTypeToCuratedExperienceQuestionType(field.GetValue("type"));

                componentFields.Add(new Field
                {
                    Id = Guid.NewGuid(),
                    Type = type.ToString(),
                    Label = field.GetValue("label").RemoveHtmlTags(),
                    Name = field.GetValue("name"),
                    Value = field.GetValue("value"),
                    IsRequired = bool.Parse(field.GetValue("required")),
                    MinLength = field.GetValue("min"),
                    MaxLength = field.GetValue("max"),
                    InvalidPrompt = field.GetValue("invalidPrompt")
                });
            }

            return componentFields;
        }

        private List<Button> GetButtons(IEnumerable<JProperty> pageProperties)
        {
            var componentButtons = new List<Button>();
            var buttonsProperties = pageProperties.GetArrayValue("buttons").FirstOrDefault().ToList();

            foreach (var buttonProperty in buttonsProperties)
            {
                var button = ((JObject)buttonProperty).Properties();
                componentButtons.Add(new Button
                {
                    Id = Guid.NewGuid(),
                    Label = button.GetValue("label").RemoveHtmlTags(),
                    Destination = button.GetValue("next"),
                    Name = button.GetValue("name"),
                    Value = button.GetValue("value")
                });
            }

            return componentButtons;
        }

        private PersonalizedPlanEvaluator GetCodes(IEnumerable<JProperty> pageProperties)
        {
            return new PersonalizedPlanEvaluator
            {
                CodeBefore = pageProperties.GetValue("codeBefore"),
                CodeAfter = pageProperties.GetValue("codeAfter")
            };
        }

        private CuratedExperienceQuestionType MapA2JFieldTypeToCuratedExperienceQuestionType(string a2jAuthorFieldtype)
        {
            switch (a2jAuthorFieldtype.ToUpperInvariant())
            {
                case "TEXT":
                    return CuratedExperienceQuestionType.text;
                case "TEXTLONG":
                    return CuratedExperienceQuestionType.richText;
                case "TEXTPICK":
                    return CuratedExperienceQuestionType.list;
                case "NUMBER":
                    return CuratedExperienceQuestionType.number;
                case "NUMBERDOLLAR":
                    return CuratedExperienceQuestionType.currency;
                case "NUMBERSSN":
                    return CuratedExperienceQuestionType.ssn;
                case "NUMBERPHONE":
                    return CuratedExperienceQuestionType.phone;
                case "NUMBERZIP":
                    return CuratedExperienceQuestionType.zipCode;
                case "NUMBERPICK":
                    return CuratedExperienceQuestionType.list;
                case "GENDER":
                    return CuratedExperienceQuestionType.radio;
                case "RADIO":
                case "RADIOBUTTON":
                    return CuratedExperienceQuestionType.radio;
                case "CHECKBOX":
                    return CuratedExperienceQuestionType.checkBox;
                case "CHECKBOXNOTA":
                    return CuratedExperienceQuestionType.checkBox;
                default:
                    return CuratedExperienceQuestionType.text;
            }
        }

        private Dictionary<string, string> GetVarsValuesFromUserAnswers(CuratedExperienceAnswers userAnswers)
        {
            var varsDic = new Dictionary<string, string>();

            foreach (var button in userAnswers.ButtonComponents)
            {
                varsDic.Add(button.Name, button.Value);
            }

            foreach (var fieldComponent in userAnswers.FieldComponents)
            {
                foreach (var field in fieldComponent.Fields)
                {
                    varsDic.Add(field.Name, field.Value);
                }
            }

            return varsDic;
        }
    }
}
