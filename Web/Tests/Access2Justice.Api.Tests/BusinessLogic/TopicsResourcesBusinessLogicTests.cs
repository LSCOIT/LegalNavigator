using Access2Justice.Api.BusinessLogic;
using Access2Justice.Api.Tests.TestData;
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
using System.Threading.Tasks;
using Xunit;

namespace Access2Justice.Api.Tests.BusinessLogic
{
    public class TopicsResourcesBusinessLogicTests
    {
        private readonly IStorageSettings storageSettings;
        private readonly IDynamicQueries dynamicQueries;
        private readonly ICosmosDbSettings cosmosDbSettings;
        private readonly IBackendDatabaseService backendDatabaseService;
        private readonly TopicsResourcesBusinessLogic topicsResourcesBusinessLogic;
        private readonly ITopicsResourcesBusinessLogic topicsResourcesSettings;

        //Mocked input data.
        private readonly string keyword = "Eviction";
        private readonly string query = "select * from t";
        private readonly string procedureName = "GetParentTopics";
        private readonly string topicId = "addf41e9-1a27-4aeb-bcbb-7959f95094ba";
        private readonly List<string> topicIds = new List<string> { "addf41e9-1a27-4aeb-bcbb-7959f95094ba" };
        private readonly Location location = new Location();
        private readonly string topicName = "Family";
        private readonly string resourceName = "Action Plan";
        private readonly List<dynamic> emptyDynamicList = new List<dynamic>();
        private readonly JArray emptyData = JArray.Parse(@"[{}]");
        private readonly JArray emptyUpsertData = JArray.Parse(@"[]");
        private readonly JArray topicsData = TopicResourceTestData.topicsData;
        private readonly List<dynamic> resourcesData = TopicResourceTestData.resourcesData;
        private readonly JArray resourceCountData = TopicResourceTestData.resourceCountData;
        private readonly ResourceFilter resourceFilter = TopicResourceTestData.resourceFilter;
        private readonly ResourceFilter resourceFilterTrue = TopicResourceTestData.resourceFilterTrue;
        private readonly JArray formData = TopicResourceTestData.formData;
        private readonly JArray actionPlanData = TopicResourceTestData.actionPlanData;
        private readonly JArray referencesInputData = TopicResourceTestData.referencesInputData;
        private readonly JArray articleData = TopicResourceTestData.articleData;
        private readonly JArray videoData = TopicResourceTestData.videoData;
        private readonly JArray organizationData = TopicResourceTestData.organizationData;
        private readonly JArray additionalReadingData = TopicResourceTestData.additionalReadingData;
        private readonly JArray relatedLinkData = TopicResourceTestData.relatedLinkData;
        private readonly JArray topicData = TopicResourceTestData.topicData;
        private readonly JArray topicUpsertData = TopicResourceTestData.topicUpsertData;
        private readonly JArray referenceTagData = TopicResourceTestData.referenceTagData;
        private readonly JArray parentTopicIdData = TopicResourceTestData.parentTopicIdData;
        private readonly JArray locationData = TopicResourceTestData.locationData;
        private readonly JArray conditionData = TopicResourceTestData.conditionData;
        private readonly JArray emptyResourceData = TopicResourceTestData.emptyResourceData;
        private readonly JArray reviewerData = TopicResourceTestData.reviewerData;
        private readonly JArray contentData = TopicResourceTestData.contentData;

        //Mocked result data.
        private readonly string expectedEmptyArrayObject = "[]";
        private readonly string emptyTopicTagData = "";
        private readonly JArray emptyLocationData = TopicResourceTestData.emptyLocationData;
        private readonly JArray emptyConditionObject = TopicResourceTestData.emptyConditionObject;
        private readonly JArray EmptyReferences = TopicResourceTestData.EmptyReferences;
        private readonly JArray emptyReviewerData = TopicResourceTestData.emptyReviewerData;
        private readonly JArray emptyContentData = TopicResourceTestData.emptyContentData;
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
        private readonly JArray expectedTopicData = TopicResourceTestData.expectedTopicData;
        private readonly JArray expectedTopicsData = TopicResourceTestData.expectedTopicsData;
        private readonly string expectedTopicTagData = TopicResourceTestData.expectedTopicTagData;
        private readonly string expectedParentTopicIdData = TopicResourceTestData.expectedParentTopicIdData;
        private readonly JArray expectedLocationData = TopicResourceTestData.expectedLocationData;
        private readonly JArray expectedReferenceLocationData = TopicResourceTestData.expectedReferenceLocationData;
        private readonly JArray expectedConditionData = TopicResourceTestData.expectedConditionData;
        private readonly JArray expectedResourceReferences = TopicResourceTestData.expectedResourceReferences;
        private readonly JArray expectedActionPlanReferences = TopicResourceTestData.expectedActionPlanReferences;
        private readonly JArray expectedReferencesData = TopicResourceTestData.expectedReferencesData;
        private readonly Location expectedLocationValue = TopicResourceTestData.expectedLocationValue;
        private readonly JArray expectedReviewerData = TopicResourceTestData.expectedReviewerData;
        private readonly JArray expectedContentData = TopicResourceTestData.expectedContentData;
        //private readonly JArray expectedIntentInputData = TopicResourceTestData.expectedIntentInputData;

        public TopicsResourcesBusinessLogicTests()
        {
            storageSettings = Substitute.For<IStorageSettings>();
            dynamicQueries = Substitute.For<IDynamicQueries>();
            cosmosDbSettings = Substitute.For<ICosmosDbSettings>();
            backendDatabaseService = Substitute.For<IBackendDatabaseService>();
            topicsResourcesSettings = Substitute.For<ITopicsResourcesBusinessLogic>();

            storageSettings.StaticResourcesRootUrl.Returns("https://storage");
            cosmosDbSettings.AuthKey.Returns("dummykey");
            cosmosDbSettings.Endpoint.Returns(new System.Uri("https://bing.com"));
            cosmosDbSettings.DatabaseId.Returns("dbname");
            cosmosDbSettings.TopicsCollectionId.Returns("TopicCollection");
            cosmosDbSettings.ResourcesCollectionId.Returns("ResourceCollection");

            topicsResourcesBusinessLogic = new TopicsResourcesBusinessLogic(storageSettings, dynamicQueries, cosmosDbSettings, backendDatabaseService);
        }

        [Fact]
        public void GetTopicAsyncTestsShouldReturnProperData()
        {
            //arrange
            var dbResponse = dynamicQueries.FindItemsWhereContainsWithLocationAsync(cosmosDbSettings.TopicsCollectionId, "keywords", keyword, location);
            dbResponse.ReturnsForAnyArgs(topicsData);

            //act
            var response = topicsResourcesBusinessLogic.GetTopicsAsync(keyword, location).Result;
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains(expectedTopicId, result, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void GetTopicAsyncTestsShouldReturnEmptyData()
        {
            //arrange
            var dbResponse = dynamicQueries.FindItemsWhereContainsWithLocationAsync(cosmosDbSettings.TopicsCollectionId, "keywords", keyword, location);
            dbResponse.ReturnsForAnyArgs(emptyData);

            //act
            var response = topicsResourcesBusinessLogic.GetTopicsAsync(keyword, location).Result;
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains(expectedEmptyArrayObject, result, StringComparison.InvariantCultureIgnoreCase);
        }

        //[Fact]
        //public void GetTopicsAsyncTestsShouldReturnProperData()
        //{
        //    //arrange
        //    var dbResponse = dynamicQueries.FindItemsWhereWithLocationAsync(cosmosDbSettings.TopicsCollectionId, query, "", location);
        //    dbResponse.ReturnsForAnyArgs<dynamic>(topicsData);
        //    //act
        //    var response = topicsResourcesBusinessLogic.GetTopLevelTopicsAsync(expectedLocationValue).Result;
        //    string result = JsonConvert.SerializeObject(response);
        //    //assert
        //    Assert.Contains(expectedTopicId, result, StringComparison.InvariantCulture);
        //}

        //[Fact]
        //public void GetTopicsAsyncTestsShouldReturnEmptyData()
        //{
        //    //arrange
        //    var dbResponse = dynamicQueries.FindItemsWhereWithLocationAsync(cosmosDbSettings.TopicsCollectionId, query, "", location);
        //    dbResponse.ReturnsForAnyArgs<dynamic>(emptyData);

        //    //act
        //    var response = topicsResourcesBusinessLogic.GetTopLevelTopicsAsync(expectedLocationValue);
        //    string result = JsonConvert.SerializeObject(response);

        //    //assert
        //    Assert.Contains("[]", result, StringComparison.InvariantCultureIgnoreCase);
        //}

        [Fact]
        public void GetResourcesAsyncTestsShouldReturnProperData()
        {
            //arrange
            var dbResponse = dynamicQueries.FindItemsWhereArrayContainsAsync(cosmosDbSettings.ResourcesCollectionId, "topicTags", "id", new List<string>());
            dbResponse.ReturnsForAnyArgs<dynamic>(resourcesData);

            //act
            var response = topicsResourcesBusinessLogic.GetResourcesAsync(topicsData).Result;
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains(expectedResourceId, result, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void GetResourcesAsyncTestsShouldReturnEmptyData()
        {
            //arrange
            var dbResponse = dynamicQueries.FindItemsWhereArrayContainsAsync(cosmosDbSettings.ResourcesCollectionId, "topicTags", "id", topicIds);
            dbResponse.ReturnsForAnyArgs<dynamic>(emptyDynamicList);

            //act
            var response = topicsResourcesBusinessLogic.GetResourcesAsync(emptyData).Result;
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains(expectedEmptyArrayObject, result, StringComparison.InvariantCultureIgnoreCase);
        }

        //[Fact]
        //public void GetSubTopicsAsyncTestsShouldReturnProperData()
        //{
        //    //arrange
        //    var dbResponse = dynamicQueries.FindItemsWhereArrayContainsAsyncWithLocation(cosmosDbSettings.TopicsCollectionId, query, "", "", location);
        //    dbResponse.ReturnsForAnyArgs<dynamic>(resourcesData);
        //    //act
        //    var response = topicsResourcesBusinessLogic.GetSubTopicsAsync(TopicResourceTestData.TopicInput).Result;
        //    string result = JsonConvert.SerializeObject(response);
        //    //assert
        //    Assert.Contains(expectedResourceId, result, StringComparison.InvariantCulture);
        //}

        //[Fact]
        //public void GetSubTopicsAsyncTestsShouldReturnProperDataForIsShared()
        //{
        //    //arrange
        //    var dbResponse = dynamicQueries.FindItemsWhereArrayContainsAsync(cosmosDbSettings.TopicsCollectionId, query, "", "");
        //    dbResponse.ReturnsForAnyArgs<dynamic>(resourcesData);
        //    //act
        //    var response = topicsResourcesBusinessLogic.GetSubTopicsAsync(TopicResourceTestData.TopicInputIsSharedTrue).Result;
        //    string result = JsonConvert.SerializeObject(response);
        //    //assert
        //    Assert.Contains("null", result, StringComparison.InvariantCulture);
        //}

        [Fact]
        public void GetSubTopicsAsyncTestsShouldReturnEmptyData()
        {
            //arrange
            var dbResponse = dynamicQueries.FindItemsWhereArrayContainsAsyncWithLocation(cosmosDbSettings.TopicsCollectionId, query, "", "", location);
            dbResponse.ReturnsForAnyArgs<dynamic>(emptyDynamicList);

            //act
            var response = topicsResourcesBusinessLogic.GetSubTopicsAsync(TopicResourceTestData.TopicInput);
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains("[]", result, StringComparison.InvariantCultureIgnoreCase);
        }

        //[Fact]
        //public void GetSubTopicDetailsAsyncTestsShouldReturnProperData()
        //{
        //    //arrange
        //    var dbResponse = dynamicQueries.FindItemsWhereArrayContainsAsyncWithLocation(cosmosDbSettings.ResourcesCollectionId, "topicTags", "id", topicId, location);
        //    dbResponse.ReturnsForAnyArgs<dynamic>(topicsData);
        //    //act
        //    var response = topicsResourcesBusinessLogic.GetResourceAsync(TopicResourceTestData.TopicInput).Result;
        //    string result = JsonConvert.SerializeObject(response);
        //    //assert
        //    Assert.Contains(topicId, result, StringComparison.InvariantCulture);
        //}

        [Fact]
        public void GetSubTopicDetailsAsyncTestsShouldReturnEmptyData()
        {
            //arrange
            var dbResponse = dynamicQueries.FindItemsWhereArrayContainsAsyncWithLocation(cosmosDbSettings.ResourcesCollectionId, "topicTags", "id", "", location);
            dbResponse.ReturnsForAnyArgs<dynamic>(emptyDynamicList);

            //act
            var response = topicsResourcesBusinessLogic.GetResourceAsync(TopicResourceTestData.TopicInput);
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains("[]", result, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void GetTopicDetailsAsyncTestsShouldReturnProperData()
        {
            //arrange
            var dbResponse = dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.TopicsCollectionId, "name", topicName);
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
            var dbResponse = dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.TopicsCollectionId, "name", "");
            dbResponse.ReturnsForAnyArgs<dynamic>(emptyData);

            //act
            var response = topicsResourcesBusinessLogic.GetTopicDetailsAsync(topicId);
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains("[]", result, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void GetResourceDetailAsyncTestsShouldReturnProperData()
        {
            //arrange
            var resourceType = "Action Plans";
            List<string> propertyNames = new List<string>() { "name", "resourceType" };
            List<string> values = new List<string>() { resourceName, resourceType };
            var dbResponse = dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.ResourcesCollectionId, propertyNames, values);
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
            var dbResponse = dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.ResourcesCollectionId, propertyNames, values);
            dbResponse.ReturnsForAnyArgs<dynamic>(emptyData);

            //act
            var response = topicsResourcesBusinessLogic.GetResourceDetailAsync(resourceName, resourceType);
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains("[]", result, StringComparison.InvariantCultureIgnoreCase);
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
            var reviewer = this.reviewerData;
            var content = this.contentData;

            //act
            var dbResponseTopicTag = topicsResourcesSettings.GetTopicTags(referenceTag).ReturnsForAnyArgs<dynamic>(expectedTopicTagData);
            var dbResponseLocation = topicsResourcesSettings.GetLocations(location).ReturnsForAnyArgs<dynamic>(expectedReferenceLocationData);
            var dbResponseConditions = topicsResourcesSettings.GetConditions(condition).ReturnsForAnyArgs<dynamic>(expectedConditionData);
            var dbResponseParentTopicId = topicsResourcesSettings.GetParentTopicIds(parentTopic).ReturnsForAnyArgs<dynamic>(expectedParentTopicIdData);
            var dbResponseReviwer = topicsResourcesSettings.GetReviewer(reviewer).ReturnsForAnyArgs<dynamic>(expectedReviewerData);
            var dbResponseContent = topicsResourcesSettings.GetContents(content).ReturnsForAnyArgs<dynamic>(expectedContentData);
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
            var reviewer = this.emptyData;
            var content = this.emptyData;

            //act
            var dbResponseTopicTag = topicsResourcesSettings.GetTopicTags(referenceTag).ReturnsForAnyArgs<dynamic>(emptyTopicTagData);
            var dbResponseLocation = topicsResourcesSettings.GetLocations(location).ReturnsForAnyArgs<dynamic>(emptyLocationData);
            var dbResponseCondition = topicsResourcesSettings.GetConditions(conditon).ReturnsForAnyArgs<dynamic>(emptyData);
            var dbResponseParentTopic = topicsResourcesSettings.GetParentTopicIds(parentTopic).ReturnsForAnyArgs<dynamic>(emptyData);
            var dbResponseReviewer = topicsResourcesSettings.GetReviewer(reviewer).ReturnsForAnyArgs<dynamic>(emptyData);
            var dbResponseContent = topicsResourcesSettings.GetContents(content).ReturnsForAnyArgs<dynamic>(emptyData);
            var response = topicsResourcesBusinessLogic.GetReferences(emptyResource[0]);
            var ActualReferenceData = JsonConvert.SerializeObject(response);
            var expectedReferencesData = JsonConvert.SerializeObject(EmptyReferences);

            //assert
            Assert.Equal(expectedReferencesData, ActualReferenceData);
        }

        [Fact]
        public void GetTopicTagsTestsShouldReturnProperData()
        {
            //arrange
            var referenceTag = this.referenceTagData;

            //act
            var response = topicsResourcesBusinessLogic.GetTopicTags(referenceTag);

            //assert
            Assert.Equal(expectedTopicTagData, response[0].TopicTags);
        }

        [Fact]
        public void GetTopicTagsTestsShouldReturnEmptyData()
        {
            //arrange
            var referenceTag = this.emptyData;

            //act
            var response = topicsResourcesBusinessLogic.GetTopicTags(referenceTag);

            //assert
            Assert.Equal(emptyTopicTagData, response[0].TopicTags);
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
        public void GetReviewerTestsShouldReturnProperData()
        {
            //arrange
            var reviewer = this.reviewerData;

            //act
            var response = topicsResourcesBusinessLogic.GetReviewer(reviewer);
            var actualReviewer = JsonConvert.SerializeObject(response);
            var expectedReviewer = JsonConvert.SerializeObject(expectedReviewerData);

            //assert
            Assert.Equal(expectedReviewer, actualReviewer);
        }

        [Fact]
        public void GetReviewerTestsShouldReturnEmptyData()
        {
            //arrange
            var reviewer = this.emptyData;

            //act
            var response = topicsResourcesBusinessLogic.GetReviewer(reviewer);
            var actualReviewer = JsonConvert.SerializeObject(response);
            var expectedReviewer = JsonConvert.SerializeObject(emptyReviewerData);

            //assert
            Assert.Equal(expectedReviewer, actualReviewer);
        }

        [Fact]
        public void GetContentsTestsShouldReturnProperData()
        {
            //arrange
            var content = this.contentData;

            //act
            var response = topicsResourcesBusinessLogic.GetContents(content);
            var actualContent = JsonConvert.SerializeObject(response);
            var expectedContent = JsonConvert.SerializeObject(expectedContentData);

            //assert
            Assert.Equal(expectedContent, actualContent);
        }

        [Fact]
        public void GetContentsTestsShouldReturnEmptyData()
        {
            //arrange
            var content = this.emptyData;

            //act
            var response = topicsResourcesBusinessLogic.GetContents(content);
            var actualContent = JsonConvert.SerializeObject(response);
            var expectedContent = JsonConvert.SerializeObject(emptyContentData);

            //assert
            Assert.Equal(expectedContent, actualContent);
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
        public void GetParentTopicIdsTestsShouldReturnEmptyData()
        {
            //arrange
            var parentTopicId = this.emptyData;

            //act
            var response = topicsResourcesBusinessLogic.GetParentTopicIds(parentTopicId);

            //assert
            Assert.Equal(emptyTopicTagData, response[0].ParentTopicIds);
        }

        [Fact]
        public void UpsertTopicUploadAsyncTestsShouldReturnProperData()
        {
            //arrange
            var topic = this.topicData;
            string id = "f47a01e9-c5dc-48f1-993f-6a69324317e6";
            var topics = JsonConvert.SerializeObject(topic);
            Document document = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(topic[0].ToString()));
            document.LoadFrom(reader);
            dynamic actualTopicData = null;
            object topicObjects = null;
            string filePath = Path.Combine(Environment.CurrentDirectory, "TestData\\TopicData.json");
            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                topicObjects = JsonConvert.DeserializeObject<List<dynamic>>(json);
            }

            //act            
            var dbResponseFind = dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.TopicsCollectionId, Constants.Id, id).ReturnsForAnyArgs(topicObjects);
            var dbResponse = backendDatabaseService.CreateItemAsync<dynamic>(topic, cosmosDbSettings.TopicsCollectionId).ReturnsForAnyArgs(document);
            var dbResponseUpdate = backendDatabaseService.UpdateItemAsync<dynamic>(id, topic, cosmosDbSettings.TopicsCollectionId).ReturnsForAnyArgs(document);
            var dbResponseResource = topicsResourcesSettings.UpsertTopicDocumentAsync(topics).ReturnsForAnyArgs(topic[0]);
            var response = topicsResourcesBusinessLogic.UpsertTopicsUploadAsync(filePath).Result;

            foreach (var result in response)
            {
                actualTopicData = result;
            }

            //assert
            Assert.Equal(expectedTopicsData[0].ToString(), actualTopicData.ToString());
        }

        [Fact]
        public void UpsertTopicsTestsShouldReturnProperData()
        {
            //arrange
            var topic = this.topicData[0];
            var referenceInput = this.referencesInputData;
            var referenceTag = this.referenceTagData;
            var location = this.locationData;
            var condition = this.conditionData;
            var parentTopic = this.parentTopicIdData;

            //act
            var dbResponseTopicTag = topicsResourcesSettings.GetTopicTags(referenceTag).ReturnsForAnyArgs<dynamic>(expectedTopicTagData);
            var dbResponseLocation = topicsResourcesSettings.GetLocations(location).ReturnsForAnyArgs<dynamic>(expectedReferenceLocationData);
            var dbResponseConditions = topicsResourcesSettings.GetConditions(condition).ReturnsForAnyArgs<dynamic>(expectedConditionData);
            var dbResponseParentTopicId = topicsResourcesSettings.GetParentTopicIds(parentTopic).ReturnsForAnyArgs<dynamic>(expectedParentTopicIdData);
            var response = topicsResourcesBusinessLogic.UpsertTopics(topic);
            var id = response.Id.ToString();
//            var result = JsonConvert.SerializeObject(response);
//            var topicResult = (JObject)JsonConvert.DeserializeObject(result);
//            result = topicResult;
//            foreach (JProperty field in result)
//            {
//                if (field.Name == "createdTimeStamp")
//                {
//                    field.Value = "";
//                }
//
//                else if (field.Name == "modifiedTimeStamp")
//                {
//                    field.Value = "";
//                }
//            }

            //assert
            Assert.Contains(id, expectedTopicData[0].ToString(), StringComparison.InvariantCulture);
        }

        [Fact]
        public void GetBreadcrumbItemsAsyncEmptyData()
        {
            //arrange
            var dbFindResponse = dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.TopicsCollectionId, Constants.Id, topicId);
            dbFindResponse.ReturnsForAnyArgs<dynamic>(new List<object>(topicData));
            var dbResponse = backendDatabaseService.ExecuteStoredProcedureAsync(cosmosDbSettings.TopicsCollectionId, procedureName, topicId);
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
            var dbResponse = dynamicQueries.FindItemsWhereContainsWithLocationAsync(cosmosDbSettings.ResourcesCollectionId, "resourceType", "Organizations", expectedLocationValue);
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
            var dbResponse = dynamicQueries.FindItemsWhereContainsWithLocationAsync(cosmosDbSettings.ResourcesCollectionId, "resourceType", "Organizations", expectedLocationValue);
            dbResponse.ReturnsForAnyArgs<dynamic>(emptyData);

            //act
            var response = topicsResourcesBusinessLogic.GetOrganizationsAsync(expectedLocationValue);
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains("[{}]", result, StringComparison.InvariantCultureIgnoreCase);
        }

        [Theory]
        [MemberData(nameof(TopicResourceTestData.LocationData), MemberType = typeof(TopicResourceTestData))]
        public void GetLocationsValidate(dynamic locationValues)
        {
            //arrange
            dynamic expectedData = locationValues;

            //act
            var dbResponse = topicsResourcesBusinessLogic.GetLocations(locationValues);
            var actualResult = JsonConvert.SerializeObject(dbResponse);
            var expectedResult = JsonConvert.SerializeObject(expectedData);

            //assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [MemberData(nameof(TopicResourceTestData.ReferencesData), MemberType = typeof(TopicResourceTestData))]
        public void GetReferencesValidate(dynamic resourceObjects, dynamic expectedObjects)
        {
            //arrange
            dynamic dbResponse = string.Empty;
            dynamic actualResult = string.Empty;
            var resource = resourceObjects[0];
            dbResponse = topicsResourcesBusinessLogic.GetReferences(resource);

            //act
            actualResult = JsonConvert.SerializeObject(dbResponse);
            var expectedResult = JsonConvert.SerializeObject(expectedObjects);

            //assert
            //Assert.NotNull(actualResult);
            Assert.Contains(expectedResult, actualResult);

        }
        [Theory]
        [MemberData(nameof(TopicResourceTestData.TopicTagsData), MemberType = typeof(TopicResourceTestData))]
        public void GetTopicTagsValidate(dynamic tagValues)
        {
            //arrange
            dynamic expectedData = tagValues[0].topicTags;

            //act
            var dbResponse = topicsResourcesBusinessLogic.GetTopicTags(expectedData);
            var actualResult = JsonConvert.SerializeObject(dbResponse);
            var expectedResult = JsonConvert.SerializeObject(expectedData);

            //assert
            Assert.Equal(expectedResult, actualResult);
        }

       
        [Theory]
        [MemberData(nameof(TopicResourceTestData.ResourceFilter), MemberType = typeof(TopicResourceTestData))]
        public void GetPersonalizedResourcesAsyncValidate(ResourceFilter resourceFilter, dynamic expectedData)
        {
            //arrange            
            var dbResponseTopics = dynamicQueries.FindItemsWhereInClauseAsync(cosmosDbSettings.TopicsCollectionId, "id", resourceFilter.TopicIds);

            //act
            var dbResponse = topicsResourcesBusinessLogic.GetPersonalizedResourcesAsync(resourceFilter);
            var actualResult = JsonConvert.SerializeObject(dbResponse.Result);
            var expectedResult = JsonConvert.SerializeObject(expectedData);

            //assert
            Assert.Contains("topics", actualResult, StringComparison.InvariantCultureIgnoreCase);

        }

        [Fact]
        public void GetTopicDetailsAsyncWithProperData()
        {
            //arrange
            var dbResponse = dynamicQueries.FindItemsWhereInClauseAsync(cosmosDbSettings.TopicsCollectionId, Constants.Name, TopicResourceTestData.IntentInputData.Intents, TopicResourceTestData.IntentInputData.Location);
            dbResponse.ReturnsForAnyArgs<dynamic>(topicData);

            //act
            var response = topicsResourcesBusinessLogic.GetTopicDetailsAsync(TopicResourceTestData.IntentInputData);
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains(expectedLocationValue.State, result, StringComparison.InvariantCulture);
        }
        [Fact]
        public void GetTopicDetailsAsyncEmptyData()
        {
            //arrange
            var dbResponse = dynamicQueries.FindItemsWhereInClauseAsync(cosmosDbSettings.TopicsCollectionId, Constants.Name, TopicResourceTestData.IntentInputData.Intents, TopicResourceTestData.IntentInputData.Location);
            dbResponse.ReturnsForAnyArgs<dynamic>(emptyData);

            //act
            var response = topicsResourcesBusinessLogic.GetTopicDetailsAsync(TopicResourceTestData.IntentInputData);
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains("[{}]", result, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void GetAbsoluteStaticResourceStoragePath()
        {
            //arrange
            var relativePath = "/static-resource/assets/images/categories/individual_rights.svg";

            //act
            var absolutePath = topicsResourcesBusinessLogic.GetAbsoluteStaticResourceStoragePath(relativePath);

            //assert
            Assert.Equal(storageSettings.StaticResourcesRootUrl + relativePath, absolutePath);
        }

        [Fact]
        public void GetAbsoluteStaticResourceStoragePathShouldReturnSamePath()
        {
            //arrange
            var relativePath = "/static-resource/assets/images/categories/individual_rights.svg";

            //act
            var absolutePath = topicsResourcesBusinessLogic.GetAbsoluteStaticResourceStoragePath(relativePath);

            //assert
            Assert.Equal(storageSettings.StaticResourcesRootUrl + relativePath, absolutePath);
        }

        [Fact]
        public void GetRelativeStaticResourceStoragePath()
        {
            //arrange
            var absolutePath = "https://example.windows.net/static-resource/assets/images/categories/individual_rights.svg";

            //act
            var relativePath = topicsResourcesBusinessLogic.GetRelativeStaticResourceStoragePath(absolutePath);

            //assert
            Assert.Equal(absolutePath, relativePath);
        }

        [Fact]
        public void GetRelativeStaticResourceStoragePathShouldReturnSamePath()
        {
            //arrange
            var originalPath = "https://example.com/static-resource/assets/images/categories/individual_rights.svg";

            //act
            var relativePath = topicsResourcesBusinessLogic.GetRelativeStaticResourceStoragePath(originalPath);

            //assert
            Assert.Equal(originalPath, relativePath);
        }
    }
}
