using System.Threading.Tasks;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Mvc;

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
        /// Get static resources by location
        /// </summary>
        /// <remarks>
        /// Helps to get static resources by location
        /// </remarks>
        /// <param name="location"></param>
        /// <response code="200">Get static resources by location for given input</response>
        /// <response code="500">Failure</response>
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
        /// <remarks>
        /// Helps to get home page static contents inserted or updated
        /// </remarks>
        /// <param name="homePageContent"></param>
        /// <param name="location"></param>
        /// <response code="200">Get home page static contents inserted or updated</response>
        /// <response code="500">Failure</response>
        [HttpPost]
        [Route("api/static-resource/upsert-static-home-page")]
        public async Task<IActionResult> UpsertStaticHomePageDataAsync(HomeContent homePageContent, Location location)
        {
            var contents = await staticResourceBusinessLogic.UpsertStaticHomePageDataAsync(homePageContent, location);
            return Ok(contents);
        }

        /// <summary>
        /// Insert and Update the privacy promise page static contents
        /// </summary>
        /// <remarks>
        /// Helps to get privacy page static contents inserted or updated
        /// </remarks>
        /// <param name="privacyPromiseContent"></param>
        /// <param name="location"></param>
        /// <response code="200">Get home privacy static contents inserted or updated</response>
        /// <response code="500">Failure</response>
        [HttpPost]
        [Route("api/static-resource/upsert-static-privacy-page")]
        public async Task<IActionResult> UpsertStaticPrivacyPromisePageDataAsync(PrivacyPromiseContent privacyPromiseContent, Location location)
        {
            var contents = await staticResourceBusinessLogic.UpsertStaticPrivacyPromisePageDataAsync(privacyPromiseContent, location);
            return Ok(contents);
        }

        /// <summary>
        /// Insert and Update the helpAndFAQ page static contents
        /// </summary>
        /// <remarks>
        /// Helps to get helpAndFAQ page static contents inserted or updated
        /// </remarks>
        /// <param name="helpAndFAQPageContent"></param>
        /// <param name="location"></param>
        /// <response code="200">Get helpAndFAQ page static contents inserted or updated</response>
        /// <response code="500">Failure</response>
        [HttpPost]
        [Route("api/static-resource/upsert-static-help-and-faq-page")]
        public async Task<IActionResult> UpsertStaticHelpAndFAQPageDataAsync(HelpAndFaqsContent helpAndFAQPageContent, Location location)
        {
            var contents = await staticResourceBusinessLogic.UpsertStaticHelpAndFAQPageDataAsync(helpAndFAQPageContent, location);
            return Ok(contents);
        }

        /// <summary>
        /// Insert and Update the navigation static contents
        /// </summary>
        /// 
        /// <remarks>
        /// Helps to get navigation page static contents inserted or updated
        /// </remarks>
        /// <param name="navigationContent"></param>
        /// <param name="location"></param>
        /// <response code="200">Get navigation page static contents inserted or updated</response>
        /// <response code="500">Failure</response>
        [HttpPost]
        [Route("api/static-resource/upsert-static-navigation")]
        public async Task<IActionResult> UpsertStaticNavigationDataAsync(Navigation navigationContent, Location location)
        {
            var contents = await staticResourceBusinessLogic.UpsertStaticNavigationDataAsync(navigationContent, location);
            return Ok(contents);
        }

        /// <summary>
        /// Insert and Update the about page static contents
        /// </summary>
        /// <remarks>
        /// Helps to get about page static contents inserted or updated
        /// </remarks>
        /// <param name="aboutContent"></param>
        /// <param name="location"></param>
        /// <response code="200">Get about page static contents inserted or updated</response>
        /// <response code="500">Failure</response>
        [HttpPost]
        [Route("api/static-resource/upsert-static-about-page")]
        public async Task<IActionResult> UpsertStaticAboutPageDataAsync(AboutContent aboutContent, Location location)
        {
            var contents = await staticResourceBusinessLogic.UpsertStaticAboutPageDataAsync(aboutContent, location);
            return Ok(contents);
        }
    }
}