using Access2Justice.Integration.Api.BusinessLogic;
using Access2Justice.Shared;
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

namespace Access2Justice.Integration.Api.Tests
{
    public class ServiceProvidersBusinessLogicTests
    {
        private readonly IDynamicQueries dynamicQueries;
        private readonly ICosmosDbSettings cosmosDbSettings;
        private readonly IBackendDatabaseService backendDatabaseService;
        private readonly ServiceProvidersBusinessLogic serviceProvidersBusinessLogic;
        private readonly ITopicsResourcesBusinessLogic topicsResourcesSettings;

        //Mocked input data.
        private readonly string serviceProviderId = ServiceProvidersTestData.serviceProviderId;
        private readonly JArray serviceProviderData = ServiceProvidersTestData.serviceProviderData;

        //Mocked result data.
        private readonly JArray emptyLocationData = ServiceProvidersTestData.emptyLocationData;
        private readonly JArray emptyReviewerData = ServiceProvidersTestData.emptyReviewerData;
        private readonly JArray expectedServiceProviderData = ServiceProvidersTestData.expectedServiceProviderdata;

        public ServiceProvidersBusinessLogicTests()
        {
            dynamicQueries = Substitute.For<IDynamicQueries>();
            cosmosDbSettings = Substitute.For<ICosmosDbSettings>();
            backendDatabaseService = Substitute.For<IBackendDatabaseService>();
            topicsResourcesSettings = Substitute.For<ITopicsResourcesBusinessLogic>();
            serviceProvidersBusinessLogic = new ServiceProvidersBusinessLogic(dynamicQueries, cosmosDbSettings, backendDatabaseService);
            cosmosDbSettings.AuthKey.Returns("dummykey");
            cosmosDbSettings.Endpoint.Returns(new System.Uri("https://bing.com"));
            cosmosDbSettings.DatabaseId.Returns("dbname");
            cosmosDbSettings.TopicsCollectionId.Returns("TopicCollection");
            cosmosDbSettings.ResourcesCollectionId.Returns("ResourceCollection");         
        }

        [Fact]
        public void GetServiceProviderAsyncTestsShouldReturnProperData()
        {
            //arrange
            var dbResponse = dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.ResourcesCollectionId, Constants.Id, serviceProviderId);            
            dbResponse.ReturnsForAnyArgs(serviceProviderData);

            //act
            var response = serviceProvidersBusinessLogic.GetServiceProviderDocumentAsync(serviceProviderId);

            //assert
            Assert.Equal(expectedServiceProviderData.ToString(), response.Result.ToString());
        }
    }
}
