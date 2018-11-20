﻿using Access2Justice.Shared.Models;
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
            IsShared = true; ;
        }
    }

    [ExcludeFromCodeCoverage]
    public class PlanTopic
    {
        [JsonProperty(PropertyName = "topicId")]
        public Guid TopicId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string TopicName { get; set; }

        [JsonProperty(PropertyName = "essentialReadings")]
        public List<EssentialReadings> EssentialReadings { get; set; }

        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }

        [JsonProperty(PropertyName = "steps")]
        public List<PlanStep> Steps { get; set; }

        public PlanTopic()
        {
            EssentialReadings = new List<EssentialReadings>();
            Steps = new List<PlanStep>();
        }
    }

    [ExcludeFromCodeCoverage]
    public class EssentialReadings
    {
        public string Text { get; set; }
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
        public List<EssentialReadings> QuickLinks { get; set; }
        public TopicDetails()
        {
            QuickLinks = new List<EssentialReadings>();
        }
    }
}