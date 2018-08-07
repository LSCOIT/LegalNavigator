using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Access2Justice.Api.ViewModels
{
    public class ShareViewModel
    {
        [JsonProperty(PropertyName = "permaLink")]
        public string PermaLink { get; set; }
    }
}
