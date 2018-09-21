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
            var RoleName = await userRoleBusinessLogic.GetRoleInfo(requirement.OId);
            if (!(string.IsNullOrEmpty(RoleName)))
            {
                if (requirement.Role.Contains(","))
                {
                    string[] roles = requirement.Role.Split(',');
                    foreach (var role in roles)
                    {
                        if (RoleName == role || requirement.Role == UserRoles.RoleEnum.Anonymous.ToString())
                        {
                            context.Succeed(requirement);
                        }
                    }
                }
                else if (RoleName == requirement.Role || requirement.Role == UserRoles.RoleEnum.Anonymous.ToString())
                {
                    context.Succeed(requirement);
                }
            }
            else if (string.IsNullOrEmpty(requirement.OId) || requirement.Role == UserRoles.RoleEnum.Anonymous.ToString())
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }

        public async Task<dynamic> GetUserRole(string oId)
        {
            dynamic roleName = await userRoleBusinessLogic.GetRoleNameById(oId);
            return roleName;
        }

        public async Task<dynamic> GetRoleInfo(string oId)
        {
            dynamic roleInfo = await userRoleBusinessLogic.GetRoleInfo(oId);
            return roleInfo;
        }

    }
}
