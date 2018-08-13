﻿using Newtonsoft.Json;
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

    public class ShareProfileViewModel
    {
        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }
        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }
        [JsonProperty(PropertyName = "resourceLink")]
        public string ResourceLink { get; set; }
    }

    public class ShareProfileResponse
    {
        [JsonProperty(PropertyName = "firstName")]
        public string FistName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "oId")]
        public string OId { get; set; }

        [JsonProperty(PropertyName = "url")]
        public Uri Url { get; set; }
    }
}
