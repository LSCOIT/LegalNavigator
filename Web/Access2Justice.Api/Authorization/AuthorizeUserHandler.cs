using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Access2Justice.Api.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Utilities;
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
            if (requirement.Role == (UserRoles.RoleEnum.Anonymous.ToString()))
            {
                context.Succeed(requirement);
            }
            else
            {
                if (context.User.Claims.FirstOrDefault() != null)
                {
                    string oId = context.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
                    requirement.OId = EncryptOid(oId);
                    if (!(string.IsNullOrEmpty(requirement.OId)))
                    {
                        var RoleName = await userRoleBusinessLogic.GetRoleInfo(requirement.OId);
                        if (!(string.IsNullOrEmpty(RoleName)))
                        {
                            if (!string.IsNullOrEmpty(requirement.Role))
                            {
                                string[] roles = requirement.Role.Split(',');
                                if (roles.Contains(RoleName))
                                {
                                    context.Succeed(requirement);
                                }
                            }
                            //else
                            //{
                            //    context.Fail();
                            //}
                        }
                    }
                }
            }
        }

        private string EncryptOid(string oId)
        {
            string encryptedOid = string.Empty;
            if (!string.IsNullOrEmpty(oId))
            {
                encryptedOid = EncryptionUtilities.GenerateSHA512String(oId);
            }
            return encryptedOid;
        }

    }
}
