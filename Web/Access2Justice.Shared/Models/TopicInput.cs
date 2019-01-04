using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Access2Justice.Shared.Models
{
    public class TopicInput
    {
        [JsonProperty(PropertyName = "location")]
        public Location Location { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "isShared")]
        public bool IsShared { get; set; } = false;
    }

    public class IntentInput
    {
        [JsonProperty(PropertyName = "location")]
        public Location Location { get; set; }

        [JsonProperty(PropertyName = "intents")]
        public List<string> Intents { get; set; }
        public IntentInput()
        {
            Intents = new List<string>();
        }
    }
}