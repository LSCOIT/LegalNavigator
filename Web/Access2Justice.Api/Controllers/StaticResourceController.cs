﻿using System.Threading.Tasks;
using Access2Justice.Api.Authorization;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Access2Justice.Api.Authorization.Permissions;

namespace Access2Justice.Api.Controllers
{
	[Produces("application/json")]
    public class StaticResourceController : Controller
    {
        private readonly IStaticResourceBusinessLogic staticResourceBusinessLogic;

        public StaticResourceController(IStaticResourceBusinessLogic staticResourceBusinessLogic)
        {
            this.staticResourceBusinessLogic = staticResourceBusinessLogic;
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
        public async Task<IActionResult> UpsertStaticHomePageDataAsync(HomeContent homePageContent, Location location)
        {
            var contents = await staticResourceBusinessLogic.UpsertStaticHomePageDataAsync(homePageContent, location);
            return Ok(contents);
        }

        /// <summary>
        /// Insert and Update the privacy promise page static contents
        /// </summary>
        /// <param name="pageContent"></param>
        /// <returns></returns>
        [Permission(PermissionName.upsertstaticprivacypage)]
        [HttpPost]
        [Route("api/staticresource/upsertstaticprivacypage")]
        public async Task<IActionResult> UpsertStaticPrivacyPromisePageDataAsync(PrivacyPromiseContent privacyPromiseContent, Location location)
        {
            var contents = await staticResourceBusinessLogic.UpsertStaticPrivacyPromisePageDataAsync(privacyPromiseContent, location);
            return Ok(contents);
        }

        /// <summary>
        /// Insert and Update the helpAndFAQ page static contents
        /// </summary>
        /// <param name="pageContent"></param>
        /// <returns></returns>
        [Permission(PermissionName.upsertstatichelpandfaqpage)]
        [HttpPost]
        [Route("api/staticresource/upsertstatichelpandfaqpage")]
        public async Task<IActionResult> UpsertStaticHelpAndFAQPageDataAsync(HelpAndFaqsContent helpAndFAQPageContent, Location location)
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
        public async Task<IActionResult> UpsertStaticNavigationDataAsync(Navigation navigationContent, Location location)
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
        public async Task<IActionResult> UpsertStaticAboutPageDataAsync(AboutContent aboutContent, Location location)
        {
            var contents = await staticResourceBusinessLogic.UpsertStaticAboutPageDataAsync(aboutContent, location);
            return Ok(contents);
        }
    }
}