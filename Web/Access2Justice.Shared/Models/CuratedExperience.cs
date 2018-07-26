using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Access2Justice.Shared.Models
{
    public class CuratedExperience
    {
        [JsonProperty(PropertyName = "id")]
        public Guid CuratedExperienceId { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "components")]
        public List<CuratedExperienceComponent> Components { get; set; }

        public CuratedExperience()
        {
            Components = new List<CuratedExperienceComponent>();
        }
    }

    public class CuratedExperienceComponent
    {
        [JsonProperty(PropertyName = "componentId")]
        public Guid ComponentId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "learn")]
        public string Learn { get; set; }

        [JsonProperty(PropertyName = "help")]
        public string Help { get; set; }

        [JsonProperty(propertyName: "tags")]
        public List<string> Tags { get; set; }

        [JsonProperty(PropertyName = "buttons")]
        public List<Button> Buttons { get; set; }

        [JsonProperty(PropertyName = "fields")]
        public List<Field> Fields { get; set; }

        public CuratedExperienceComponent()
        {
            Tags = new List<string>();
            Buttons = new List<Button>();
            Fields = new List<Field>();
        }
    }

    public class Button
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        [JsonProperty(PropertyName = "destination")]
        public string Destination { get; set; }

        [JsonProperty(PropertyName = "stepTitle")]
        public string StepTitle { get; set; }

        [JsonProperty(PropertyName = "stepDescription")]
        public string StepDescription { get; set; }

        [JsonProperty(PropertyName = "resourceIds")]
        public List<Guid> ResourceIds { get; set; }

        [JsonProperty(PropertyName = "topicIds")]
        public List<Guid> TopicIds { get; set; }

        public Button()
        {
            ResourceIds = new List<Guid>();
            TopicIds = new List<Guid>();
        }
    }

    public class Field
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        [JsonProperty(PropertyName = "isRequired")]
        public bool IsRequired { get; set; }

        [JsonProperty(PropertyName = "min")]
        public string MinLength { get; set; }

        [JsonProperty(PropertyName = "max")]
        public string MaxLength { get; set; }

        [JsonProperty(PropertyName = "invalidPrompt")]
        public string InvalidPrompt { get; set; }

        [JsonProperty(PropertyName = "stepTitle")]
        public string StepTitle { get; set; }

        [JsonProperty(PropertyName = "stepDescription")]
        public string StepDescription { get; set; }

        [JsonProperty(PropertyName = "resourceIds")]
        public List<Guid> ResourceIds { get; set; }

        [JsonProperty(PropertyName = "topicIds")]
        public List<Guid> TopicIds { get; set; }

        public Field()
        {
            ResourceIds = new List<Guid>();
            TopicIds = new List<Guid>();
        }
    }
}
