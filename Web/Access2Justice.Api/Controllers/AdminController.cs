using Access2Justice.Api.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Access2Justice.Api.Controllers
{
    [Route("api/admin")]
    public class AdminController : Controller
    {
        private IAdminBusinessLogic adminBusinessLogic;

        public AdminController(IHostingEnvironment hostingEnvironment, IAdminBusinessLogic adminBusinessLogic)
        {
            this.adminBusinessLogic = adminBusinessLogic;
        }

        [HttpPost("upload-curated-experience-template")]
        public async Task<IActionResult> UploadCuratedExperienceTemplate()
        {
            var response = await adminBusinessLogic.UploadCuratedContentPackage(Request.Form.Files[0]);
            return Ok(response);
            
        }
    }
}