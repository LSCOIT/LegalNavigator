using Access2Justice.Api.Authorization;
using Access2Justice.Api.Interfaces;
using Access2Justice.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using System;
using System.Threading.Tasks;
using static Access2Justice.Api.Authorization.Permissions;

namespace Access2Justice.Api.Controllers
{
    [Route("api/admin")]
    public class AdminController : Controller
    {
        private IAdminBusinessLogic adminBusinessLogic;
        private IAdminSettings adminSettings;

        public AdminController(IAdminBusinessLogic adminBusinessLogic, IAdminSettings adminSettings)
        {
            this.adminBusinessLogic = adminBusinessLogic;
            this.adminSettings = adminSettings;
        }

        [Permission(PermissionName.importa2jtemplate)]
        [HttpPost("curated-experience")]
        public async Task<IActionResult> UploadCuratedExperienceTemplate([FromForm] CuratedTemplate curatedTemplate)
        {
            try
            {
                if (TryValidateModel(curatedTemplate))
                {
                    var response = await adminBusinessLogic.UploadCuratedContentPackage(curatedTemplate);
                    if (response != null && response.ToString() == adminSettings.SuccessMessage)
                        return Ok(response);
                    else
                        return BadRequest(response);
                }
                return BadRequest(adminSettings.ValidationMessage);
                
            }
            catch (Exception ex)
            {
                return BadRequest(adminSettings.FailureMessage + ex.Message);
            }
        }
    }
}