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
        [Route("api/user/createuserpersonalizedplan/")]
        public async Task<IActionResult> CreateUserPersonalizedPlanAsync(dynamic userData)
        {
            var query = "select * from c where c.id = 'e3736497-7f7b-40e7-b388-0975603db857'";
            userData =await backendDatabaseService.QueryItemsAsync(cosmosDbSettings.ResourceCollectionId, query);
            var users = await userProfileBusinessLogic.CreateUserPersonalizedPlanAsync(userData);
            return Ok(users);

        }

    }
        
}