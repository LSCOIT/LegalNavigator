using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Access2Justice.Api.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class CuratedExperiencePackageTemplateModel
    {
        [JsonProperty(PropertyName = "templateName")]
        public string TemplateName { get; set; }

        [JsonProperty(PropertyName = "template")]
        public JObject Template { get; set; }
    }
}
