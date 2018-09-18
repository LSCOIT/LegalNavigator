using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Access2Justice.Api.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Authorization;

namespace Access2Justice.Api.Authorization
{
    public class AuthorizeUserHandler : AuthorizationHandler<AuthorizeUser>
    {
        private readonly IUserRoleBusinessLogic userRoleBusinessLogic;
        public AuthorizeUserHandler(IUserRoleBusinessLogic userRoleBusinessLogicInstance)
        {
            userRoleBusinessLogic = userRoleBusinessLogicInstance;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizeUser requirement)
        {
            //dynamic result = null;
            //result = await GetUserRole(requirement.OId);
            RolePermissionAccess result = await userRoleBusinessLogic.GetRoleInfo(requirement.OId);
            if (result != null)
            {
                if (requirement.Role.Contains(","))
                {
                    string[] roles = requirement.Role.Split(',');
                    foreach (var role in roles)
                    {
                        if (result.RoleName == role)
                        {
                            context.Succeed(requirement);
                        }
                    }
                }
                else if (result.RoleName == requirement.Role)
                {
                    context.Succeed(requirement);
                }
            }
        }

        public async Task<dynamic> GetUserRole(string oId)
        {
            dynamic roleName = await userRoleBusinessLogic.GetRoleNameById(oId);
            return roleName;
        }

        public async Task<RolePermissionAccess> GetRoleInfo(string oId)
        {
            RolePermissionAccess roleInfo = await userRoleBusinessLogic.GetRoleInfo(oId);
            return roleInfo;
        }

    }
}
