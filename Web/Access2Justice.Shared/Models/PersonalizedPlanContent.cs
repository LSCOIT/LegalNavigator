using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Access2Justice.Shared.Models
{
    public class PersonalizedPlanContent:NameLocation
    {
    
        [DefaultValue("")]
        [JsonProperty(PropertyName = "organizationalUnit")]
        public string OrganizationalUnit { get; set; }


        [DefaultValue("")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
         

        [DefaultValue("")]
        [JsonProperty(PropertyName = "sponsors")]
        public List<Sponsors> Sponsors { get; set; }
       
    }
}
