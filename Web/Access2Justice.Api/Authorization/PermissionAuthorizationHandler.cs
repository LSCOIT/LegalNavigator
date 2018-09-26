using Access2Justice.Api.BusinessLogic;
using Access2Justice.Api.Interfaces;
using Access2Justice.Shared.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static Access2Justice.Api.Authorization.Permissions;

namespace Access2Justice.Api.Authorization
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class PermissionAttribute : TypeFilterAttribute
    {
        public PermissionAttribute(PermissionType item, PermissionName action) : base(typeof(PermissionActionFilter))
        {
            Arguments = new object[] { item, action };
        }
    }

    public class PermissionActionFilter : IAsyncActionFilter
    {
        private readonly PermissionType item;
        private readonly PermissionName action;
        private readonly IUserRoleBusinessLogic userRoleBusinessLogic;

        public PermissionActionFilter(PermissionType item, PermissionName action, IUserRoleBusinessLogic userRoleBusinessLogic)
        {
            this.item = item;
            this.action = action;
            this.userRoleBusinessLogic = userRoleBusinessLogic;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            bool isAuthorized = await MumboJumboFunction(context, item, action);

            if (!isAuthorized)
            {
                context.Result = new UnauthorizedResult();

            }
            else
            {
                await next();
            }
        }

        public async Task<bool> MumboJumboFunction(ActionExecutingContext context, PermissionType item, PermissionName action)
        {
            if (item == PermissionType.Anonymous)
            {                
                return true;
            }
            else
            {
                if (context.HttpContext.User.Claims.FirstOrDefault() != null)
                {
                    string oId = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
                    oId = EncryptOId(oId);
                    if (!(string.IsNullOrEmpty(oId)))
                    {
                        if ((await userRoleBusinessLogic.GetPermissionDataAsyn(oId)).Contains(action.ToString())) {
                            return true;
                        }
                        return false;
                    }
                }
                return false;
            }
        }

        private string EncryptOId(string oId)
        {
            string encryptedOId = string.Empty;
            if (!string.IsNullOrEmpty(oId))
            {
                encryptedOId = EncryptionUtilities.GenerateSHA512String(oId);
            }
            return encryptedOId;
        }
    }

}
