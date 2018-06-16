using Access2Justice.CosmosDb.Interfaces;
using Access2Justice.Shared.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Api.BusinessLogic
{
    public class TopicsResourcesBusinessLogic : ITopicsResourcesBusinessLogic
    {
        private readonly IDynamicQueries dbClient;
        private readonly ICosmosDbSettings dbSettings;
        public TopicsResourcesBusinessLogic(IDynamicQueries dynamicQueries, ICosmosDbSettings cosmosDbSettings)
        {
            dbClient = dynamicQueries;
            dbSettings = cosmosDbSettings;
        }

        public async Task<dynamic> GetResourcesAsync(dynamic topics)
        {
            var ids = new List<string>();
            foreach (var topic in topics)
            {
                ids.Add(topic.id);
            }

            return await dbClient.FindItemsWhereArrayContainsAsync(dbSettings.ResourceCollectionId, "topicTags", "id", ids);
        }

        public async Task<dynamic> GetTopicsAsync(string keyword)
        {
            return await dbClient.FindItemsWhereContainsAsync(dbSettings.TopicCollectionId, "keywords", keyword);
        }

        public async Task<dynamic> GetTopLevelTopicsAsync()
        {
            return await dbClient.FindItemsWhereAsync(dbSettings.TopicCollectionId, "parentTopicID", "");
        }

        public async Task<dynamic> GetSubTopicsAsync(string ParentTopicId)
        {
            return await dbClient.FindItemsWhereAsync(dbSettings.TopicCollectionId, "parentTopicID", ParentTopicId);
        }

        public async Task<dynamic> GetResourceAsync(string ParentTopicId)
        {
            return await dbClient.FindItemsWhereArrayContainsAsync(dbSettings.ResourceCollectionId, "topicTags", "id", ParentTopicId);
        }

        public async Task<dynamic> GetDocumentAsync(string id)
        {
            return await dbClient.FindItemsWhereAsync(dbSettings.TopicCollectionId, "id", id);
        }

        public async Task<dynamic> GetBreadCrumbDataByIdAsync(string id)
        {
            return await dbClient.ExecuteStoredProcedureAsync(id);
        }
    }
}