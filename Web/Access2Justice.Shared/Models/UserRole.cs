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

    public class Permissions
    {

        [JsonProperty(PropertyName = "id")]
        public Guid RoleInformationId { get; set; }

        [JsonProperty(PropertyName = "permissionName")]
        public string PermissionName { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "accessType")]
        public AccessType AccessType { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty(PropertyName = "createdTimeStamp")]
        public DateTime? CreatedTimeStamp { get; set; } = DateTime.UtcNow;

        [JsonProperty(PropertyName = "modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty(PropertyName = "modifiedTimeStamp")]
        public DateTime? ModifiedTimeStamp { get; set; } = DateTime.UtcNow;
    }

    public class AccessType
    {
        [JsonProperty(PropertyName = "read")]
        public bool Read { get; set; }

        [JsonProperty(PropertyName = "create")]
        public bool Create { get; set; }

        [JsonProperty(PropertyName = "modify")]
        public bool Modify { get; set; }

        [JsonProperty(PropertyName = "delete")]
        public bool Delete { get; set; }
    }

    public class RolePermissionAccess
    {
        [JsonProperty(PropertyName = "roleName")]
        public string RoleName { get; set; }

        [JsonProperty(PropertyName = "permissions")]
        public List<RolePermission> RolePermissions { get; set; }

        public RolePermissionAccess()
        {
            RolePermissions = new List<RolePermission>();
        }
    }

    public class RolePermission
    {
        [JsonProperty(PropertyName = "permissionName")]
        public string PermissionName { get; set; }

        [JsonProperty(PropertyName = "accessType")]
        public AccessType AccessType { get; set; }
    }
}
