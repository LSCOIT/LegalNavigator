﻿using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Access2Justice.Shared
{
    public class ShareInput
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public Uri Url { get; set; }

        [JsonIgnore]
        public Guid UniqueId { get; set; }

        [Required]
        public Guid ResourceId { get; set; }

        public Location Location { get; set; }

        public ShareInput()
        {
            UniqueId = Guid.Empty;
            ResourceId = Guid.Empty;
        }
    }

    public class SendLinkInput
    {
        [JsonProperty(PropertyName = "oId")]
        public string UserId { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [JsonProperty(PropertyName = "itemId")]
        public string ResourceId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "resourceType")]
        public string ResourceType { get; set; }

        [JsonProperty(PropertyName = "resourceDetails")]
        public dynamic ResourceDetails { get; set; }

        [JsonProperty(PropertyName = "plan")]
        public Plan Plan { get; set; }
    }

    public class Plan
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "topicIds")]
        public List<string> TopicIds { get; set; }
    }
}
