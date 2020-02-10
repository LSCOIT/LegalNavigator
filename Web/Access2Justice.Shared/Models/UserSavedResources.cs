using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Access2Justice.Shared.Models
{
    public class UserSavedResources
    {
        [JsonProperty(PropertyName = "id")]
        public Guid SavedResourcesId { get; set; }

        [JsonProperty(PropertyName = "resources")]
        public List<SavedResource> Resources { get; set; }

        public UserSavedResources()
        {
            Resources = new List<SavedResource>();
        }
    }

    public class SavedResource
    {
        [JsonProperty(PropertyName = "itemId")]
        public string ResourceId { get; set; }

        [JsonProperty(PropertyName = "resourceType")]
        public string ResourceType { get; set; }

        [JsonProperty(PropertyName = "resourceDetails")]
        public dynamic ResourceDetails { get; set; }
    }

    public class ProfileResources
    {
        [JsonProperty(PropertyName = "oId")]
        public string OId { get; set; }

        [JsonProperty(PropertyName = "resourceTags")]
        public List<SavedResource> Resources { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        public ProfileResources()
        {
            Resources = new List<SavedResource>();
        }
    }

    public class UserProfileResource
    {
        [JsonProperty(PropertyName = "oId")]
        public string OId { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string ResourcesType { get; set; }

        [JsonProperty(PropertyName = "itemId")]
        public string ResourceId { get; set; }

        [JsonProperty(PropertyName = "resourceType")]
        public string ResourceType { get; set; }

        [JsonProperty(PropertyName = "sharedBy")]
        public string SharedBy { get; set; }

        [JsonProperty(PropertyName = "topicId")]
        public string TopicId { get; set; }
    }
}
