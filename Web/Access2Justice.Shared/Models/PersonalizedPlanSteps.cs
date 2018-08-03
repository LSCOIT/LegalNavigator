using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Access2Justice.Shared.Models
{
    public class PersonalizedPlanSteps
    {
        [JsonProperty(PropertyName = "id")]
        public Guid PersonalizedPlanId { get; set; }

        [JsonProperty(PropertyName = "topics")]
        public List<PersonalizedPlanTopic> Topics { get; set; }

        [JsonProperty(PropertyName = "isShared")]
        public bool IsShared { get; set; }

        public PersonalizedPlanSteps()
        {
            Topics = new List<PersonalizedPlanTopic>();
        }
    }

    public class PersonalizedPlanTopic
    {
        [JsonProperty(PropertyName = "topicId")]
        public Guid TopicId { get; set; }

        [JsonProperty(PropertyName = "steps")]
        public List<PersonalizedPlanStep> PlanSteps { get; set; }

        public PersonalizedPlanTopic()
        {
            PlanSteps = new List<PersonalizedPlanStep>();
        }
    }
    public class PersonalizedPlanStep
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
        public List<Guid> Resources { get; set; }

        [JsonIgnore]
        [JsonProperty(PropertyName = "topicIds")]
        public List<Guid> TopicIds { get; set; }

        public PersonalizedPlanStep()
        {
            Resources = new List<Guid>();
            TopicIds = new List<Guid>();
        }
    }
}
