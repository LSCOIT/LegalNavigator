using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Access2Justice.CosmosDb;
using Access2Justice.CosmosDb.Interfaces;
using Access2Justice.Shared.Interfaces;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace Access2Justice.Api.BusinessLogic
{
    public class TopicsProxy: ITopicsProxy
    {

        private readonly IDocumentClient _documentClient;
        private readonly ICosmosDbSettings _cosmosDbSettings;

        public TopicsProxy(IDocumentClient documentClient, ICosmosDbSettings cosmosDbSettings)
        {
            _documentClient = documentClient;
            _cosmosDbSettings = cosmosDbSettings;          
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

    }
}
