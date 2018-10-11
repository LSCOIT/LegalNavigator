using Access2Justice.Api.Interfaces;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var unprocessedPlan = await backendDatabaseService.GetItemAsync<UnprocessedPersonalizedPlan>("31aabd1e-db81-4079-ad8d-7b30f3f4fee1",
                cosmosDbSettings.PersonalizedActionPlanCollectionId);

            return Ok(personalizedPlanViewModelMapper.MapViewModel(unprocessedPlan));
        }
    }
}
