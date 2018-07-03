﻿using Access2Justice.CosmosDb.Interfaces;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Api.BusinessLogic
{
    public class UserProfileBusinessLogic : IUserProfileBusinessLogic
    {
        private readonly IDynamicQueries dbClient;
        private readonly ICosmosDbSettings dbSettings;
        private readonly IBackendDatabaseService dbService;
        public UserProfileBusinessLogic(IDynamicQueries dynamicQueries, ICosmosDbSettings cosmosDbSettings, IBackendDatabaseService backendDatabaseService)
        {
            dbClient = dynamicQueries;
            dbSettings = cosmosDbSettings;
            dbService = backendDatabaseService;
        }

        public async Task<dynamic> GetUserProfileDataAsync(string oId)
        {
            return await dbClient.FindItemsWhereAsync(dbSettings.ResourceCollectionId, Constants.OId, oId);
        }
                
        public async Task<dynamic> CreateUserProfileDataAsync(UserProfile userProfile)
        {
            List<dynamic> userprofiles = new List<dynamic>();

            var resultUP = GetUserProfileDataAsync(userProfile.OId);
            var userprofileObjects = JsonConvert.SerializeObject(resultUP);
            if (!userprofileObjects.Contains(userProfile.OId))
            {
                var userDeserialisedObjects = JsonConvert.DeserializeObject(userprofileObjects);
                var result = await dbService.CreateItemAsync(userDeserialisedObjects, dbSettings.UserProfileCollectionId);
                userprofiles.Add(result);
            }
            return userprofiles;
        }

        public async Task<dynamic> UpsertUserPersonalizedPlanAsync(dynamic userData)
        {
            var serializedResult = JsonConvert.SerializeObject(userData);
            var userDocument = JsonConvert.DeserializeObject<object>(serializedResult);
            string oId = userDocument.oId;
            dynamic result = null;
            var userDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourceCollectionId, Constants.OId, oId);

            if (userDBData.Count == 0)
            {
                result = CreateUserPersonalizedPlanAsync(userData);
            }
            else
            {
                result = UpdateUserPersonalizedPlanAsync(userData);
            }
            return result;
        }

        public async Task<dynamic> CreateUserPersonalizedPlanAsync(dynamic userData)
        {
            var serializedResult = JsonConvert.SerializeObject(userData);
            var userDocument = JsonConvert.DeserializeObject<object>(serializedResult);
            var result = await dbService.CreateItemAsync(userDocument, dbSettings.ResourceCollectionId);
            return result;
        }

        public async Task<dynamic> UpdateUserPersonalizedPlanAsync(dynamic userUIData)
        {
            var serializedResult = JsonConvert.SerializeObject(userUIData);
            var userUIDocument = JsonConvert.DeserializeObject<dynamic>(serializedResult);
            string oId = userUIDocument?.oId;
            string id = userUIDocument?.id;
            var userDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourceCollectionId, Constants.OId, oId);
            var serializedDBResult = JsonConvert.SerializeObject(userDBData);
            var userDBDocument = JsonConvert.DeserializeObject<dynamic>(serializedDBResult);            

            for (int planTagsIterator = 0; planTagsIterator < userUIDocument.planTags.Count; planTagsIterator++)
            {
                for (int stepsIterator = 0; stepsIterator < userUIDocument.planTags[planTagsIterator].steps.Count; stepsIterator++)
                {
                    bool uiMarkCompleted = userUIDocument?.planTags[planTagsIterator].steps[stepsIterator].markCompleted;
                    bool dbMarkCompleted = userDBDocument[0]?.planTags[planTagsIterator].steps[stepsIterator].markCompleted;

                    if ((uiMarkCompleted.Equals(true)) && (dbMarkCompleted.Equals(false)))
                    {
                        userDBDocument[0].planTags[planTagsIterator].steps[stepsIterator].markCompleted = userUIDocument.planTags[planTagsIterator].steps[stepsIterator].markCompleted;
                    }
                }
            }            
            var result = await dbService.UpdateItemAsync(id, userDBDocument[0], dbSettings.ResourceCollectionId);
            return result;
        }
    }
}