using Access2Justice.CosmosDb.Interfaces;
using Access2Justice.Shared.Interfaces;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.BusinessLogic
{
    public class TopicBusinessLogic: ITopicBusinessLogic
    {

        private readonly IBackendDatabaseService _backendDatabaseService;
        private readonly ICosmosDbSettings _cosmosDbSettings;
        public TopicBusinessLogic(IBackendDatabaseService backendDatabaseService, ICosmosDbSettings cosmosDbSettings)
        {
            _backendDatabaseService = backendDatabaseService;
            _cosmosDbSettings = cosmosDbSettings;
        }

        public async Task<T> GetTopicsAsync<T>()
        {  
            var query = "SELECT * FROM c where c.parentTopicID=''";
            var result = await _backendDatabaseService.QueryItemsAsync(_cosmosDbSettings.TopicCollectionId, query);
            return result;
        }
        public async Task<dynamic> GetSubTopicsAsync(string id)
        {
            var query = "SELECT * FROM c WHERE c.parentTopicID='"+id+"'";
            var result = await _backendDatabaseService.QueryItemsAsync(_cosmosDbSettings.TopicCollectionId, query);
            return result;
        }

        public async Task<dynamic> GetSubTopicDetailAsync(string id)
        {
            var query = "SELECT * FROM c WHERE ARRAY_CONTAINS(c.topicTags, { 'id' : '" + id + "'})";
            var result = await _backendDatabaseService.QueryItemsAsync(_cosmosDbSettings.ResourceCollectionId, query);
            return result;
        }
    }
}
