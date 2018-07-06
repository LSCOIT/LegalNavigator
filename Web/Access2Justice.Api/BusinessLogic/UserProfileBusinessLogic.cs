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

        public async Task<dynamic> GetUserProfileDataAsync(string oId,string collectionId)
        {
            return await dbClient.FindItemsWhereAsync(collectionId, Constants.OId, oId);
        }

        public async Task<dynamic> CreateUserProfileDataAsync(UserProfile userProfile)
        {
            var userprofiles = new List<dynamic>();
            var resultUP = GetUserProfileDataAsync(userProfile.OId, dbSettings.UserProfileCollectionId);
            var userprofileObjects = JsonConvert.SerializeObject(resultUP);
            if (!userprofileObjects.Contains(userProfile.OId))
            {
                var result = await dbService.CreateItemAsync(ResourceDeserialized(userProfile), dbSettings.UserProfileCollectionId);
                userprofiles.Add(result);
            }
            return userprofiles;
        }
        public async Task<dynamic> UpdateUserProfileDataAsync(UserProfile userProfile, string userIdGuid)
        {
            var userprofiles = new List<dynamic>();
            
            var resultUP = GetUserProfileDataAsync(userProfile.OId, dbSettings.UserProfileCollectionId);
            var userprofileObjects = JsonConvert.SerializeObject(resultUP);
            
            if (userprofileObjects.Contains(userProfile.OId)) // condition to verify oId and update the details
            {
                userProfile.Id = userIdGuid; // guid id of the document
               
                var result = await dbService.UpdateItemAsync(userIdGuid, ResourceDeserialized(userProfile), dbSettings.UserProfileCollectionId);
                userprofiles.Add(result);
            }
            return userprofiles;
        }
        private object ResourceDeserialized(UserProfile userProfile)
        {
            var serializedResult = JsonConvert.SerializeObject(userProfile);
            return JsonConvert.DeserializeObject<object>(serializedResult);
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
            var userUIDocument = JsonConvert.DeserializeObject(serializedResult);
            string oId = userUIDocument.oId;
            string planId = userUIDocument.planId;
            List<string> propertyNames = new List<string>() { Constants.OId, Constants.PlanId };
            List<string> values = new List<string>() { oId, planId };
            var userDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourceCollectionId, propertyNames, values);
            var serializedDBResult = JsonConvert.SerializeObject(userDBData[0]);
            JObject dbObject = JObject.Parse(serializedDBResult);
            JObject uiObject = JObject.Parse(serializedResult);

            foreach (var prop in uiObject.Properties())
            {
                var targetProperty = dbObject.Property(prop.Name);
                if (targetProperty == null)
                {
                    dbObject.Add(prop.Name, prop.Value);
                }
                else
                {
                    targetProperty.Value = prop.Value;
                }
            }            
            return await dbService.UpdateItemAsync(id, dbObject, dbSettings.ResourceCollectionId);
        }
    }
}