using Access2Justice.Api.Authentication;
using Access2Justice.Api.Interfaces;
using Access2Justice.Shared.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Access2Justice.Api.Authorization.Permissions;

namespace Access2Justice.Api.Authorization
{
    public class PermissionActionFilter : IAsyncActionFilter
    {
        private readonly PermissionName action;
        private readonly IUserRoleBusinessLogic userRoleBusinessLogic;
        private readonly AzureAdOptions azureOptions;

        public PermissionActionFilter(PermissionName action, IUserRoleBusinessLogic userRoleBusinessLogic, IOptions<AzureAdOptions> azureOptions)
        {
            this.action = action;
            this.userRoleBusinessLogic = userRoleBusinessLogic;
            this.azureOptions = azureOptions.Value;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            bool isAuthorized = await PermissionValidator(context);
            if (!isAuthorized)
            {
                context.Result = new UnauthorizedResult();
            }
            else { await next(); }
        }

        public async Task<bool> PermissionValidator(ActionExecutingContext context)
        {
            if (context.HttpContext.User.Claims.FirstOrDefault() != null)
            {
                string oId = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == azureOptions.UserClaimsUrl).Value;
                if (!(string.IsNullOrEmpty(oId)))
                {
                    oId = EncryptionUtilities.GenerateSHA512String(oId);
                    string requestPath = context.HttpContext.Request.Path.ToString();
                    if (requestPath.Contains("get-user-profile"))
                    {
                        if (context.ActionArguments.Count > 0)
                        {
                            Dictionary<string, object> parameters = new Dictionary<string, object>();
                            foreach (var param in context.ActionArguments)
                            {
                                parameters.Add(param.Key, param.Value);
                                if (param.Value.ToString() == oId)
                                {
                                    return await CheckPermissions(oId);
                                }
                            }
                        }
                        return false;
                    }
                    return await CheckPermissions(oId);
                }
            }
            return false;
        }

        public async Task<bool> CheckPermissions(string oId)
        {
            return ((await userRoleBusinessLogic.GetPermissionDataAsync(oId)).Contains(action.ToString())) ? true : false;           
        }
    }
}
