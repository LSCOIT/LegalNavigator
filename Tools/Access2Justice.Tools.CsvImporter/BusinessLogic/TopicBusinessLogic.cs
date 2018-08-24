using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Access2Justice.Tools.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Access2Justice.Tools.BusinessLogic
{
    public class TopicBusinessLogic : IDisposable
    {
        private readonly string EndpointUrl = "https://access2justicedb.documents.azure.com:443/";
        private readonly string PrimaryKey = "zOHFmt7MZoGPPLiBOL3pZ1ompDcPIn7qHrvHWvot6ScBaIWl4hJrIlvAQnlIwcqeqFODFg8o4SXL9OoFo6j49Q==";
        private readonly string Database = "access2justicedb-tool";
        private readonly string TopicCollection = "Topics";
        private DocumentClient client;
        static HttpClient clientHttp = new HttpClient();

        public async Task<IEnumerable<Topic>> GetTopics()
        {
            this.client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
            clientHttp.BaseAddress = new Uri("http://localhost:4200/");
            clientHttp.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            InsertTopics obj = new InsertTopics();
            var content = obj.CreateJsonFromCSV();
            List<dynamic> topicsList = new List<dynamic>();
            Topics topics = new Topics
            {
                TopicsList = content.TopicsList
            };

            foreach (var topicList in topics.TopicsList)
            {
                var serializedResult = JsonConvert.SerializeObject(topicList);
                JObject jsonResult = (JObject)JsonConvert.DeserializeObject(serializedResult);
                topicsList.Add(jsonResult);
            }
            var serializedTopics = JsonConvert.SerializeObject(topicsList);
            var result = JsonConvert.DeserializeObject(serializedTopics);
            HttpResponseMessage response = await clientHttp.PostAsJsonAsync("api/createtopicdocument", result).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var items = await this.GetItemsFromCollectionAsync().ConfigureAwait(true);
            return items;
        }

        private async Task CreateTopicDocumentIfNotExists(string databaseName, string collectionName, object td)
        {
            await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), td).ConfigureAwait(true);
        }

        public async Task<IEnumerable<Topic>> GetItemsFromCollectionAsync()
        {
            var documents = client.CreateDocumentQuery<Topic>(
                  UriFactory.CreateDocumentCollectionUri(Database, TopicCollection),
                  new FeedOptions { MaxItemCount = -1 }).AsDocumentQuery();
            List<Topic> td = new List<Topic>();
            while (documents.HasMoreResults)
            {
                td.AddRange(await documents.ExecuteNextAsync<Topic>().ConfigureAwait(true));
            }
            return td;
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