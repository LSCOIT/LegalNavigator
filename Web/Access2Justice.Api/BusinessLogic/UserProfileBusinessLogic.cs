using Access2Justice.CosmosDb.Interfaces;
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
            return await dbClient.FindItemsWhereAsync(dbSettings.UserProfileCollectionId, Constants.OId, oId);
        }
        public async Task<dynamic> CreateUserProfileDataAsync(UserProfile userProfile)
        {
            List<dynamic> userprofiles = new List<dynamic>();

            var resultUP = GetUserProfileDataAsync(userProfile.OId);
            var userprofileObjects = JsonConvert.SerializeObject(resultUP);
            if (!userprofileObjects.Contains(userProfile.OId))
            {
                var serializedResult = JsonConvert.SerializeObject(userProfile);
                var resourceDocument = JsonConvert.DeserializeObject<object>(serializedResult);               
                var result = await dbService.CreateItemAsync(resourceDocument, dbSettings.UserProfileCollectionId);
                userprofiles.Add(result);
            }
            return userprofiles;
        }
        public async Task<dynamic> UpdateUserProfileDataAsync(UserProfile userProfile, string userIdGuid)
        {
            List<dynamic> userprofiles = new List<dynamic>();
            
            var resultUP = GetUserProfileDataAsync(userProfile.OId);
            var userprofileObjects = JsonConvert.SerializeObject(resultUP);
            
            if (userprofileObjects.Contains(userProfile.OId)) // condition to verify oId and update the details
            {
                userProfile.Id = userIdGuid; // guid id of the document
                var serializedResult = JsonConvert.SerializeObject(userProfile);
                var resourceDocument = JsonConvert.DeserializeObject<object>(serializedResult);
               
                var result = await dbService.UpdateItemAsync(userIdGuid, resourceDocument, dbSettings.UserProfileCollectionId);
                userprofiles.Add(result);
            }
            return userprofiles;
        }


    }
}