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
    public class HelpAndFaqsContent
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "id")]
        public string Name { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "image")]
        public Image ImageUrl { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "details")]
        public IEnumerable<ContentLink> Link { get; set; }
    }

    public class ContentLink
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "order")]
        public int Order { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "urltext")]
        public string UrlsText { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "urlLink")]
        public string UrlsLink { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "description")]
        public IEnumerable<Paragraph> Description { get; set; }
    }

    public class Paragraph
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "paragraph")]
        public string Paragraphs { get; set; }
    }
}