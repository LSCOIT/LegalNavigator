﻿using Access2Justice.Api.Authorization;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using static Access2Justice.Api.Authorization.Permissions;
using Pomelo.AntiXSS;

namespace Access2Justice.Api.Controllers
{
    [ExcludeFromCodeCoverage]
    [Produces("application/json")]
    [Route("api/user")]
    public class UserProfileController : Controller
    {
        private readonly IUserProfileBusinessLogic userProfileBusinessLogic;

        [ExcludeFromCodeCoverage]
        public UserProfileController(IUserProfileBusinessLogic userProfileBusinessLogic)
        {
            this.userProfileBusinessLogic = userProfileBusinessLogic;
        }

        /// <summary>
        /// Get the user resource and plan details by a user OId
        /// </summary>
        /// <remarks>
        /// Helps to get user resource and plan details by user id
        /// </remarks>
        /// <param name="oid"></param>
        /// <param name="type"></param>
        /// <response code="200">Get user resource and plan details for given id</response>
        /// <response code="500">Failure</response>
        [ExcludeFromCodeCoverage]
        [HttpPost]
        [Route("profile")]
        public async Task<IActionResult> GetUserDataAsync(string oid, string type)
        {
            oid = Instance.Sanitize(oid);
            type = Instance.Sanitize(type);
            return Ok(await userProfileBusinessLogic.GetUserResourceProfileDataAsync(oid, type));
        }

        /// <summary>
        /// Get the user details by a user OId
        /// </summary>
        /// <remarks>
        /// Helps to get user details by user id
        /// </remarks>
        /// <param name="oid"></param>
        /// <response code="200">Get user details for given id</response>
        /// <response code="500">Failure</response>
        [ExcludeFromCodeCoverage]
        [Permission(PermissionName.getuserprofiledata)]
        [HttpGet]
        [Route("profile/{oid}")]
        public async Task<IActionResult> GetUserProfileDataAsync(string oid)
        {
            oid = Instance.Sanitize(oid);
            return Ok(await userProfileBusinessLogic.GetUserProfileDataAsync(oid));
        }

        /// <summary>
        /// Insert and Update the user profile personalized plan
        /// </summary>
        /// <remarks>
        /// Helps to create and update user profile personalized plans
        /// </remarks>
        /// <param name="profileResources"></param>
        /// <response code="200">Get user personalized plan created or updated</response>
        /// <response code="500">Failure</response>
        [ExcludeFromCodeCoverage]
        [Permission(PermissionName.upsertuserpersonalizedplan)]
        [HttpPost]
        [Route("personalized-plan/upsert")]
        public async Task<IActionResult> UpsertUserPersonalizedPlanAsync([FromBody]ProfileResources profileResources)
        {
            if (profileResources != null)
            {
                return Ok(await userProfileBusinessLogic.UpsertUserSavedResourcesAsync(profileResources));
            }
            return StatusCode(400);
        }

        /// <summary>
        /// Deletes a resource from user's profile
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="resource"></param>
        /// <response code="200">Get user's profile resources</response>
        /// <response code="500">Failure</response>
        [ExcludeFromCodeCoverage]
        //[Permission(PermissionName.upsertuserpersonalizedplan)]
        [HttpPost]
        [Route("resources/delete")]
        public async Task<IActionResult> DeleteUserProfileResourceAsync([FromBody]UserProfileResource resource)
        {
            if (resource != null)
            {
                return Ok(await userProfileBusinessLogic.DeleteUserProfileResourceAsync(resource));
            }
            return StatusCode(400);
        }

        /// <summary>
        ///  Upsert user profile
        /// </summary>
        /// <remarks>
        /// Helps to upsert user profile
        /// </remarks>
        /// <param name="userProfile"></param>
        /// <response code="200">Get user profile</response>
        /// <response code="404">Nor found</response>
        /// <response code="500">Failure</response>
        [ExcludeFromCodeCoverage]
        [HttpPost]
        [Route("profile/upsert")]
        public async Task<IActionResult> UpsertUserProfile([FromBody]UserProfile userProfile)
        {
            if (userProfile != null)
            {
                return Ok(await userProfileBusinessLogic.UpsertUserProfileAsync(userProfile));
            }
            return StatusCode(400);
        }
    }
}
