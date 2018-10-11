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
        [Route("api/static-resource/get-static-resources")]
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
        [Route("api/static-resource/upsert-static-home-page")]
        public async Task<IActionResult> UpsertStaticHomePageDataAsync([FromBody]HomeContent homePageContent, Location location)
        {
            if (await userRoleBusinessLogic.ValidateOrganizationalUnit(homePageContent.OrganizationalUnit))
            {
                var contents = await staticResourceBusinessLogic.UpsertStaticHomePageDataAsync(homePageContent, location);
                return Ok(contents);
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
        [Route("api/static-resource/upsert-static-privacy-page")]
        public async Task<IActionResult> UpsertStaticPrivacyPromisePageDataAsync([FromBody]PrivacyPromiseContent privacyPromiseContent, Location location)
        {
            if (await userRoleBusinessLogic.ValidateOrganizationalUnit(privacyPromiseContent.OrganizationalUnit))
            {
                var contents = await staticResourceBusinessLogic.UpsertStaticPrivacyPromisePageDataAsync(privacyPromiseContent, location);
                return Ok(contents);
            }
            return StatusCode(403);
        }

        /// <summary>
        /// Insert and Update the helpAndFAQ page static contents
        /// </summary>
        /// <param name="pageContent"></param>
        /// <returns></returns>
        [Permission(PermissionName.upsertstatichelpandfaqpage)]
        [HttpPost]
        [Route("api/static-resource/upsert-static-help-and-faq-page")]
        public async Task<IActionResult> UpsertStaticHelpAndFAQPageDataAsync(HelpAndFaqsContent helpAndFAQPageContent, Location location)
        {
            if (await userRoleBusinessLogic.ValidateOrganizationalUnit(helpAndFAQPageContent.OrganizationalUnit))
            {
                var contents = await staticResourceBusinessLogic.UpsertStaticHelpAndFAQPageDataAsync(helpAndFAQPageContent, location);
                return Ok(contents);
            }
            return StatusCode(403);
        }

        /// <summary>
        /// Insert and Update the navigation static contents
        /// </summary>
        /// <param name="navigationContent"></param>
        /// <returns></returns>
        [Permission(PermissionName.upsertstaticnavigation)]
        [HttpPost]
        [Route("api/static-resource/upsert-static-navigation")]
        public async Task<IActionResult> UpsertStaticNavigationDataAsync([FromBody]Navigation navigationContent, Location location)
        {
            if (await userRoleBusinessLogic.ValidateOrganizationalUnit(navigationContent.OrganizationalUnit))
            {
                var contents = await staticResourceBusinessLogic.UpsertStaticNavigationDataAsync(navigationContent, location);
                return Ok(contents);
            }
            return StatusCode(403);
        }

        /// <summary>
        /// Insert and Update the about page static contents
        /// </summary>
        /// <param name="aboutContent"></param>
        /// <returns></returns>
        [Permission(PermissionName.upsertstaticaboutpage)]
        [HttpPost]
        [Route("api/static-resource/upsert-static-about-page")]
        public async Task<IActionResult> UpsertStaticAboutPageDataAsync([FromBody]AboutContent aboutContent, Location location)
        {
            if (await userRoleBusinessLogic.ValidateOrganizationalUnit(aboutContent.OrganizationalUnit))
            {
                var contents = await staticResourceBusinessLogic.UpsertStaticAboutPageDataAsync(aboutContent, location);
                return Ok(contents);
            }
            return StatusCode(403);
        }
    }
}