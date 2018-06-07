using Access2Justice.CosmosDb.Interfaces;
using Access2Justice.Shared.Interfaces;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Access2Justice.CosmosDb
{
    public class CosmosDbService : IBackendDatabaseService, IDynamicQueries
    {
        private readonly IDocumentClient documentClient;
        private readonly ICosmosDbSettings cosmosDbSettings;

        public CosmosDbService(IDocumentClient documentClient, ICosmosDbSettings cosmosDbSettings)
        {
            this.documentClient = documentClient;
            this.cosmosDbSettings = cosmosDbSettings;

            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();
        }

        public async Task<Document> CreateItemAsync<T>(T item)
        {
            return await documentClient.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(cosmosDbSettings.DatabaseId, cosmosDbSettings.TopicCollectionId), item);
        }

        public async Task<T> GetItemAsync<T>(string id)
        {
            try
            {
                Document document = await documentClient.ReadDocumentAsync(
                        UriFactory.CreateDocumentUri(cosmosDbSettings.DatabaseId, cosmosDbSettings.TopicCollectionId, id));

                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return default(T);
                }
                else
                {
                    // todo: log error
                    throw;
                }
            }
        }

        public async Task<IEnumerable<T>> GetItemsAsync<T>(Expression<Func<T, bool>> predicate,string collectionId)
        {
            IDocumentQuery<T> query = documentClient.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(cosmosDbSettings.DatabaseId, collectionId),
                new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true }).Where(predicate).AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public async Task<Document> UpdateItemAsync<T>(string id, T item)
        {
            return await documentClient.ReplaceDocumentAsync(
                UriFactory.CreateDocumentUri(cosmosDbSettings.DatabaseId, cosmosDbSettings.TopicCollectionId, id), item);
        }

        public async Task DeleteItemAsync(string id)
        {
            await documentClient.DeleteDocumentAsync(
                UriFactory.CreateDocumentUri(cosmosDbSettings.DatabaseId, cosmosDbSettings.TopicCollectionId, id));
        }

        public async Task<T> ExecuteStoredProcedureAsyncWithParameters<T>(string storedProcName, params dynamic[] procedureParams)
        {
            return await documentClient.ExecuteStoredProcedureAsync<T>(UriFactory.CreateStoredProcedureUri(cosmosDbSettings.DatabaseId, cosmosDbSettings.TopicCollectionId, storedProcName), procedureParams);
        }

        public async Task<dynamic> FindItemsWhere(string collectionId, string propertyName, string value)
        {
            var query = $"SELECT * FROM c WHERE c.{propertyName}='{value}'";
            return await QueryItemsAsync(collectionId, query);
        }

        public async Task<dynamic> FindItemsWhereContains(string collectionId, string propertyName, string value)
        {
            var query = $"SELECT * FROM c WHERE CONTAINS(c.{propertyName}, '{value.ToUpperInvariant()}')";
            var result = await QueryItemsAsync(cosmosDbSettings.TopicCollectionId, query);

            return result;
        }

        public async Task<dynamic> FindItemsWhereArrayContains(string collectionId, string arrayName, string propertyName, string value)
        {
            var ids = new List<string> { value };
            return await FindItemsWhereArrayContains(collectionId, arrayName, propertyName, ids);
        }

        public async Task<dynamic> FindItemsWhereArrayContains(string collectionId, string arrayName, string propertyName, IEnumerable<string> values)
        {
            var arrayContainsClause = string.Empty;
            var lastItem = values.Last();

            foreach (var value in values)
            {
                arrayContainsClause += $" ARRAY_CONTAINS(c.{arrayName}, {{ '{propertyName}' : '" + value + "'})";
                if (value != lastItem)
                {
                    arrayContainsClause += "OR";
                }
            }

            var query = $"SELECT * FROM c WHERE {arrayContainsClause}";
            return await QueryItemsAsync(collectionId, query);
        }

        public async Task<dynamic> QueryItemsAsync(string collectionId, string query)
        {
            var docQuery = documentClient.CreateDocumentQuery<dynamic>(
                UriFactory.CreateDocumentCollectionUri(cosmosDbSettings.DatabaseId, collectionId), query).AsDocumentQuery();

            var results = new List<dynamic>();
            while (docQuery.HasMoreResults)
            {
                results.AddRange(await docQuery.ExecuteNextAsync());
            }

            return results;
        }

        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await documentClient.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(cosmosDbSettings.DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await documentClient.CreateDatabaseAsync(new Database { Id = cosmosDbSettings.DatabaseId });
                }
                else
                {
                    // todo: log error
                    throw;
                }
            }
        }

        private async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await documentClient.ReadDocumentCollectionAsync(
                    UriFactory.CreateDocumentCollectionUri(cosmosDbSettings.DatabaseId, cosmosDbSettings.TopicCollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await documentClient.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(cosmosDbSettings.DatabaseId),
                        new DocumentCollection
                        {
                            Id = cosmosDbSettings.TopicCollectionId
                        },
                        new RequestOptions { OfferThroughput = 400 });
                }
                else
                {
                    // todo: log error
                    throw;
                }
            }
        }
    }
}
