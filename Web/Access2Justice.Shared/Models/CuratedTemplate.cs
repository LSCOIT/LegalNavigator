using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Access2Justice.Shared
{
    public class CuratedTemplate
    {
        [JsonProperty(PropertyName = "subjectarea")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "templateFile")]
        public List<IFormFile> TemplateFile { get; set; }
    }
}
