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
    public class PrivacyPromiseContent: NameLocation
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "image")]
        public Image Image { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "details")]
        public IEnumerable<Detail> Details { get; set; }
    }

    public class Detail
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }
}