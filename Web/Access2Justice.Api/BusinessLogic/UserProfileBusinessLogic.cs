using Access2Justice.CosmosDb.Interfaces;
using Access2Justice.Shared.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public async Task<dynamic> CreateUserPersonalizedPlanAsync(dynamic userData)
        {
            var serializedResult = JsonConvert.SerializeObject(userData);
            var userDocument = JsonConvert.DeserializeObject<object>(serializedResult);
            var result = await dbService.CreateItemAsync(userDocument[0], dbSettings.UserProfileCollectionId);
            return result;
        }

        public async Task<dynamic> UpdateUserPersonalizedPlanAsync(dynamic userUIData)
        {
            var serializedResult = JsonConvert.SerializeObject(userUIData);
            var userUIDocument = JsonConvert.DeserializeObject<object>(serializedResult);
            string oId = userUIDocument[0].oId;            
            var userDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourceCollectionId, Constants.OId, oId);
            var serializedDBResult = JsonConvert.SerializeObject(userDBData);
            var userDBDocument = JsonConvert.DeserializeObject<object>(serializedDBResult);

            for (int topicTagIterator= 0; topicTagIterator < userUIDocument[0].topicTags.Count; topicTagIterator++)
            {
                for (int stepTagIterator = 0; stepTagIterator < userUIDocument[0].topicTags[topicTagIterator].stepTags.Count; stepTagIterator++)
                {
                    bool uiMarkCompleted = userUIDocument[0].topicTags[topicTagIterator].stepTags[stepTagIterator].markCompleted;
                    bool dbMarkCompleted = userDBDocument[0].topicTags[topicTagIterator].stepTags[stepTagIterator].markCompleted;

                    if ((uiMarkCompleted.Equals(true)) && (dbMarkCompleted.Equals(false)))
                    {
                        userDBDocument[0].topicTags[topicTagIterator].stepTags[stepTagIterator].markCompleted = userUIDocument[0].topicTags[topicTagIterator].stepTags[stepTagIterator].markCompleted;
                    }
                }
            }
            string id = userUIDocument[0].id;
            var result = await dbService.UpdateItemAsync(id, userDBDocument[0], dbSettings.ResourceCollectionId);
            return result;
        }
    }
}