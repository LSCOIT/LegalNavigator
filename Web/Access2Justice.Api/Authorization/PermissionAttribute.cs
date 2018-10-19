using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using static Access2Justice.Api.Authorization.Permissions;

namespace Access2Justice.Api.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class PermissionAttribute : TypeFilterAttribute
    {
        public PermissionAttribute(PermissionName action) : base(typeof(PermissionActionFilter))
        {
            Arguments = new object[] { action };
        }
    }
}
