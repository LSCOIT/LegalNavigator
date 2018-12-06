using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.ViewModels
{
    public class StateProvinceViewModel
    {
        [JsonProperty(PropertyName = "id")]
        public string StateCodesId { get; set; }

        [JsonProperty(PropertyName = "stateProvinces")]
        public List<StateCode> StateProvinces { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        public StateProvinceViewModel()
        {
            StateProvinces = new List<StateCode>();
        }
    }

    public class StateCode
    {
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
