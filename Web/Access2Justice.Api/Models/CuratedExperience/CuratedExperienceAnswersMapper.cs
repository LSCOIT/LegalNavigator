using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Access2Justice.Api.Models.CuratedExperience
{
    public class CuratedExperienceAnswersMapper
    {
        [JsonProperty(PropertyName = "id")]
        public Guid CuratedExperienceId { get; set; }
        public Dictionary<Guid, string> Answers { get; set; }
    }
}
