using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Shared.Models
{
    public class UserRole
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
        public List<Guid> Permissions { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty(PropertyName = "createdTimeStamp")]
        public DateTime? CreatedTimeStamp { get; set; } = DateTime.UtcNow;

        [JsonProperty(PropertyName = "modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty(PropertyName = "modifiedTimeStamp")]
        public DateTime? ModifiedTimeStamp { get; set; } = DateTime.UtcNow;

        public UserRole()
        {
            Permissions = new List<Guid>();
        }
    }

    public class Permission
    {

        [JsonProperty(PropertyName = "permissionId")]
        public Guid PermissionId { get; set; }

        [JsonProperty(PropertyName = "permissionName")]
        public string PermissionName { get; set; }

        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; }
    }

    public class PermissionDetails
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string type { get; set; }

        [JsonProperty(PropertyName = "permissions")]
        public List<Permission> Permissions { get; set; }

        public PermissionDetails()
        {
            Permissions = new List<Permission>();
        }
    }
}
