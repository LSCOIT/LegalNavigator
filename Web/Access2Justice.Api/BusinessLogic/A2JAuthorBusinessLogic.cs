using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.BusinessLogic
{
    public class A2JAuthorBusinessLogic : IA2JAuthorBusinessLogic
    {
        private readonly ICosmosDbSettings dbSettings;
        private readonly IBackendDatabaseService dbService;

        public A2JAuthorBusinessLogic(ICosmosDbSettings cosmosDbSettings, IBackendDatabaseService backendDatabaseService)
        {
            dbSettings = cosmosDbSettings;
            dbService = backendDatabaseService;
        }

        public CuratedExperience ConvertA2JAuthorToCuratedExperience(JObject a2jSchema)
        {
            var cx = new CuratedExperience();
            var a2jProperties = (a2jSchema).Properties();

            cx.CuratedExperienceId = Guid.NewGuid();
            cx.Title = a2jProperties.GetValue("title");
            var resource = MapResourceProperties(a2jProperties, cx.CuratedExperienceId);

            var pages = ((JObject)a2jProperties.Where(x => x.Name == "pages").FirstOrDefault()?.Value).Properties();
            foreach (var page in pages)
            {
                var pageProperties = ((JObject)page.FirstOrDefault()).Properties();
                var componentFields = GetFields(pageProperties);
                var componentButtons = GetButtons(pageProperties);

                cx.Components.Add(new CuratedExperienceComponent
                {
                    ComponentId = Guid.NewGuid(),
                    Name = pageProperties.GetValue("name"),
                    Help = pageProperties.GetValue("help"),
                    Learn = pageProperties.GetValue("learn"),
                    Text = pageProperties.GetValue("text"),
                    CodeBefore = pageProperties.GetValue("codeBefore"),
                    CodeAfter = pageProperties.GetValue("codeAfter"),
                    Fields = componentFields,
                    Buttons = componentButtons
                });
            }

            // Todo: we should figure a way to do upsert, we currently can't do that because we don't have an identifier 
            dbService.CreateItemAsync(cx, dbSettings.CuratedExperienceCollectionId);
            dbService.CreateItemAsync(resource, dbSettings.ResourceCollectionId);

            return cx;
        }

        // Todo:@Alaa remove - returning full peronalized plan for demo and testing
        public async Task<A2JPersonalizedPlan> GetA2JPersonalizedPlanStepsAsync()
        {
            return await dbService.GetItemAsync<A2JPersonalizedPlan>("432e7473-02df-4807-8d45-39ed821c5eb1", dbSettings.A2JAuthorTemplatesCollectionId);
        }

        // Todo:@Alaa naming could be revisited
        public async Task<A2JPersonalizedPlan> ExtractA2JPersonalizedPlanStepsInScopeAsync(A2JPersonalizedPlan a2JPersonalizedPlan, 
            CuratedExperienceAnswers userAnswers)
        {

            var breakpoint = string.Empty; // Todo:@Alaa - remove this temp code
            throw new NotImplementedException();
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

        private CuratedExperienceQuestionType MapA2JFieldTypeToCuratedExperienceQuestionType(string a2jAuthorFieldtype)
        {
            switch (a2jAuthorFieldtype)
            {
                case "text":
                    return CuratedExperienceQuestionType.Text;
                case "textlong":
                    return CuratedExperienceQuestionType.RichText;
                case "textpick":
                    return CuratedExperienceQuestionType.List;
                case "number":
                    return CuratedExperienceQuestionType.Number;
                case "numberdollar":
                    return CuratedExperienceQuestionType.Currency;
                case "numberssn":
                    return CuratedExperienceQuestionType.Ssn;
                case "numberphone":
                    return CuratedExperienceQuestionType.Phone;
                case "numberzip":
                    return CuratedExperienceQuestionType.ZipCode;
                case "numberpick":
                    return CuratedExperienceQuestionType.List;
                case "gender":
                    return CuratedExperienceQuestionType.RadioButton;
                case "radio":
                    return CuratedExperienceQuestionType.RadioButton;
                case "checkbox":
                    return CuratedExperienceQuestionType.CheckBox;
                case "checkboxNOTA":
                    return CuratedExperienceQuestionType.CheckBox;
                default:
                    return CuratedExperienceQuestionType.Text;
            }
        }
    }
}
