using System.Threading.Tasks;
using Access2Justice.CosmosDb.Interfaces;
using Access2Justice.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

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

            var users = await userProfileBusinessLogic.GetUserProfileDataAsync(oid);
            return Ok(users);
        }
        
        /// <summary>
        /// Insert and Update the user profile personalized plan
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/user/upsetuserpersonalizedplan")]
        public async Task<IActionResult> UpSetUserPersonalizedPlanAsync([FromBody]dynamic userData)
        {
            var users = await userProfileBusinessLogic.UpSetUserPersonalizedPlanAsync(userData);
            return Ok(users);
        }
    }        
}