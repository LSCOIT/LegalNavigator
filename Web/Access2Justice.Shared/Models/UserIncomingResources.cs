using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Access2Justice.Shared.Models
{
    public class UserIncomingResources
    {
        [JsonProperty(PropertyName = "id")]
        public Guid IncomingResourcesId { get; set; }

        [JsonProperty(PropertyName = "resources")]
        public List<IncomingResource> Resources { get; set; }

        public UserIncomingResources()
        {
            Resources = new List<IncomingResource>();   
        }
    }

    public class IncomingResource
    {
        [JsonProperty(PropertyName = "itemId")]
        public string ResourceId { get; set; }

        [JsonProperty(PropertyName = "resourceType")]
        public string ResourceType { get; set; }

        [JsonProperty(PropertyName = "sharedBy")]
        public string SharedBy { get; set; }

        [JsonProperty(PropertyName = "resourceDetails")]
        public dynamic ResourceDetails { get; set; }

        [JsonProperty(PropertyName = "sharedFromResourceId")]
        public Guid SharedFromResourceId { get; set; }

        [JsonProperty(PropertyName = "plan")]
        public Plan Plan { get; set; }

        [JsonProperty(PropertyName = "topicId")]
        public string TopicId { get; set; }
    }

    public class Plan
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "topicIds")]
        public List<string> TopicIds { get; set; }
    }

    public class SharedPlan
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "topicIds")]
        public List<TopicId> TopicIds { get; set; }
    }

    public class TopicId
    {
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        [JsonProperty(PropertyName = "isShared")]
        public bool IsShared { get; set; }
    }

    public class ProfileIncomingResources
    {
        [JsonProperty(PropertyName = "oId")]
        public string OId { get; set; }

        [JsonProperty(PropertyName = "resourceTags")]
        public List<IncomingResource> Resources { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        public ProfileIncomingResources()
        {
            Resources = new List<IncomingResource>();
        }
    }
}
