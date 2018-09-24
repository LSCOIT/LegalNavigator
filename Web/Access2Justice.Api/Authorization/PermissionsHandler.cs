using Access2Justice.Api.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.Authorization
{
    public class PermissionsHandler : AuthorizationHandler<AuthorizeUser>
    {
        private readonly IUserRoleBusinessLogic userRoleBusinessLogic;
        public PermissionsHandler(IUserRoleBusinessLogic userRoleBusinessLogicInstance)
        {
            userRoleBusinessLogic = userRoleBusinessLogicInstance;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizeUser requirement)
        {
            if (requirement.Role == (UserRoles.RoleEnum.Anonymous.ToString()))
            {
                context.Succeed(requirement);
            }
            List<string> permissions = await userRoleBusinessLogic.GetPermissionDataAsyn(requirement.OId);
            int permissionsCount = permissions.Count();
            string routePath = ((Microsoft.AspNetCore.Http.Internal.DefaultHttpRequest)((Microsoft.AspNetCore.Http.DefaultHttpContext)((Microsoft.AspNetCore.Mvc.ActionContext)context.Resource).HttpContext).Request).Path;
            if (permissionsCount > 0)
            {
                if (permissions.Contains(routePath))
                {
                    if (routePath.Contains(Constants.GetProfileData) && permissions[0].Equals(UserRoles.RoleEnum.Authenticated.ToString())) 
                    {
                        if (routePath.Contains(permissions[1]))
                        {
                            context.Succeed(requirement);
                        }
                        else
                        {
                            context.Fail();
                        }
                    }
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
        }
    }
}
