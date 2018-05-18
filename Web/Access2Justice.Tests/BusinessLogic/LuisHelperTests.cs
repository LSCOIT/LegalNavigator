namespace Access2Justice.Tests.ServiceUnitTestCases
{
    using NUnit.Framework;
    using NSubstitute;    
    using System.Net.Http;
    using System.Net;
    using Access2Justice.Shared;
    using Access2Justice.Api;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;

    [TestFixture]
    public class LuisHelperTests
    {
        private IApp options;
        private IHttpClientService httpClientService;
        private LuisProxy luisProxy;
        private List<string> intents;
        private LuisInput luisInput;


        [SetUp]
        public void SetUp()
        {

            options = Substitute.For<IApp>();
            httpClientService = Substitute.For<IHttpClientService>();
            luisProxy = new LuisProxy(httpClientService, options);            

            options.LuisUrl.Returns(new System.Uri("http://www.bing.com"));
            options.TopIntentsCount.Returns("3");            
        }

        [Test]
        public void GetLuisIntentWithProperResponseFromLuis()
        {
            luisInput = new LuisInput();

            var responseq = new HttpResponseMessage();
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\r\n  \"query\": \"child abuse\",\r\n  \"topScoringIntent\": {\r\n    \"intent\": \"ChildAbuse\",\r\n    \"score\": 0.239329442\r\n  },\r\n  \"intents\": [\r\n    {\r\n      \"intent\": \"ChildAbuse\",\r\n      \"score\": 0.239329442\r\n    },\r\n    {\r\n      \"intent\": \"Above18Age\",\r\n      \"score\": 0.09217278\r\n    },\r\n    {\r\n      \"intent\": \"Age\",\r\n      \"score\": 0.06267241\r\n    },\r\n    {\r\n      \"intent\": \"Greetings\",\r\n      \"score\": 0.00997853652\r\n    },\r\n    {\r\n      \"intent\": \"None\",\r\n      \"score\": 0.00248154555\r\n    }\r\n  ],\r\n  \"entities\": []\r\n}")
            };

            var luisResponse = httpClientService.GetAsync(options.LuisUrl);
            luisResponse.Returns(httpResponseMessage);

            //Act
            IntentWithScore intentWithScore = luisProxy.GetLuisIntent(luisInput).Result;

            //Assert
            Assert.AreEqual(true, intentWithScore.IsSuccessful);
        }

        [Test]
        public void GetLuisIntentWithNullObjectResponseFromLuis()
        {
            luisInput = new LuisInput();

            var responseq = new HttpResponseMessage();
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\r\n  \"query\":null,\"intents\":[],\"entities\":[]}")
            };

            var luisResponse = httpClientService.GetAsync(options.LuisUrl);
            luisResponse.Returns(httpResponseMessage);
            //Act
            IntentWithScore intentWithScore = luisProxy.GetLuisIntent(luisInput).Result;

            //Assert
            Assert.AreEqual(true, intentWithScore.IsSuccessful);
        }

        [Test]
        public void GetLuisIntentWithEmptyResponseFromLuis()
        {
            luisInput = new LuisInput();

            var responseq = new HttpResponseMessage();
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("")
            };

            var luisResponse = httpClientService.GetAsync(options.LuisUrl);
            luisResponse.Returns(httpResponseMessage);
            //Act
            IntentWithScore intentWithScore = luisProxy.GetLuisIntent(luisInput).Result;

            //Assert
            Assert.AreEqual(true, intentWithScore.IsSuccessful);
        }

        [Test]
        public void FilterLuisIntentWithNoneIntent()
        {
            intents = new List<string>
            {
                "child abuse",
                "eviction"
            };

            IntentWithScore intentWithScore = new IntentWithScore { TopScoringIntent= "None",Score= 93, TopNIntents = intents };
            //Act
            string[] result = luisProxy.FilterLuisIntents(intentWithScore);
            //Assert
            Assert.AreEqual(null, result);
        }

        [Test]
        public void FilterLuisIntentWithProperIntent()
        {
            intents = new List<string>
            {
                "child abuse",
                "eviction"
            };

            IntentWithScore intentWithScore = new IntentWithScore { TopScoringIntent = "child custody", Score = 93, TopNIntents = intents };
            //Act
            string[] result = luisProxy.FilterLuisIntents(intentWithScore);
            //Assert            
            Assert.IsNotEmpty(result);
        }
    }
}
