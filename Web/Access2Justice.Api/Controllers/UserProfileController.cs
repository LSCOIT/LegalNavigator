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
        /// Get the user details by a user Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/user/getuserprofile/{id}")]
        public async Task<IActionResult> GetUserProfileDataAsync(string id)
        {

            var users = await UserProfileBusinessLogic.GetUserProficeDataAsync(id);
            return Ok(users);
        }
    }
        
}