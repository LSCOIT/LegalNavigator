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
        /// Get StaticResource by page name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/staticresource/getstaticresource/{name}")]
        public async Task<IActionResult> GetStaticResourceDataAsync(string name)
        {
            var users = await staticResourceBusinessLogic.GetPageStaticResourceDataAsync(name);
            return Ok(users);
        }

        /// <summary>
        /// Insert and Update the home page static contents
        /// </summary>
        /// <param name="pageContent"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/staticresource/upsertstatichomepage")]
        public async Task<IActionResult> UpsertStaticHomePageDataAsync(HomeContent homePageContent)
        {
            var users = await staticResourceBusinessLogic.UpsertStaticHomePageDataAsync(homePageContent);
            return Ok(users);
        }

        /// <summary>
        /// Insert and Update the privacy promise page static contents
        /// </summary>
        /// <param name="pageContent"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/staticresource/upsertstaticprivacypage")]
        public async Task<IActionResult> UpsertStaticPrivacyPromisePageDataAsync(PrivacyPromiseContent privacyPromiseContent)
        {
            var users = await staticResourceBusinessLogic.UpsertStaticPrivacyPromisePageDataAsync(privacyPromiseContent);
            return Ok(users);
        }

        /// <summary>
        /// Insert and Update the helpAndFAQ page static contents
        /// </summary>
        /// <param name="pageContent"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/staticresource/upsertstatichelpandfaqpage")]
        public async Task<IActionResult> UpsertStaticHelpAndFAQPageDataAsync(HelpAndFaqsContent helpAndFAQPageContent)
        {
            var users = await staticResourceBusinessLogic.UpsertStaticHelpAndFAQPageDataAsync(helpAndFAQPageContent);
            return Ok(users);
        }

        /// <summary>
        /// Insert and Update the header static contents
        /// </summary>
        /// <param name="headerContent"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/staticresource/upsertstaticheader")]
        public async Task<IActionResult> UpsertStaticHeaderDataAsync(Header headerContent)
        {
            var users = await staticResourceBusinessLogic.UpsertStaticHeaderDataAsync(headerContent);
            return Ok(users);
        }

        /// <summary>
        /// Insert and Update the footer static contents
        /// </summary>
        /// <param name="footerContent"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/staticresource/upsertstaticfooter")]
        public async Task<IActionResult> UpsertStaticFooterDataAsync(Footer footerContent)
        {
            var users = await staticResourceBusinessLogic.UpsertStaticFooterDataAsync(footerContent);
            return Ok(users);
        }

        /// <summary>
        /// Insert and Update the navigation static contents
        /// </summary>
        /// <param name="navigationContent"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/staticresource/upsertstaticnavigation")]
        public async Task<IActionResult> UpsertStaticNavigationDataAsync(Navigation navigationContent)
        {
            var users = await staticResourceBusinessLogic.UpsertStaticNavigationDataAsync(navigationContent);
            return Ok(users);
        }
    }   
}