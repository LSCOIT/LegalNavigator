using Access2Justice.Api.BusinessLogic;
using Access2Justice.Api.Tests.TestData;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Access2Justice.Api.Tests.BusinessLogic
{
    public class StaticResourceBusinessLogicTests
    {
        private readonly IDynamicQueries dynamicQueries;
        private readonly ICosmosDbSettings cosmosDbSettings;
        private readonly IBackendDatabaseService backendDatabaseService;
        private readonly StaticResourceBusinessLogic staticResourceBusinessLogic;
        //Mocked input data
        private readonly JArray homePageData = StaticResourceTestData.homePageData;
        private readonly JArray emptyData = JArray.Parse(@"[{}]");
        //expected data
        private readonly string expectedPageId = "HomePage";
        
        public StaticResourceBusinessLogicTests()
        {
            dynamicQueries = Substitute.For<IDynamicQueries>();
            cosmosDbSettings = Substitute.For<ICosmosDbSettings>();
            backendDatabaseService = Substitute.For<IBackendDatabaseService>();
            staticResourceBusinessLogic = new StaticResourceBusinessLogic(dynamicQueries, cosmosDbSettings, backendDatabaseService);
            cosmosDbSettings.AuthKey.Returns("dummykey");
            cosmosDbSettings.Endpoint.Returns(new System.Uri("https://bing.com"));
            cosmosDbSettings.DatabaseId.Returns("dbname");
            cosmosDbSettings.TopicCollectionId.Returns("TopicCollection");
            cosmosDbSettings.ResourceCollectionId.Returns("ResourceCollection");
            cosmosDbSettings.UserProfileCollectionId.Returns("UserProfile");
            cosmosDbSettings.StaticResourceCollectionId.Returns("StaticResource");
        }

        [Fact]
        public void GetStaticResourceDataAsyncTestsShouldReturnProperData()
        {
            //arrange      
            var dbResponse = dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.StaticResourceCollectionId, Constants.Id, expectedPageId);
            dbResponse.ReturnsForAnyArgs<dynamic>(homePageData);

            //act
            var response = staticResourceBusinessLogic.GetPageStaticResourceDataAsync(expectedPageId);
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains(expectedPageId, result, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void GetStaticResourceDataAsyncShouldReturnEmptyData()
        {
            //arrange      
            var dbResponse = dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.StaticResourceCollectionId, Constants.Id, expectedPageId);
            dbResponse.ReturnsForAnyArgs<dynamic>(emptyData);

            //act
            var response = staticResourceBusinessLogic.GetPageStaticResourceDataAsync(expectedPageId);
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.DoesNotContain(expectedPageId, result, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
