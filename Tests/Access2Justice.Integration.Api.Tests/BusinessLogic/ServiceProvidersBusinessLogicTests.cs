using Access2Justice.Integration.Api.BusinessLogic;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Models.Integration;
using Access2Justice.Shared.Utilities;
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
        public void GetServiceProviderAsyncTestsShouldReturnProperData(string serviceProviderId, JArray serviceProviderData, dynamic expectedServiceProviderdata)
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
        [MemberData(nameof(ServiceProvidersTestData.ServiceProviderUpsertInputData), MemberType = typeof(ServiceProvidersTestData))]
        public void UpsertServiceProviderAsyncTestsShouldReturnProperData(string topicName, JArray topic, string externalId, JArray serviceProviderResponseData, string serviceProviderId, JArray serviceProviderData, List<ServiceProvider> serviceProvider, dynamic expectedServiceProviderdata)
        {
            //arrange
            dynamic actualServiceProviderData = null;
            Location location = new Location();
            var dbResponseFindTopic = dynamicQueries.FindItemsWhereContainsWithLocationAsync(cosmosDbSettings.TopicsCollectionId, Constants.Name, topicName, location);
            dbResponseFindTopic.ReturnsForAnyArgs(topic);
            List<string> propertyNames = new List<string>() { Constants.ExternalId, Constants.ResourceType };
            List<string> values = new List<string>() { externalId, Constants.ServiceProviderResourceType };
            var dbResponseFindItems = dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.ResourcesCollectionId, propertyNames, values);
            dbResponseFindItems.ReturnsForAnyArgs(serviceProviderResponseData);
            Document updatedDocument = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(serviceProviderData[0].ToString()));
            updatedDocument.LoadFrom(reader);
            backendDatabaseService.UpdateItemAsync<dynamic>(serviceProviderId, serviceProviderData, cosmosDbSettings.ResourcesCollectionId)
               .ReturnsForAnyArgs<Document>(updatedDocument);
            backendDatabaseService.CreateItemAsync<dynamic>(serviceProviderData, cosmosDbSettings.ResourcesCollectionId)
               .ReturnsForAnyArgs<Document>(updatedDocument);

            //act
            var response = serviceProvidersBusinessLogic.UpsertServiceProviderDocumentAsync(serviceProvider, topicName).Result;
            foreach (var result in response)
            {
                actualServiceProviderData = result;
            }

            //assert
            Assert.Equal(expectedServiceProviderdata[0].ToString(), actualServiceProviderData.ToString());
        }

        [Theory]
        [MemberData(nameof(ServiceProvidersTestData.GetServiceProviderTopicTagsInputData), MemberType = typeof(ServiceProvidersTestData))]
        public void GetServiceProviderTopicTagsTestsShouldReturnProperData(string id, dynamic expectedTopicTag)
        {
            //arrage
            dynamic actualTopicTagData = null;

            //act
            var response = serviceProvidersBusinessLogic.GetServiceProviderTopicTags(id);
            actualTopicTagData = JsonUtilities.DeserializeDynamicObject<object>(response);

            //assert
            Assert.Equal(expectedTopicTag.ToString(), actualTopicTagData.ToString());
        }
    }
}
