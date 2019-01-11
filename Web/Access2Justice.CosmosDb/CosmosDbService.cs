using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
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
        }

        public async Task<Document> CreateItemAsync<T>(T item, string collectionId)
        {
            return await documentClient.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(cosmosDbSettings.DatabaseId, collectionId), item);
        }

        public async Task<T> GetItemAsync<T>(string id, string collectionId)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(cosmosDbSettings.DatabaseId, collectionId);
            var query = new SqlQuerySpec
            {
                QueryText = "SELECT * FROM c WHERE c.id = @id",
                Parameters = new SqlParameterCollection() { new SqlParameter("@id", id) }
            };
            var options = new FeedOptions
            {
                EnableCrossPartitionQuery = true
            };

            // I'm not using the documentClient.ReadDocumentAsync() because Microsoft recently made cosmos partitioning a 
            // mandatory query param and I don't want to explicitly specify a partition key.
            var docQuery = documentClient.CreateDocumentQuery<dynamic>(uri, query, options).AsDocumentQuery();

            var results = new List<dynamic>();
            while (docQuery.HasMoreResults)
            {
                results.AddRange(await docQuery.ExecuteNextAsync());
            }
            if (results == null || !results.Any())
            {
                return default(T);
            }

            return (T)results.FirstOrDefault();
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
                UriFactory.CreateDocumentUri(cosmosDbSettings.DatabaseId, cosmosDbSettings.TopicsCollectionId, id), item);
        }

        public async Task<Document> UpdateItemAsync<T>(string id, T item, string collectionId)
        {
            return await documentClient.ReplaceDocumentAsync(
                UriFactory.CreateDocumentUri(cosmosDbSettings.DatabaseId, collectionId, id), item);
        }

        public async Task DeleteItemAsync(string id, string collectionId)
        {
            await documentClient.DeleteDocumentAsync(
                UriFactory.CreateDocumentUri(cosmosDbSettings.DatabaseId, collectionId, id));
        }

        public async Task<dynamic> QueryItemsAsync(string collectionId, string query)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(cosmosDbSettings.DatabaseId, collectionId);
            var options = new FeedOptions { EnableCrossPartitionQuery = true };
            var docQuery = documentClient.CreateDocumentQuery<dynamic>(uri, query, options).AsDocumentQuery();

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
            List<dynamic> resources = new List<dynamic>();
            var queryResult = await docQuery.ExecuteNextAsync();
            if (!queryResult.Any())
            {
                return results;
            }
            results.ContinuationToken = queryResult.ResponseContinuation;
            resources.AddRange(queryResult);
            results.Results = resources;

            return results;
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

        public async Task<dynamic> QueryResourcesCountAsync(string query)
        {
            return await GetFirstPageResourceAsync(query, true);
        }

        public async Task<dynamic> GetFirstPageResourceAsync(string query, bool isInitialPage = false)
        {
            var feedOptions = new FeedOptions { EnableCrossPartitionQuery = true };

            if (!isInitialPage)
            {
                feedOptions.MaxItemCount = cosmosDbSettings.PageResultsCount;
            }

            return await QueryItemsPaginationAsync(cosmosDbSettings.ResourcesCollectionId, query, feedOptions); ;
        }

        public async Task<dynamic> GetNextPageResourcesAsync(string query, string continuationToken)
        {
            FeedOptions feedOptions = new FeedOptions()
            {
                MaxItemCount = cosmosDbSettings.PageResultsCount,
                RequestContinuation = continuationToken,
                EnableCrossPartitionQuery = true
            };

            return await QueryItemsPaginationAsync(cosmosDbSettings.ResourcesCollectionId, query, feedOptions); ;
        }

        public async Task<dynamic> ExecuteStoredProcedureAsync(string collectionId, string storedProcName, params dynamic[] procedureParams)
        {
            return await documentClient.ExecuteStoredProcedureAsync<dynamic>(UriFactory.CreateStoredProcedureUri(cosmosDbSettings.DatabaseId, collectionId, storedProcName), procedureParams);
        }
    }
}