using Newtonsoft.Json;
using System.ComponentModel;


namespace Access2Justice.Shared.Models
{
    public class GuidedAssistantPageContent:NameLocation
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "organizationalUnit")]
        public string OrganizationalUnit { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }
}
