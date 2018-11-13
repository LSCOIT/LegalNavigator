using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Shared.Models.Integration
{
    public class OnboardingInfo
    {
        [JsonProperty(PropertyName = "userFields")]
        public IEnumerable<Field> UserFields { get; set; }
    }
}