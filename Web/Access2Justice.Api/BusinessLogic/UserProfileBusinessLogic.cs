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

        public async Task<dynamic> GetUserProficeDataAsync(string oId)
        {
            return await dbClient.FindItemsWhereAsync(dbSettings.UserProfileCollectionId, Constants.OId, oId);
        }
        public async Task<dynamic> CreateUserProfileDataAsync(UserProfile userProfile)
        {
            List<dynamic> userprofiles = new List<dynamic>();
            string oId = GetUserProficeDataAsync(userProfile.OId).ToString();
            if (!oId.Contains(userProfile.OId))
            {
                var userprofileObjects = JsonConvert.SerializeObject(userProfile);
                var userDeserialisedObjects = JsonConvert.DeserializeObject(userprofileObjects);
                var result = await dbService.CreateItemAsync(userDeserialisedObjects, dbSettings.UserProfileCollectionId);
                userprofiles.Add(result);
            }
            return userprofiles;
        }

    }

  

 


}