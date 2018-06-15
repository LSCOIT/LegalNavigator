using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Access2Justice.Tools.Models;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Access2Justice.Tools.BusinessLogic
{
    class ResourceBusinessLogic: IDisposable
    {
        private string EndpointUrl = "";
        private string PrimaryKey = "";
        private string Database = "access2justicedb";
        private string ResourceCollection = "Resources";
        private string TopicCollection = "Topics";
        private DocumentClient client;

        public async Task<IEnumerable<Models.Resource>> GetResources()
        {
            this.client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
            await this.client.CreateDatabaseIfNotExistsAsync(new Database { Id = Database }).ConfigureAwait(true);
            await this.client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(Database),
            new DocumentCollection { Id = ResourceCollection }).ConfigureAwait(true);

            InsertResources obj = new InsertResources();
            var content = obj.CreateJsonFromCSV();
            
            Resources Resources = new Resources
            {
                ResourcesList = content.ResourcesList
            };

            foreach (var resourceList in Resources.ResourcesList)
            {
                var referenceTags = new List<ReferenceTag>();
                foreach (var referenceId in resourceList.ReferenceTags)
                {
                    if (referenceId.ReferenceTags != "")
                    {
                        var referenceTagId = await this.GetTopicAsync(referenceId.ReferenceTags);
                        foreach (var tag in referenceTagId)
                        {
                            referenceTags.Add(new ReferenceTag { ReferenceTags = tag.id });
                        }
                    }
                }
                resourceList.ReferenceTags = referenceTags;
                var serializedResult = JsonConvert.SerializeObject(resourceList);
                JObject result = (JObject)JsonConvert.DeserializeObject(serializedResult);

                await this.CreateResourceDocumentIfNotExists(Database, ResourceCollection, result).ConfigureAwait(true);
            }

            var items = await this.GetItemsFromCollectionAsync().ConfigureAwait(true);
            return items;
        }

        private async Task CreateResourceDocumentIfNotExists(string databaseName, string collectionName, object td)
        {
            await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), td).ConfigureAwait(true);
        }

        public async Task<IEnumerable<Models.Resource>> GetItemsFromCollectionAsync()
        {
            var documents = client.CreateDocumentQuery<Models.Resource>(
                  UriFactory.CreateDocumentCollectionUri(Database, ResourceCollection),
                  new FeedOptions { MaxItemCount = -1 }).AsDocumentQuery();
            List<Models.Resource> td = new List<Models.Resource>();
            while (documents.HasMoreResults)
            {
                td.AddRange(await documents.ExecuteNextAsync<Models.Resource>().ConfigureAwait(true));
            }
            return td;
        }

        private async Task<dynamic> GetTopicAsync(string topicName)
        {
            var _query = "SELECT c.id FROM c WHERE c.name = " + "\"" + topicName + "\"";
            var result = await QueryTopicAsync(TopicCollection, _query).ConfigureAwait(true);
            return result;            
        }

        //private void createCosmosDbInstance() {
        //    IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
        //    configurationBuilder.AddJsonFile("AppSettings.json");
        //    IConfiguration configuration = configurationBuilder.Build();

        //    CosmosDb.Interfaces.ICosmosDbSettings cosmosDbSettings = new CosmosDbSettings(configuration.GetSection("CosmosDb"));

        //    IDocumentClient  documentClient= new DocumentClient(cosmosDbSettings.Endpoint, cosmosDbSettings.AuthKey);
        //    CosmosDbService cosmosDbService = new CosmosDbService(documentClient, cosmosDbSettings);
        //}

        private async Task<dynamic> QueryTopicAsync(string TopicCollection, string query)
        {
            var docQuery = client.CreateDocumentQuery<dynamic>(
                UriFactory.CreateDocumentCollectionUri(Database, TopicCollection), query).AsDocumentQuery();

            var results = new List<dynamic>();
            while (docQuery.HasMoreResults)
            {
                results.AddRange(await docQuery.ExecuteNextAsync().ConfigureAwait(true));
            }
            return results;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}