﻿using Microsoft.AspNetCore.JsonPatch.Operations;
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
    public class PrivacyPromiseContent
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "image")]
        public Image ImageUrl { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "details")]
        public Detail Details { get; set; }
    }

    public class Detail
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "order")]
        public int Order { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "image")]
        public Image ImageUrl { get; set; }
    }
}