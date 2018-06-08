using Access2Justice.CosmosDb.Interfaces;
using Access2Justice.Shared.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Access2Justice.Shared.Models;
using Microsoft.Azure.Documents.Client;
using System.Globalization;

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
            foreach(var topic in topics)
            {
                ids.Add(topic.id);
            }

            return await dbClient.FindItemsWhereArrayContains(dbSettings.ResourceCollectionId, "topicTags", "id", ids);
        }

        public async Task<dynamic> GetTopicsAsync(string keyword)
        {
            return await dbClient.FindItemsWhereContains(dbSettings.TopicCollectionId, "keywords", keyword);
        }

        public async Task<dynamic> GetTopLevelTopicsAsync()
        {
            return await dbClient.FindItemsWhere(dbSettings.TopicCollectionId, "parentTopicID", "");
        }

        public async Task<dynamic> GetSubTopicsAsync(string ParentTopicId)
        {
            return await dbClient.FindItemsWhere(dbSettings.TopicCollectionId, "parentTopicID", ParentTopicId);
        }

        public async Task<dynamic> GetResourceAsync(string ParentTopicId)
        {
            return await dbClient.FindItemsWhereArrayContains(dbSettings.ResourceCollectionId, "topicTags", "id", ParentTopicId);
        }

        public async Task<dynamic> GetDocumentAsync(string id)
        {
            return await dbClient.FindItemsWhere(dbSettings.TopicCollectionId, "id", id);
        }

        public async Task<dynamic> GetFirstPageResource(ResourceFilter resourceFilter, string queryFilter)
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
            var query = string.Format(CultureInfo.InvariantCulture, "SELECT * FROM c  WHERE {0}", queryFilter);
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
            else
            {
                queryFilter = " c.resourceType = '" + resourceFilter.ResourceType + "'";
            }

            return topicIds + queryFilter;
        }
    }
}
