using Xunit;
using NSubstitute;
using System.Collections.Generic;
using System.Net.Http;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Api;
using Newtonsoft.Json.Linq;
using Access2Justice.CosmosDb.Interfaces;
using Access2Justice.Api.BusinessLogic;

namespace Access2Justice.Api.Tests.BusinessLogic
{
    public class TopicsResourcesBusinessLogicTests
    {
        private readonly IBackendDatabaseService _backendDatabaseService;
        private readonly ICosmosDbSettings _cosmosDbSettings;
        private readonly TopicsResourcesBusinessLogic topicsResourcesBusinessLogic;

        public TopicsResourcesBusinessLogicTests()
        {
            _backendDatabaseService = Substitute.For<IBackendDatabaseService>();
            _cosmosDbSettings = Substitute.For<ICosmosDbSettings>();

            topicsResourcesBusinessLogic = new TopicsResourcesBusinessLogic(_backendDatabaseService, _cosmosDbSettings);
            _cosmosDbSettings.AuthKey.Returns("69kXp6uzHNUkG8gr==");
            _cosmosDbSettings.Endpoint.Returns(new System.Uri("https://access2justicedb.documents.azure.com:443/"));
            _cosmosDbSettings.DatabaseId.Returns("a2jdb");
            _cosmosDbSettings.TopicCollectionId.Returns("Topic");
            _cosmosDbSettings.ResourceCollectionId.Returns("Resource");

        }

        //[Fact]
        //public void GetResourcesAsyncMethod()
        //{
        //    //arrange
        //    string topicIds = "7894654656";
        //    string query = "select * from t";
        //    JArray topicsData = JArray.Parse(@"[{'id':'addf41e9-1a27-4aeb-bcbb-7959f95094ba','name':'Family','parentTopicID':'','keywords':'eviction','location':[{'state':'Hawaii','county':'Kalawao County','city':'Kalawao','zipCode':'96742'},{'zipCode':'96741'},{'state':'Hawaii','county':'Honolulu County','city':'Honolulu'},{'state':'Hawaii','city':'Hawaiian Beaches'},{'state':'Hawaii','city':'Haiku-Pauwela'},{'state':'Alaska'}],'jsonContent':'','icon':'./assets/images/topics/topic14.png','createdBy':'','createdTimeStamp':'','modifiedBy':'','modifiedTimeStamp':'','_rid':'mwoSALHtpAEBAAAAAAAAAA==','_self':'dbs/mwoSAA==/colls/mwoSALHtpAE=/docs/mwoSALHtpAEBAAAAAAAAAA==/','_etag':'\'05008e57-0000-0000-0000-5b0797c10000\'','_attachments':'attachments/','_ts':1527224257},{'id':'3e278591-ec50-479d-8e38-ae9a9d4cabd9','name':'Eviction','parentTopicID':'','keywords':'eviction','location':[{'state':'Hawaii','county':'Kalawao County','city':'Kalawao','zipCode':'96742'},{'zipCode':'96741'},{'state':'Hawaii','county':'Honolulu County','city':'Honolulu'},{'state':'Hawaii','city':'Hawaiian Beaches'},{'state':'Hawaii','city':'Haiku-Pauwela'},{'state':'Alaska'}],'jsonContent':'','icon':'./assets/images/topics/topic14.png','createdBy':'','createdTimeStamp':'','modifiedBy':'','modifiedTimeStamp':'','_rid':'mwoSALHtpAEBAAAAAAAAAA==','_self':'dbs/mwoSAA==/colls/mwoSALHtpAE=/docs/mwoSALHtpAEBAAAAAAAAAA==/','_etag':'\'05008e57-0000-0000-0000-5b0797c10000\'','_attachments':'attachments/','_ts':1527224257},{'id':'3aa3a1be-8291-42b1-85c2-252f756febbc','name':'Housing','parentTopicID':'','keywords':'eviction','location':[{'state':'Hawaii','county':'Kalawao County','city':'Kalawao','zipCode':'96742'},{'zipCode':'96741'},{'state':'Hawaii','county':'Honolulu County','city':'Honolulu'},{'state':'Hawaii','city':'Hawaiian Beaches'},{'state':'Hawaii','city':'Haiku-Pauwela'},{'state':'Alaska'}],'jsonContent':'','icon':'./assets/images/topics/topic14.png','createdBy':'','createdTimeStamp':'','modifiedBy':'','modifiedTimeStamp':'','_rid':'mwoSALHtpAEBAAAAAAAAAA==','_self':'dbs/mwoSAA==/colls/mwoSALHtpAE=/docs/mwoSALHtpAEBAAAAAAAAAA==/','_etag':'\'05008e57-0000-0000-0000-5b0797c10000\'','_attachments':'attachments/','_ts':1527224257}]");            
           
        //    var dbResponse = _backendDatabaseService.QueryItemsAsync(_cosmosDbSettings.ResourceCollectionId, query);
        //    dbResponse.Returns(topicsData);

        //    var response = topicsResourcesBusinessLogic.GetResourcesAsync(topicIds);
        //}
    }
}
