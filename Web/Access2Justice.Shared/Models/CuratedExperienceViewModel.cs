using System;
using Newtonsoft.Json;

namespace Access2Justice.Shared.Models
{
    public class CuratedExperienceViewModel
    {
        [JsonProperty(PropertyName = "id")]
        public Guid CuratedExperienceId { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty("_ts")]
        public string ts { get; set; }

    }
}
