using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Shared.Models
{
    public class UserProfile
    {
        [JsonProperty(PropertyName = "id")]
        public dynamic Id { get; set; }

        [JsonProperty(PropertyName = "oId")]
        public string OId { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "eMail")]
        public string EMail { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        public string IsActive { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty(PropertyName = "createdTimeStamp")]
        public string CreatedTimeStamp { get; set; }

        [JsonProperty(PropertyName = "modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty(PropertyName = "modifiedTimeStamp")]
        public string ModifiedTimeStamp { get; set; }

        [JsonProperty(PropertyName = "sharedResource")]
        public List<SharedResource> SharedResource { get; set; }

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
