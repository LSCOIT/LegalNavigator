using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Access2Justice.Api.Models.CuratedExperience
{
    public class CuratedExperienceAnswers
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "curatedExperienceId")]
        public string CuratedExperienceId { get; set; }
        [JsonProperty(PropertyName = "answers")]
        public Dictionary<Guid, string> Answers { get; set; }

        public CuratedExperienceAnswers()
        {
            Answers = new Dictionary<Guid, string>();
        }
    }
}
