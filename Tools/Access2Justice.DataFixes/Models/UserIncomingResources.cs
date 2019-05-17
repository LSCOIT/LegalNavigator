using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Access2Justice.DataFixes.Models
{
    public class UserIncomingResources
    {
        [JsonProperty(PropertyName = "id")]
        public Guid IncomingResourcesId { get; set; }

        [JsonProperty(PropertyName = "resources")]
        public List<IncomingResource> Resources { get; set; }

        public UserIncomingResources()
        {
            Resources = new List<IncomingResource>();
        }
    }

    public class IncomingResource
    {
        [JsonProperty(PropertyName = "itemId")]
        public string ResourceId { get; set; }

        [JsonProperty(PropertyName = "resourceType")]
        public string ResourceType { get; set; }

        [JsonProperty(PropertyName = "sharedBy")]
        public string SharedBy { get; set; }

        [JsonProperty(PropertyName = "resourceDetails")]
        public dynamic ResourceDetails { get; set; }

        [JsonProperty(PropertyName = "sharedFromResourceId")]
        public Guid SharedFromResourceId { get; set; }
    }
}
