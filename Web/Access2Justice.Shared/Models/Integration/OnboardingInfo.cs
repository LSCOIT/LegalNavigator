using Newtonsoft.Json;
using System.Collections.Generic;

namespace Access2Justice.Shared.Models.Integration
{
    public class OnboardingInfo
    {
        public OnboardingInfo()
        {
            UserFields = new List<UserField>();
        }

        [JsonProperty(PropertyName = "userFields")]
        public IEnumerable<UserField> UserFields { get; set; }
    }
}