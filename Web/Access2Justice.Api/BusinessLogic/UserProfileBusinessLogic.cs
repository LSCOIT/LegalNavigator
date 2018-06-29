using Access2Justice.CosmosDb.Interfaces;
using Access2Justice.Shared.Interfaces;
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
        public async Task<dynamic> CreateUserProfileDataAsync(dynamic userProfile)
        {
            List<dynamic> userprofiles = new List<dynamic>();
            var userprofileObjects = JsonConvert.SerializeObject(userProfile);
            var userDeserialisedObjects = JsonConvert.DeserializeObject(userprofileObjects);
            var result = await dbService.CreateItemAsync(userDeserialisedObjects, dbSettings.UserProfileCollectionId);
            userprofiles.Add(result);
            return userprofiles;
        }
    }

    public class UserProfile
    {
        [JsonProperty(PropertyName = "id")]
        public dynamic Id { get; set; }

        [JsonProperty(PropertyName = "oId")]
        public string OId { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "eMail")]
        public string EMail { get; set; }
    }


}