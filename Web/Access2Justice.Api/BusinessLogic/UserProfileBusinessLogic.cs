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
            var userDocument = JsonConvert.DeserializeObject(serializedResult);
            return await dbService.CreateItemAsync(userDocument, dbSettings.ResourceCollectionId);
        }

        public async Task<dynamic> UpdateUserPersonalizedPlanAsync(dynamic userUIData)
        {
            var serializedUIResult = JsonConvert.SerializeObject(userUIData);
            string oId = userUIData?.oId;
            string id = userUIData?.id;
            var userDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourceCollectionId, Constants.OId, oId);
            var serializedDBResult = JsonConvert.SerializeObject(userDBData[0]);
            JObject uiData = JObject.Parse(serializedUIResult);
            JObject dbData = JObject.Parse(serializedDBResult);
            var DbStepTags = from a in dbData["planTags"].Children() select a["stepTags"];
            var UiStepTags = from a in uiData["planTags"].Children() select a["stepTags"];
            var indexOfPlanTags = 0;
            foreach (var dbItem in DbStepTags)
            {
                var indexOfStepTags = 0;
                foreach (var item in UiStepTags)
                {
                    if ((dbData["planTags"][indexOfPlanTags]["stepTags"][indexOfStepTags]["id"]).Value<string>() == (uiData["planTags"][indexOfPlanTags]["stepTags"][indexOfStepTags]["id"]).Value<string>())
                    {
                        dbData["planTags"][indexOfPlanTags]["stepTags"][indexOfStepTags]["markCompleted"] = uiData["planTags"][indexOfPlanTags]["stepTags"][indexOfStepTags]["markCompleted"]
                            .Value<string>();
                    }
                    indexOfStepTags++;
                }
                indexOfPlanTags++;
            }
            return await dbService.UpdateItemAsync(id, dbData, dbSettings.ResourceCollectionId);
        }
    }
}