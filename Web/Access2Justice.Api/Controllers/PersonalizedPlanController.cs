using Access2Justice.Api.Interfaces;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Access2Justice.Api.Controllers
{
    [Route("api/[controller]")]
    public class PersonalizedPlanController : Controller
    {
        private readonly ICosmosDbSettings cosmosDbSettings;
        private readonly IBackendDatabaseService backendDatabaseService;
        private readonly IPersonalizedPlanViewModelMapper personalizedPlanViewModelMapper;

        public PersonalizedPlanController(ICosmosDbSettings cosmosDbSettings, IBackendDatabaseService backendDatabaseService, 
            IPersonalizedPlanViewModelMapper personalizedPlanViewModelMapper)
        {
            this.cosmosDbSettings = cosmosDbSettings;
            this.backendDatabaseService = backendDatabaseService;
            this.personalizedPlanViewModelMapper = personalizedPlanViewModelMapper;
        }

        [HttpGet("test-personalized-plan-ui")]
        public async Task<IActionResult> GetPersonalizedPlan()
        {
            var unprocessedPlan = await backendDatabaseService.GetItemAsync<UnprocessedPersonalizedPlan>("ee9a0e48-1855-e236-9f45-485f388a53ae",
                cosmosDbSettings.PersonalizedActionPlanCollectionId);
			return Ok(await personalizedPlanViewModelMapper.MapViewModel(unprocessedPlan));
        }
    }
}
