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
    public class TopicsResourcesBusinessLogicTests
    {
        private readonly IDynamicQueries dynamicQueries;
        private readonly ICosmosDbSettings cosmosDbSettings;
        private readonly IBackendDatabaseService backendDatabaseService;
        private readonly TopicsResourcesBusinessLogic topicsResourcesBusinessLogic;
        private readonly ITopicsResourcesBusinessLogic topicsResourcesSettings;

        //Mocked input data.
        private readonly string keyword = "eviction";
        private readonly string query = "select * from t";
        private readonly string procedureName = "GetParentTopics";
        private readonly string topicId = "addf41e9-1a27-4aeb-bcbb-7959f95094ba";
        private readonly List<string> topicIds = new List<string> { "addf41e9-1a27-4aeb-bcbb-7959f95094ba" };
        private readonly Location location = new Location();  
        private readonly string topicName = "Family";
        private readonly string resourceName = "Action Plan";
        private readonly JArray emptyData = JArray.Parse(@"[{}]");
        private readonly JArray topicsData = TopicResourceTestData.topicsData;
        private readonly JArray resourcesData = TopicResourceTestData.resourcesData;
        private readonly JArray breadcrumbData = TopicResourceTestData.breadcrumbData;
        private readonly JArray resourceCountData = TopicResourceTestData.resourceCountData;
        private readonly ResourceFilter resourceFilter = TopicResourceTestData.resourceFilter;
        private readonly JArray formData = TopicResourceTestData.formData;
        private readonly JArray actionPlanData = TopicResourceTestData.actionPlanData;
        private readonly JArray referencesInputData = TopicResourceTestData.referencesInputData;
        private readonly JArray articleData = TopicResourceTestData.articleData;
        private readonly JArray videoData = TopicResourceTestData.videoData;
        private readonly JArray organizationData = TopicResourceTestData.organizationData;
        private readonly JArray essentialReadingData = TopicResourceTestData.essentialReadingData;
        private readonly JArray topicData = TopicResourceTestData.topicData;
        private readonly JArray referenceTagData = TopicResourceTestData.referenceTagData;
        private readonly JArray parentTopicIdData = TopicResourceTestData.parentTopicIdData;
        private readonly JArray locationData = TopicResourceTestData.locationData;
        private readonly JArray conditionData = TopicResourceTestData.conditionData;
        private readonly JArray emptyResourceData = TopicResourceTestData.emptyResourceData;

        //Mocked result data.
        private readonly string expectedEmptyArrayObject = "[{}]";
        private readonly string emptyReferenceTagData = "";
        private readonly JArray emptyLocationData = TopicResourceTestData.emptyLocationData;
        private readonly JArray emptyConditionObject = TopicResourceTestData.emptyConditionObject;
        private readonly JArray EmptyReferences = TopicResourceTestData.EmptyReferences;
        private readonly string expectedTopicId = TopicResourceTestData.expectedTopicId;
        private readonly string expectedResourceId = TopicResourceTestData.expectedResourceId;
        private readonly string expectedpagedResource = TopicResourceTestData.expectedpagedResource;
        private readonly string expectedResourceCount = TopicResourceTestData.expectedResourceCount;
        private readonly string expectedEmptyResourceCount = TopicResourceTestData.expectedEmptyResourceCount;
        private readonly JArray expectedformData = TopicResourceTestData.expectedformData;
        private readonly JArray expectedActionPlanData = TopicResourceTestData.expectedActionPlanData;
        private readonly JArray expectedArticleData = TopicResourceTestData.expectedArticleData;
        private readonly JArray expectedVideoData = TopicResourceTestData.expectedVideoData;
        private readonly JArray expectedOrganizationData = TopicResourceTestData.expectedOrganizationData;
        private readonly JArray expectedEssentialReadingData = TopicResourceTestData.expectedEssentialReadingData;
        private readonly JArray expectedTopicData = TopicResourceTestData.expectedTopicData;
        private readonly JArray expectedTopicsData = TopicResourceTestData.expectedTopicsData;
        private readonly string expectedReferenceTagData = TopicResourceTestData.expectedReferenceTagData;
        private readonly string expectedParentTopicIdData = TopicResourceTestData.expectedParentTopicIdData;
        private readonly JArray expectedLocationData = TopicResourceTestData.expectedLocationData;
        private readonly JArray expectedReferenceLocationData = TopicResourceTestData.expectedReferenceLocationData;
        private readonly JArray expectedConditionData = TopicResourceTestData.expectedConditionData;
        private readonly JArray expectedResourceReferences = TopicResourceTestData.expectedResourceReferences;
        private readonly JArray expectedActionPlanReferences = TopicResourceTestData.expectedActionPlanReferences;
        private readonly JArray expectedReferencesData = TopicResourceTestData.expectedReferencesData;
        private readonly Location expectedLocationValue = TopicResourceTestData.expectedLocationValue;
        public TopicsResourcesBusinessLogicTests()
        {
            dynamicQueries = Substitute.For<IDynamicQueries>();
            cosmosDbSettings = Substitute.For<ICosmosDbSettings>();
            backendDatabaseService = Substitute.For<IBackendDatabaseService>();
            topicsResourcesSettings = Substitute.For<ITopicsResourcesBusinessLogic>();

            topicsResourcesBusinessLogic = new TopicsResourcesBusinessLogic(dynamicQueries, cosmosDbSettings, backendDatabaseService);
            cosmosDbSettings.AuthKey.Returns("dummykey");
            cosmosDbSettings.Endpoint.Returns(new System.Uri("https://bing.com"));
            cosmosDbSettings.DatabaseId.Returns("dbname");
            cosmosDbSettings.TopicCollectionId.Returns("TopicCollection");
            cosmosDbSettings.ResourceCollectionId.Returns("ResourceCollection");

            topicsResourcesBusinessLogic = new TopicsResourcesBusinessLogic(dynamicQueries, cosmosDbSettings, backendDatabaseService);            
        }

        [Fact]
        public void GetTopicAsyncTestsShouldReturnProperData()
        {
            //arrange
            var dbResponse = dynamicQueries.FindItemsWhereContainsWithLocationAsync(cosmosDbSettings.TopicCollectionId, "keywords", keyword, location);            
            dbResponse.ReturnsForAnyArgs(topicsData);

            //act
            var response = topicsResourcesBusinessLogic.GetTopicsAsync(keyword, location);
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains(expectedTopicId, result, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void GetTopicAsyncTestsShouldReturnEmptyData()
        {
            //arrange
            var dbResponse = dynamicQueries.FindItemsWhereContainsWithLocationAsync(cosmosDbSettings.TopicCollectionId, "keywords", keyword, location);
            dbResponse.ReturnsForAnyArgs(emptyData);

            //act
            var response = topicsResourcesBusinessLogic.GetTopicsAsync(keyword, location);
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains(expectedEmptyArrayObject, result, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void GetPagedResourceAsyncTestsShouldReturnProperData()
        {
            //arrange
            PagedResources pagedResources = new PagedResources { Results = resourcesData, ContinuationToken = "[]" };
            var dbResponse = topicsResourcesBusinessLogic.ApplyPaginationAsync(resourceFilter);            
            dbResponse.ReturnsForAnyArgs(pagedResources);

            //act
            var response = topicsResourcesBusinessLogic.GetPagedResourceAsync(resourceFilter);
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains(expectedResourceId, result, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void GetPagedResourceAsyncTestsShouldReturnEmptyData()
        {
            //arrange
            PagedResources pagedResources = new PagedResources { Results = emptyData, ContinuationToken = null };
            var dbResponse = topicsResourcesBusinessLogic.ApplyPaginationAsync(resourceFilter);
            dbResponse.ReturnsForAnyArgs(pagedResources);            

            //act
            var response = topicsResourcesBusinessLogic.GetPagedResourceAsync(resourceFilter);
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains("continuationToken", result, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void GetTopicsAsyncTestsShouldReturnProperData()
        {
            //arrange
            var dbResponse = dynamicQueries.FindItemsWhereWithLocationAsync(cosmosDbSettings.TopicCollectionId, query, "",location);
            dbResponse.ReturnsForAnyArgs<dynamic>(topicsData);
            //act
            var response = topicsResourcesBusinessLogic.GetTopLevelTopicsAsync(expectedLocationValue).Result;
            string result = JsonConvert.SerializeObject(response);
            //assert
            Assert.Contains(expectedTopicId, result, StringComparison.InvariantCulture);
        }

        [Fact]
        public void GetTopicsAsyncTestsShouldReturnEmptyData()
        {
            //arrange
            var dbResponse = dynamicQueries.FindItemsWhereWithLocationAsync(cosmosDbSettings.TopicCollectionId, query, "",location);
            dbResponse.ReturnsForAnyArgs<dynamic>(emptyData);

            //act
            var response = topicsResourcesBusinessLogic.GetTopLevelTopicsAsync(expectedLocationValue);
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains("[{}]", result, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void GetResourcesAsyncTestsShouldReturnProperData()
        {
            //arrange
            var dbResponse = dynamicQueries.FindItemsWhereArrayContainsAsync(cosmosDbSettings.ResourceCollectionId, "topicTags", "id", new List<string>());
            dbResponse.ReturnsForAnyArgs<dynamic>(resourcesData);

            //act
            var response = topicsResourcesBusinessLogic.GetResourcesAsync(topicsData);
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains(expectedResourceId, result, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void GetResourcesAsyncTestsShouldReturnEmptyData()
        {
            //arrange
            var dbResponse = dynamicQueries.FindItemsWhereArrayContainsAsync(cosmosDbSettings.ResourceCollectionId, "topicTags", "id", topicIds);
            dbResponse.ReturnsForAnyArgs<dynamic>(emptyData);

            //act
            var response = topicsResourcesBusinessLogic.GetResourcesAsync(emptyData);
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains(expectedEmptyArrayObject, result, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void GetSubTopicsAsyncTestsShouldReturnProperData()
        {
            //arrange
            var dbResponse = dynamicQueries.FindItemsWhereArrayContainsAsyncWithLocation(cosmosDbSettings.TopicCollectionId, query, "", "",location);
            dbResponse.ReturnsForAnyArgs<dynamic>(resourcesData);
            //act
            var response = topicsResourcesBusinessLogic.GetSubTopicsAsync(TopicResourceTestData.TopicInput).Result;
            string result = JsonConvert.SerializeObject(response);
            //assert
            Assert.Contains(expectedResourceId, result, StringComparison.InvariantCulture);
        }

        [Fact]
        public void GetSubTopicsAsyncTestsShouldReturnEmptyData()
        {
            //arrange
            var dbResponse = dynamicQueries.FindItemsWhereArrayContainsAsyncWithLocation(cosmosDbSettings.TopicCollectionId, query, "", "",location);
            dbResponse.ReturnsForAnyArgs<dynamic>(emptyData);

            //act
            var response = topicsResourcesBusinessLogic.GetSubTopicsAsync(TopicResourceTestData.TopicInput);
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains("[{}]", result, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void GetSubTopicDetailsAsyncTestsShouldReturnProperData()
        {
            //arrange
            var dbResponse = dynamicQueries.FindItemsWhereArrayContainsAsyncWithLocation(cosmosDbSettings.ResourceCollectionId, "topicTags", "id", topicId,location);
            dbResponse.ReturnsForAnyArgs<dynamic>(topicsData);
            //act
            var response = topicsResourcesBusinessLogic.GetResourceAsync(TopicResourceTestData.TopicInput).Result;
            string result = JsonConvert.SerializeObject(response);
            //assert
            Assert.Contains(topicId, result, StringComparison.InvariantCulture);
        }

        [Fact]
        public void GetSubTopicDetailsAsyncTestsShouldReturnEmptyData()
        {
            //arrange
            var dbResponse = dynamicQueries.FindItemsWhereArrayContainsAsyncWithLocation(cosmosDbSettings.ResourceCollectionId, "topicTags", "id", "",location);
            dbResponse.ReturnsForAnyArgs<dynamic>(emptyData);

            //act
            var response = topicsResourcesBusinessLogic.GetResourceAsync(TopicResourceTestData.TopicInput);
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains("[{}]", result, StringComparison.InvariantCultureIgnoreCase);
        }
                
        [Fact]
        public void GetTopicDetailsAsyncTestsShouldReturnProperData()
        {
            //arrange
            var dbResponse = dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.TopicCollectionId, "name", topicName);
            dbResponse.ReturnsForAnyArgs<dynamic>(topicData);

            //act
            var response = topicsResourcesBusinessLogic.GetTopicDetailsAsync(topicName).Result;
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains(topicName, result, StringComparison.InvariantCulture);
        }

        [Fact]
        public void GetTopicDetailsAsyncTestsShouldReturnEmptyData()

        {
            //arrange
            var dbResponse = dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.TopicCollectionId, "name", "");
            dbResponse.ReturnsForAnyArgs<dynamic>(emptyData);

            //act
            var response = topicsResourcesBusinessLogic.GetTopicDetailsAsync(topicId);
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains("[{}]", result, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void GetResourceDetailAsyncTestsShouldReturnProperData()
        {
            //arrange
            var resourceType = "Action Plans";
            List<string> propertyNames = new List<string>() { "name", "resourceType" };
            List<string> values = new List<string>() { resourceName, resourceType };
            var dbResponse = dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.ResourceCollectionId, propertyNames, values);
            dbResponse.ReturnsForAnyArgs<dynamic>(actionPlanData);
            
            //act
            var response = topicsResourcesBusinessLogic.GetResourceDetailAsync(resourceName, resourceType).Result;
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains(resourceName, result, StringComparison.InvariantCulture);
        }

        [Fact]
        public void GetResourceDetailAsyncTestsShouldReturnEmptyData()

        {
            //arrange
            var resourceName = "";
            var resourceType = "Action Plans";
            List<string> propertyNames = new List<string>() { "name", "resourceType" };
            List<string> values = new List<string>() { resourceName, resourceType };
            var dbResponse = dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.ResourceCollectionId, propertyNames, values);
            dbResponse.ReturnsForAnyArgs<dynamic>(emptyData);

            //act
            var response = topicsResourcesBusinessLogic.GetResourceDetailAsync(resourceName, resourceType);
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains("[{}]", result, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void GetReferncesTestsShouldReturnProperData()
        {
            //arrange
            var referenceInput = this.referencesInputData;
            var referenceTag = this.referenceTagData;
            var location = this.locationData;
            var condition = this.conditionData;
            var parentTopic = this.parentTopicIdData;

            //act
            var dbResponseReferenceTag = topicsResourcesSettings.GetReferenceTags(referenceTag).ReturnsForAnyArgs<dynamic>(expectedReferenceTagData);
            var dbResponseLocation = topicsResourcesSettings.GetLocations(location).ReturnsForAnyArgs<dynamic>(expectedReferenceLocationData);
            var dbResponseConditions = topicsResourcesSettings.GetConditions(condition).ReturnsForAnyArgs<dynamic>(expectedConditionData);
            var dbResponseParentTopicId = topicsResourcesSettings.GetParentTopicIds(parentTopic).ReturnsForAnyArgs<dynamic>(expectedParentTopicIdData);
            var response = topicsResourcesBusinessLogic.GetReferences(referenceInput[0]);
            var expectedReferenceData = JsonConvert.SerializeObject(expectedReferencesData);
            var actualReferenceData = JsonConvert.SerializeObject(response);
            
            //assert
            Assert.Equal(expectedReferenceData, actualReferenceData);
        }

        [Fact]
        public void GetReferncesTestsShouldReturnEmptyData()
        {
            //arrange
            var emptyResource = this.emptyResourceData;
            var referenceTag = this.emptyData;
            var location = this.emptyLocationData;
            var conditon = this.emptyData;
            var parentTopic = this.emptyData;

            //act
            var dbResponseReferenceTag = topicsResourcesSettings.GetReferenceTags(referenceTag).ReturnsForAnyArgs<dynamic>(emptyReferenceTagData);
            var dbResponseLocation = topicsResourcesSettings.GetLocations(location).ReturnsForAnyArgs<dynamic>(emptyLocationData);
            var dbResponseCondition = topicsResourcesSettings.GetConditions(conditon).ReturnsForAnyArgs<dynamic>(emptyData);
            var dbResponseParentTopic = topicsResourcesSettings.GetParentTopicIds(parentTopic).ReturnsForAnyArgs<dynamic>(emptyData);
            var response = topicsResourcesBusinessLogic.GetReferences(emptyResource[0]);
            var ActualReferenceData = JsonConvert.SerializeObject(response);
            var expectedReferencesData = JsonConvert.SerializeObject(EmptyReferences);

            //assert
            Assert.Equal(expectedReferencesData, ActualReferenceData);
        }

        [Fact]
        public void GetReferenceTagsTestsShouldReturnProperData()
        {
            //arrange
            var referenceTag = this.referenceTagData;

            //act
            var response = topicsResourcesBusinessLogic.GetReferenceTags(referenceTag);
            
            //assert
            Assert.Equal(expectedReferenceTagData, response[0].ReferenceTags);
        }

        [Fact]
        public void GetReferenceTagsTestsShouldReturnEmptyData()
        {
            //arrange
            var referenceTag = this.emptyData;            

            //act
            var response = topicsResourcesBusinessLogic.GetReferenceTags(referenceTag);
            
            //assert
            Assert.Equal(emptyReferenceTagData, response[0].ReferenceTags);
        }

        [Fact]
        public void GetLocationTestsShouldReturnProperData()
        {
            //arrange
            var location = this.locationData;

            //act
            var response = topicsResourcesBusinessLogic.GetLocations(location);
            var actualLocation = JsonConvert.SerializeObject(response);
            var expectedLocation = JsonConvert.SerializeObject(expectedLocationData);

            //assert
            Assert.Equal(expectedLocation, actualLocation);
        }

        [Fact]
        public void GetLocationTestsShouldReturnEmptyData()
        {
            //arrange
            var location = this.emptyData;

            //act
            var response = topicsResourcesBusinessLogic.GetLocations(location);
            var actualLocation = JsonConvert.SerializeObject(response);
            var expectedLocation = JsonConvert.SerializeObject(emptyLocationData);

            //assert
            Assert.Equal(expectedLocation, actualLocation);
        }

        [Fact]
        public void GetConditionTestsShouldReturnProperData()
        {
            //arrange
            var condition = this.conditionData;

            //act
            var response = topicsResourcesBusinessLogic.GetConditions(condition);
            var actualCondition = JsonConvert.SerializeObject(response);
            var expectedCondition = JsonConvert.SerializeObject(expectedConditionData);

            //assert
            Assert.Equal(expectedCondition, actualCondition);
        }

        [Fact]
        public void GetConditionTestsShouldReturnEmptyData()
        {
            //arrange
            var condition = this.emptyData;

            //act
            var response = topicsResourcesBusinessLogic.GetConditions(condition);
            var actualCondition = JsonConvert.SerializeObject(response);
            var expectedCondition = JsonConvert.SerializeObject(emptyConditionObject);

            //assert
            Assert.Equal(expectedCondition, actualCondition);
        }

        [Fact]
        public void CreateResourceUploadAsyncTestsShouldReturnProperData()
        {
            //arrange
            var form = this.formData;
            var resource = JsonConvert.SerializeObject(form);
            Document document = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(form[0].ToString()));
            document.LoadFrom(reader);
            dynamic actualResourceData = null;

            //act
            var dbResponse = backendDatabaseService.CreateItemAsync<dynamic>(form, cosmosDbSettings.ResourceCollectionId).ReturnsForAnyArgs(document);
            var dbResponseResource = topicsResourcesSettings.CreateResourceDocumentAsync(resource).ReturnsForAnyArgs(form[0]);
            string filePath = Path.Combine(Environment.CurrentDirectory, "TestData\\ResourceData.json");
            var response = topicsResourcesBusinessLogic.CreateResourcesUploadAsync(filePath).Result;
            foreach (var result in response)
            {
                actualResourceData = result;
            }

            //assert
            Assert.Equal(expectedformData[0].ToString(), actualResourceData.ToString());
        }

        [Fact]
        public void CreateResourceAsyncTestsShouldReturnProperData()
        {
            //arrange
            var form = this.formData;
            var resource = JsonConvert.SerializeObject(form);
            Document document = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(form[0].ToString()));
            document.LoadFrom(reader);
            dynamic actualResourceData = null;

            //act
            var dbResponse = backendDatabaseService.CreateItemAsync<dynamic>(form, cosmosDbSettings.ResourceCollectionId).ReturnsForAnyArgs(document);
            var dbResponseReferenceTag = topicsResourcesSettings.CreateResourcesForms(form[0]).ReturnsForAnyArgs<dynamic>(expectedformData[0]);
            var response = topicsResourcesBusinessLogic.CreateResourceDocumentAsync(resource).Result;
            foreach (var result in response)
            {
                actualResourceData = result;
            }

            //assert
            Assert.Equal(expectedformData[0].ToString(), actualResourceData.ToString());
        }

        [Fact]
        public void CreateResourcesFormsTestsShouldReturnProperData()  //To do - CreateFormsAsyncEmptyData after excpetion logging
        {
            //arrange
            var form = this.formData[0];

            //act
            var dbResponseReferenceTag = topicsResourcesSettings.GetReferenceTags(referenceTagData).ReturnsForAnyArgs<dynamic>(expectedReferenceTagData);
            var dbResponseLocation = topicsResourcesSettings.GetLocations(locationData).ReturnsForAnyArgs<dynamic>(expectedReferenceLocationData);
            var dbResponse = topicsResourcesSettings.GetReferences(form).ReturnsForAnyArgs<dynamic>(expectedResourceReferences);
            var response = topicsResourcesBusinessLogic.CreateResourcesForms(form);
            var result = JsonConvert.SerializeObject(response);
            var formResult = (JObject)JsonConvert.DeserializeObject(result);
            result = formResult;
            foreach (JProperty field in result)
            {
                if (field.Name == "createdTimeStamp")
                {
                    field.Value = "";
                }

                else if (field.Name == "modifiedTimeStamp")
                {
                    field.Value = "";
                }
            }

            //assert
            Assert.Equal(expectedformData[0].ToString(), result.ToString());
        }

        [Fact]
        public void CreateResourcesActionPlansTestsShouldReturnProperData()  //To do - CreateActionPlansAsyncEmptyData after excpetion logging
        {
            //arrange
            var actionPlan = this.actionPlanData[0];

            //act
            var dbResponseReferenceTag = topicsResourcesSettings.GetReferenceTags(referenceTagData).ReturnsForAnyArgs<dynamic>(expectedReferenceTagData);
            var dbResponseLocation = topicsResourcesSettings.GetLocations(locationData).ReturnsForAnyArgs<dynamic>(expectedReferenceLocationData);
            var dbResponseCondition = topicsResourcesSettings.GetConditions(conditionData).ReturnsForAnyArgs<dynamic>(expectedConditionData);
            var dbResponse = topicsResourcesSettings.GetReferences(actionPlan).ReturnsForAnyArgs<dynamic>(expectedActionPlanReferences);
            var response = topicsResourcesBusinessLogic.CreateResourcesActionPlans(actionPlan);
            var result = JsonConvert.SerializeObject(response);
            var actionPlanResult = (JObject)JsonConvert.DeserializeObject(result);
            result = actionPlanResult;
            foreach (JProperty field in result)
            {
                if (field.Name == "createdTimeStamp")
                {
                    field.Value = "";
                }

                else if (field.Name == "modifiedTimeStamp")
                {
                    field.Value = "";
                }
            }

            //assert
            Assert.Equal(expectedActionPlanData[0].ToString(), result.ToString());
        }

        [Fact]
        public void CreateResourcesArticlesTestsShouldReturnProperData()
        {
            //arrange
            var article = this.articleData[0];

            //act
            var dbResponseReferenceTag = topicsResourcesSettings.GetReferenceTags(referenceTagData).ReturnsForAnyArgs<dynamic>(expectedReferenceTagData);
            var dbResponseLocation = topicsResourcesSettings.GetLocations(locationData).ReturnsForAnyArgs<dynamic>(expectedReferenceLocationData);
            var dbResponse = topicsResourcesSettings.GetReferences(article).ReturnsForAnyArgs<dynamic>(expectedResourceReferences);
            var response = topicsResourcesBusinessLogic.CreateResourcesArticles(article);
            var result = JsonConvert.SerializeObject(response);
            var articleResult = (JObject)JsonConvert.DeserializeObject(result);
            result = articleResult;
            foreach (JProperty field in result)
            {
                if (field.Name == "createdTimeStamp")
                {
                    field.Value = "";
                }

                else if (field.Name == "modifiedTimeStamp")
                {
                    field.Value = "";
                }
            }

            //assert
            Assert.Equal(expectedArticleData[0].ToString(), result.ToString());
        }

        [Fact]
        public void CreateResourcesVideosTestsShouldReturnProperData()
        {
            //arrange
            var video = this.videoData[0];

            //act
            var dbResponseReferenceTag = topicsResourcesSettings.GetReferenceTags(referenceTagData).ReturnsForAnyArgs<dynamic>(expectedReferenceTagData);
            var dbResponseLocation = topicsResourcesSettings.GetLocations(locationData).ReturnsForAnyArgs<dynamic>(expectedReferenceLocationData);
            var dbResponse = topicsResourcesSettings.GetReferences(video).ReturnsForAnyArgs<dynamic>(expectedResourceReferences);
            var response = topicsResourcesBusinessLogic.CreateResourcesVideos(video);
            var result = JsonConvert.SerializeObject(response);
            var videoResult = (JObject)JsonConvert.DeserializeObject(result);
            result = videoResult;
            foreach (JProperty field in result)
            {
                if (field.Name == "createdTimeStamp")
                {
                    field.Value = "";
                }

                else if (field.Name == "modifiedTimeStamp")
                {
                    field.Value = "";
                }
            }

            //assert
            Assert.Equal(expectedVideoData[0].ToString(), result.ToString());
        }

        [Fact]
        public void CreateResourcesOrganizationsTestsShouldReturnProperData()
        {
            //arrange
            var organization = this.organizationData[0];

            //act
            var dbResponseReferenceTag = topicsResourcesSettings.GetReferenceTags(referenceTagData).ReturnsForAnyArgs<dynamic>(expectedReferenceTagData);
            var dbResponseLocation = topicsResourcesSettings.GetLocations(locationData).ReturnsForAnyArgs<dynamic>(expectedReferenceLocationData);
            var dbResponse = topicsResourcesSettings.GetReferences(organization).ReturnsForAnyArgs<dynamic>(expectedResourceReferences);
            var response = topicsResourcesBusinessLogic.CreateResourcesOrganizations(organization);
            var result = JsonConvert.SerializeObject(response);
            var organizationResult = (JObject)JsonConvert.DeserializeObject(result);
            result = organizationResult;
            foreach (JProperty field in result)
            {
                if (field.Name == "createdTimeStamp")
                {
                    field.Value = "";
                }

                else if (field.Name == "modifiedTimeStamp")
                {
                    field.Value = "";
                }
            }

            //assert
            Assert.Equal(expectedOrganizationData[0].ToString(), result.ToString());
        }

        [Fact]
        public void CreateResourcesEssentialReadingsTestsShouldReturnProperData()
        {
            //arrange
            var essentialReading = this.essentialReadingData[0];

            //act
            var dbResponseReferenceTag = topicsResourcesSettings.GetReferenceTags(referenceTagData).ReturnsForAnyArgs<dynamic>(expectedReferenceTagData);
            var dbResponseLocation = topicsResourcesSettings.GetLocations(locationData).ReturnsForAnyArgs<dynamic>(expectedReferenceLocationData);
            var dbResponse = topicsResourcesSettings.GetReferences(essentialReading).ReturnsForAnyArgs<dynamic>(expectedResourceReferences);
            var response = topicsResourcesBusinessLogic.CreateResourcesEssentialReadings(essentialReading);
            var result = JsonConvert.SerializeObject(response);
            var essentialReadingResult = (JObject)JsonConvert.DeserializeObject(result);
            result = essentialReadingResult;
            foreach (JProperty field in result)
            {
                if (field.Name == "createdTimeStamp")
                {
                    field.Value = "";
                }

                else if (field.Name == "modifiedTimeStamp")
                {
                    field.Value = "";
                }
            }

            //assert
            Assert.Equal(essentialReading.ToString(), result.ToString());
        }

        [Fact]
        public void GetParentTopicIdsTestsShouldReturnProperData()
        {
            //arrange
            var parentId = this.parentTopicIdData;

            //act
            var response = topicsResourcesBusinessLogic.GetParentTopicIds(parentId);

            //assert
            Assert.Equal(expectedParentTopicIdData, response[0].ParentTopicIds);
        }

        [Fact]
        public void GetParentTopicIdsTestsShouldReturnEmptyData()
        {
            //arrange
            var parentTopicId = this.emptyData;

            //act
            var response = topicsResourcesBusinessLogic.GetParentTopicIds(parentTopicId);

            //assert
            Assert.Equal(emptyReferenceTagData, response[0].ParentTopicIds);
        }

        [Fact]
        public void CreateTopicUploadAsyncTestsShouldReturnProperData()
        {
            //arrange
            var topic = this.topicData;
            var topics = JsonConvert.SerializeObject(topic);
            Document document = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(topic[0].ToString()));
            document.LoadFrom(reader);
            dynamic actualTopicData = null;

            //act
            var dbResponse = backendDatabaseService.CreateItemAsync<dynamic>(topic, cosmosDbSettings.TopicCollectionId).ReturnsForAnyArgs(document);
            var dbResponseResource = topicsResourcesSettings.CreateTopicDocumentAsync(topics).ReturnsForAnyArgs(topic[0]);
            string filePath = Path.Combine(Environment.CurrentDirectory, "TestData\\TopicData.json");
            var response = topicsResourcesBusinessLogic.CreateTopicsUploadAsync(filePath).Result;
            
            foreach (var result in response)
            {
                actualTopicData = result;
            }

            //assert
            Assert.Equal(expectedTopicsData[0].ToString(), actualTopicData.ToString());
        }

        [Fact]
        public void CreateTopicDocumentAsyncTestsShouldReturnProperData()
        {
            //arrange
            var topic = this.topicData;
            var resource = JsonConvert.SerializeObject(topic);
            Document document = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(topic[0].ToString()));
            document.LoadFrom(reader);
            dynamic actualTopicData = null;

            //act
            var dbResponse = backendDatabaseService.CreateItemAsync<dynamic>(topic, cosmosDbSettings.TopicCollectionId).ReturnsForAnyArgs(document);
            var dbResponseReferenceTag = topicsResourcesSettings.CreateTopics(topic[0]).ReturnsForAnyArgs<dynamic>(expectedTopicData[0]);
            var response = topicsResourcesBusinessLogic.CreateTopicDocumentAsync(resource).Result;
            foreach (var result in response)
            {
                actualTopicData = result;
            }

            //assert
            Assert.Equal(expectedTopicsData[0].ToString(), actualTopicData.ToString());
        }

        [Fact]
        public void CreateTopicsTestsShouldReturnProperData()
        {
            //arrange
            var topic = this.topicData[0];
            var referenceInput = this.referencesInputData;
            var referenceTag = this.referenceTagData;
            var location = this.locationData;
            var condition = this.conditionData;
            var parentTopic = this.parentTopicIdData;

            //act
            var dbResponseReferenceTag = topicsResourcesSettings.GetReferenceTags(referenceTag).ReturnsForAnyArgs<dynamic>(expectedReferenceTagData);
            var dbResponseLocation = topicsResourcesSettings.GetLocations(location).ReturnsForAnyArgs<dynamic>(expectedReferenceLocationData);
            var dbResponseConditions = topicsResourcesSettings.GetConditions(condition).ReturnsForAnyArgs<dynamic>(expectedConditionData);
            var dbResponseParentTopicId = topicsResourcesSettings.GetParentTopicIds(parentTopic).ReturnsForAnyArgs<dynamic>(expectedParentTopicIdData);
            var response = topicsResourcesBusinessLogic.CreateTopics(topic);
            var result = JsonConvert.SerializeObject(response);
            var topicResult = (JObject)JsonConvert.DeserializeObject(result);
            result = topicResult;
            foreach (JProperty field in result)
            {
                if (field.Name == "createdTimeStamp")
                {
                    field.Value = "";
                }

                else if (field.Name == "modifiedTimeStamp")
                {
                    field.Value = "";
                }
            }

            //assert
            Assert.Equal(expectedTopicData[0].ToString(), result.ToString());
        }

        [Fact]
        public void GetBreadcrumbItemsAsyncEmptyData()
        {
            //arrange
            var dbResponse = backendDatabaseService.ExecuteStoredProcedureAsync(cosmosDbSettings.TopicCollectionId, procedureName, topicId);
            dbResponse.ReturnsForAnyArgs<dynamic>(emptyData);

            //act
            var response = topicsResourcesBusinessLogic.GetBreadcrumbDataAsync(topicId).Result;
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains("[{}]", result, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void ApplyPaginationAsyncTestsShouldReturnProperData()
        {
            //arrange
            PagedResources pagedResources = new PagedResources { Results = resourcesData, ContinuationToken = "[]" };
            var dbResponse = dynamicQueries.FindItemsWhereArrayContainsWithAndClauseAsync("topicTags", "id", "resourceType", resourceFilter);            
            dbResponse.ReturnsForAnyArgs(pagedResources);

            //act
            var response = topicsResourcesBusinessLogic.ApplyPaginationAsync(resourceFilter);
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains(expectedResourceId, result, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void ApplyPaginationAsyncTestsShouldReturnEmptyData()
        {
            //arrange
            PagedResources pagedResources = new PagedResources { ContinuationToken = "[]" };
            var dbResponse = dynamicQueries.FindItemsWhereArrayContainsWithAndClauseAsync("topicTags", "id", "resourceType", resourceFilter);
            dbResponse.ReturnsForAnyArgs(pagedResources);

            //act
            var response = topicsResourcesBusinessLogic.ApplyPaginationAsync(resourceFilter);
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains(expectedpagedResource, result, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void GetResourcesCountAsyncTestsShouldReturnProperData()
        {
            //arrange
            PagedResources pagedResources = new PagedResources { Results = resourceCountData, ContinuationToken = "[]" };
            var dbResponse = dynamicQueries.FindItemsWhereArrayContainsWithAndClauseAsync("topicTags", "id", "resourceType", resourceFilter, true);
            dbResponse.ReturnsForAnyArgs(pagedResources);

            //act
            var response = topicsResourcesBusinessLogic.GetResourcesCountAsync(resourceFilter);
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains(expectedResourceCount, result, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void GetResourcesCountAsyncTestsShouldReturnEmptyData()
        {
            //arrange
            PagedResources pagedResources = new PagedResources { Results = new List<string>(), ContinuationToken = "[]" };
            var dbResponse = dynamicQueries.FindItemsWhereArrayContainsWithAndClauseAsync("topicTags", "id", "resourceType", resourceFilter, true);
            dbResponse.ReturnsForAnyArgs(pagedResources);

            //act
            var response = topicsResourcesBusinessLogic.GetResourcesCountAsync(resourceFilter);
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains(expectedEmptyResourceCount, result, StringComparison.InvariantCultureIgnoreCase);
        }




        [Fact]
        public void GetOrganizationsAsyncWithProperData()
        {
            //arrange
            var dbResponse = dynamicQueries.FindItemsWhereContainsWithLocationAsync(cosmosDbSettings.ResourceCollectionId, "resourceType", "Organizations", expectedLocationValue);
            dbResponse.ReturnsForAnyArgs<dynamic>(organizationData);


            //act
            var response = topicsResourcesBusinessLogic.GetOrganizationsAsync(expectedLocationValue);
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains(expectedLocationValue.State, result, StringComparison.InvariantCulture);
        }
        [Fact]
        public void GetOrganizationsAsyncEmptyData()
        {
            //arrange
            var dbResponse = dynamicQueries.FindItemsWhereContainsWithLocationAsync(cosmosDbSettings.ResourceCollectionId, "resourceType", "Organizations", expectedLocationValue);
            dbResponse.ReturnsForAnyArgs<dynamic>(emptyData);

            //act
            var response = topicsResourcesBusinessLogic.GetOrganizationsAsync(expectedLocationValue);
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains("[{}]", result, StringComparison.InvariantCultureIgnoreCase);
        }

    }
}
