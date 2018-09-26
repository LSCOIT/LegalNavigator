using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.Authorization
{
    public class PermissionAuthorizationRequirement : IAuthorizationRequirement
    {
        public string OId { get; internal set; }
        public string Role { get; private set; }

        //Add any custom requirement properties if you have them
        public PermissionAuthorizationRequirement()
        {
            OId = string.Empty;
            Role = string.Empty;
        }
    }
}
