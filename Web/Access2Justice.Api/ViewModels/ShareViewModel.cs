using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;
using Access2Justice.Shared.Models;

namespace Access2Justice.Api.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class ShareViewModel
    {
        [JsonProperty(PropertyName = "permaLink")]
        public string PermaLink { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ShareProfileViewModel
    {
        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }
        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }
        [JsonProperty(PropertyName = "resourceLink")]
        public string ResourceLink { get; set; }
        [JsonProperty(PropertyName = "location")]
        public Location Location { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ShareProfileDetails
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Link { get; set; }

        [JsonProperty(PropertyName = "location")]
        public Location Location { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ShareProfileResponse
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FistName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "oId")]
        public string OId { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Link { get; set; }
    }
}
