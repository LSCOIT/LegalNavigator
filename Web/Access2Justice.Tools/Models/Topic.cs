using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Tools.Models
{
    public class Topics
    {
        public string Id { get; set; }
        public IEnumerable<Topic> TopicsList { get; set; }
        public IEnumerable<ParentTopic> ParentTopicList { get; set; }
    }

    public class ParentTopic
    {
        public string DummyId { get; set; }
        public Guid NewId { get; set; }
    }

    public class Topic
    {
        [JsonProperty(PropertyName = "id")]
        public dynamic Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "parentTopicID")]
        public IEnumerable<ParentTopicID> ParentTopicID { get; set; }

        [JsonProperty(PropertyName = "keywords")]
        public string Keywords { get; set; }

        [JsonProperty(PropertyName = "jsonContent")]
        public string JsonContent { get; set; }

        [JsonProperty(PropertyName = "location")]
        public IEnumerable<Location> Location { get; set; }

        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty(PropertyName = "createdTimeStamp")]
        public DateTime? CreatedTimeStamp { get; set; } = DateTime.UtcNow;

        [JsonProperty(PropertyName = "modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty(PropertyName = "modifiedTimeStamp")]
        public DateTime? ModifiedTimeStamp { get; set; } = DateTime.UtcNow;
    }

    public class ParentTopicID
    {
        [JsonProperty(PropertyName = "id")]
        public dynamic ParentTopicId { get; set; }
    }

    public class Location
    {
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "county")]
        public string County { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "zipCode")]
        public string ZipCode { get; set; }
    }
}