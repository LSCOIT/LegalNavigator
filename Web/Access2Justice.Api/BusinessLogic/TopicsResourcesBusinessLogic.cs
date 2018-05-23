using Access2Justice.CosmosDb.Interfaces;
using Access2Justice.Shared.Interfaces;
using System.Threading.Tasks;

namespace Access2Justice.Api.BusinessLogic
{
    public class TopicsResourcesBusinessLogic : ITopicsResourcesBusinessLogic
    {
        private readonly IBackendDatabaseService _backendDatabaseService;
        private readonly ICosmosDbSettings _cosmosDbSettings;
        public TopicsResourcesBusinessLogic(IBackendDatabaseService backendDatabaseService, ICosmosDbSettings cosmosDbSettings)
        {
            _backendDatabaseService = backendDatabaseService;
            _cosmosDbSettings = cosmosDbSettings;
        }

        public async Task<dynamic> GetResourcesAsync(string topicIds)
        {
            // we need to use a query format to retrieve items because we are returning a dynamic object.
            var query = string.Format("SELECT c.name,c.id,c.type,c.resourceType,c.externalUrl,c.url,c.location,c.overview,c.IsRecommened FROM c JOIN a IN c.topicTags WHERE a.id IN({0})", topicIds);
            var result = await _backendDatabaseService.QueryItemsAsync(_cosmosDbSettings.ResourceCollectionId, query);

            return result;
        }

        public async Task<dynamic> GetTopicAsync(string keyword)
        {
            // we need to use a query format to retrieve items because we are returning a dynamic object.
            var query = string.Format("SELECT * FROM c WHERE CONTAINS(c.keywords, '{0}')", keyword.ToLower());
            var result = await _backendDatabaseService.QueryItemsAsync(_cosmosDbSettings.TopicCollectionId, query);

            return result;
        }
    }
}
