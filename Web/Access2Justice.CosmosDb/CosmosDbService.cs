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
    public class CosmosDbService : IBackendDatabaseService
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

        public async Task<IEnumerable<T>> GetItemsAsync<T>(Expression<Func<T, bool>> predicate, string collectionId)
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

        public async Task<dynamic> ExecuteStoredProcedureAsync(string collectionId, string storedProcName, params dynamic[] procedureParams)
        {
            return await documentClient.ExecuteStoredProcedureAsync<dynamic>(UriFactory.CreateStoredProcedureUri(cosmosDbSettings.DatabaseId, collectionId, storedProcName), procedureParams);
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