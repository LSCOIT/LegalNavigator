using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.DataFixes.DataAccess
{
    public class CosmosDbService
    {
        private readonly IDocumentClient _documentClient;
        private readonly CosmosDbSettings _cosmosDbSettings;

        public CosmosDbService(
            IDocumentClient documentClient,
            CosmosDbSettings cosmosDbSettings)
        {
            _documentClient = documentClient;
            _cosmosDbSettings = cosmosDbSettings;
        }

        public async Task<dynamic> FindAllItemsAsync(string collectionId)
        {
            var query = $"SELECT * FROM c";
            return await QueryItemsAsync(collectionId, query);
        }

        public async Task<Document> UpdateItemAsync<T>(string id, T item, string collectionId)
        {
            return await _documentClient.ReplaceDocumentAsync(
                UriFactory.CreateDocumentUri(_cosmosDbSettings.DatabaseId, collectionId, id), item);
        }

        public async Task<Document> DeleteItemAsync(string id, object partition, string collectionId)
        {
            return await _documentClient.DeleteDocumentAsync(
                UriFactory.CreateDocumentUri(_cosmosDbSettings.DatabaseId, collectionId, id), new RequestOptions{PartitionKey = new PartitionKey(partition) });
        }

        public async Task<dynamic> QueryItemsAsync(string collectionId, string query)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(
                _cosmosDbSettings.DatabaseId,
                collectionId);

            var options = new FeedOptions { EnableCrossPartitionQuery = true };
            var docQuery = _documentClient.CreateDocumentQuery<dynamic>(uri, query, options).
                AsDocumentQuery();

            var results = new List<dynamic>();
            while (docQuery.HasMoreResults)
            {
                results.AddRange(await docQuery.ExecuteNextAsync());
            }

            return results;
        }
    }
}
