using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Access2Justice.Api.Interfaces;
using Access2Justice.Shared.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Access2Justice.Api.Authorization
{
	public class OrganizationalUnitActionFilter : IAsyncActionFilter
	{
		private readonly IUserRoleBusinessLogic userRoleBusinessLogic;

		public OrganizationalUnitActionFilter(IUserRoleBusinessLogic userRoleBusinessLogic)
		{
			this.userRoleBusinessLogic = userRoleBusinessLogic;
		}
		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			bool isAuthorized = await OrganizationalUnitValidator(context);
			if (!isAuthorized)
			{
				context.Result = new UnauthorizedResult();
			}
			else { await next(); }
		}

		public async Task<bool> OrganizationalUnitValidator(ActionExecutingContext context)
		{
			if (context.HttpContext.User.Claims.FirstOrDefault() != null)
			{
				string oId = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
				if (!(string.IsNullOrEmpty(oId)))
				{
					oId = EncryptionUtilities.GenerateSHA512String(oId);
					string requestPath = context.HttpContext.Request.Path.ToString();
					if (requestPath.Contains("getuserprofile"))
					{
						if (context.ActionArguments.Count > 0)
						{
							//Dictionary<string, object> parameters = new Dictionary<string, object>();
							foreach (var param in context.ActionArguments)
							{
								if (param.Value.ToString() == oId)
								{
									//isOId = true;
								}
							}
						}
						return false;
					}
				}
			}
			return false;
		}

	}
}
