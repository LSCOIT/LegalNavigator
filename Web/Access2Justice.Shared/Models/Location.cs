using Newtonsoft.Json;
using System.ComponentModel;

namespace Access2Justice.Shared.Models
{
    public class Location
    {
        public Location() { }

        public Location(string state, string county, string city, string zipCode)
        {
            State = state;
            County = county;
            City = city;
            ZipCode = zipCode;
        }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "county")]
        public string County { get; set; }

        [DefaultValue("")]
        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }
        
        [DefaultValue("")]
        [JsonProperty(PropertyName = "zipCode")]
        public string ZipCode { get; set; }
    }
}
