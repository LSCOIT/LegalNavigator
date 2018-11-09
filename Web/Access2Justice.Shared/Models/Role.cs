using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Shared.Models
{
    public class Role
    {
        [JsonProperty(PropertyName = "id")]
        public Guid RoleInformationId { get; set; }

        [JsonProperty(PropertyName = "roleName")]
        public string RoleName { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        
        [JsonProperty(PropertyName = "organizationalUnit")]
        public string OrganizationalUnit { get; set; }

        [JsonProperty(PropertyName = "permissions")]
        public List<string> Permissions { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty(PropertyName = "createdTimeStamp")]
        public DateTime? CreatedTimeStamp { get; set; } = DateTime.UtcNow;

        [JsonProperty(PropertyName = "modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty(PropertyName = "modifiedTimeStamp")]
        public DateTime? ModifiedTimeStamp { get; set; } = DateTime.UtcNow;

        public Role()
        {
            Permissions = new List<string>();
        }
    }

    public class RoleViewModel
    {
        [JsonProperty(PropertyName = "roleName")]
        public string RoleName { get; set; }

        [JsonProperty(PropertyName = "organizationalUnit")]
        public string OrganizationalUnit { get; set; }
    }

}
