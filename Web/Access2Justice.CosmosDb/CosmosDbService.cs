using Access2Justice.CosmosDb.Interfaces;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public async Task<T> ExecuteStoredProcedureAsyncWithParameters<T>(string storedProcName, params dynamic[] procedureParams)
        {
            return await documentClient.ExecuteStoredProcedureAsync<T>(UriFactory.CreateStoredProcedureUri(cosmosDbSettings.DatabaseId, cosmosDbSettings.TopicCollectionId, storedProcName), procedureParams);
        }

        public async Task<dynamic> FindItemsWhere(string collectionId, string propertyName, string value)
        {
            EnsureParametersAreNotOrEmpty(collectionId, propertyName);

            var query = $"SELECT * FROM c WHERE c.{propertyName}='{value}'";
            return await QueryItemsAsync(collectionId, query);
        }

        public async Task<dynamic> FindItemsWhereContains(string collectionId, string propertyName, string value)
        {
            EnsureParametersAreNotOrEmpty(collectionId, propertyName);

            var query = $"SELECT * FROM c WHERE CONTAINS(c.{propertyName}, '{value.ToUpperInvariant()}')";
            return await QueryItemsAsync(cosmosDbSettings.TopicCollectionId, query);
        }

        public async Task<dynamic> FindItemsWhereArrayContains(string collectionId, string arrayName, string propertyName, string value)
        {
            EnsureParametersAreNotOrEmpty(collectionId, arrayName, propertyName);

            var ids = new List<string> { value };
            return await FindItemsWhereArrayContains(collectionId, arrayName, propertyName, ids);
        }

        public async Task<dynamic> FindItemsWhereArrayContains(string collectionId, string arrayName, string propertyName, IEnumerable<string> values)
        {
            EnsureParametersAreNotOrEmpty(collectionId, arrayName, propertyName);

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

        public dynamic FindItemsWhereArrayContainsWithAndClause(string arrayName, string propertyName, string andPropertyName, string andPropertyValue, IEnumerable<string> values)
        {
            EnsureParametersAreNotOrEmpty(arrayName, propertyName, andPropertyName, andPropertyValue);
            var arrayContainsWithAndClause = string.Empty;
            var lastItem = values.Last();

            foreach (var value in values)
            {
                arrayContainsWithAndClause += $" ARRAY_CONTAINS(c.{arrayName}, {{ '{propertyName}' : '" + value + "'})";
                // arrayContainsWithAndClause += "  ARRAY_CONTAINS(c.topicTags, { 'id' : '" + value + "'}) ";
                if (value != lastItem)
                {
                    arrayContainsWithAndClause += "OR";
                }
            }
            //if (!string.IsNullOrEmpty(arrayContainsWithAndClause))
            //{
            // remove the last OR from the db query
            arrayContainsWithAndClause = "(" + arrayContainsWithAndClause + ")";
            //if (resourceFilter.ResourceType.ToUpperInvariant() != "ALL")
            //{
            arrayContainsWithAndClause += $" AND c.{andPropertyName} = '" + andPropertyValue + "'";
            //arrayContainsWithAndClause += $" AND c.resourceType = '" + resourceFilter.ResourceType + "'";
            //}
            //}
            var query = $"SELECT * FROM c WHERE {arrayContainsWithAndClause}";
            return query;
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

        public async Task<dynamic> QueryItemsPaginationAsync(string collectionId, string query, FeedOptions feedOptions)
        {

            var docQuery = documentClient.CreateDocumentQuery<dynamic>(
                UriFactory.CreateDocumentCollectionUri(cosmosDbSettings.DatabaseId, collectionId), query, feedOptions).AsDocumentQuery();

            var results = new PagedResources();
            var queryResult = await docQuery.ExecuteNextAsync();
            if (!queryResult.Any())
            {
                return results;
            }
            results.ContinuationToken = queryResult.ResponseContinuation;
            results.Results.AddRange(queryResult);

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

        private void EnsureParametersAreNotOrEmpty(params string[] parameters)
        {
            foreach (var param in parameters)
            {
                if (string.IsNullOrWhiteSpace(param))
                {
                    throw new ArgumentException("Paramters can not be null or empty spaces.");
                }
            }
        }

        public async Task<dynamic> QueryPagedResourcesAsync(string query, string continuationToken)
        {
            dynamic result = null;
            if (string.IsNullOrEmpty(continuationToken))
            {
                result = await GetFirstPageResourceAsync(query);
            }
            else
            {
                result = await GetNextPageResourcesAsync(query, continuationToken);
            }
            return result;
        }

        public async Task<dynamic> GetFirstPageResourceAsync(string query)
        {
            FeedOptions feedOptions = new FeedOptions()
            {
                MaxItemCount = cosmosDbSettings.DefaultCount
            };
            var result = await QueryItemsPaginationAsync(cosmosDbSettings.ResourceCollectionId, query, feedOptions);

            return result;
        }

        public async Task<dynamic> GetNextPageResourcesAsync(string query, string continuationToken)
        {
            FeedOptions feedOptions = new FeedOptions()
            {
                MaxItemCount = cosmosDbSettings.DefaultCount,
                RequestContinuation = continuationToken
            };

            var result = await QueryItemsPaginationAsync(cosmosDbSettings.ResourceCollectionId, query, feedOptions);

            return result;
        }

    }
}