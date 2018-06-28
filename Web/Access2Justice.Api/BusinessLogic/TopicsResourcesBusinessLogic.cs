using Access2Justice.CosmosDb.Interfaces;
using Access2Justice.Shared.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Access2Justice.Api.BusinessLogic
{
    public class TopicsResourcesBusinessLogic : ITopicsResourcesBusinessLogic
    {
        private readonly IDynamicQueries dbClient;
        private readonly ICosmosDbSettings dbSettings;
        private readonly IBackendDatabaseService dbService;
        public TopicsResourcesBusinessLogic(IDynamicQueries dynamicQueries, ICosmosDbSettings cosmosDbSettings, IBackendDatabaseService backendDatabaseService)
        {
            dbClient = dynamicQueries;
            dbSettings = cosmosDbSettings;
            dbService = backendDatabaseService;
        }

        public async Task<dynamic> GetResourcesAsync(dynamic topics)
        {
            var ids = new List<string>();
            foreach (var topic in topics)
            {
                string topicId = topic.id;
                ids.Add(topicId);
            }

            return await dbClient.FindItemsWhereArrayContainsAsync(dbSettings.ResourceCollectionId, Constants.TopicTags, Constants.Id, ids);
        }

        public async Task<dynamic> GetTopicsAsync(string keyword,Location location)
        {
            return await dbClient.FindItemsWhereContainsWithLocationAsync(dbSettings.TopicCollectionId, "keywords", keyword, location);
        }

        public async Task<dynamic> GetTopLevelTopicsAsync()
        {
            return await dbClient.FindItemsWhereAsync(dbSettings.TopicCollectionId, Constants.ParentTopicId, "");
        }

        public async Task<dynamic> GetSubTopicsAsync(string ParentTopicId)
        {
            return await dbClient.FindItemsWhereAsync(dbSettings.TopicCollectionId, Constants.ParentTopicId, ParentTopicId);
        }

        public async Task<dynamic> GetResourceAsync(string ParentTopicId)
        {
            return await dbClient.FindItemsWhereArrayContainsAsync(dbSettings.ResourceCollectionId, Constants.TopicTags, Constants.Id, ParentTopicId);
        }

        public async Task<dynamic> GetDocumentAsync(string id)
        {
            return await dbClient.FindItemsWhereAsync(dbSettings.TopicCollectionId, Constants.Id, id);
        }

        public async Task<dynamic> GetBreadcrumbDataAsync(string id)
        {
            List<dynamic> procedureParams = new List<dynamic>() { id };
            return await dbService.ExecuteStoredProcedureAsync(dbSettings.TopicCollectionId, Constants.BreadcrumbStoredProcedureName, procedureParams);
        }

        public async Task<dynamic> GetPagedResourceAsync(ResourceFilter resourceFilter)
        {
            dynamic serializedResources = "[]";            
            dynamic serializedTopicIds = "[]";

            PagedResources pagedResources = await ApplyPaginationAsync(resourceFilter);
            serializedResources = JsonConvert.SerializeObject(pagedResources?.Results);
            dynamic serializedToken = pagedResources?.ContinuationToken ?? "[]";
            serializedTopicIds = JsonConvert.SerializeObject(pagedResources?.TopicIds);

            JObject internalResources = new JObject {
                { "resources", JsonConvert.DeserializeObject(serializedResources) },
                {"continuationToken", JsonConvert.DeserializeObject(serializedToken) },
                {"topicIds" , JsonConvert.DeserializeObject(serializedTopicIds)}
            };

            return internalResources.ToString();
        }

        public async Task<dynamic> ApplyPaginationAsync(ResourceFilter resourceFilter)
        {
            PagedResources pagedResources = await dbClient.FindItemsWhereArrayContainsWithAndClauseAsync("topicTags", "id", "resourceType", resourceFilter);

            return pagedResources;
        }

        public async Task<dynamic> GetResourcesCountAsync(ResourceFilter resourceFilter)
        {
            PagedResources pagedResources = await dbClient.FindItemsWhereArrayContainsWithAndClauseAsync("topicTags", "id", "resourceType", resourceFilter, true);
            
           return ResourcesCount(pagedResources);
        }

        private dynamic ResourcesCount(PagedResources resources)
        {
            List<dynamic> allResources = new List<dynamic>
            {
                new
                {
                    ResourceName = "All",
                    ResourceCount = resources.Results.Count()
                }
            };

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