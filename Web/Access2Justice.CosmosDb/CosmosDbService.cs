using Access2Justice.Shared;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Access2Justice.CosmosDb
{
    public class CosmosDbService<T> : IBackendDatabaseService<T> where T : class
    {
        private readonly ICosmosDbConfigurations _config;
        private readonly IConfigurationManager _configurationManager;
        private readonly IDocumentClient _documentClient;

        public CosmosDbService(IDocumentClient documentClient, IConfigurationManager configurationManager)
        {
            _documentClient = documentClient;
            _configurationManager = configurationManager;
            _config = _configurationManager.Bind<CosmosDbConfigurations>(Directory.GetCurrentDirectory(), "cosmosDb");

            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();
        }

        public async Task<T> GetItemAsync(string id)
        {
            try
            {
                Document document =
                    await _documentClient.ReadDocumentAsync(UriFactory.CreateDocumentUri(_config.DatabaseId, _config.CollectionId, id));
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> query = _documentClient.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(_config.DatabaseId, _config.CollectionId),
                new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public async Task<Document> CreateItemAsync(T item)
        {
            return await _documentClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_config.DatabaseId, _config.CollectionId), item);
        }

        public async Task<Document> UpdateItemAsync(string id, T item)
        {
            return await _documentClient.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(_config.DatabaseId, _config.CollectionId, id), item);
        }

        public async Task DeleteItemAsync(string id)
        {
            await _documentClient.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_config.DatabaseId, _config.CollectionId, id));
        }

        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await _documentClient.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(_config.DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await _documentClient.CreateDatabaseAsync(new Database { Id = _config.DatabaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await _documentClient.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(_config.DatabaseId, _config.CollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await _documentClient.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(_config.DatabaseId),
                        new DocumentCollection
                        {
                            Id = _config.CollectionId
                        },
                        new RequestOptions { OfferThroughput = 400 });
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
