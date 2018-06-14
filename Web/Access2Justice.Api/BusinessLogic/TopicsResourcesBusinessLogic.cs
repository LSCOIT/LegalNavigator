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

            return await dbClient.FindItemsWhereArrayContainsAsync(dbSettings.ResourceCollectionId, "topicTags", "id", ids);
        }

        public async Task<dynamic> GetTopicsAsync(string keyword,Location location)
        {
            return await dbClient.FindItemsWhereContainsWithLocationAsync(dbSettings.TopicCollectionId, "keywords", keyword, location);
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
            PagedResources pagedResources = await dbClient.FindItemsWhereArrayContainsWithAndClauseAsync("topicTags", "id","resourceType",resourceFilter.ResourceType,resourceFilter);            

            return pagedResources;
        }

        public async Task<dynamic> GetResourcesCountAsync(ResourceFilter resourceFilter)
        {
            PagedResources pagedResources = await dbClient.FindItemsWhereArrayContainsWithAndClauseAsync("topicTags", "id", "resourceType", resourceFilter.ResourceType, resourceFilter, true);
            
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
