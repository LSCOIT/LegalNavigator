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

        [JsonProperty(PropertyName = "subjectAreas")]
        public IList<string> SubjectAreas { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }

        [JsonProperty(PropertyName = "components")]
        public IList<Component> Components { get; set; }

        public CuratedExperience()
        {
            Components = new List<Component>();
            SubjectAreas = new List<string>();
        }
    }

    public class Component
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "learn")]
        public string Learn { get; set; }

        [JsonProperty(PropertyName = "help")]
        public string Help { get; set; }

        //[JsonProperty(PropertyName = "parentId")]
        //public Guid ParentId { get; set; }

        [JsonProperty(propertyName: "tags")]
        public IList<string> Tags { get; set; }

        [JsonProperty(PropertyName = "buttons")]
        public IList<Button> Buttons { get; set; }

        [JsonProperty(PropertyName = "fields")]
        public IList<Field> Fields { get; set; }

        public Component()
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

        // alaa:todo maybe we could use a field like this to encampus logic
        // [JsonProperty(propertyName: "action")]
        // public string Action { get; set; }

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
    }
}
