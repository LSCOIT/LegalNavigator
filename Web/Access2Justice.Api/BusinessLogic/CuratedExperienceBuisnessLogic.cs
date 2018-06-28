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
    public class CuratedExperienceBuisnessLogic : ICuratedExperienceBuisnessLogic
    {
        public CuratedExperience ConvertA2JAuthorToCuratedExperience(JObject a2jSchema)
        {
            var cx = new CuratedExperience();
            var a2jProperties = (a2jSchema).Properties();

            cx.CuratedExperienceId = Guid.NewGuid();
            cx.Version = a2jProperties.GetValue("version");
            cx.Title = a2jProperties.GetValue("title");
            cx.SubjectAreas.Add(a2jProperties.GetValue("subjectarea"));
            cx.Description = a2jProperties.GetValue("description");
            cx.Authors = GetAuthors(a2jProperties);

            var pages = ((JObject)a2jProperties.Where(x => x.Name == "pages").FirstOrDefault()?.Value).Properties();
            foreach (var page in pages)
            {
                var pageProperties = ((JObject)page.FirstOrDefault()).Properties();
                var componentFields = GetFields(pageProperties);
                var componentButtons = GetButtons(pageProperties);

                cx.Components.Add(new Component
                {
                    Id = Guid.NewGuid(),
                    Name = pageProperties.GetValue("name"),
                    Help = pageProperties.GetValue("help"),
                    Learn = pageProperties.GetValue("learn"),
                    Text = pageProperties.GetValue("text"),
                    Fields = componentFields,
                    Buttons = componentButtons
                });
            }

            return cx;
        }

        private List<Author> GetAuthors(IEnumerable<JProperty> rootProperties)
        {
            var authors = new List<Author>();
            var a2jAuthors = rootProperties.GetArrayValue("authors");
            foreach (var author in a2jAuthors.FirstOrDefault())
            {
                var authorProperties = ((JObject)author).Properties();

                authors.Add(new Author
                {
                    Name = authorProperties.GetValue("name"),
                    Title = authorProperties.GetValue("title"),
                    Organization = authorProperties.GetValue("organization"),
                    Email = authorProperties.GetValue("email")
                });
            }

            return authors;
        }

        private List<Field> GetFields(IEnumerable<JProperty> pageProperties)
        {
            var componentFields = new List<Field>();
            var fieldsProperties = pageProperties.GetArrayValue("fields").FirstOrDefault().ToList();

            foreach (var buttonProperty in fieldsProperties)
            {
                var field = ((JObject)buttonProperty).Properties();
                componentFields.Add(new Field
                {
                    Id = Guid.NewGuid(),
                    Type = field.GetValue("type"),
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
    }
}
