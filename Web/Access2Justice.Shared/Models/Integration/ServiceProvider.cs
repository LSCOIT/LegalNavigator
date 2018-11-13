using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Shared.Models.Integration
{

    public class ServiceProvider : Organization
    {
        [JsonProperty(PropertyName = "siteId")]
        public int SiteId { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "availability")]
        public Availability Availability { get; set; }

        [JsonProperty(PropertyName = "acceptanceCriteria")]
        public AcceptanceCriteria AcceptanceCriteria { get; set; }

        [JsonProperty(PropertyName = "onboardingInfo")]
        public OnboardingInfo OnboardingInfo { get; set; }
    }
}