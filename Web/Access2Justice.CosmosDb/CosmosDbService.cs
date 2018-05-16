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
        private readonly IDocumentClient _documentClient;
        private readonly ICosmosDbSettings _cosmosDbSettings;

        public CosmosDbService(IDocumentClient documentClient, ICosmosDbSettings cosmosDbSettings)
        {
            _documentClient = documentClient;
            _cosmosDbSettings = cosmosDbSettings;

            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();
        }

        public async Task<T> GetItemAsync<T>(string id)
        {
            try
            {
                Document document = await _documentClient.ReadDocumentAsync(
                        UriFactory.CreateDocumentUri(_cosmosDbSettings.DatabaseId, _cosmosDbSettings.CollectionId, id));

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

        public async Task<IEnumerable<T>> GetItemsAsync<T>(Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> query = _documentClient.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(_cosmosDbSettings.DatabaseId, _cosmosDbSettings.CollectionId),
                new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true }).Where(predicate).AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public async Task<Document> CreateItemAsync<T>(T item)
        {
            return await _documentClient.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(_cosmosDbSettings.DatabaseId, _cosmosDbSettings.CollectionId), item);
        }

        public async Task<Document> UpdateItemAsync<T>(string id, T item)
        {
            return await _documentClient.ReplaceDocumentAsync(
                UriFactory.CreateDocumentUri(_cosmosDbSettings.DatabaseId, _cosmosDbSettings.CollectionId, id), item);
        }

        public async Task DeleteItemAsync(string id)
        {
            await _documentClient.DeleteDocumentAsync(
                UriFactory.CreateDocumentUri(_cosmosDbSettings.DatabaseId, _cosmosDbSettings.CollectionId, id));
        }

        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await _documentClient.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(_cosmosDbSettings.DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await _documentClient.CreateDatabaseAsync(new Database { Id = _cosmosDbSettings.DatabaseId });
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
                await _documentClient.ReadDocumentCollectionAsync(
                    UriFactory.CreateDocumentCollectionUri(_cosmosDbSettings.DatabaseId, _cosmosDbSettings.CollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await _documentClient.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(_cosmosDbSettings.DatabaseId),
                        new DocumentCollection
                        {
                            Id = _cosmosDbSettings.CollectionId
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





        /// <summary>
        /// Asynchronous method to get documents by the given property name
        /// </summary>
        /// <typeparam name="T">Document details</typeparam>
        /// <param name="storedProcName">procedure name</param>
        /// <param name="procedureParams">params for procedure</param>
        /// <returns>Returns documents for the given property name</returns>
        public async Task<T> ExecuteStoredProcedureAsyncWithParameters<T>(string storedProcName, params dynamic[] procedureParams)
        {
            return await _documentClient.ExecuteStoredProcedureAsync<T>(UriFactory.CreateStoredProcedureUri(_cosmosDbSettings.DatabaseId, _cosmosDbSettings.CollectionId, storedProcName), procedureParams);
        }
    }
}
