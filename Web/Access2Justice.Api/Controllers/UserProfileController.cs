using System.Threading.Tasks;
using Access2Justice.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
    public class UserProfileController : Controller
    {
        private readonly IUserProfileBusinessLogic UserProfileBusinessLogic;

        public UserProfileController(IUserProfileBusinessLogic UserProfileBusinessLogic)
        {
            this.UserProfileBusinessLogic = UserProfileBusinessLogic;
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

            var users = await UserProfileBusinessLogic.GetUserProfileDataAsync(oid);
            return Ok(users);
        }

    }
        
}