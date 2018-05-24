using Access2Justice.CosmosDb.Interfaces;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.BusinessLogic
{
    public class TopicBusinessLogic
    {
        private readonly IDocumentClient _documentClient;
        private readonly ICosmosDbSettings _cosmosDbSettings;

        public TopicBusinessLogic(IDocumentClient documentClient, ICosmosDbSettings cosmosDbSettings)
        {
            _documentClient = documentClient;
            _cosmosDbSettings = cosmosDbSettings;

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

    }
}
