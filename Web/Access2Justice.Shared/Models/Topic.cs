using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Access2Justice.Shared.Models
{
    public class Topic : Resource
    {        
        [JsonProperty(PropertyName = "id")]
        public dynamic Id { get; set; }

        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

        [JsonProperty(PropertyName = "parentTopicId")]
        public IEnumerable<ParentTopicId> ParentTopicId { get; set; }

        [JsonProperty(PropertyName = "keywords")]
        public string Keywords { get; set; }

        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }
    }

    public class ParentTopicId
    {
        [JsonProperty(PropertyName = "id")]
        public dynamic ParentTopicIds { get; set; }
    }

    public class PrintableTopicView
    {
        public Topic Topic { get; set; }
        public List<Organization> Organizations { get; set; }
        public List<ActionPlan> ActionPlans { get; set; }
        public List<Article> Articles { get; set; }
        public List<Video> Videos { get; set; }
        public List<Form> Forms { get; set; }
        public List<RelatedLink> RelatedLinks { get; set; }
        public List<AdditionalReading> AdditionalReadings { get; set; }
    }
}