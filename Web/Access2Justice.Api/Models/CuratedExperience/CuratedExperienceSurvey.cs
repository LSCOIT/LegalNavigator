using Newtonsoft.Json;
using System.Collections.Generic;

namespace Access2Justice.Api.Models.CuratedExperience
{
    public class CuratedExperienceSurvey
    {
        [JsonProperty(PropertyName = "id")]
        public string CuratedExperienceId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "surveyTree")]
        public List<SurveyTree> SurveyTree { get; }

        public CuratedExperienceSurvey()
        {
            SurveyTree = new List<SurveyTree>();
        }
    }

    public class SurveyTree
    {
        [JsonProperty(PropertyName = "id")]
        public string QuestionId { get; set; }
        [JsonProperty(PropertyName = "text")]
        public string QuestionText { get; set; }
        [JsonProperty(PropertyName = "parentId")]
        public string ParentId { get; set; }
        [JsonProperty(PropertyName = "questionType")]
        public string QuestionType { get; set; }
        [JsonProperty(PropertyName = "choices")]
        public List<Choice> Choices { get; }

        public SurveyTree(List<Choice> choices)
        {
            Choices = choices;
        }
    }

    public class Choice
    {
        [JsonProperty(PropertyName = "id")]
        public string ChoiceId { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string ChoiceText { get; set; }

        [JsonProperty(propertyName: "nextQuestion")]
        public string NextQuestion { get; set; }

        [JsonProperty(propertyName: "tags")]
        public List<string> Tags { get; set; }

    }
}
