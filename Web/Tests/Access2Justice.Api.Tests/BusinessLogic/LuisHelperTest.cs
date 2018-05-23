//using Access2Justice.Api;
//using Access2Justice.Shared;
//using Access2Justice.Shared.Luis;
//using NSubstitute;
//using NUnit.Framework;
//using System.Collections.Generic;
//using System.Net;
//using System.Net.Http;

//namespace Access2Justice.Tests.ServiceUnitTestCases
//{
//    [TestFixture]
//    public class LuisHelperTests
//    {
//        private ILuisSettings options;
//        private IHttpClientService httpClientService;
//        private LuisProxy luisProxy;
//        private List<string> intents;


//        [SetUp]
//        public void SetUp()
//        {

//            options = Substitute.For<ILuisSettings>();
//            httpClientService = Substitute.For<IHttpClientService>();
//            luisProxy = new LuisProxy(httpClientService, options);

//            options.Endpoint.Returns(new System.Uri("http://www.bing.com"));
//            options.TopIntentsCount.Returns("3");
//        }

//        [Test]
//        public void GetLuisIntentWithProperResponseFromLuis()
//        {
//            var query = "I need help to file for a child abuse";

//            var responseq = new HttpResponseMessage();
//            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
//            {
//                Content = new StringContent("{\r\n  \"query\": \"child abuse\",\r\n  \"topScoringIntent\": {\r\n    \"intent\": \"ChildAbuse\",\r\n    \"score\": 0.239329442\r\n  },\r\n  \"intents\": [\r\n    {\r\n      \"intent\": \"ChildAbuse\",\r\n      \"score\": 0.239329442\r\n    },\r\n    {\r\n      \"intent\": \"Above18Age\",\r\n      \"score\": 0.09217278\r\n    },\r\n    {\r\n      \"intent\": \"Age\",\r\n      \"score\": 0.06267241\r\n    },\r\n    {\r\n      \"intent\": \"Greetings\",\r\n      \"score\": 0.00997853652\r\n    },\r\n    {\r\n      \"intent\": \"None\",\r\n      \"score\": 0.00248154555\r\n    }\r\n  ],\r\n  \"entities\": []\r\n}")
//            };

//            var luisResponse = httpClientService.GetAsync(options.Endpoint);
//            luisResponse.Returns(httpResponseMessage);

//            //Act
//            string intentWithScore = luisProxy.GetIntents(query).Result;

//            //TO DO : Assert
//            Assert.AreEqual(true, intentWithScore);
//        }

//        [Test]
//        public void GetLuisIntentWithNullObjectResponseFromLuis()
//        {
//            var query = "help with divorce";

//            var responseq = new HttpResponseMessage();
//            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
//            {
//                Content = new StringContent("{\r\n  \"query\":null,\"intents\":[],\"entities\":[]}")
//            };

//            var luisResponse = httpClientService.GetAsync(options.Endpoint);
//            luisResponse.Returns(httpResponseMessage);
//            //Act
//            string intentWithScore = luisProxy.GetIntents(query).Result;

//            //TO DO : Assert
//            Assert.AreEqual(true, intentWithScore);
//        }

//        [Test]
//        public void GetLuisIntentWithEmptyResponseFromLuis()
//        {

//            var responseq = new HttpResponseMessage();
//            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
//            {
//                Content = new StringContent("")
//            };

//            var luisResponse = httpClientService.GetAsync(options.Endpoint);
//            luisResponse.Returns(httpResponseMessage);
//            //Act
//            string intentWithScore = luisProxy.GetIntents("").Result;

//            //TO DO : Assert
//            Assert.AreEqual(true, intentWithScore);
//        }

//        [Test]
//        public void FilterLuisIntentWithNoneIntent()
//        {
//            intents = new List<string>
//            {
//                "child abuse",
//                "eviction"
//            };

//            IntentWithScore intentWithScore = new IntentWithScore { TopScoringIntent = "None", Score = 93, TopNIntents = intents };
//            //Act
//            var result = luisProxy.FilterLuisIntents(intentWithScore);
//            //Assert
//            Assert.AreEqual(null, result);
//        }

//        [Test]
//        public void FilterLuisIntentWithProperIntent()
//        {
//            intents = new List<string>
//            {
//                "child abuse",
//                "eviction"
//            };

//            IntentWithScore intentWithScore = new IntentWithScore { TopScoringIntent = "child custody", Score = 93, TopNIntents = intents };
//            //Act
//            var result = luisProxy.FilterLuisIntents(intentWithScore);
//            //Assert            
//            Assert.IsNotEmpty(result);
//        }
//    }
//}
