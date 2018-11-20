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
        
        [Theory]
        [MemberData(nameof(ServiceProvidersTestData.ServiceProviderGetInputData), MemberType = typeof(ServiceProvidersTestData))]
        public void GetServiceProviderAsyncTestsShouldReturnProperData(string serviceProviderId , JArray serviceProviderData, dynamic expectedServiceProviderdata)
        {
            //arrange
            var dbResponse = dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.ResourcesCollectionId, Constants.Id, serviceProviderId);
            dbResponse.ReturnsForAnyArgs(serviceProviderData);

            //act
            var response = serviceProvidersBusinessLogic.GetServiceProviderDocumentAsync(serviceProviderId);

            //assert
            Assert.Equal(expectedServiceProviderdata.ToString(), response.Result.ToString());
        }

        [Theory]
        [MemberData(nameof(ServiceProvidersTestData.ServiceProviderDeleteInputData), MemberType = typeof(ServiceProvidersTestData))]
        public void DeleteServiceProviderAsyncTestsShouldReturnProperData(string serviceProviderId, string serviceProviderData, dynamic expectedServiceProviderdata)
        {
            //arrange
            var dbResponse = backendDatabaseService.DeleteItemAsync(serviceProviderId, cosmosDbSettings.ResourcesCollectionId);
            dbResponse.Returns(serviceProviderData);

            //act
            var response = serviceProvidersBusinessLogic.DeleteServiceProviderDocumentAsync(serviceProviderId);

            //assert
            Assert.Equal(expectedServiceProviderdata.ToString(), response.Result.ToString());
        }
    }
}
