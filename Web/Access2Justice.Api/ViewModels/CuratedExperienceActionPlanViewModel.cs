using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Access2Justice.Api.ViewModels
{
    public class PersonalizedActionPlanViewModel
    {
        [JsonProperty(PropertyName = "id")]
        public Guid PersonalizedPlanId { get; set; }

        [JsonProperty(PropertyName = "topics")]
        public List<PlanTopic> Topics { get; set; }

        public PersonalizedActionPlanViewModel()
        {
            Topics = new List<PlanTopic>();
        }
    }

    public class PlanTopic
    {
        [JsonProperty(PropertyName = "topicId")]
        public Guid TopicId { get; set; }

        public List<PlanQuickLink> QuickLinks { get; set; }

        [JsonProperty(PropertyName = "steps")]
        public List<PlanStep> Steps { get; set; }

        public PlanTopic()
        {
            QuickLinks = new List<PlanQuickLink>();
            Steps = new List<PlanStep>();
        }
    }

    public class PlanQuickLink
    {
        public string Title { get; set; }
        public Uri Url { get; set; }
    }

    public class PlanStep
    {
        [JsonProperty(PropertyName = "stepId")]
        public Guid StepId { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }  // Todo:@Alaa create an enum

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "order")]
        public int Order { get; set; }

        [JsonProperty(PropertyName = "markCompleted")]
        public bool IsComplete { get; set; }

        [JsonProperty(PropertyName = "resources")]
        public List<Guid> ResourcesIds { get; set; }

        public PlanStep()
        {
            ResourcesIds = new List<Guid>();
        }
    }
}