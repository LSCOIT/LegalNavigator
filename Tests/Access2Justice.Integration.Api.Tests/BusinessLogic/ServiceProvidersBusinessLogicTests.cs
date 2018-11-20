using Access2Justice.Integration.Api.BusinessLogic;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
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
        
        [Theory]
        [MemberData(nameof(ServiceProvidersTestData.ServiceProviderUpsertInputData), MemberType = typeof(ServiceProvidersTestData))]
        public void UpsertServiceProviderAsyncTestsShouldReturnProperData(string topicName, JArray topic, string siteId, JArray serviceProviderResponseData, string serviceProviderId, JArray serviceProviderData, JArray serviceProviderJson, JArray providerDetailJson, dynamic expectedServiceProviderdata)
        {
            //arrange
            dynamic actualServiceProviderData = null;
            var dbResponse = dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.TopicsCollectionId, Constants.Name, topicName);
            dbResponse.ReturnsForAnyArgs(topic);
            string resourceType = Constants.ServiceProviderResourceType;
            List<string> propertyNames = new List<string>() { Constants.SiteId, Constants.ResourceType };
            List<string> values = new List<string>() { siteId, resourceType };
            var dbResponseFindItems = dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.ResourcesCollectionId, propertyNames, values);
            dbResponse.ReturnsForAnyArgs(serviceProviderResponseData);
            Document updatedDocument = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(serviceProviderData[0].ToString()));
            updatedDocument.LoadFrom(reader);
            backendDatabaseService.UpdateItemAsync<dynamic>(serviceProviderId, serviceProviderData, cosmosDbSettings.ResourcesCollectionId)
               .ReturnsForAnyArgs<Document>(updatedDocument);
            backendDatabaseService.CreateItemAsync<dynamic>(serviceProviderData, cosmosDbSettings.ResourcesCollectionId)
               .ReturnsForAnyArgs<Document>(updatedDocument);            

            //act
            var response = serviceProvidersBusinessLogic.UpsertServiceProviderDocumentAsync(serviceProviderJson, providerDetailJson, topicName).Result;
            foreach (var result in response)
            {
                actualServiceProviderData = result;
            }

            //assert
            Assert.Equal(expectedServiceProviderdata[0].ToString(), actualServiceProviderData.ToString());
        }

        [Theory]
        [MemberData(nameof(ServiceProvidersTestData.ServiceProviderUpsertMethodInputData), MemberType = typeof(ServiceProvidersTestData))]
        public void UpsertServiceProviderTestsShouldReturnProperData(JArray site, string id, dynamic topicTag, string description, dynamic expectedServiceProviderdata)
        {
            //arrange
            dynamic actualServiceProviderData = null;                    

            //act
            var response = serviceProvidersBusinessLogic.UpsertServiceProvider(site[0], id, topicTag, description);
            actualServiceProviderData = JsonUtilities.DeserializeDynamicObject<object>(response);
            actualServiceProviderData.createdTimeStamp = string.Empty;
            actualServiceProviderData.modifiedTimeStamp = string.Empty;

            //assert
            Assert.Equal(expectedServiceProviderdata[0].ToString(), actualServiceProviderData.ToString());
        }

        [Theory]
        [MemberData(nameof(ServiceProvidersTestData.GetServiceProviderReferencesInputData), MemberType = typeof(ServiceProvidersTestData))]
        public void GetServiceProviderReferencesTestsShouldReturnProperData(JArray site, dynamic expectedReferencesdata)
        {
            //arrange
            dynamic actualReferencesData = null;

            //act
            var response = serviceProvidersBusinessLogic.GetServiceProviderReferences(site[0]);
            actualReferencesData = JsonUtilities.DeserializeDynamicObject<object>(response);

            //assert
            Assert.Equal(expectedReferencesdata[0].ToString(), actualReferencesData[0].ToString());
        }

        [Theory]
        [MemberData(nameof(ServiceProvidersTestData.GetServiceProviderAddressInputData), MemberType = typeof(ServiceProvidersTestData))]
        public void GetServiceProviderAddressTestsShouldReturnProperData(JArray address, dynamic expectedAddress)
        {
            //act
            var response = serviceProvidersBusinessLogic.GetServiceProviderAddress(address);

            //assert
            Assert.Equal(expectedAddress, response);
        }

        [Theory]
        [MemberData(nameof(ServiceProvidersTestData.FormAddressInputData), MemberType = typeof(ServiceProvidersTestData))]
        public static void FormAddressTestsShouldReturnProperData(string address, dynamic expectedAddress)
        {
            //act
            var response = ServiceProvidersBusinessLogic.FormAddress(address);

            //assert
            Assert.Equal(expectedAddress, response);
        }

        [Theory]
        [MemberData(nameof(ServiceProvidersTestData.GetServiceProviderLocationInputData), MemberType = typeof(ServiceProvidersTestData))]
        public void GetServiceProviderLocationTestsShouldReturnProperData(JArray siteAddress, dynamic expectedLocationdata)
        {
            //arrange
            dynamic actualLocationData = null;

            //act
            var response = serviceProvidersBusinessLogic.GetServiceProviderLocation(siteAddress);
            actualLocationData = JsonUtilities.DeserializeDynamicObject<object>(response);

            //assert
            Assert.Equal(expectedLocationdata[0].ToString(), actualLocationData[0].ToString());
        }

        [Theory]
        [MemberData(nameof(ServiceProvidersTestData.GetServiceProviderOrgUnitInputData), MemberType = typeof(ServiceProvidersTestData))]
        public static void GetServiceProviderOrgUnitTestsShouldReturnProperData(JArray siteAddress, dynamic expectedOrgdata)
        {
            //act
            var response = ServiceProvidersBusinessLogic.GetServiceProviderOrgUnit(siteAddress);

            //assert
            Assert.Equal(expectedOrgdata, response);
        }

        [Theory]
        [MemberData(nameof(ServiceProvidersTestData.GetServiceProviderPhoneInputData), MemberType = typeof(ServiceProvidersTestData))]
        public static void GetServiceProviderPhoneTestsShouldReturnProperData(JArray sitePhone, dynamic expectedPhonedata)
        {
            //act
            var response = ServiceProvidersBusinessLogic.GetServiceProviderPhone(sitePhone);

            //assert
            Assert.Equal(expectedPhonedata, response);
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

        [Theory]
        [MemberData(nameof(ServiceProvidersTestData.GetServiceProviderDescriptionInputData), MemberType = typeof(ServiceProvidersTestData))]
        public static void GetServiceProviderDescriptionTestsShouldReturnProperData(string siteId, JArray providerJson, dynamic expectedDescription)
        {
            var providerDetailObjects = JsonUtilities.DeserializeDynamicObject<List<dynamic>>(providerJson);

            //act
            var response = ServiceProvidersBusinessLogic.GetServiceProviderDescription(providerDetailObjects, siteId);

            //assert
            Assert.Equal(expectedDescription, response);
        }

        [Theory]
        [MemberData(nameof(ServiceProvidersTestData.GetReviewerInputData), MemberType = typeof(ServiceProvidersTestData))]
        public void GetReviewerTestsShouldReturnProperData(JArray reviewer, dynamic expectedReviewer)
        {
            //act
            var response = serviceProvidersBusinessLogic.GetReviewer(reviewer);
            var actualReviewer = JsonUtilities.DeserializeDynamicObject<List<dynamic>>(response);
            
            //assert
            Assert.Equal(expectedReviewer[0].ToString(), actualReviewer[0].ToString());
        }

        [Theory]
        [MemberData(nameof(ServiceProvidersTestData.GetAvailabilityInputData), MemberType = typeof(ServiceProvidersTestData))]
        public void GetAvailabilityTestsShouldReturnProperData(JArray availability, dynamic expectedAvailability)
        {
            //act
            var response = serviceProvidersBusinessLogic.GetAvailability(availability[0]);
            var actualAvailability = JsonConvert.SerializeObject(response);
            var actualResult = JsonConvert.DeserializeObject(actualAvailability);

            //assert
            Assert.Equal(expectedAvailability[0].ToString(), actualResult.ToString());
        }

        [Theory]
        [MemberData(nameof(ServiceProvidersTestData.GetBusinessHoursInputData), MemberType = typeof(ServiceProvidersTestData))]
        public void GetBusinessHoursTestsShouldReturnProperData(JArray hours, dynamic expectedHours)
        {
            //act
            var response = serviceProvidersBusinessLogic.GetBusinessHours(hours);
            var actualHours = JsonConvert.SerializeObject(response);
            var actualResult = JsonConvert.DeserializeObject(actualHours);

            //assert
            Assert.Equal(expectedHours.ToString(), actualResult.ToString());
        }

        [Theory]
        [MemberData(nameof(ServiceProvidersTestData.GetAcceptanceCriteriaInputData), MemberType = typeof(ServiceProvidersTestData))]
        public void GetAcceptanceCriteriaTestsShouldReturnProperData(JArray acceptanceCriteria, dynamic expectedAcceptanceCriteria)
        {
            //act
            var response = serviceProvidersBusinessLogic.GetAcceptanceCriteria(acceptanceCriteria[0]);
            var actualAcceptanceCriteria = JsonConvert.SerializeObject(response);
            var actualResult = JsonConvert.DeserializeObject(actualAcceptanceCriteria);

            //assert
            Assert.Equal(expectedAcceptanceCriteria[0].ToString(), actualResult.ToString());
        }

        [Theory]
        [MemberData(nameof(ServiceProvidersTestData.GetEvaluatedRequirementsInputData), MemberType = typeof(ServiceProvidersTestData))]
        public void GetEvaluatedRequirementsTestsShouldReturnProperData(JArray evaluatedRequirements, dynamic expectedEvaluatedRequirements)
        {
            //act
            var response = serviceProvidersBusinessLogic.GetEvaluatedRequirements(evaluatedRequirements);
            var actualEvaluatedRequirements = JsonConvert.SerializeObject(response);
            var actualResult = JsonConvert.DeserializeObject(actualEvaluatedRequirements);

            //assert
            Assert.Equal(expectedEvaluatedRequirements.ToString(), actualResult.ToString());
        }

        [Theory]
        [MemberData(nameof(ServiceProvidersTestData.GetOnboardingInfoInputData), MemberType = typeof(ServiceProvidersTestData))]
        public void GetOnboardingInfoTestsShouldReturnProperData(JArray onboardingInfo, dynamic expectedOnboardingInfo)
        {
            //act
            var response = serviceProvidersBusinessLogic.GetOnboardingInfo(onboardingInfo[0]);
            var actualOnboardingInfo = JsonConvert.SerializeObject(response);
            var actualResult = JsonConvert.DeserializeObject(actualOnboardingInfo);

            //assert
            Assert.Equal(expectedOnboardingInfo[0].ToString(), actualResult.ToString());
        }
    }
}
