using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Access2Justice.Api.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class PersonalizedPlanViewModel
    {
        [JsonProperty(PropertyName = "id")]
        public Guid PersonalizedPlanId { get; set; }

        [JsonProperty(PropertyName = "topics")]
        public List<PlanTopic> Topics { get; set; }

        [JsonProperty(PropertyName = "isShared")]
        public bool IsShared { get; set; }

        public PersonalizedPlanViewModel()
        {
            Topics = new List<PlanTopic>();
        }
    }

    [ExcludeFromCodeCoverage]
    public class PlanTopic
    {
        [JsonProperty(PropertyName = "topicId")]
        public Guid TopicId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string TopicName { get; set; }

        [JsonProperty(PropertyName = "additionalReadings")]
        public List<AdditionalReadings> AdditionalReadings { get; set; }

        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }

        [JsonProperty(PropertyName = "steps")]
        public List<PlanStep> Steps { get; set; }

        public PlanTopic()
        {
            AdditionalReadings = new List<AdditionalReadings>();
            Steps = new List<PlanStep>();
        }
    }

    [ExcludeFromCodeCoverage]
    public class AdditionalReadings
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class PlanStep
    {
        [JsonProperty(PropertyName = "stepId")]
        public Guid StepId { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "order")]
        public int Order { get; set; }

        [JsonProperty(PropertyName = "isComplete")]
        public bool IsComplete { get; set; }

        [JsonProperty(PropertyName = "resources")]
        public List<Resource> Resources { get; set; }

        public PlanStep()
        {
            Resources = new List<Resource>();
        }
    }

    [ExcludeFromCodeCoverage]
    public class TopicDetails
    {
        [JsonProperty(PropertyName = "id")]
        public Guid TopicId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string TopicName { get; set; } 

        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }

        [JsonProperty(PropertyName = "quickLinks")]
        public List<AdditionalReadings> QuickLinks { get; set; }
        public TopicDetails()
        {
            QuickLinks = new List<AdditionalReadings>();
        }
    }

    [ExcludeFromCodeCoverage]
    public class UserPlan
	{
		[JsonProperty(PropertyName = "personalizedPlan")]
		public PersonalizedPlanViewModel PersonalizedPlan { get; set; }

		[JsonProperty(PropertyName = "oId")]
		public string UserId { get; set; }


        [JsonProperty(PropertyName = "saveActionPlan")]
        public bool saveActionPlan { get; set; } = false;
    }
}