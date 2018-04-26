using Newtonsoft.Json;
using System.Collections.Generic;

namespace Access2Justice.Api.Models.CuratedExperience
{
    public class CuratedExperience
    {
        [JsonProperty(PropertyName = "id")]
        public string CuratedExperienceId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "survayTree")]
        public List<SurvayTree> SurvayTree { get; set; }

        public CuratedExperience()
        {
            SurvayTree = new List<SurvayTree>();
        }
    }

    public class SurvayTree
    {
        [JsonProperty(PropertyName = "id")]
        public string SurvayItemId { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "parentId")]
        public string ParentId { get; set; }
        [JsonProperty(PropertyName = "questionType")]
        public string QuestionType { get; set; }
        [JsonProperty(PropertyName = "questions")]
        public List<Questoin> Questoins { get; set; }

        public SurvayTree(List<Questoin> questoins)
        {
            Questoins = questoins;
        }
    }


    public class Questoin
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }
}
