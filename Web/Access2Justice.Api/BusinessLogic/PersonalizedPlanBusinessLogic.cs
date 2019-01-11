using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Utilities;
using Microsoft.Azure.Documents;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

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

        public async Task<Document> UpsertPersonalizedPlanAsync(UserPlan userPlan)
        {
            try
            {
                PersonalizedPlanViewModel personalizedPlan = userPlan.PersonalizedPlan;
                string oId = userPlan.UserId;
                dynamic response = null;

                var userPersonalizedPlan = await GetPersonalizedPlanAsync(personalizedPlan.PersonalizedPlanId);

                if (userPersonalizedPlan == null || userPersonalizedPlan?.PersonalizedPlanId == Guid.Empty)
                {
                    var newPlan = await backendDatabaseService.CreateItemAsync(personalizedPlan, cosmosDbSettings.ActionPlansCollectionId);
                    if (!Guid.TryParse(newPlan.Id, out Guid guid))
                    {
                        return response;
                    }
                    response = newPlan;
                }
                else
                {
                    response = await backendDatabaseService.UpdateItemAsync(
                        personalizedPlan.PersonalizedPlanId.ToString(), personalizedPlan, cosmosDbSettings.ActionPlansCollectionId);
                }
                if (!userPlan.saveActionPlan)
                {
                    await UpdatePlanToProfile(personalizedPlan.PersonalizedPlanId, oId, personalizedPlan);
                }
                return response;
            }
            catch
            {
                return null;
            }
        }

        public async Task UpdatePlanToProfile(Guid planId, string oId, PersonalizedPlanViewModel personalizedPlan)
        {
            UserProfile userProfile = await this.userProfileBusinessLogic.GetUserProfileDataAsync(oId);
            if (userProfile?.PersonalizedActionPlanId != null && userProfile?.PersonalizedActionPlanId != Guid.Empty)
            {
                var userExistingPersonalizedPlan = await GetPersonalizedPlanAsync(userProfile.PersonalizedActionPlanId);
                var i = 0;
                bool isMatched = false;
                foreach (var planTopic in personalizedPlan.Topics)
                {
                    foreach (var topic in userExistingPersonalizedPlan.Topics.ToArray())
                    {
                        if (topic.TopicName == planTopic.TopicName)
                        {
                            isMatched = true;
                            break;
                        }
                        i++;
                    }
                    if (isMatched)
                    {
                        userExistingPersonalizedPlan.Topics[i] = planTopic;
                    }
                    else
                    {
                        userExistingPersonalizedPlan.Topics.Add(planTopic);
                    }
                }
                await backendDatabaseService.UpdateItemAsync(
                        userExistingPersonalizedPlan.PersonalizedPlanId.ToString(), userExistingPersonalizedPlan, cosmosDbSettings.ActionPlansCollectionId);
            }
            else
            {
                userProfile.PersonalizedActionPlanId = planId;
                await backendDatabaseService.UpdateItemAsync(userProfile.Id, userProfile, cosmosDbSettings.ProfilesCollectionId);
            }
        }
    }
}
