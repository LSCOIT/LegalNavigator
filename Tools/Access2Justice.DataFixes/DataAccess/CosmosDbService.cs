using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<dynamic> FindAllArticles(string collectionId)
        {
            var query = $"SELECT * FROM c WHERE c.resourceType = 'Articles'";
            return await QueryItemsAsync(collectionId, query);
        }

        public async Task<dynamic> FindAllOrganizations(string collectionId)
        {
            var query = $"SELECT * FROM c WHERE c.resourceType = 'Organizations'";
            return await QueryItemsAsync(collectionId, query);
        }

        public async Task<dynamic> FindAllCuratedExperienceItems(string collectionId)
        {
            var query = $"SELECT * FROM c WHERE c.resourceType = 'Guided Assistant'";
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

        public async Task<T> GetItemAsync<T>(string id, string collectionId)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(_cosmosDbSettings.DatabaseId, collectionId);
            var query = new SqlQuerySpec
            {
                QueryText = "SELECT * FROM c WHERE c.id = @id",
                Parameters = new SqlParameterCollection { new SqlParameter("@id", id) }
            };
            var options = new FeedOptions
            {
                EnableCrossPartitionQuery = true
            };

            // I'm not using the documentClient.ReadDocumentAsync() because Microsoft recently made cosmos partitioning a 
            // mandatory query param and I don't want to explicitly specify a partition key.
            var docQuery = _documentClient.CreateDocumentQuery<dynamic>(uri, query, options).AsDocumentQuery();

            var results = new List<dynamic>();
            while (docQuery.HasMoreResults)
            {
                results.AddRange(await docQuery.ExecuteNextAsync());
            }
            if (!results.Any())
            {
                return default(T);
            }

            return (T)results.FirstOrDefault();
        }
        public async Task<T> GetItemAsyncByTitle<T>(string name, string collectionId)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(_cosmosDbSettings.DatabaseId, collectionId);
            var query = new SqlQuerySpec
            {
                QueryText = "SELECT * FROM c WHERE c.title = @name",
                Parameters = new SqlParameterCollection { new SqlParameter("@name", name)},
            };

            var options = new FeedOptions
            {
                EnableCrossPartitionQuery = true
            };

            // I'm not using the documentClient.ReadDocumentAsync() because Microsoft recently made cosmos partitioning a 
            // mandatory query param and I don't want to explicitly specify a partition key.
            var docQuery = _documentClient.CreateDocumentQuery<dynamic>(uri, query, options).AsDocumentQuery();

            var results = new List<dynamic>();
            while (docQuery.HasMoreResults)
            {
                results.AddRange(await docQuery.ExecuteNextAsync());
            }
            if (!results.Any())
            {
                return default(T);
            }

            return (T)results.FirstOrDefault();
        }
        public async Task<T> GetItemAsyncByName<T>(string name, string collectionId, string state, bool toLower = true)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(_cosmosDbSettings.DatabaseId, collectionId);
            var query = new SqlQuerySpec
            {
                QueryText = toLower ? "SELECT * FROM c WHERE LOWER(c.name) = @name AND c.organizationalUnit = @state" : "SELECT * FROM c WHERE c.name = @name AND c.organizationalUnit = @state",
                Parameters = new SqlParameterCollection { new SqlParameter("@name", name), new SqlParameter("@state", state) },
            };

            var options = new FeedOptions
            {
                EnableCrossPartitionQuery = true
            };

            // I'm not using the documentClient.ReadDocumentAsync() because Microsoft recently made cosmos partitioning a 
            // mandatory query param and I don't want to explicitly specify a partition key.
            var docQuery = _documentClient.CreateDocumentQuery<dynamic>(uri, query, options).AsDocumentQuery();

            var results = new List<dynamic>();
            while (docQuery.HasMoreResults)
            {
                results.AddRange(await docQuery.ExecuteNextAsync());
            }
            if (!results.Any())
            {
                return default(T);
            }

            return (T)results.FirstOrDefault();
        }
    }
}
