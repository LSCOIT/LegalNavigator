using Newtonsoft.Json;
using System.Collections.Generic;

namespace Access2Justice.Api.Models.CuratedExperience
{
    public class CuratedExperience
    {
        public CuratedExperience()
        {
            SurvayTree = new List<SurvayTree>();
        }

        [JsonProperty(PropertyName = "id")]
        public string CuratedExperienceId { get; set; }
        public string Name { get; set; }
        public List<SurvayTree> SurvayTree { get; set; }
    }

    public class SurvayTree
    {
        [JsonProperty(PropertyName = "id")]
        public string ChoiceSetId { get; set; }
        public string Description { get; set; }
        public string ParentId { get; set; }
        public List<string> Childern { get; set; }
    }

}
