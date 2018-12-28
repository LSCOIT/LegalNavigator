using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using Access2Justice.Shared;
using System.Diagnostics.CodeAnalysis;

namespace Access2Justice.Api.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class PagedResourceViewModel
    {
        [JsonProperty(PropertyName = "resources")]
        public dynamic Resources { get; set; }

        [JsonProperty(PropertyName = "continuationToken")]
        public dynamic ContinuationToken { get; set; }

        [JsonProperty(PropertyName = "resourceTypeFilter")]
        public dynamic ResourceTypeFilter { get; set; }

        [JsonProperty(PropertyName = "topicIds")]
        public dynamic TopicIds { get; set; }

        [JsonProperty(PropertyName = "searchFilter")]
        public dynamic SearchFilter { get; set; }

        public PagedResourceViewModel()
        {
            Resources = JsonConvert.DeserializeObject(Constants.EmptyArray);
            TopicIds = JsonConvert.DeserializeObject(Constants.EmptyArray);
            ResourceTypeFilter = JsonConvert.DeserializeObject(Constants.EmptyArray);
            ContinuationToken = JsonConvert.DeserializeObject(Constants.EmptyArray);
        }
    }
}
