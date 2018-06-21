using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Shared.Models
{
   public class Location
    {
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }
        [JsonProperty(PropertyName = "county")]
        public string County { get; set; }
        [JsonProperty(PropertyName = "zipCode")]
        public string ZipCode { get; set; }
    }
}
