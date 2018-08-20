using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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
        [HttpPost]
        [Route("api/user/getuserprofile")]
        public async Task<IActionResult> GetUserDataAsync(string oid, string type)
        {
            var users = await userProfileBusinessLogic.GetUserResourceProfileDataAsync(oid, type);
            return Ok(users);
        }

        /// <summary>
        /// Get the user details by a user OId
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/user/getuserprofiledata/{oid}")]
        public async Task<IActionResult> GetUserProfileDataAsync(string oid)
        {
            UserProfile users = await userProfileBusinessLogic.GetUserProfileDataAsync(oid);
            return Ok(users);
        }

        /// <summary>
        /// Update User Profile Document
        /// </summary>
        /// <param name="oId"></param>
        /// <param name="userProfile"></param>
        /// <returns>1-Success,0-Fail</returns>
        [HttpPost]
        [Route("api/user/updateuserprofile")]
        public async Task<IActionResult> UpdateUserProfileDocumentAsync(string oId, Guid planId)
        {
            var profile = await userProfileBusinessLogic.UpdateUserProfilePlanIdAsync(oId, planId);
            return Ok(profile);
        }

        /// <summary>
        /// Insert and Update the user profile personalized plan
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/user/upsertuserpersonalizedplan")]
        public async Task<IActionResult> UpsertUserPersonalizedPlanAsync([FromBody]ProfileResources profileResources)
        {
            var users = await userProfileBusinessLogic.UpsertUserSavedResourcesAsync(profileResources);
            return Ok(users);
        }
    }
}