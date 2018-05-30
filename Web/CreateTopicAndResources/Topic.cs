﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CreateTopicAndResources
{
    public class Topics : Topic_BL
    {
        public string Id { get; set; }
        public List<Topic> TopicsList { get; set; }
        public List<ParentTopic> ParentTopicList { get; set; }
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
        public ParentTopicID[] ParentTopicID { get; set; }

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
        public string CreatedTimeStamp { get; set; }

        [JsonProperty(PropertyName = "modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty(PropertyName = "modifiedTimeStamp")]
        public string ModifiedTimeStamp { get; set; }
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
