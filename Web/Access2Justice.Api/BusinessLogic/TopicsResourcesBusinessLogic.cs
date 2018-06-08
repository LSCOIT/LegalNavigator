using Access2Justice.CosmosDb.Interfaces;
using Access2Justice.Shared.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Access2Justice.Shared.Models;

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

        public async Task<dynamic> GetPagedResourceAsync(ResourceFilter resourceFilter)
        {

            PagedResources pagedResources = await ApplyPaginationAsync(resourceFilter);

            return pagedResources;
        }

        public async Task<dynamic> ApplyPaginationAsync(ResourceFilter resourceFilter)
        {
            PagedResources pagedResources = new PagedResources();            
            string query = dbClient.FindItemsWhereArrayContainsWithAndClause("topicTags", "id","resourceType",resourceFilter.ResourceType,resourceFilter.TopicIds);
            if (resourceFilter.PageNumber == 0)
            {
                pagedResources = await dbClient.QueryPagedResourcesAsync(query, "");
                pagedResources.TopicIds = resourceFilter.TopicIds;
            }
            else
            {
                pagedResources = await dbClient.QueryPagedResourcesAsync(query, resourceFilter.ContinuationToken);
                pagedResources.TopicIds = resourceFilter.TopicIds;
            }

            return pagedResources;
        }
    }
}
