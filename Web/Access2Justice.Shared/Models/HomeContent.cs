using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Globalization;
using System.Text;
using System.ComponentModel;

namespace Access2Justice.Shared.Models
{
    public class HomeContent
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "hero")]
        public HeroContent Hero { get; set; }

        [JsonProperty(PropertyName = "guidedAssistant")]
        public GuidedAssistantContent GuidedAssistant { get; set; }

        [JsonProperty(PropertyName = "topicAndResources")]
        public TopicAndResourcesContent TopicAndResources { get; set; }

        [JsonProperty(PropertyName = "carousel")]
        public IEnumerable<CarouselContent> Carousel { get; set; }

        [JsonProperty(PropertyName = "information")]
        public InformationContent Information { get; set; }

        [JsonProperty(PropertyName = "privacy")]
        public PrivacyContent Privacy { get; set; }
    }

    public class HeroContent
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "routerText")]
        public string RouterText { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "routerLink")]
        public string RouterLink { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "image")]
        public Image ImageUrl { get; set; }
    }

    public class GuidedAssistantContent
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "buttonText")]
        public string ButtonText { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "buttonLink")]
        public string ButtonLink { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "image")]
        public Image ImageUrl { get; set; }
    }

    public class TopicAndResourcesContent
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "buttonText")]
        public string ButtonText { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "buttonLink")]
        public string ButtonLink { get; set; }
    }

    public class CarouselContent  //Need to check feasibility for html
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "byName")]
        public string ByName { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "buttonText")]
        public string ButtonText { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "buttonLink")]
        public string ButtonLink { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "image")]
        public Image ImageUrl { get; set; }
    }

    public class InformationContent
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "buttonText")]
        public string ButtonText { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "buttonLink")]
        public string ButtonLink { get; set; }
    }

    public class PrivacyContent
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "buttonText")]
        public string ButtonText { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "buttonLink")]
        public string ButtonLink { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "image")]
        public Image ImageUrl { get; set; }
    }

    public class Image
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "id")]
        public dynamic Id { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; }
    }
}