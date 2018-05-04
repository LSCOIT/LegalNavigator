using System;
using System.Collections.Generic;
using System.Text;
using Access2Justice.Repositories.Interface;
using Access2Justice.CosmosDbService.Models;
using Access2Justice.CosmosDbService;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;

namespace Access2Justice.Repositories.Implement
{
   public class TopicRepository : ITopicRepository
    {
        //private static readonly string Endpoint ="https://access2justicedb.documents.azure.com:443/";
        //private static readonly string Key = "gg5xMQt1F7oEo3vyKVDdUK4E3ZGCs410DusXEM0bm4Jm00VH2LbRN1jAmw3NYF6Rg79NKAvIu0GTOq6o0qwg2g==";
        //private static readonly string DatabaseId = "access2justicedb";
        //private static readonly string CollectionId = "TopicsDoccument";
        //private static DocumentClient docClient;

        //public TopicRepository()
        //{
        //    docClient = new DocumentClient(new Uri(Endpoint), Key);
        //    CreateDatabaseIfNotExistsAsync().Wait();
        //    CreateCollectionIfNotExistsAsync().Wait();
        //}
       
        //private static async Task CreateDatabaseIfNotExistsAsync()
        //{
        //    try
        //    {
        //        //1.
        //        await docClient.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
        //    }
        //    catch (DocumentClientException e)
        //    {
        //        if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
        //        {
        //            //2.
        //            await docClient.CreateDatabaseAsync(new Database { Id = DatabaseId });
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //}
       
        //private static async Task CreateCollectionIfNotExistsAsync()
        //{
        //    try
        //    {
        //        //1.
        //        await docClient.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
        //    }
        //    catch (DocumentClientException e)
        //    {
        //        if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
        //        {
        //            //2.
        //            await docClient.CreateDocumentCollectionAsync(
        //                UriFactory.CreateDatabaseUri(DatabaseId),
        //                new DocumentCollection { Id = CollectionId },
        //                new RequestOptions { OfferThroughput = 1000 });
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //}
      

        //public async Task<IEnumerable<TopicModel>> GetTopicsFromCollectionAsync()
        //{
        //    var documents = docClient.CreateDocumentQuery<TopicModel>(
        //          UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
        //          new FeedOptions { MaxItemCount = -1 })
        //          .AsDocumentQuery();
        //    List<TopicModel> persons = new List<TopicModel>();
        //    while (documents.HasMoreResults)
        //    {
        //        persons.AddRange(await documents.ExecuteNextAsync<TopicModel>());
        //    }
        //    return persons;
        //}

        //public async Task<TopicModel> GetTopicsFromCollectionAsync(string id)
        //{
        //    try
        //    {
        //        Document doc = await docClient.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));

        //        return JsonConvert.DeserializeObject<TopicModel>(doc.ToString());
        //    }
        //    catch (DocumentClientException e)
        //    {
        //        if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
        //        {
        //            return null;
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //}
    }
}
