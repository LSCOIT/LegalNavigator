using System.Threading.Tasks;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
    public class UserProfileController : Controller
    {
        private readonly IUserProfileBusinessLogic userProfileBusinessLogic;

        public UserProfileController(IUserProfileBusinessLogic userProfileBusinessLogic)
        {
            this.userProfileBusinessLogic = userProfileBusinessLogic;
        }

        /// <summary>
        /// Get the user details by a user OId
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/user/getuserprofile/{oid}")]
        public async Task<IActionResult> GetUserProfileDataAsync(string oid, string collectionId)
        {
            var users = await userProfileBusinessLogic.GetUserProfileDataAsync(oid, collectionId);
            return Ok(users);
        }

        /// <summary>
        /// Create User Profile Document
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>        
        [HttpPost]
        [Route("api/user/createuserprofile")]
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
        [Route("api/user/updateuserprofile")]
        public async Task<IActionResult> UpdateUserProfileDocumentAsync(string userIdGuid, UserProfile userProfile)
        {
            var profile = await userProfileBusinessLogic.UpdateUserProfileDataAsync(userProfile, userIdGuid);
            return Ok(profile);
        }

        /// <summary>
        /// Insert and Update the user profile personalized plan
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/user/upsertuserpersonalizedplan")]
        public async Task<IActionResult> UpsertUserPersonalizedPlanAsync([FromBody]dynamic userData)
        {
            var users = await userProfileBusinessLogic.UpsertUserPersonalizedPlanAsync(userData);
            return Ok(users);
        }

        /// <summary>
        /// Insert and Update the user plan
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/user/upsertuserplan")]
        public async Task<IActionResult> UpsertUserPlanAsync([FromBody]dynamic userData)
        {
            var users = await userProfileBusinessLogic.UpsertUserPlanAsync(userData);
            return Ok(users);
        }
    }   
}