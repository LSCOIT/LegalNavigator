using Access2Justice.Api;
using Access2Justice.Api.Tests.TestData;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace Access2Justice.Tests.ServiceUnitTestCases
{

    public class LuisBusinessLogicTests
    {
        #region variables
        private readonly ILuisProxy luisProxy;
        private readonly ILuisSettings luisSettings;
        private readonly ITopicsResourcesBusinessLogic topicsResourcesBusinessLogic;
        private readonly IWebSearchBusinessLogic webSearchBusinessLogic;
        private readonly ILuisBusinessLogic luis;
        private readonly IBingSettings bingSettings;
        private readonly LuisBusinessLogic luisBusinessLogic;
        #endregion

        #region Mocked Input Data
        private readonly string properLuisResponse = LuisBusinessLogicTestData.properLuisResponse;
        private readonly string EmptyLuisResponse = LuisBusinessLogicTestData.EmptyLuisResponse;
        private readonly string emptyLuisResponse = LuisBusinessLogicTestData.emptyLuisResponse;
        private readonly string noneLuisResponse = LuisBusinessLogicTestData.noneLuisResponse;
        private readonly string keyword = LuisBusinessLogicTestData.keyword;
        private readonly JArray topicsData = LuisBusinessLogicTestData.topicsData;
        private readonly JObject emptyTopicObject = LuisBusinessLogicTestData.emptyTopicObject;
        private readonly string searchText = LuisBusinessLogicTestData.searchText;
        private readonly JArray resourcesData = LuisBusinessLogicTestData.resourcesData;
        private readonly JArray guidedAssistantResourcesData = LuisBusinessLogicTestData.guidedAssistantResourcesData;
        private readonly string webData = LuisBusinessLogicTestData.webData;
        private readonly Location location = new Location();
        private readonly LuisInput luisInput = new LuisInput();
        private readonly ResourceFilter resourceFilter = LuisBusinessLogicTestData.resourceFilter;
        private readonly List<dynamic> allResourcesCount = LuisBusinessLogicTestData.allResourcesCount;
        private readonly List<string> topicIds = LuisBusinessLogicTestData.topicIds;
        private readonly Location locationInput = LuisBusinessLogicTestData.location;
        #endregion

        #region Mocked Output Data         
        private readonly string expectedLuisNoneIntent = LuisBusinessLogicTestData.expectedLuisNoneIntent;
        private readonly string expectedLuisTopIntent = LuisBusinessLogicTestData.expectedLuisTopIntent;
        private readonly string expectedTopicId = LuisBusinessLogicTestData.expectedTopicId;
        private readonly string expectedInternalResponse = LuisBusinessLogicTestData.expectedInternalResponse;
        private readonly string expectedGuidedAssistant = LuisBusinessLogicTestData.expectedGuidedResponse;
        private readonly string expectedEmptyGuidedAssistant = LuisBusinessLogicTestData.expectedEmptyGuidedResponse;
        private readonly string expectedWebResponse = LuisBusinessLogicTestData.expectedWebResponse;
        #endregion


        public LuisBusinessLogicTests()
        {
            luisProxy = Substitute.For<ILuisProxy>();
            luisSettings = Substitute.For<ILuisSettings>();
            topicsResourcesBusinessLogic = Substitute.For<ITopicsResourcesBusinessLogic>();
            webSearchBusinessLogic = Substitute.For<IWebSearchBusinessLogic>();
            luis = Substitute.For<ILuisBusinessLogic>();
            bingSettings = Substitute.For<IBingSettings>();
            luisBusinessLogic = new LuisBusinessLogic(luisProxy, luisSettings, topicsResourcesBusinessLogic, webSearchBusinessLogic, bingSettings);

            luisSettings.Endpoint.Returns(new Uri("http://www.bing.com"));
            luisSettings.TopIntentsCount.Returns(3);
            luisSettings.IntentAccuracyThreshold.Returns(0.1M);
            bingSettings.BingSearchUrl.Returns(new Uri("http://www.bing.com?{0}{1}{2}"));
            bingSettings.CustomConfigId.Returns("0");
            bingSettings.PageResultsCount.Returns((short)10);
            bingSettings.PageOffsetValue.Returns((short)1);
        }

        [Fact]
        public void ParseLuisIntentTestsShouldReturnProperIntent()
        {

            // act
            IntentWithScore intentWithScore = luisBusinessLogic.ParseLuisIntent(properLuisResponse);

            //assert            
            Assert.Equal(expectedLuisTopIntent, intentWithScore.TopScoringIntent);
        }

        [Fact]
        public void ParseLuisIntentTestsShouldReturnEmptyObject()
        {
            // act
            IntentWithScore intentWithScore = luisBusinessLogic.ParseLuisIntent(emptyLuisResponse);

            //assert            
            Assert.Null(intentWithScore.TopScoringIntent);
        }

        [Fact]
        public void ParseLuisIntentTestsShouldReturnNoneIntent()
        {
            // act
            IntentWithScore intentWithScore = luisBusinessLogic.ParseLuisIntent(noneLuisResponse);

            //assert            
            Assert.Equal(expectedLuisNoneIntent, intentWithScore.TopScoringIntent);
        }

        [Fact]
        public void ApplyThresholdTestsShouldMatchUpperthreshold()
        {
            // arrange
            List<string> topNIntents = new List<string> { "Eviction", "child abuse", "traffic ticket", "divorce" };
            IntentWithScore intentWithScore = new IntentWithScore
            {
                IsSuccessful = true,
                Score = 0.1M,
                TopScoringIntent = "Eviction",
                TopNIntents = topNIntents
            };
            var expectedhreshold = true;

            //act 
            var threshold = luisBusinessLogic.IsIntentAccurate(intentWithScore);

            //assert
            Assert.Equal(expectedhreshold, threshold);
        }

        [Fact]
        public void ApplyThresholdTestsShouldMatchMediumthreshold()
        {
            // arrange
            List<string> topNIntents = new List<string> { "Eviction", "child abuse", "traffic ticket", "divorce" };
            IntentWithScore intentWithScore = new IntentWithScore
            {
                IsSuccessful = true,
                Score = 0.01M,
                TopScoringIntent = "Eviction",
                TopNIntents = topNIntents
            };
            var expectedhreshold = false;


            //act 
            var threshold = luisBusinessLogic.IsIntentAccurate(intentWithScore);

            //assert            
            Assert.Equal(expectedhreshold, threshold);
        }

        [Fact]
        public void GetInternalResourcesAsyncTestsShouldReturnTopIntent()
        {
            //arrange
            PagedResources pagedResources = new PagedResources() { Results = resourcesData, ContinuationToken = "[]", TopicIds = topicIds };
            var topicResponse = topicsResourcesBusinessLogic.GetTopicsAsync(keyword, location);
            topicResponse.ReturnsForAnyArgs(topicsData);
            var resourceCount = topicsResourcesBusinessLogic.GetResourcesCountAsync(resourceFilter);
            resourceCount.ReturnsForAnyArgs<dynamic>(allResourcesCount);
            var paginationResult = topicsResourcesBusinessLogic.ApplyPaginationAsync(resourceFilter);
            paginationResult.ReturnsForAnyArgs<dynamic>(pagedResources);

            //act
            var result = luisBusinessLogic.GetInternalResourcesAsync(keyword, luisInput, Arg.Any<IEnumerable<string>>()).Result;
            result = JsonConvert.SerializeObject(result);

            //assert            
            Assert.Contains(keyword, result);
        }

        [Fact]
        public void ParseLuisIntentTestsShouldValidateData()
        {
            //act
            var result = luisBusinessLogic.ParseLuisIntent(properLuisResponse);

            //assert
            Assert.Equal(expectedLuisTopIntent, result.TopScoringIntent);
        }

        [Fact]
        public void ParseLuisIntentTestsShouldValidateEmptyObject()
        {
            //act
            var result = luisBusinessLogic.ParseLuisIntent("");

            //assert
            Assert.Null(result.TopScoringIntent);
        }

        [Fact]
        public void ParseLuisIntentTestsShouldValidateEmptyLuisResponse()
        {
            //act
            var result = luisBusinessLogic.ParseLuisIntent(EmptyLuisResponse);

            //assert
            Assert.Null(result.TopScoringIntent);
        }
        

       //[Fact]
       // public void GetResourceBasedOnThresholdAsyncTestsShouldValidateLuisTopScoringIntentAndFetchResources()
       // {
       //     //arrange
       //     luisInput.Sentence = searchText;
       //     luisInput.Location = locationInput;
       //     luisInput.LuisTopScoringIntent = "Eviction";

       //     //arrange
       //     PagedResources pagedResources = new PagedResources() { Results = resourcesData, ContinuationToken = "[]", TopicIds = topicIds };
       //     var topicResponse = topicsResourcesBusinessLogic.GetTopicsAsync(keyword, locationInput);
       //     topicResponse.Returns(topicsData);
       //     var resourceCount = topicsResourcesBusinessLogic.GetResourcesCountAsync(resourceFilter);
       //     resourceCount.ReturnsForAnyArgs<dynamic>(allResourcesCount);
       //     var paginationResult = topicsResourcesBusinessLogic.ApplyPaginationAsync(resourceFilter);
       //     paginationResult.ReturnsForAnyArgs<dynamic>(pagedResources);

       //     var internalResponse = luis.GetInternalResourcesAsync(keyword, luisInput, Arg.Any<IEnumerable<string>>());
       //     internalResponse.ReturnsForAnyArgs<dynamic>(internalResponse);

       //     //act
       //     var result = luisBusinessLogic.GetResourceBasedOnThresholdAsync(luisInput).Result;

       //     //assert
       //     Assert.Contains(expectedInternalResponse, result.ToString());
       // }

        //[Fact]
        //public void GetResourceBasedOnThresholdAsyncTestsShouldValidateInternalResources()
        //{
        //    //arrange
        //    luisInput.Sentence = searchText;
        //    luisInput.Location = locationInput;
        //    var luisResponse = luisProxy.GetIntents(searchText);
        //    luisResponse.ReturnsForAnyArgs(properLuisResponse);

        //    //arrange
        //    PagedResources pagedResources = new PagedResources() { Results = resourcesData, ContinuationToken = "[]", TopicIds = topicIds };
        //    var topicResponse = topicsResourcesBusinessLogic.GetTopicsAsync(keyword, locationInput);
        //    topicResponse.Returns(topicsData);
        //    var resourceCount = topicsResourcesBusinessLogic.GetResourcesCountAsync(resourceFilter);
        //    resourceCount.ReturnsForAnyArgs<dynamic>(allResourcesCount);
        //    var paginationResult = topicsResourcesBusinessLogic.ApplyPaginationAsync(resourceFilter);
        //    paginationResult.ReturnsForAnyArgs<dynamic>(pagedResources);

        //    var internalResponse = luis.GetInternalResourcesAsync(keyword, luisInput, Arg.Any<IEnumerable<string>>());
        //    internalResponse.ReturnsForAnyArgs<dynamic>(internalResponse);

        //    //act
        //    var result = luisBusinessLogic.GetResourceBasedOnThresholdAsync(luisInput).Result;

        //    //assert
        //    Assert.Contains(expectedInternalResponse, result.ToString());

        //}

        [Fact]
        public void GetResourceBasedOnThresholdAsyncTestsShouldValidateEmptyResources()
        {
            //arrange
            luisInput.LuisTopScoringIntent = keyword;
            var luisResponse = luisProxy.GetIntents(searchText);
            luisResponse.ReturnsForAnyArgs(properLuisResponse);

            var topicResponse = topicsResourcesBusinessLogic.GetTopicsAsync(keyword, location);
            topicResponse.ReturnsForAnyArgs<dynamic>(emptyTopicObject);

            var internalResponse = luis.GetInternalResourcesAsync(keyword, luisInput, Arg.Any<IEnumerable<string>>());
            internalResponse.ReturnsForAnyArgs<dynamic>(internalResponse);

            var webResponse = webSearchBusinessLogic.SearchWebResourcesAsync(new Uri("http://www.bing.com"));
            webResponse.ReturnsForAnyArgs<dynamic>(webData);

            //act
            var result = luisBusinessLogic.GetResourceBasedOnThresholdAsync(luisInput).Result;

            //assert
            Assert.Contains(expectedInternalResponse, result);
        }

        //[Fact]
        //public void GetResourceBasedOnThresholdAsyncTestsShouldValidateWebResources()
        //{
        //    //arrange
        //    var luisResponse = luisProxy.GetIntents(searchText);
        //    luisResponse.ReturnsForAnyArgs(properLuisResponse);

        //    var topicResponse = topicsResourcesBusinessLogic.GetTopicsAsync(keyword, location);
        //    topicResponse.ReturnsForAnyArgs<dynamic>(emptyTopicObject);

        //    var internalResponse = luis.GetInternalResourcesAsync(keyword, luisInput, Arg.Any<IEnumerable<string>>());
        //    internalResponse.ReturnsForAnyArgs<dynamic>(internalResponse);

        //    var webResponse = webSearchBusinessLogic.SearchWebResourcesAsync(new Uri("http://www.bing.com"));
        //    webResponse.ReturnsForAnyArgs<dynamic>(webData);

        //    //act
        //    var result = luisBusinessLogic.GetResourceBasedOnThresholdAsync(luisInput).Result;

        //    //assert
        //    Assert.Contains(expectedWebResponse, result);
        //}

        [Fact]
        public void GetWebResourcesAsyncTestsShouldValidateWebResources()
        {
            //arrange
            string searchText = "Eviction";

            var webResponse = webSearchBusinessLogic.SearchWebResourcesAsync(new Uri("http://www.bing.com"));
            webResponse.ReturnsForAnyArgs<dynamic>(webData);

            //act
            var result = luisBusinessLogic.GetWebResourcesAsync(searchText).Result;

            //assert
            Assert.Contains(expectedWebResponse, result);
        }
    }
}
