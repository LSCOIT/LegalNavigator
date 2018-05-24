using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Shared.Models
{
    public class TopicModel 
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        [JsonIgnore]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }
        [JsonIgnore]
     
        [JsonProperty(PropertyName = "parentTopicID")]
        public string ParentTopicID { get; set; }
        [JsonIgnore]
        [JsonProperty(PropertyName = "keywords")]
        public string Keywords { get; set; }
        [JsonIgnore]
        [JsonProperty(PropertyName = "notForLocation")]
        public string NotForLocation { get; set; }
        [JsonIgnore]
        [JsonProperty(PropertyName = "jsonContent")]
        public string JsonContent { get; set; }

    }

    public class Resource : CommonFields
    {
        [JsonProperty(PropertyName = "id")]
        public Guid ResourceId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "resourceType")]
        public string ResourceType { get; set; }

        [JsonProperty(PropertyName = "externalUrl")]
        public Uri ExternalUrl { get; set; }

        [JsonProperty(PropertyName = "url")]
        public Uri Url { get; set; }

        [JsonProperty(PropertyName = "topicTags")]
        public System.Guid TopicTags { get; set; }

        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }

        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }

    }
    public class CommonFields
    {
        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty(PropertyName = "createdTimeStamp")]
        public string CreatedTimeStamp { get; set; }

        [JsonProperty(PropertyName = "modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty(PropertyName = "modifiedTimeStamp")]
        public string ModifiedTimeStamp { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "_etag")]
        public string ETag { get; }
    }

}
