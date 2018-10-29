using Access2Justice.Api.Interfaces;
using Access2Justice.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        //[Permission(PermissionName.importa2jtemplate)]
        [HttpPost("upload-curated-experience-template")]
        public async Task<IActionResult> UploadCuratedExperienceTemplate([FromForm] CuratedTemplate curatedTemplate)
        {
            var response = await adminBusinessLogic.UploadCuratedContentPackage(curatedTemplate);
            return Ok(response);
        }
    }
}