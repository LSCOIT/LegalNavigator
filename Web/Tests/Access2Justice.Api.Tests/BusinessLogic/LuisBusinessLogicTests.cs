using Xunit;
using NSubstitute;
using System.Collections.Generic;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Api;
using Newtonsoft.Json.Linq;
using Access2Justice.Shared.Models;
using System;
using Access2Justice.Api.Tests.TestData;

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
        private readonly string lowScoreLuisResponse = LuisBusinessLogicTestData.lowScoreLuisResponse;
        private readonly string meduimScoreLuisResponse = LuisBusinessLogicTestData.meduimScoreLuisResponse;
        private readonly string emptyLuisResponse = LuisBusinessLogicTestData.emptyLuisResponse;
        private readonly string noneLuisResponse = LuisBusinessLogicTestData.noneLuisResponse;
        private readonly string keyword = LuisBusinessLogicTestData.keyword;
        private readonly JArray topicsData = LuisBusinessLogicTestData.topicsData;
        private readonly JObject emptyTopicObject = LuisBusinessLogicTestData.emptyTopicObject;
        private readonly JObject emptyResourceObject = LuisBusinessLogicTestData.emptyResourceObject;
        private readonly string searchText = LuisBusinessLogicTestData.searchText;
        private readonly JArray resourcesData = LuisBusinessLogicTestData.resourcesData;
        private readonly string webData = LuisBusinessLogicTestData.webData;
        private readonly Location location = new Location();
        private readonly LuisInput luisInput = new LuisInput();
        private readonly ResourceFilter resourceFilter = LuisBusinessLogicTestData.resourceFilter;
        private readonly List<dynamic> allResourcesCount = LuisBusinessLogicTestData.allResourcesCount;
        private readonly List<string> topicIds = LuisBusinessLogicTestData.topicIds;
        #endregion

        #region Mocked Output Data         
        private readonly string expectedLuisNoneIntent = LuisBusinessLogicTestData.expectedLuisNoneIntent;
        private readonly int expectedLowerthreshold = LuisBusinessLogicTestData.expectedLowerthreshold;
        private readonly string expectedLuisTopIntent = LuisBusinessLogicTestData.expectedLuisTopIntent;
        private readonly string expectedTopicId = LuisBusinessLogicTestData.expectedTopicId;
        private readonly string expectedInternalResponse = LuisBusinessLogicTestData.expectedInternalResponse;
        private readonly string expectedWebResponse = LuisBusinessLogicTestData.expectedWebResponse;
        private readonly string expectedEmptyInternalResponse = LuisBusinessLogicTestData.expectedEmptyInternalResponse;
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
            List<string> topNIntents = new List<string> { "eviction", "child abuse", "traffic ticket", "divorce" };
            IntentWithScore intentWithScore = new IntentWithScore
            {
                IsSuccessful = true,
                Score = 0.1M,
                TopScoringIntent = "eviction",
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
            List<string> topNIntents = new List<string> { "eviction", "child abuse", "traffic ticket", "divorce" };
            IntentWithScore intentWithScore = new IntentWithScore
            {
                IsSuccessful = true,
                Score = 0.01M,
                TopScoringIntent = "eviction",
                TopNIntents = topNIntents
            };
            var expectedhreshold = false;


            //act 
            var threshold = luisBusinessLogic.IsIntentAccurate(intentWithScore);

            //assert            
            Assert.Equal(expectedhreshold, threshold);
        }


        [Fact]
        public void GetInternalResourcesAsyncTestsShouldReturnProperResult()
        {
            //arrange
            PagedResources pagedResources = new PagedResources() { Results = resourcesData, ContinuationToken = "[]", TopicIds = topicIds };
            var topicResponse = topicsResourcesBusinessLogic.GetTopicsAsync(keyword, location);
            topicResponse.Returns(topicsData);
            var resourceCount = topicsResourcesBusinessLogic.GetResourcesCountAsync(resourceFilter);
            resourceCount.ReturnsForAnyArgs<dynamic>(allResourcesCount);
            var paginationResult = topicsResourcesBusinessLogic.ApplyPaginationAsync(resourceFilter);
            paginationResult.ReturnsForAnyArgs<dynamic>(pagedResources);

            //act
            var result = luisBusinessLogic.GetInternalResourcesAsync(keyword, location, Arg.Any<IEnumerable<string>>()).Result;

            //assert            
            Assert.Contains(expectedTopicId, result);
        }


        [Fact]
        public void GetInternalResourcesAsyncTestsShouldReturnEmptyResources()
        {
            //arrange
            PagedResources pagedResources = new PagedResources() { TopicIds = new List<string>() };
            var topicResponse = topicsResourcesBusinessLogic.GetTopicsAsync(keyword, location);
            topicResponse.Returns(topicsData);
            var resourceCount = topicsResourcesBusinessLogic.GetResourcesCountAsync(resourceFilter);
            resourceCount.ReturnsForAnyArgs<dynamic>(new List<dynamic>());
            var paginationResult = topicsResourcesBusinessLogic.ApplyPaginationAsync(resourceFilter);
            paginationResult.ReturnsForAnyArgs<dynamic>(pagedResources);

            //act
            var result = luisBusinessLogic.GetInternalResourcesAsync(keyword, location, Arg.Any<IEnumerable<string>>()).Result;

            //assert            
            Assert.Contains(expectedTopicId, result);
        }

        [Fact]
        public void GetInternalResourcesAsyncTestsShouldReturnTopIntent()
        {
            //arrange
            PagedResources pagedResources = new PagedResources() { Results = resourcesData, ContinuationToken = "[]", TopicIds = topicIds };
            var topicResponse = topicsResourcesBusinessLogic.GetTopicsAsync(keyword, location);
            topicResponse.Returns(topicsData);
            var resourceCount = topicsResourcesBusinessLogic.GetResourcesCountAsync(resourceFilter);
            resourceCount.ReturnsForAnyArgs<dynamic>(allResourcesCount);
            var paginationResult = topicsResourcesBusinessLogic.ApplyPaginationAsync(resourceFilter);
            paginationResult.ReturnsForAnyArgs<dynamic>(pagedResources);

            //act
            var result = luisBusinessLogic.GetInternalResourcesAsync(keyword, location, Arg.Any<IEnumerable<string>>()).Result;

            //assert            
            Assert.Contains(keyword, result);
        }


        [Fact]
        public void GetResourceBasedOnThresholdAsyncTestsShouldValidateScoreAccuracy()
        {
            //arrange
            luisInput.Sentence = searchText;
            var luisResponse = luisProxy.GetIntents(searchText);
            luisResponse.ReturnsForAnyArgs(properLuisResponse);

            var topicResponse = topicsResourcesBusinessLogic.GetTopicsAsync(keyword, location);
            topicResponse.ReturnsForAnyArgs<dynamic>(emptyTopicObject);

            var internalResponse = luis.GetInternalResourcesAsync(keyword, location, Arg.Any<IEnumerable<string>>());
            internalResponse.ReturnsForAnyArgs<dynamic>(internalResponse);

            //act
            var result = luisBusinessLogic.GetResourceBasedOnThresholdAsync(luisInput).Result;

            //assert
            Assert.Contains(expectedInternalResponse, result);

        }

        [Fact]
        public void GetResourceBasedOnThresholdAsyncTestsWithoutLuisApiCall()
        {
            //arrange
            luisInput.LuisTopScoringIntent = keyword;
            var luisResponse = luisProxy.GetIntents(searchText);
            luisResponse.ReturnsForAnyArgs(properLuisResponse);

            var topicResponse = topicsResourcesBusinessLogic.GetTopicsAsync(keyword, location);
            topicResponse.ReturnsForAnyArgs<dynamic>(emptyTopicObject);

            var internalResponse = luis.GetInternalResourcesAsync(keyword, location, Arg.Any<IEnumerable<string>>());
            internalResponse.ReturnsForAnyArgs<dynamic>(internalResponse);

            //act
            var result = luisBusinessLogic.GetResourceBasedOnThresholdAsync(luisInput).Result;

            //assert
            Assert.Contains(expectedInternalResponse, result);
        }

    }
}
