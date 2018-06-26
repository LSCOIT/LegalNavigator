using Access2Justice.CosmosDb.Interfaces;
using Access2Justice.Shared.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

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

        public async Task<dynamic> GetTopicsAsync(string keyword)
        {
            return await dbClient.FindItemsWhereContainsAsync(dbSettings.TopicCollectionId, Constants.Keywords, keyword);
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

        public async Task<dynamic> GetPlanDataAsync(string planId)
        {
            List<dynamic> procedureParams = new List<dynamic>() { planId };
            var result = await dbService.ExecuteStoredProcedureAsync(dbSettings.ResourceCollectionId, Constants.PlanStoredProcedureName, procedureParams);
            var planDetails = result.Response;
            int i = 0;
            foreach (var item in planDetails)
            {
                string topicId = item.topicId;
                var topicData = await dbClient.FindItemsWhereAsync(dbSettings.TopicCollectionId, Constants.Id, topicId);
                planDetails[i].topicId = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(topicData));
                i++;
            }

            return planDetails;
        }
    }
}