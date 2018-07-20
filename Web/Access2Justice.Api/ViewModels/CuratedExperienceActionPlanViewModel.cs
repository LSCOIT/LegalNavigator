using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Access2Justice.Api.ViewModels
{
    public class CuratedExperienceActionPlanViewModel
    {
        [JsonProperty(PropertyName = "id")]
        public Guid ActionPlanId { get; set; }

        [JsonProperty(PropertyName = "topics")]
        public List<PlanTopic> Topics { get; set; }
    }

    public class PlanTopic
    {
        [JsonProperty(PropertyName = "topicId")]
        public Guid TopicId { get; set; }

        [JsonProperty(PropertyName = "steps")]
        public List<Step> Steps { get; set; }
    }

    public class Step
    {
        [JsonProperty(PropertyName = "stepId")]
        public Guid StepId { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "order")]
        public int Order { get; set; }

        [JsonProperty(PropertyName = "markCompleted")]
        public bool IsComplete { get; set; }

        [JsonProperty(PropertyName = "resources")]
        public List<PlanResource> Resources { get; set; }
    }

    public class PlanResource
    {
        [JsonProperty(PropertyName = "id")]
        public Guid ResourceId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "resourceType")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "externalUrl")]
        public Uri ExternalUrl { get; set; }

        //[JsonProperty(PropertyName = "url")]
        //public Uri Url { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public List<string> Tags { get; set; }

        [JsonProperty(PropertyName = "location")]
        public List<PlanLocation> Location { get; set; }

        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }

        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

        [JsonProperty(PropertyName = "isRecommended")]
        public string IsRecommended { get; set; }
    }

    public class PlanLocation
    {
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "zipCode")]
        public string ZipCode { get; set; }
    }
}