using Access2Justice.CosmosDb.Interfaces;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var userprofiles = new List<dynamic>();
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
            var userDocument = JsonConvert.DeserializeObject(serializedResult);
            string oId = userDocument.oId;
            string planId = userDocument.planId;
            List<string> propertyNames = new List<string>() { Constants.OId, Constants.PlanId };
            List<string> values = new List<string>() { oId, planId };
            dynamic result = null;
            var userDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourceCollectionId, propertyNames, values);           
            if (userDBData.Count == 0)
            {
                result = CreateUserPersonalizedPlanAsync(userData);
            }
            else
            {
                string id = userDBData[0].id;
                result = UpdateUserPersonalizedPlanAsync(id, userData);
            }
            return result;
        }

        public async Task<dynamic> CreateUserPersonalizedPlanAsync(dynamic userData)
        {
            var serializedResult = JsonConvert.SerializeObject(userData);
            var userDocument = JsonConvert.DeserializeObject(serializedResult);
            return await dbService.CreateItemAsync(userDocument, dbSettings.ResourceCollectionId);
        }

        public async Task<dynamic> UpdateUserPersonalizedPlanAsync(string id, dynamic userUIData)
        {
            var serializedResult = JsonConvert.SerializeObject(userUIData);
            var userDocument = JsonConvert.DeserializeObject(serializedResult);
            return await dbService.UpdateItemAsync(id, userDocument, dbSettings.ResourceCollectionId);
        }

        public async void DeleteUserPersonalizedPlanAsync(dynamic userUIData)
        {
            var serializedResult = JsonConvert.SerializeObject(userUIData);
            var userDocument = JsonConvert.DeserializeObject(serializedResult);
            string oId = userDocument.oId;
            string planId = userDocument.planId;
            List<string> propertyNames = new List<string>() { Constants.OId, Constants.PlanId };
            List<string> values = new List<string>() { oId, planId };
            var userDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourceCollectionId, propertyNames, values);
            string id = userDBData[0].id;
            await dbService.DeleteItemAsync(id, dbSettings.ResourceCollectionId);
        }
    }
}