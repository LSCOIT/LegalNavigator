using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models.CuratedExperience;
using System;
using System.Threading.Tasks;

namespace Access2Justice.Api.BusinessLogic
{
    public class PersonalizedPlanBusinessLogic : IPersonalizedPlanBusinessLogic
    {
        private readonly ICosmosDbSettings dbSettings;
        private readonly IBackendDatabaseService dbService;
        private readonly ITopicsResourcesBusinessLogic topicsResourcesBusinessLogic;

        public PersonalizedPlanBusinessLogic(ICosmosDbSettings cosmosDbSettings, IBackendDatabaseService backendDatabaseService, 
            ITopicsResourcesBusinessLogic topicsResourcesBusinessLogic)
        {
            dbSettings = cosmosDbSettings;
            dbService = backendDatabaseService;
            this.topicsResourcesBusinessLogic = topicsResourcesBusinessLogic;
        }

        public async Task<PersonalizedActionPlanViewModel> GeneratePersonalizedPlan(CuratedExperienceTree curatedExperience, Guid answersDocId)
        {
            var temp = await dbService.GetItemAsync<string>("", "");
            return null;
        }
    }
}
