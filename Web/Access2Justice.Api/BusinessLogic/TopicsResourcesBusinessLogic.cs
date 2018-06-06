using Access2Justice.CosmosDb.Interfaces;
using Access2Justice.Shared.Interfaces;
using System.Globalization;
using System.Threading.Tasks;

namespace Access2Justice.Api.BusinessLogic
{
    public class TopicsResourcesBusinessLogic : ITopicsResourcesBusinessLogic
    {
        private readonly IBackendDatabaseService backendDatabaseService;
        private readonly ICosmosDbSettings cosmosDbSettings;
        public TopicsResourcesBusinessLogic(IBackendDatabaseService backendDatabaseService, ICosmosDbSettings cosmosDbSettings)
        {
            this.backendDatabaseService = backendDatabaseService;
            this.cosmosDbSettings = cosmosDbSettings;
        }

        public async Task<dynamic> GetResourcesAsync(string topicIds)
        {
            var query = string.Format(CultureInfo.InvariantCulture, "SELECT * FROM c  WHERE {0}", topicIds);
            var result = await backendDatabaseService.QueryItemsAsync(cosmosDbSettings.ResourceCollectionId, query);

            return result;

            // return await FindItemsWhere(cosmosDbSettings.ResourceCollectionId, topicIds);
        }


        //public async Task<dynamic> FindItemsWhere(string collectionId, string condition)
        //{

        //    var query = string.Format(CultureInfo.InvariantCulture, "SELECT * FROM c  WHERE {0}", condition);
        //    var result = await backendDatabaseService.QueryItemsAsync(cosmosDbSettings.ResourceCollectionId, query);

        //    return result;
        //}

        public async Task<dynamic> GetTopicAsync(string keyword)
        {
            var query = string.Format(CultureInfo.InvariantCulture, "SELECT * FROM c WHERE CONTAINS(c.keywords, '{0}')", keyword.ToUpperInvariant());
            var result = await backendDatabaseService.QueryItemsAsync(cosmosDbSettings.TopicCollectionId, query);

            return result;
        }

        public async Task<dynamic> GetTopLevelTopicsAsync()
        {
            return await FindItemsWhere(cosmosDbSettings.TopicCollectionId, "parentTopicID", "");
        }

        public async Task<dynamic> GetSubTopicsAsync(string ParentTopicId)
        {
            return await FindItemsWhere(cosmosDbSettings.TopicCollectionId, "parentTopicID", ParentTopicId);
        }

        public async Task<dynamic> GetReourceAsync(string ParentTopicId)
        {
            var query = "SELECT * FROM c WHERE ARRAY_CONTAINS(c.topicTags, { 'id' : '" + ParentTopicId + "'})";
            var result = await backendDatabaseService.QueryItemsAsync(cosmosDbSettings.ResourceCollectionId, query);
            return result;
        }

        public async Task<dynamic> GetDocumentAsync(string id)
        {
            return await FindItemsWhere(cosmosDbSettings.TopicCollectionId, "id", id);
        }


        public async Task<dynamic> FindItemsWhere(string collectionId, string propertyName, string value)
        {
            var query = $"SELECT * FROM c WHERE c.{propertyName}='{value}'";
            var result = await backendDatabaseService.QueryItemsAsync(collectionId, query);
            return result;
        }
    }
}
