using Access2Justice.Api.Authorization;
using Access2Justice.Api.Interfaces;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using static Access2Justice.Api.Authorization.Permissions;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
	public class StaticResourceController : Controller
	{
		private readonly IStaticResourceBusinessLogic staticResourceBusinessLogic;
		private readonly IUserRoleBusinessLogic userRoleBusinessLogic;

		public StaticResourceController(IStaticResourceBusinessLogic staticResourceBusinessLogic, IUserRoleBusinessLogic userRoleBusinessLogic)
		{
			this.staticResourceBusinessLogic = staticResourceBusinessLogic;
			this.userRoleBusinessLogic = userRoleBusinessLogic;
		}

		/// <summary>
		/// Get StaticResources by Location
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("api/staticresource/getstaticresources")]
		public async Task<IActionResult> GetStaticResourcesDataAsync([FromBody]Location location)
		{
			var contents = await staticResourceBusinessLogic.GetPageStaticResourcesDataAsync(location);
			return Ok(contents);
		}

		/// <summary>
		/// Insert and Update the home page static contents
		/// </summary>
		/// <param name="pageContent"></param>
		/// <returns></returns>
		[Permission(PermissionName.upsertstatichomepage)]
		[HttpPost]
		[Route("api/staticresource/upsertstatichomepage")]
		public async Task<IActionResult> UpsertStaticHomePageDataAsync([FromBody]HomeContent homePageContent, Location location)
		{
			if (HttpContext.User.Claims.FirstOrDefault() != null)
			{
				string oId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
				if (await userRoleBusinessLogic.GetOrganizationalUnit(oId, homePageContent.OrganizationalUnit))
				{
					var contents = await staticResourceBusinessLogic.UpsertStaticHomePageDataAsync(homePageContent, location);
					return Ok(contents);
				}
			}
			return StatusCode(403);
		}

		/// <summary>
		/// Insert and Update the privacy promise page static contents
		/// </summary>
		/// <param name="pageContent"></param>
		/// <returns></returns>
		[Permission(PermissionName.upsertstaticprivacypage)]
		[HttpPost]
		[Route("api/staticresource/upsertstaticprivacypage")]
		public async Task<IActionResult> UpsertStaticPrivacyPromisePageDataAsync([FromBody]PrivacyPromiseContent privacyPromiseContent, Location location)
		{
			if (await userRoleBusinessLogic.GetOrganizationalUnit("", privacyPromiseContent.OrganizationalUnit))
			{
				var contents = await staticResourceBusinessLogic.UpsertStaticPrivacyPromisePageDataAsync(privacyPromiseContent, location);
				return Ok(contents);
			}
			return Forbid("Cannot access unauthorized data");
		}

		/// <summary>
		/// Insert and Update the helpAndFAQ page static contents
		/// </summary>
		/// <param name="pageContent"></param>
		/// <returns></returns>
		[Permission(PermissionName.upsertstatichelpandfaqpage)]
		[HttpPost]
		[Route("api/staticresource/upsertstatichelpandfaqpage")]
		public async Task<IActionResult> UpsertStaticHelpAndFAQPageDataAsync([FromBody]HelpAndFaqsContent helpAndFAQPageContent, Location location)
		{
			var contents = await staticResourceBusinessLogic.UpsertStaticHelpAndFAQPageDataAsync(helpAndFAQPageContent, location);
			return Ok(contents);
		}

		/// <summary>
		/// Insert and Update the navigation static contents
		/// </summary>
		/// <param name="navigationContent"></param>
		/// <returns></returns>
		[Permission(PermissionName.upsertstaticnavigation)]
		[HttpPost]
		[Route("api/staticresource/upsertstaticnavigation")]
		public async Task<IActionResult> UpsertStaticNavigationDataAsync([FromBody]Navigation navigationContent, Location location)
		{
			var contents = await staticResourceBusinessLogic.UpsertStaticNavigationDataAsync(navigationContent, location);
			return Ok(contents);
		}

		/// <summary>
		/// Insert and Update the about page static contents
		/// </summary>
		/// <param name="aboutContent"></param>
		/// <returns></returns>
		[Permission(PermissionName.upsertstaticaboutpage)]
		[HttpPost]
		[Route("api/staticresource/upsertstaticaboutpage")]
		public async Task<IActionResult> UpsertStaticAboutPageDataAsync([FromBody]AboutContent aboutContent, Location location)
		{
			var contents = await staticResourceBusinessLogic.UpsertStaticAboutPageDataAsync(aboutContent, location);
			return Ok(contents);
		}
	}
}