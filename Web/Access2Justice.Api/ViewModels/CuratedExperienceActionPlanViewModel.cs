﻿using Newtonsoft.Json;
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

        public PlanStep()
        {
            Resources = new List<PlanResource>();
        }
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
        public bool IsRecommended { get; set; }

        public PlanResource()
        {
            Tags = new List<string>();
            Location = new List<PlanLocation>();
        }
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