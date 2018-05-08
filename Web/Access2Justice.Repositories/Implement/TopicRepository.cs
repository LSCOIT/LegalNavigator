using System;
using System.Collections.Generic;
using System.Text;
using Access2Justice.Repositories.Interface;

using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using Access2Justice.Repositories.Models;

namespace Access2Justice.Repositories.Implement
{
   public class TopicRepository : ITopicRepository<TopicModel, string>
    {
        private static readonly string Endpoint = "https://access2justicedb.documents.azure.com:443/";
        private static readonly string Key = "gg5xMQt1F7oEo3vyKVDdUK4E3ZGCs410DusXEM0bm4Jm00VH2LbRN1jAmw3NYF6Rg79NKAvIu0GTOq6o0qwg2g==";
        private static readonly string DatabaseId = "access2justicedb";
        private static readonly string CollectionId = "TopicsDocument";
        private static DocumentClient docClient;

        public TopicRepository()
        {
            docClient = new DocumentClient(new Uri(Endpoint), Key);
           
        }

        public async Task<IEnumerable<TopicModel>> GetTopicsFromCollectionAsync()
        {
            var documents = docClient.CreateDocumentQuery<TopicModel>(
                  UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                  new FeedOptions { MaxItemCount = -1 })
                  .AsDocumentQuery();
            List<TopicModel> topics = new List<TopicModel>();
            while (documents.HasMoreResults)
            {
                topics.AddRange(await documents.ExecuteNextAsync<TopicModel>());
            }
            return topics;
        }

        public async Task<TopicModel> GetTopicsFromCollectionAsync(string id)
        {
            TopicModel model = new TopicModel();
            try
            {

                Document doc = await docClient.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId,id));

                return JsonConvert.DeserializeObject<TopicModel>(doc.ToString());
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

    }
}
