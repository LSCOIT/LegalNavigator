using Access2Justice.Api.Authentication;
using Access2Justice.Api.Interfaces;
using Access2Justice.Shared;
using Access2Justice.Shared.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static Access2Justice.Api.Authorization.Permissions;

namespace Access2Justice.Api.Authorization
{
    public class PermissionActionFilter : IAsyncActionFilter
    {
        private readonly PermissionName action;
        private readonly IUserRoleBusinessLogic userRoleBusinessLogic;
        private readonly AzureAdOptions azureOptions;
        private readonly IConfiguration configuration;

        public PermissionActionFilter(PermissionName action, IUserRoleBusinessLogic userRoleBusinessLogic, IOptions<AzureAdOptions> azureOptions, IConfiguration configuration)
        {
            this.action = action;
            this.userRoleBusinessLogic = userRoleBusinessLogic;
            this.azureOptions = azureOptions.Value;
            this.configuration = configuration;
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
                    if (this.IsApplication(context.HttpContext.User.Claims, oId))
                    {
                        return true;
                    }

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

        private bool IsApplication(IEnumerable<Claim> claims, string oId)
        {
            try
            {
                IConfigurationSection apiOids = configuration.GetSection("ApiOids");
                IEnumerable<string> oids = apiOids.GetSection("oIds").Get<IEnumerable<string>>();
                string azpacr = apiOids.GetSection(Constants.Azpacr).Get<string>();
                string sub = claims.FirstOrDefault(c => c.Type == Constants.NameIdentifier)?.Value;
                string azpacrClaim = claims.FirstOrDefault(c => c.Type == Constants.Azpacr)?.Value;

                // Application tokens always have the same value of oId and sub claims
                // azpacr 1 means that token was generated using client secret
                if (oids.Contains(oId) && oId == sub && azpacr == azpacrClaim)
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }
    }
}
