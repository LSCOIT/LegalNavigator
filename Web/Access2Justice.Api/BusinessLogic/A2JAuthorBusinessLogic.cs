using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Access2Justice.Api.BusinessLogic
{
    public class A2JAuthorBusinessLogic : IA2JAuthorBusinessLogic
    {
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
                    Fields = componentFields,
                    Buttons = componentButtons
                });
            }

            // Todo:@Alaa persist the curated experience and the resource
            // save curated experience
            // save resources

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
                    Destination = button.GetValue("next")
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
