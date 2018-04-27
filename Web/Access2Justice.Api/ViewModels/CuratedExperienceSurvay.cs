using Newtonsoft.Json;
using System.Collections.Generic;

namespace Access2Justice.Api.ViewModels
{
    public class CuratedExperienceSurvey
    {
        // todo:@alaa turn IDs into Guids
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Title { get; set; }
        public string QuestionType { get; set; }
        public string UserAnswer { get; set; }
        public List<Choice> Choices { get; set; }

        public CuratedExperienceSurvey()
        {
            Choices = new List<Choice>();
        }
    }

    public class Choice
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string ChoiceText { get; set; }
    }

}