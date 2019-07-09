using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Access2Justice.Shared.Models
{
    public class CuratedExperience
    {
        public CuratedExperience()
        {
            Components = new List<CuratedExperienceComponent>();
        }

        [JsonProperty(PropertyName = "id")]
        public Guid CuratedExperienceId { get; set; }

        [JsonProperty(PropertyName = "a2jPersonalizedPlanId")]
        public Guid A2jPersonalizedPlanId { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "components")]
        public List<CuratedExperienceComponent> Components { get; set; }

        [JsonProperty("nsmiCode")]
        public string NsmiCode { get; set; }
    }

    public class CuratedExperienceComponent
    {
        public CuratedExperienceComponent()
        {
            Tags = new List<string>();
            Fields = new List<Field>();
            Buttons = new List<Button>();
            Code = new PersonalizedPlanEvaluator();
        }

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

        [JsonProperty(PropertyName = "fields")]
        public List<Field> Fields { get; set; }

        [JsonProperty(PropertyName = "buttons")]
        public List<Button> Buttons { get; set; }

        [JsonProperty(PropertyName = "code")]
        public PersonalizedPlanEvaluator Code { get; set; }
    }

    public class Field
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        [JsonProperty(PropertyName = "isRequired")]
        public bool IsRequired { get; set; }

        [JsonProperty(PropertyName = "min")]
        public string MinLength { get; set; }

        [JsonProperty(PropertyName = "max")]
        public string MaxLength { get; set; }

        [JsonProperty(PropertyName = "invalidPrompt")]
        public string InvalidPrompt { get; set; }
    }

    public class Button
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        [JsonProperty(PropertyName = "destination")]
        public string Destination { get; set; }
    }

    public class PersonalizedPlanEvaluator
    {
        [JsonProperty(PropertyName = "codeBefore")]
        public string CodeBefore { get; set; }

        [JsonProperty(PropertyName = "codeAfter")]
        public string CodeAfter { get; set; }
    }
}