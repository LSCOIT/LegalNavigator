using Access2Justice.CosmosDb.Interfaces;
using Access2Justice.Shared.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Api.BusinessLogic
{
    public class TopicsResourcesBusinessLogic : ITopicsResourcesBusinessLogic
    {
        private readonly IBackendDatabaseService _backendDatabaseService;
        private readonly ICosmosDbSettings _cosmosDbSettings;
        public TopicsResourcesBusinessLogic(IBackendDatabaseService backendDatabaseService, ICosmosDbSettings cosmosDbSettings)
        {
            _backendDatabaseService = backendDatabaseService;
            _cosmosDbSettings = cosmosDbSettings;
        }

        public Task GetResources(IEnumerable<string> resourcesIds)
        {
            // todo: use cosmos db methods to get resources of the provided topics ids
            throw new NotImplementedException();
        }

        public async Task<dynamic> GetTopicAsync(string keyword)
        {
            // we need to use a quey format to retrieve items because we are returning a dynmaic object.
            var qeury = string.Format("SELECT * FROM c WHERE CONTAINS(c.keywords, '{0}')", keyword.ToLower());
            var result = await _backendDatabaseService.QueryItemsAsync(_cosmosDbSettings.TopicCollectionId, qeury);

            return JsonConvert.SerializeObject(result);
        }
    }
}
