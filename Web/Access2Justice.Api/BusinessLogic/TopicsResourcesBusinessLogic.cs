using Access2Justice.CosmosDb.Interfaces;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

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

        public async Task<dynamic> GetSubTopicsAsync(string parentTopicId)
        {
            return await dbClient.FindItemsWhereAsync(dbSettings.TopicCollectionId, Constants.ParentTopicId, parentTopicId);
        }

        public async Task<dynamic> GetResourceAsync(string parentTopicId)
        {
            return await dbClient.FindItemsWhereArrayContainsAsync(dbSettings.ResourceCollectionId, Constants.TopicTags, Constants.Id, parentTopicId);
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

        public async Task<dynamic> GetPlanDataAsync(string planId)
        {
            List<dynamic> procedureParams = new List<dynamic>() { planId };
            var result = await dbService.ExecuteStoredProcedureAsync(dbSettings.ResourceCollectionId, Constants.PlanStoredProcedureName, procedureParams);
            var planDetails = result.Response;
            int indexOfTopicTags = 0;
            foreach (var item in planDetails.planTags)
            {
                string topicId = item.id;
                var topicData = await dbClient.FindItemsWhereAsync(dbSettings.TopicCollectionId, Constants.Id, topicId);
                planDetails.planTags[indexOfTopicTags].id = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(topicData));
                indexOfTopicTags++;
            }

            return planDetails;
        }

        public async Task<dynamic> GetPersonalizedResourcesAsync(ResourceFilter resourceFilter)
        {
            dynamic Topics = Array.Empty<string>();
            dynamic Resources = Array.Empty<string>();
            if (resourceFilter.TopicIds != null && resourceFilter.TopicIds.Count() > 0)
            {
                Topics = await dbClient.FindItemsWhereInClauseAsync(dbSettings.TopicCollectionId, "id", resourceFilter.TopicIds) ?? Array.Empty<string>();
            }
            if (resourceFilter.ResourceIds != null && resourceFilter.ResourceIds.Count() > 0)
            {
                Resources = await dbClient.FindItemsWhereInClauseAsync(dbSettings.ResourceCollectionId, "id", resourceFilter.ResourceIds) ?? Array.Empty<string>();
            }

            Topics = JsonConvert.SerializeObject(Topics);
            Resources = JsonConvert.SerializeObject(Resources);

            JObject personalizedResources = new JObject {
                { "topics", JsonConvert.DeserializeObject(Topics) },
                {"resources", JsonConvert.DeserializeObject(Resources) }
            };

            return personalizedResources.ToString();

        }

    }
}