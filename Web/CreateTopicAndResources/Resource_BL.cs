using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CreateTopicAndResources
{
    class Resource_BL
    {

        private string EndpointUrl = ConfigurationManager.AppSettings["EndpointUrl"];
        private string PrimaryKey = ConfigurationManager.AppSettings["PrimaryKey"];
        private string Database = ConfigurationManager.AppSettings["Database"];
        private string ResourceCollection = ConfigurationManager.AppSettings["ResourceCollection"];
        private string TopicCollection = ConfigurationManager.AppSettings["TopicCollection"];
        private DocumentClient client;

        public async Task<IEnumerable<Resource>> GetResources()
        {
            this.client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);

            await this.client.CreateDatabaseIfNotExistsAsync(new Database { Id = Database });

            await this.client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(Database),
            new DocumentCollection { Id = ResourceCollection });

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

                await this.CreateResourceDocumentIfNotExists(Database, ResourceCollection, resourceList);
            }

            var items = await this.GetItemsFromCollectionAsync();
            return items;
        }

        private async Task CreateResourceDocumentIfNotExists(string databaseName, string collectionName, Resource td)
        {
            await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), td);
        }

        public async Task<IEnumerable<Resource>> GetItemsFromCollectionAsync()
        {
            var documents = client.CreateDocumentQuery<Resource>(
                  UriFactory.CreateDocumentCollectionUri(Database, ResourceCollection),
                  new FeedOptions { MaxItemCount = -1 }).AsDocumentQuery();
            List<Resource> td = new List<Resource>();
            while (documents.HasMoreResults)
            {
                td.AddRange(await documents.ExecuteNextAsync<Resource>());
            }
            return td;
        }

        private async Task<dynamic> GetTopicAsync(string topicName)
        {
            var _query = string.Format("SELECT c.id FROM c WHERE c.name = " + "\"" + topicName + "\"");
            var result = await QueryTopicAsync(TopicCollection, _query);

            return result;
        }
        private async Task<dynamic> QueryTopicAsync(string TopicCollection, string query)
        {
            var docQuery = client.CreateDocumentQuery<dynamic>(
                UriFactory.CreateDocumentCollectionUri(Database, TopicCollection), query).AsDocumentQuery();

            var results = new List<dynamic>();
            while (docQuery.HasMoreResults)
            {
                results.AddRange(await docQuery.ExecuteNextAsync());
            }

            return results;
        }
    }
}