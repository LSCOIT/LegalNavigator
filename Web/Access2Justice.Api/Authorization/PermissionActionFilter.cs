using Access2Justice.Api.Interfaces;
using Access2Justice.Shared.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;
using static Access2Justice.Api.Authorization.Permissions;

namespace Access2Justice.Api.Authorization
{
    public class PermissionActionFilter : IAsyncActionFilter
    {
        private readonly PermissionName action;
        private readonly IUserRoleBusinessLogic userRoleBusinessLogic;

        public PermissionActionFilter(PermissionName action, IUserRoleBusinessLogic userRoleBusinessLogic)
        {
            this.action = action;
            this.userRoleBusinessLogic = userRoleBusinessLogic;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            bool isAuthorized = await PermissionValidator(context, action);
            if (!isAuthorized)
            {
                context.Result = new UnauthorizedResult();
            }
            else
            {
                await next();
            }
        }

        public async Task<bool> PermissionValidator(ActionExecutingContext context,PermissionName action)
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

        private string EncryptOId(string oId)
        {
            if (!string.IsNullOrEmpty(oId))
            {
                return EncryptionUtilities.GenerateSHA512String(oId);
            }
            return "";
        }
    }
}
