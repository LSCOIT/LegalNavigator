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

    public class SharedResourceView: SharedResource
    {
        public SharedResourceView(SharedResource other)
        {
            Url = other.Url;
            IsShared = other.IsShared;
            ExpirationDate = other.ExpirationDate;
            PermaLink = other.PermaLink;
        }

        public SharedResourceView()
        {
        }

        [JsonIgnore]
        public string Id
        {
            get
            {
                var url = Url.OriginalString.TrimEnd('/');
                return url.Substring(url.Length - Constants.StrigifiedGuidLength);
            }
        }

        [JsonProperty("sharedTo", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> SharedTo { get; set; }
    }

}
