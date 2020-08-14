using Access2Justice.Api.Authorization;
using Access2Justice.Api.Interfaces;
using Access2Justice.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Access2Justice.Shared.Admin;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using static Access2Justice.Api.Authorization.Permissions;

namespace Access2Justice.Api.Controllers
{
    [ExcludeFromCodeCoverage]
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

        //[Permission(PermissionName.importa2jtemplate)]
        [HttpPost("curated-experience")]
        [SwaggerResponse(400, typeof(JsonUploadResult))]
        public async Task<IActionResult> UploadCuratedExperienceTemplate([FromForm] CuratedTemplate curatedTemplate)
        {
            try
            {
                if (!TryValidateModel(curatedTemplate))
                {
                    return BadRequest(new JsonUploadResult
                    {
                        Message = adminSettings.ModelStateInvalidMessage,
                        ErrorCode = JsonUploadError.ModelStateInvalid,
                        Details = JsonConvert.SerializeObject(
                            ModelState.Values
                                .SelectMany(x => x.Errors)
                                .Select(error => error.ErrorMessage))
                    });
                }
                var response = await adminBusinessLogic.UploadCuratedContentPackage(curatedTemplate);
                if (response!= null && response.ErrorCode == null)
                {
                    return Ok(response.Message);
                }
                return BadRequest(response);

            }
            catch (Exception ex)
            {
                return BadRequest(new JsonUploadResult
                {
                    Message = adminSettings.FailureMessage + ex.Message,
                    Details = ex.StackTrace,
                    ErrorCode = JsonUploadError.UnhandledError
                });
            }
        }
    }
}