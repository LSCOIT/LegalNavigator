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
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "image")]
        public Image Image { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "imageExpand")]
        public Image ImageExpand { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "imageCollapse")]
        public Image ImageCollapse { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "faqs")]
        public IEnumerable<HelpDetails> HelpDetails { get; set; }
    }

    public class HelpDetails
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "question")]
        public string UrlsText { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "answer")]
        public IEnumerable<Paragraph> Description { get; set; }
    }

    public class Paragraph
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }
}