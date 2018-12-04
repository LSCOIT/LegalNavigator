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
    [Route("api/static-resources")]
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
        /// Get static resources by location
        /// </summary>
        /// <remarks>
        /// Helps to get static resources by location
        /// </remarks>
        /// <param name="location"></param>
        /// <response code="200">Get static resources by location for given input</response>
        /// <response code="500">Failure</response>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> GetStaticResourcesDataAsync([FromBody]Location location)
        {
            var contents = await staticResourceBusinessLogic.GetPageStaticResourcesDataAsync(location);
            return Ok(contents);
        }

        /// <summary>
        /// Insert and Update the home page static contents
        /// </summary>
        /// <remarks>
        /// Helps to get home page static contents inserted or updated
        /// </remarks>
        /// <param name="homePageContent"></param>        
        /// <response code="200">Get home page static contents inserted or updated</response>
        /// <response code="500">Failure</response>
        [Permission(PermissionName.upsertstatichomepage)]
        [HttpPost]
        [Route("home/upsert")]
        public async Task<IActionResult> UpsertStaticHomePageDataAsync([FromBody]HomeContent homePageContent)
        {
            if (await userRoleBusinessLogic.ValidateOrganizationalUnit(homePageContent.OrganizationalUnit))
            {
                var contents = await staticResourceBusinessLogic.UpsertStaticHomePageDataAsync(homePageContent);
                return Ok(contents);
            }
            return StatusCode(403);
        }

        /// <summary>
        /// Insert and Update the privacy promise page static contents
        /// </summary>
        /// <remarks>
        /// Helps to get privacy page static contents inserted or updated
        /// </remarks>
        /// <param name="privacyPromiseContent"></param>
        /// <response code="200">Get home privacy static contents inserted or updated</response>
        /// <response code="500">Failure</response>
        [Permission(PermissionName.upsertstaticprivacypage)]
        [HttpPost]
        [Route("privacy/upsert")]
        public async Task<IActionResult> UpsertStaticPrivacyPromisePageDataAsync([FromBody]PrivacyPromiseContent privacyPromiseContent)
        {
            if (await userRoleBusinessLogic.ValidateOrganizationalUnit(privacyPromiseContent.OrganizationalUnit))
            {
                var contents = await staticResourceBusinessLogic.UpsertStaticPrivacyPromisePageDataAsync(privacyPromiseContent);
                return Ok(contents);
            }
            return StatusCode(403);
        }

        /// <summary>
        /// Insert and Update the helpAndFAQ page static contents
        /// </summary>
        /// <remarks>
        /// Helps to get helpAndFAQ page static contents inserted or updated
        /// </remarks>
        /// <param name="helpAndFAQPageContent"></param>
        /// <response code="200">Get helpAndFAQ page static contents inserted or updated</response>
        /// <response code="500">Failure</response>
        [Permission(PermissionName.upsertstatichelpandfaqpage)]
        [HttpPost]
        [Route("help-and-faq/upsert")]
        public async Task<IActionResult> UpsertStaticHelpAndFAQPageDataAsync([FromBody]HelpAndFaqsContent helpAndFAQPageContent)
        {
            if (await userRoleBusinessLogic.ValidateOrganizationalUnit(helpAndFAQPageContent.OrganizationalUnit))
            {
                var contents = await staticResourceBusinessLogic.UpsertStaticHelpAndFAQPageDataAsync(helpAndFAQPageContent);
                return Ok(contents);
            }
            return StatusCode(403);
        }

        /// <summary>
        /// Insert and Update the navigation static contents
        /// </summary>
        /// 
        /// <remarks>
        /// Helps to get navigation page static contents inserted or updated
        /// </remarks>
        /// <param name="navigationContent"></param>        
        /// <response code="200">Get navigation page static contents inserted or updated</response>
        /// <response code="500">Failure</response>
        [Permission(PermissionName.upsertstaticnavigation)]
        [HttpPost]
        [Route("navigation/upsert")]
        public async Task<IActionResult> UpsertStaticNavigationDataAsync([FromBody]Navigation navigationContent)
        {
            if (await userRoleBusinessLogic.ValidateOrganizationalUnit(navigationContent.OrganizationalUnit))
            {
                var contents = await staticResourceBusinessLogic.UpsertStaticNavigationDataAsync(navigationContent);
                return Ok(contents);
            }
            return StatusCode(403);
        }

        /// <summary>
        /// Insert and Update the about page static contents
        /// </summary>
        /// <remarks>
        /// Helps to get about page static contents inserted or updated
        /// </remarks>
        /// <param name="aboutContent"></param>        
        /// <response code="200">Get about page static contents inserted or updated</response>
        /// <response code="500">Failure</response>
        [Permission(PermissionName.upsertstaticaboutpage)]
        [HttpPost]
        [Route("about/upsert")]
        public async Task<IActionResult> UpsertStaticAboutPageDataAsync([FromBody]AboutContent aboutContent)
        {
            if (await userRoleBusinessLogic.ValidateOrganizationalUnit(aboutContent.OrganizationalUnit))
            {
                var contents = await staticResourceBusinessLogic.UpsertStaticAboutPageDataAsync(aboutContent);
                return Ok(contents);
            }
            return StatusCode(403);
        }
    }
}