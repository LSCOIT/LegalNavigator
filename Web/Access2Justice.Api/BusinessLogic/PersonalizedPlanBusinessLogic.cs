using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.Azure.Documents;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Access2Justice.Api.BusinessLogic
{
    public class PersonalizedPlanBusinessLogic : IPersonalizedPlanBusinessLogic
    {
        private readonly ICosmosDbSettings cosmosDbSettings;
        private readonly IBackendDatabaseService backendDatabaseService;
        private readonly IDynamicQueries dynamicQueries;
        private readonly IUserProfileBusinessLogic userProfileBusinessLogic;
        private readonly IPersonalizedPlanEngine personalizedPlanEngine;
        private readonly IPersonalizedPlanViewModelMapper personalizedPlanViewModelMapper;

        public PersonalizedPlanBusinessLogic(ICosmosDbSettings cosmosDbSettings, IBackendDatabaseService backendDatabaseService,
            IDynamicQueries dynamicQueries, IUserProfileBusinessLogic userProfileBusinessLogic, IPersonalizedPlanEngine personalizedPlanEngine,
            IPersonalizedPlanViewModelMapper personalizedPlanViewModelMapper)
        {
            this.cosmosDbSettings = cosmosDbSettings;
            this.backendDatabaseService = backendDatabaseService;
            this.dynamicQueries = dynamicQueries;
            this.userProfileBusinessLogic = userProfileBusinessLogic;
            this.personalizedPlanEngine = personalizedPlanEngine;
            this.personalizedPlanViewModelMapper = personalizedPlanViewModelMapper;
        }

        public async Task<PersonalizedPlanViewModel> GeneratePersonalizedPlanAsync(CuratedExperience curatedExperience, Guid answersDocId)
        {
            var a2jPersonalizedPlan = await dynamicQueries.FindItemWhereAsync<JObject>(cosmosDbSettings.A2JAuthorDocsCollectionId, Constants.Id,
                curatedExperience.A2jPersonalizedPlanId.ToString());

            var userAnswers = await backendDatabaseService.GetItemAsync<CuratedExperienceAnswers>(answersDocId.ToString(),
                cosmosDbSettings.GuidedAssistantAnswersCollectionId);

            if (a2jPersonalizedPlan == null || userAnswers == null || userAnswers.AnswersDocId == default(Guid))
            {
                return null;
            }

            return await personalizedPlanViewModelMapper.MapViewModel(personalizedPlanEngine.Build(a2jPersonalizedPlan, userAnswers));
        }

        public async Task<PersonalizedPlanViewModel> GetPersonalizedPlanAsync(Guid personalizedPlanId)
        {
            return await backendDatabaseService.GetItemAsync<PersonalizedPlanViewModel>(personalizedPlanId.ToString(), cosmosDbSettings.ActionPlansCollectionId);
        }

        public async Task<Document> UpsertPersonalizedPlanAsync(PersonalizedPlanViewModel personalizedPlan)
        {
            try
            {
                var userPersonalizedPlan = await GetPersonalizedPlanAsync(personalizedPlan.PersonalizedPlanId);

                if (userPersonalizedPlan == null || userPersonalizedPlan?.PersonalizedPlanId == Guid.Empty)
                {
                    var newPlan = await backendDatabaseService.CreateItemAsync(personalizedPlan, cosmosDbSettings.ActionPlansCollectionId);
                    if (!Guid.TryParse(newPlan.Id, out Guid guid))
                    {
                        return null;
                    }
                    return newPlan;
                }
                else
                {
                    return await backendDatabaseService.UpdateItemAsync(
                        personalizedPlan.PersonalizedPlanId.ToString(), personalizedPlan, cosmosDbSettings.ActionPlansCollectionId);
                }
            }
            catch
            {
                // Todo: log exception
                return null;
            }
        }
    }
}