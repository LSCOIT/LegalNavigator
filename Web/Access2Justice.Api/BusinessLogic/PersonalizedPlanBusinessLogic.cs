﻿using Access2Justice.Api.Interfaces;
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

        public async Task<PersonalizedPlanViewModel> GeneratePersonalizedPlanAsync(CuratedExperience curatedExperience, Guid answersDocId, string userId = null)
        {
            if (curatedExperience is null)
            {
                return null;
            }
            var a2jPersonalizedPlan = await dynamicQueries.FindItemWhereAsync<JObject>(cosmosDbSettings.A2JAuthorDocsCollectionId, Constants.Id,
                curatedExperience.A2jPersonalizedPlanId.ToString());

            var userAnswers = await backendDatabaseService.GetItemAsync<CuratedExperienceAnswers>(answersDocId.ToString(),
                cosmosDbSettings.GuidedAssistantAnswersCollectionId);

            if (a2jPersonalizedPlan == null || userAnswers == null || userAnswers.AnswersDocId == default(Guid))
            {
                return null;
            }

            var plan = await personalizedPlanViewModelMapper.MapViewModel(
                personalizedPlanEngine.Build(a2jPersonalizedPlan, userAnswers));

            if (plan == null)
            {
                return null;
            }

            plan.AnswersBatchId = answersDocId.ToString();
            plan.CuratedExperienceId = curatedExperience.CuratedExperienceId.ToString();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return plan;
            }

            var userProfile = await userProfileBusinessLogic.GetUserProfileDataAsync<UserProfile>(userId);
            if (userProfile == null || userProfile.CuratedExperienceAnswersId == Guid.Empty)
            {
                return plan;
            }
            userProfile.CuratedExperienceAnswersId = Guid.Empty;
            backendDatabaseService.UpdateItemAsync(userProfile.Id, userProfile, cosmosDbSettings.ProfilesCollectionId);

            return plan;
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

        public async Task UpsertExternalPersonalizedPlanAsync(PersonalizedPlanViewModel personalizedPlan, string oId)
        {
            await UpdatePlanToProfile(personalizedPlan.PersonalizedPlanId, oId, personalizedPlan);
        }

        public async Task UpdatePlanToProfile(Guid planId, string oId, PersonalizedPlanViewModel personalizedPlan)
        {
            UserProfile userProfile = await userProfileBusinessLogic.GetUserProfileDataAsync(oId);
            if (userProfile?.PersonalizedActionPlanId != null && userProfile?.PersonalizedActionPlanId != Guid.Empty)
            {
                var userExistingPersonalizedPlan = await GetPersonalizedPlanAsync(userProfile.PersonalizedActionPlanId);
                var existingTopicNames = userExistingPersonalizedPlan.Topics.Select(x => x.TopicName);
                for (int i = 0; i < personalizedPlan.Topics.Count; i++)
                {
                    if (!existingTopicNames.Contains(personalizedPlan.Topics[i].TopicName))
                    {
                        userExistingPersonalizedPlan.Topics.Add(personalizedPlan.Topics[i]);      
                    }
                    else
                    {
                        for(int j = 0; j < userExistingPersonalizedPlan.Topics.Count;j++)
                        {
                            if(userExistingPersonalizedPlan.Topics[j].TopicName == personalizedPlan.Topics[i].TopicName)
                            {
                                userExistingPersonalizedPlan.Topics[j] = personalizedPlan.Topics[i];
                            }
                        }
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

        public async Task<List<PlanTopic>> GetPlanTopicsAsync(List<string> planIds)
        {
            var topicsPlans = await backendDatabaseService.GetItemsAsync<PlanTopic>(x => planIds.Contains(x.TopicId.ToString()), cosmosDbSettings.ActionPlansCollectionId);
            return topicsPlans.ToList();
        }
    }
}
