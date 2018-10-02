using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Access2Justice.Api.Authorization
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
	public class OrganizationalUnitAttribute : TypeFilterAttribute
	{
		public OrganizationalUnitAttribute() : base(typeof(OrganizationalUnitActionFilter))
		{
			//Arguments = new object[] { action };
		}
	}
}
