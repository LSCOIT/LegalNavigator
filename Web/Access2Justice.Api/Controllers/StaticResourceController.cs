using Access2Justice.Api.Authorization;
using Access2Justice.Api.Interfaces;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using static Access2Justice.Api.Authorization.Permissions;

namespace Access2Justice.Api.Controllers
{
    [ExcludeFromCodeCoverage]
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
        /// Get all static resources grouped by OrganizationUnit and Name
        /// </summary>
        /// <returns>Array of Organization Units with static resources</returns>
        [HttpGet]
        [Route("")]
        public async Task<ActionResult> GetAllStaticResources()
        {
            var content = await staticResourceBusinessLogic.GetAllStaticResourcesAsync();

            return Ok(content);
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
            if (contents == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return Ok(contents);
        }

        /// <summary>
        /// Creates a set of static resources for a new state
        /// </summary>
        /// <param name="state">Abbreviation of state to create static resources for</param>
        /// <returns>Static resources created for a new state</returns>
        [Permission(PermissionName.upsertstatichomepage)]
        [Permission(PermissionName.upsertstaticprivacypage)]
        [Permission(PermissionName.upsertstatichelpandfaqpage)]
        [Permission(PermissionName.upsertstaticnavigation)]
        [Permission(PermissionName.upsertstaticaboutpage)]
        [Permission(PermissionName.upsertstaticpersonalizedplanpage)]
        [Permission(PermissionName.upsertstaticguidedassistantpage)]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateStaticResourcesAsync(string state)
        {
            if (string.IsNullOrWhiteSpace(state) ||
                USState.GetByAbbreviation(state) == null)
            {
                return BadRequest("invalid state");
            }

            var location = new Location { State = state.ToUpperInvariant() };
            if (await userRoleBusinessLogic.ValidateOrganizationalUnit(location.State))
            {
                var content = await staticResourceBusinessLogic.CreateStaticResourcesFromDefaultAsync(location);
                return Ok(content);
            }

            return StatusCode(StatusCodes.Status403Forbidden);
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

        /// <summary>
        /// Insert and Update the personalized plan static contents
        /// </summary>
        /// <remarks>
        /// Helps to get personalized plan static contents inserted or updated
        /// </remarks>
        /// <param name="personalizedplanContent"></param>        
        /// <response code="200">Get personalized plan page static contents inserted or updated</response>
        /// <response code="500">Failure</response>
        [Permission(PermissionName.upsertstaticpersonalizedplanpage)]
        [HttpPost]
        [Route("personalizedplan/upsert")]
        public async Task<IActionResult> UpsertStaticPersonalizedPageDataAsync([FromBody]PersonalizedPlanContent personalizedplanContent)
        {
            if (await userRoleBusinessLogic.ValidateOrganizationalUnit(personalizedplanContent.OrganizationalUnit))
            {
                var contents = await staticResourceBusinessLogic.UpsertStaticPersnalizedPlanPageDataAsync(personalizedplanContent);
                return Ok(contents);
            }
            return StatusCode(403);
        }

        /// <summary>
        /// Insert and Update the guided assistant page static contents
        /// </summary>
        /// <remarks>
        /// Helps to get guided assistant page static contents inserted or updated
        /// </remarks>
        /// <param name="guidedAssistantPagContent"></param>        
        /// <response code="200">Get guided assistant page static contents inserted or updated</response>
        /// <response code="403">Returns if there is no valid orgranization present with given input</response>
        /// <response code="400">Returns if there is no organization input has been passed</response>
        /// 
        [Permission(PermissionName.upsertstaticguidedassistantpage)]
        [HttpPost]
        [Route("guidedassistant/upsert")]
        public async Task<IActionResult> UpsertStaticGuidedAssistantPageDataAsync([FromBody]GuidedAssistantPageContent guidedAssistantPagContent)
        {
            if (await userRoleBusinessLogic.ValidateOrganizationalUnit(guidedAssistantPagContent.OrganizationalUnit))
            {
                var contents = await staticResourceBusinessLogic.UpsertStaticGuidedAssistantPageDataAsync(guidedAssistantPagContent);
                return Ok(contents);
            }
            return StatusCode(403);
        }
    }
}