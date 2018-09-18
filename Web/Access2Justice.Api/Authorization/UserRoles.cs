using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.Authorization
{
    public class UserRoles
    {
        public enum RoleEnum
        {
            GlobalAdmin,
            StateAdmin,
            Developer,
            Authenticated,
            Anonymous
        }

        public enum PolicyEnum
        {
            AdminPolicy,
            GlobalAdminPolicy,
            StateAdminPolicy,
            DeveloperPolicy,
            AuthenticatedPolicy,
            AnonymousPolicy
        }

        public enum PermissionEnum
        {
            Topics,
            Resources,
            Articles
        }

        public enum AccessTypeEnum
        {
            Read,
            Create,
            Modify,
            Delete
        }
    }
}
