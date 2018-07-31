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
        [JsonProperty(PropertyName = "id")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "hero")]
        public HeroContent Hero { get; set; }

        [JsonProperty(PropertyName = "guidedAssistant")]
        public GuidedAssistantContent GuidedAssistant { get; set; }

        [JsonProperty(PropertyName = "topicAndResources")]
        public TopicAndResourcesContent TopicAndResources { get; set; }

        [JsonProperty(PropertyName = "carousel")]
        public CarouselContent Carousel { get; set; }

        [JsonProperty(PropertyName = "information")]
        public InformationContent Information { get; set; }

        [JsonProperty(PropertyName = "privacy")]
        public PrivacyContent Privacy { get; set; }
    }

    public class HeroContent
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "image")]
        public Image ImageUrl { get; set; }
    }

    public class GuidedAssistantContent
    {
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

    public class CarouselContent
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "overview")]
        public IEnumerable<Overview> Overviewdetails { get; set; }        
    }

    public class InformationContent
    {
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

    public class Image
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "id")]
        public dynamic Id { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; }
    }

    public class Step
    {
        [JsonProperty(PropertyName = "order")]
        public int Order { get; set; }

        [JsonProperty(PropertyName = "descritpion")]
        public int Descritpion { get; set; }
    }
    
    public class Overview
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "html")]
        public string Description { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "image")]
        public Image ImageUrl { get; set; }
    }
}