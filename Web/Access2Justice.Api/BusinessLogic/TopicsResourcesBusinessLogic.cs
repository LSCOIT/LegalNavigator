using Access2Justice.CosmosDb.Interfaces;
using Access2Justice.Shared.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;

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

        public async Task<dynamic> GetTopicsAsync(string keyword,Location location)
        {
            return await dbClient.FindItemsWhereContainsWithLocation(dbSettings.TopicCollectionId, "keywords", keyword, location);
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
            dynamic serializedResources = "[]";
            dynamic serializedToken = "[]";
            dynamic serializedTopicIds = "[]";

            PagedResources pagedResources = await ApplyPaginationAsync(resourceFilter);
            if (pagedResources != null)
            {
                serializedResources = JsonConvert.SerializeObject(pagedResources.Results);
                serializedToken = pagedResources.ContinuationToken ?? "[]";
                serializedTopicIds = JsonConvert.SerializeObject(pagedResources.TopicIds);
            }

            JObject internalResources = new JObject {
                { "resources", JsonConvert.DeserializeObject(serializedResources) },
                {"continuationToken", JsonConvert.DeserializeObject(serializedToken) },
                {"topicIds" , JsonConvert.DeserializeObject(serializedTopicIds)}                
            };

            return internalResources.ToString();
        }

        public async Task<dynamic> ApplyPaginationAsync(ResourceFilter resourceFilter)
        {
            PagedResources pagedResources = new PagedResources();            
            string query = dbClient.FindItemsWhereArrayContainsWithAndClause("topicTags", "id","resourceType",resourceFilter.ResourceType,resourceFilter.TopicIds,resourceFilter.Location);
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

        public async Task<dynamic> GetResourcesCountAsync(ResourceFilter resourceFilter)
        {
            string query = dbClient.FindItemsWhereArrayContainsWithAndClause("topicTags", "id", "resourceType", resourceFilter.ResourceType, resourceFilter.TopicIds, resourceFilter.Location);
            query = query.Replace("*", "c.resourceType",System.StringComparison.InvariantCulture);
            PagedResources pagedResources = await dbClient.QueryResourcesCountAsync(query);
           return ResourcesCount(pagedResources);
        }

        private dynamic ResourcesCount(PagedResources resources)
        {
            List<dynamic> allResources = new List<dynamic>();
            allResources.Add(new
            {
                ResourceName = "All",
                ResourceCount = resources.Results.Count()
            });

            var groupedResourceType = resources.Results.GroupBy(u => u.resourceType)
                  .OrderBy(group => group.Key)
                  .Select(n => new
                  {
                      ResourceName = n.Key,
                      ResourceCount = n.Count()
                  }).OrderBy(n => n.ResourceName);

            dynamic resourceList = groupedResourceType.Concat(allResources);
            return resourceList;
        }
    }
}
