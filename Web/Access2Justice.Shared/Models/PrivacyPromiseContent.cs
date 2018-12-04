using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;

namespace Access2Justice.Shared.Models
{    
    public class PrivacyPromiseContent: NameLocation
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "image")]
        public Image Image { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "details")]
        public IEnumerable<Detail> Details { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "organizationalUnit")]
        public string OrganizationalUnit { get; set; }
    }

    public class Detail
    {
        [DefaultValue("")]
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }
}