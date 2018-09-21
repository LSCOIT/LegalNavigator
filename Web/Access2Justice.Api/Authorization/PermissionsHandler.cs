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
            List<string> permissions = await userRoleBusinessLogic.GetPermissionDataAsyn(requirement.OId);
            int permissionsCount = permissions.Count();
            if (permissionsCount > 0)
            {
                string routePath = ((Microsoft.AspNetCore.Http.Internal.DefaultHttpRequest)((Microsoft.AspNetCore.Http.DefaultHttpContext)((Microsoft.AspNetCore.Mvc.ActionContext)context.Resource).HttpContext).Request).Path;
                if (permissions.Contains(routePath))
                {
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
