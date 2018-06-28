using System.Threading.Tasks;
using Access2Justice.CosmosDb.Interfaces;
using Access2Justice.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Access2Justice.Api.Controllers
{
    [Produces("application/json")]
    public class UserProfileController : Controller
    {
        private readonly IUserProfileBusinessLogic userProfileBusinessLogic;
        private readonly IBackendDatabaseService backendDatabaseService;
        private readonly ICosmosDbSettings cosmosDbSettings;

        public UserProfileController(IUserProfileBusinessLogic userProfileBusinessLogic, IBackendDatabaseService backendDatabaseService, ICosmosDbSettings cosmosDbSettings)
        {
            this.userProfileBusinessLogic = userProfileBusinessLogic;
            this.backendDatabaseService = backendDatabaseService;
            this.cosmosDbSettings = cosmosDbSettings;

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
        /// Create the user profile personalized plan
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/user/createuserprofile/")]
        public async Task<IActionResult> CreateUserProfileDataAsync(dynamic userData)
        {
            var query = "select * from c where c.id = 'bf8d7e7e-2574-7b39-efc7-83cb94adae07'";
            userData = backendDatabaseService.QueryItemsAsync(cosmosDbSettings.UserProfileCollectionId, query);
            var users = await userProfileBusinessLogic.CreateUserProfileDataAsync(userData);
            return Ok(users);
        }

    }
        
}