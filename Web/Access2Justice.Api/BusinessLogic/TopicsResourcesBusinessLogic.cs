using Access2Justice.CosmosDb.Interfaces;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.Azure.Documents.Client;
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
            // we need to use a query format to retrieve items because we are returning a dynamic object.
            var query = string.Format(CultureInfo.InvariantCulture,"SELECT * FROM c  WHERE {0}", topicIds);
            var result = await backendDatabaseService.QueryItemsAsync(cosmosDbSettings.ResourceCollectionId, query);
            
            return result;
        }

        public async Task<dynamic> GetTopicAsync(string keyword)
        {
            // we need to use a query format to retrieve items because we are returning a dynamic object.
            var query = string.Format(CultureInfo.InvariantCulture,"SELECT * FROM c WHERE CONTAINS(c.keywords, '{0}')", keyword.ToUpperInvariant());
            var result = await backendDatabaseService.QueryItemsAsync(cosmosDbSettings.TopicCollectionId, query);

            return result;
        }

        public async Task<dynamic> GetTopicsAsync()
        {
            var query = "SELECT * FROM c where c.parentTopicID=''";
            var result = await backendDatabaseService.QueryItemsAsync(cosmosDbSettings.TopicCollectionId, query);
            return result;
        }

        public async Task<dynamic> GetSubTopicsAsync(string ParentTopicId)
        {
            var query = "SELECT * FROM c WHERE c.parentTopicID='" + ParentTopicId + "'";
            var result = await backendDatabaseService.QueryItemsAsync(cosmosDbSettings.TopicCollectionId, query);
            return result;
        }

        public async Task<dynamic> GetReourceDetailAsync(string ParentTopicId)
        {
            var query = "SELECT * FROM c WHERE ARRAY_CONTAINS(c.topicTags, { 'id' : '" + ParentTopicId + "'})";
            var result = await backendDatabaseService.QueryItemsAsync(cosmosDbSettings.ResourceCollectionId, query);
            return result;
        }

        public async Task<dynamic> GetDocumentData(string id)
        {
            var query = "SELECT * FROM c WHERE c.id='" + id + "'";
            var result = await backendDatabaseService.QueryItemsAsync(cosmosDbSettings.TopicCollectionId, query);
            return result;
        }

        public async Task<dynamic> GetFirstPageResource(ResourceFilter resourceFilter,string queryFilter)
        {
            var query = string.Format(CultureInfo.InvariantCulture, "SELECT * FROM c  WHERE {0}", queryFilter);
            FeedOptions feedOptions = new FeedOptions()
            {
                MaxItemCount = cosmosDbSettings.DefaultCount              
            };
            var result = await backendDatabaseService.QueryItemsPaginationAsync(cosmosDbSettings.ResourceCollectionId, query, feedOptions);
            
            return result;
        }

        public async Task<dynamic> GetPagedResourcesAsync(ResourceFilter resourceFilter, string queryFilter)
        {
            // we need to use a query format to retrieve items because we are returning a dynamic object.
            var query =  string.Format(CultureInfo.InvariantCulture, "SELECT * FROM c  WHERE {0}", queryFilter);
            FeedOptions feedOptions = new FeedOptions()
            {
                MaxItemCount = cosmosDbSettings.DefaultCount,
                RequestContinuation = resourceFilter.ContinuationToken
            };

            var result = await backendDatabaseService.QueryItemsPaginationAsync(cosmosDbSettings.ResourceCollectionId, query, feedOptions);

            return result;
        }

        public string FilterPagedResource(ResourceFilter resourceFilter)
        {
            string queryFilter = "";
            string topicIds = "";
            
            foreach (var topic in resourceFilter.TopicIds)
            {
                topicIds += "  ARRAY_CONTAINS(c.topicTags, { 'id' : '" + topic + "'}) OR";
            }
            if (!string.IsNullOrEmpty(topicIds))
            {
                // remove the last OR from the db query 
                topicIds = topicIds.Remove(topicIds.Length - 2);
                topicIds = "(" + topicIds + ")";
                if (resourceFilter.ResourceType.ToUpperInvariant() != "ALL")
                {
                    queryFilter = " AND c.resourceType = '" + resourceFilter.ResourceType + "'";
                }
            }
            else {             
                queryFilter = " c.resourceType = '" + resourceFilter.ResourceType + "'";
            }

            return topicIds + queryFilter;
        }

    }
}
