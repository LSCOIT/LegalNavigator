using Access2Justice.Api.Authorization;
using Access2Justice.Api.Interfaces;
using Access2Justice.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static Access2Justice.Api.Authorization.Permissions;

namespace Access2Justice.Api.Controllers
{
    [Route("api/admin")]
    public class AdminController : Controller
    {
        private IAdminBusinessLogic adminBusinessLogic;

        public AdminController(IAdminBusinessLogic adminBusinessLogic)
        {
            this.adminBusinessLogic = adminBusinessLogic;
        }

        [Permission(PermissionName.importa2jtemplate)]
        [HttpPost("upload-curated-experience-template")]
        public async Task<IActionResult> UploadCuratedExperienceTemplate([FromForm] CuratedTemplate curatedTemplate)
        {
            try
            {
                if (TryValidateModel(curatedTemplate))
                {
                    var response = await adminBusinessLogic.UploadCuratedContentPackage(curatedTemplate);
                    if (response != null)
                        return Ok();
                    else
                        return BadRequest("Upload failed.");
                }
                return BadRequest("Please provide the required fields.");
                
            }
            catch (Exception ex)
            {
                return BadRequest("Upload failed" + ex.Message);
            }
        }
    }
}