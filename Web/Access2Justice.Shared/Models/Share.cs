﻿using Newtonsoft.Json;
using System;
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

        public ShareInput()
        {
            UniqueId = Guid.Empty;
            ResourceId = Guid.Empty;
        }
    }

    public class SendLinkInput
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Link { get; set; }
    }
}
