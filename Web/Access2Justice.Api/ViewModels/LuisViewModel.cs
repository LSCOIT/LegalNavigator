﻿using System.Collections.Generic;
using Access2Justice.Shared;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Access2Justice.Api.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class LuisViewModel
    {
        [JsonProperty(PropertyName = "topIntent")]
        public string TopIntent { get; set; }

        [JsonProperty(PropertyName = "relevantIntents")]
        public dynamic RelevantIntents { get; set; }

        [JsonProperty(PropertyName = "topics")]
        public dynamic Topics { get; set; }

        [JsonProperty(PropertyName = "resources")]
        public dynamic Resources { get; set; }

        [JsonProperty(PropertyName = "continuationToken")]
        public dynamic ContinuationToken { get; set; }

        [JsonProperty(PropertyName = "topicIds")]
        public dynamic TopicIds { get; set; }

        [JsonProperty(PropertyName = "resourceTypeFilter")]
        public dynamic ResourceTypeFilter { get; set; }

        [JsonProperty(PropertyName = "curatedExperiences")]
        public dynamic CuratedExperiences { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "guidedAssistants")]
        public IEnumerable<dynamic> GuidedAssistants { get; set; }

        [JsonProperty(PropertyName = "searchFilter")]
        public dynamic SearchFilter { get; set; }

        public LuisViewModel()
        {
            RelevantIntents = JsonConvert.DeserializeObject(Constants.EmptyArray);
            Topics = JsonConvert.DeserializeObject(Constants.EmptyArray);
            TopicIds = JsonConvert.DeserializeObject(Constants.EmptyArray);
            ResourceTypeFilter = JsonConvert.DeserializeObject(Constants.EmptyArray);
            ContinuationToken = JsonConvert.DeserializeObject(Constants.EmptyArray);
            Resources = JsonConvert.DeserializeObject(Constants.EmptyArray);
        }
    }
}
