using System.Threading.Tasks;
using Access2Justice.Api.BusinessLogic;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
    public class UserProfileController : Controller
    {
        private readonly IUserProfileBusinessLogic userProfileBusinessLogic;

        public UserProfileController(IUserProfileBusinessLogic UserProfileBusinessLogic)
        {
            this.userProfileBusinessLogic = UserProfileBusinessLogic;
        }

        /// <summary>
        /// Get the user details by a user OId
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/user/getuserprofile/{oid}")]
        public async Task<IActionResult> GetUserProfileDataAsync(string oid)
        {

            var users = await userProfileBusinessLogic.GetUserProfileDataAsync(oid);
            return Ok(users);
        }

        /// <summary>
        /// Create User Profile Document
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/user/createuserprofiledocument")]
        public async Task<IActionResult> CreateUserProfileDocumentAsync(UserProfile userProfile)
        {
            var profile = await userProfileBusinessLogic.CreateUserProfileDataAsync(userProfile);
            return Ok(profile);
        }

        /// <summary>
        /// Update User Profile Document
        /// </summary>
        /// <param name="oId"></param>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/user/updateuserprofiledocument")]
        public async Task<IActionResult> UpdateUserProfileDocumentAsync(string userIdGuid, UserProfile userProfile)
        {
            var profile = await userProfileBusinessLogic.UpdateUserProfileDataAsync(userProfile, userIdGuid);
            return Ok(profile);
        }

    }

}