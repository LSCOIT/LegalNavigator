using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Shared.Models
{
    public class SharedResources
    {
        [JsonProperty(PropertyName = "id")]
        public Guid SharedResourceId { get; set; }

        [JsonProperty(PropertyName = "sharedResources")]
        public List<SharedResource> SharedResource { get; set; }

        public SharedResources()
        {
            SharedResource = new List<SharedResource>();
        }
    }

    public class SharedResource
    {
        [JsonProperty(PropertyName = "isShared")]
        public bool IsShared { get; set; }

        [JsonProperty(PropertyName = "expirationDate")]
        public DateTime ExpirationDate { get; set; }

        [JsonProperty(PropertyName = "permaLink")]
        public string PermaLink { get; set; }

        [JsonProperty(PropertyName = "url")]
        public Uri Url { get; set; }
    }
}
