using Access2Justice.Api.Authorization;
using Access2Justice.Api.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Access2Justice.Api.Authorization.Permissions;

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

        [Permission(PermissionName.importa2jtemplate)]
        [HttpPost("upload-curated-experience-template")]
        public async Task<IActionResult> UploadCuratedExperienceTemplate()
        {
            List<IFormFile> files = new List<IFormFile>();
            foreach(var file in Request.Form.Files)
            {
                files.Add(file);
            }

            if (files.Count > 0)
            {
                var response = await adminBusinessLogic.UploadCuratedContentPackage(files);
                return Ok(response);
                
            }
            return Ok("Provide file to upload");
        }
    }
}