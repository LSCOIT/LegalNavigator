using System.Threading.Tasks;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;

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
        public async Task<IActionResult> GetUserProfileDataAsync(string oid)
        {
            var users = await userProfileBusinessLogic.GetUserResourceProfileDataAsync(oid);
            return Ok(users);
        }

        /// <summary>
        /// Create User Profile Document
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns>1-Success,0-Fail</returns>        
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
        /// <returns>1-Success,0-Fail</returns>
        [HttpPost]
        [Route("api/user/updateuserprofile")]
        public async Task<IActionResult> UpdateUserProfileDocumentAsync(string userIdGuid, UserProfile userProfile)
        {
            var profile = await userProfileBusinessLogic.UpdateUserProfileDataAsync(userProfile);
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

        [HttpPost]
        [Route("share")]
        public async Task<IActionResult> ShareAsync([FromQuery]Guid resourceGuid, [FromQuery]string oId)
        {
            var users = await userProfileBusinessLogic.ShareResourceDataAsync(resourceGuid, oId);
            return Ok(users);
        }

        [HttpPost]
        [Route("unshare")]
        public async Task<IActionResult> UnshareAsync([FromQuery]Guid resourceGuid, [FromQuery]string oId)
        {
            var users = await userProfileBusinessLogic.UnshareResourceDataAsync(resourceGuid, oId);
            return Ok(users);
        }
    }   
}