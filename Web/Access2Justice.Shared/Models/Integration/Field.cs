using Newtonsoft.Json;

namespace Access2Justice.Shared.Models.Integration
{
    public class UserField
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }
}