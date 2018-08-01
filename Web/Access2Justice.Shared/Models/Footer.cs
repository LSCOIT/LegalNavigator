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
    public class Footer
    {
        [JsonProperty(PropertyName = "id")]
        public string Name { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "home")]
        public ButtonStaticContent Home { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "guidedAssistant")]
        public ButtonStaticContent GuidedAssistant { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "topicAndResources")]
        public ButtonStaticContent TopicAndResources { get; set; }
        
        [DefaultValue("")]
        [JsonProperty(PropertyName = "about")]
        public ButtonStaticContent About { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "privacyPromise")]
        public ButtonStaticContent PrivacyPromise { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "helpAndFAQ")]
        public ButtonStaticContent HelpAndFAQ { get; set; }
    }
}