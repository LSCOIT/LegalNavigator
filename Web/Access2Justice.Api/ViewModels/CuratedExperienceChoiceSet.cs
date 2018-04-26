using Newtonsoft.Json;
using System.Collections.Generic;

namespace Access2Justice.Api.ViewModels
{
    public class CuratedExperienceChoiceSet
    {
        // todo:@alaa turn IDs into Guids
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Title { get; set; }
        public string QuestionType { get; set; }
        public List<Choice> Choice { get; set; }

        public CuratedExperienceChoiceSet()
        {
            Choice = new List<Choice>();
        }
    }

    public class Choice
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string ChoiceText { get; set; }
    }

}