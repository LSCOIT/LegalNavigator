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
            GlobalAdmin=1,
            StateAdmin,
            Developer,
            Authenticated,
            Anonymous
        }

        public enum PolicyEnum
        {
            GlobalAdminPolicy =1,
            StateAdminPolicy,
            DeveloperPolicy,
            AuthenticatedPolicy,
            AnonymousPolicy,
			AdminRolesPolicy,
			AuthenticatedUserPolicy
        }
    }
}
