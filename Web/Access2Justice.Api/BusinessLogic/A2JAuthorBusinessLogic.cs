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
                    Help = pageProperties.GetValue("help"),
                    Learn = pageProperties.GetValue("learn"),
                    Text = pageProperties.GetValue("text"),
                    Fields = componentFields,
                    Buttons = componentButtons,
                    Code = componentCodes
                });
            }

            // Todo: we should figure a way to do upsert, we currently can't do that because we don't have an identifier 
            dbService.CreateItemAsync(cx, dbSettings.CuratedExperienceCollectionId);
            dbService.CreateItemAsync(resource, dbSettings.ResourceCollectionId);

            return cx;
        }

        private Resource MapResourceProperties(IEnumerable<JProperty> a2jProperties, Guid curatedExperienceId)
        {
            return new Resource
            {
                ResourceId = Guid.NewGuid(),
                ExternalUrls = curatedExperienceId.ToString(),
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
                    Label = field.GetValue("label"),
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
                    Label = button.GetValue("label"),
                    Destination = button.GetValue("next"),
                    Name = button.GetValue("name"),
                    Value = button.GetValue("value")
                });
            }

            return componentButtons;
        }

        private Code GetCodes(IEnumerable<JProperty> pageProperties)
        {
            return new Code
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
                    return CuratedExperienceQuestionType.Text;
                case "TEXTLONG":
                    return CuratedExperienceQuestionType.RichText;
                case "TEXTPICK":
                    return CuratedExperienceQuestionType.List;
                case "NUMBER":
                    return CuratedExperienceQuestionType.Number;
                case "NUMBERDOLLAR":
                    return CuratedExperienceQuestionType.Currency;
                case "NUMBERSSN":
                    return CuratedExperienceQuestionType.Ssn;
                case "NUMBERPHONE":
                    return CuratedExperienceQuestionType.Phone;
                case "NUMBERZIP":
                    return CuratedExperienceQuestionType.ZipCode;
                case "NUMBERPICK":
                    return CuratedExperienceQuestionType.List;
                case "GENDER":
                    return CuratedExperienceQuestionType.RadioButton;
                case "RADIOBUTTON":
                    return CuratedExperienceQuestionType.RadioButton;
                case "CHECKBOX":
                    return CuratedExperienceQuestionType.CheckBox;
                case "CHECKBOXNOTA":
                    return CuratedExperienceQuestionType.CheckBox;
                default:
                    return CuratedExperienceQuestionType.Text;
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
