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
    public class HelpAndFaqsContent: NameLocation
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "image")]
        public Image Image { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "faqs")]
        public IEnumerable<HelpDetails> HelpDetails { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "organizationalUnit")]
        public string OrganizationalUnit { get; set; }
    }

    public class HelpDetails
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "question")]
        public string Question { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "answer")]
        public IEnumerable<Paragraph> Answer { get; set; }
    }

    public class Paragraph
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }
}