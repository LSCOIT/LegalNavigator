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
    public class Header
    {
        [JsonProperty(PropertyName = "id")]
        public string Name { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "language")]
        public Language Language { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "location")]
        public HeaderLocation Location { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "privacyPromise")]
        public ButtonImage PrivacyPromise { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "helpAndFAQ")]
        public ButtonImage HelpAndFAQ { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "login")]
        public ButtonImage Login { get; set; }
    }
}