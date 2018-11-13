using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Access2Justice.Api.ViewModels
{
    public class CuratedExperiencePackageTemplateModel
    {
        [JsonProperty(PropertyName = "templateName")]
        public string TemplateName { get; set; }

        [JsonProperty(PropertyName = "template")]
        public JObject Template { get; set; }
    }
}
