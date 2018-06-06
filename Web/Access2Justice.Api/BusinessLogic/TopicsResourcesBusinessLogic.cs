using Access2Justice.CosmosDb.Interfaces;
using Access2Justice.Shared.Interfaces;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

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
        }

        public async Task<dynamic> GetResourcesAsyncV2(dynamic topics)
        {
            var ids = new List<string>();
            foreach(var topic in topics)
            {
                ids.Add(topic.id);
            }
            var result2 = await FindItemsWhereArrayContains(cosmosDbSettings.ResourceCollectionId, "topicTags", "id", ids);

            return result2;
        }

        public async Task<dynamic> GetTopicsAsync(string keyword)
        {
            return await FindItemsWhereContains(cosmosDbSettings.TopicCollectionId, "keywords", keyword);
        }

        public async Task<dynamic> GetTopLevelTopicsAsync()
        {
            return await FindItemsWhere(cosmosDbSettings.TopicCollectionId, "parentTopicID", "");
        }

        public async Task<dynamic> GetSubTopicsAsync(string ParentTopicId)
        {
            return await FindItemsWhere(cosmosDbSettings.TopicCollectionId, "parentTopicID", ParentTopicId);
        }

        public async Task<dynamic> GetResourceAsync(string ParentTopicId)
        {
            return await FindItemsWhereArrayContains(cosmosDbSettings.ResourceCollectionId, "topicTags", "id", ParentTopicId);
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

        public async Task<dynamic> FindItemsWhereContains(string collectionId, string propertyName, string value)
        {
            var query = $"SELECT * FROM c WHERE CONTAINS(c.{propertyName}, '{value.ToUpperInvariant()}')";
            var result = await backendDatabaseService.QueryItemsAsync(cosmosDbSettings.TopicCollectionId, query);

            return result;
        }

        public async Task<dynamic> FindItemsWhereArrayContains(string collectionId, string arrayName, string propertyName, string value)
        {
            var ids = new List<string> { value };
            return await FindItemsWhereArrayContains(collectionId, arrayName, propertyName, ids);
        }

        public async Task<dynamic> FindItemsWhereArrayContains(string collectionId, string arrayName, string propertyName, IEnumerable<string> values)
        {
            var arrayContainsClause = string.Empty;
            var lastItem = values.Last();
            foreach (var value in values)
            {
                arrayContainsClause += $" ARRAY_CONTAINS(c.{arrayName}, {{ '{propertyName}' : '" + value + "'})";
                if (value != lastItem)
                {
                    arrayContainsClause += "OR";
                }
            }

            var query = $"SELECT * FROM c WHERE {arrayContainsClause}";
            var result = await backendDatabaseService.QueryItemsAsync(collectionId, query);
            return result;
        }
    }
}
