using Newtonsoft.Json;

namespace Access2Justice.Integration.Models
{
    public class AdapterSettings
    {
        [JsonProperty(PropertyName = "namespace")]
        public string Namespace { get; set; }

        [JsonProperty(PropertyName = "methodName")]
        public string MethodName { get; set; }

        [JsonProperty(PropertyName = "adapterSettings")]
        public dynamic AdapterDetails { get; set; }
    }
}
