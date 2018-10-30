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
        [Required(ErrorMessage = "Name is a required field.")]
        [JsonProperty(PropertyName = "subjectarea")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is a required field.")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Template file is a required field.")]
        [JsonProperty(PropertyName = "templateFile")]
        public List<IFormFile> TemplateFile { get; set; }
    }
}
