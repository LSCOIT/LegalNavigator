using System;
using Xunit;
using NSubstitute;
using Access2Justice.Shared.Luis;
using System.Net.Http;
using System.Net;

namespace Access2Justice.Shared.Tests.Luis
{
    public class LuisProxyTests
    {
        #region variables
        private readonly ILuisSettings luisSettings;
        private readonly IHttpClientService httpClientService;
        private readonly LuisProxy luisProxy;
        #endregion

        #region Mocked Input Data
        private readonly string query = "eviction";
        private readonly string properLuisResponse =
                   "{\r\n  \"query\": \"child abuse\",\r\n  \"topScoringIntent\": {\r\n    " +
                   "\"intent\": \"eviction\",\r\n    \"score\": 0.919329442\r\n  },\r\n  \"intents\": [\r\n    {\r\n     " +
                   " \"intent\": \"eviction\",\r\n      \"score\": 0.239329442\r\n    },\r\n    {\r\n      " +
                   "\"intent\": \"child abuse\",\r\n      \"score\": 0.09217278\r\n    },\r\n    {\r\n      " +
                   "\"intent\": \"child\",\r\n      \"score\": 0.06267241\r\n    },\r\n    {\r\n      " +
                   "\"intent\": \"divorce\",\r\n      \"score\": 0.00997853652\r\n    },\r\n    {\r\n     " +
                   " \"intent\": \"None\",\r\n      \"score\": 0.00248154555\r\n    }\r\n  ],\r\n  \"entities\": []\r\n}";
        private readonly string noneLuisResponse =
                   "{\r\n  \"query\": \"good bye\",\r\n  \"topScoringIntent\": {\r\n    " +
                   "\"intent\": \"None\",\r\n    \"score\": 0.7257252\r\n  },\r\n  " +
                   "\"intents\": [\r\n    {\r\n      \"intent\": \"None\",\r\n      " +
                   "\"score\": 0.06429157\r\n    },\r\n    {\r\n      \"intent\": \"Divorce\",\r\n      " +
                   "\"score\": 0.05946025\r\n    },\r\n    {\r\n      \"intent\": \"Eviction\",\r\n     " +
                   "\"score\": 4.371685E-05\r\n    }\r\n  ],\r\n  \"entities\": []\r\n}";
        #endregion

        #region Mocked Output Data
        private readonly string expectedProperResult = "eviction";
        private readonly string expectedNoneResult = "None";
        #endregion

        public LuisProxyTests()
        {
            luisSettings = Substitute.For<ILuisSettings>();
            httpClientService = Substitute.For<IHttpClientService>();
            luisProxy = new LuisProxy(httpClientService, luisSettings);

            luisSettings.Endpoint.Returns(new Uri("https://www.luis.ai/home"));
            luisSettings.TopIntentsCount.Returns(3);
            luisSettings.IntentAccuracyThreshold.Returns(0.9M);
        }

        [Fact]
        public void GetIntentsWithProperIntent()
        {
            //arrange
            var responseq = new HttpResponseMessage();
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(properLuisResponse)
            };

            var luisResponse = httpClientService.GetAsync(luisSettings.Endpoint);
            luisResponse.Returns(httpResponseMessage);

            //act
            var result = luisProxy.GetIntents(query).Result;

            //assert
            Assert.Contains(expectedProperResult, result,StringComparison.InvariantCulture);
        }

        [Fact]
        public void GetIntentsWithNoneIntent()
        {
            //arrange
            var responseq = new HttpResponseMessage();
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(noneLuisResponse)
            };

            var luisResponse = httpClientService.GetAsync(luisSettings.Endpoint);
            luisResponse.Returns(httpResponseMessage);

            //act
            var result = luisProxy.GetIntents(query).Result;

            //assert
            Assert.Contains(expectedNoneResult, result, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void GetIntentsWithEmptyObject()
        {
            //arrange
            var responseq = new HttpResponseMessage();
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("")
            };

            var luisResponse = httpClientService.GetAsync(luisSettings.Endpoint);
            luisResponse.Returns(httpResponseMessage);

            //act
            var result = luisProxy.GetIntents(query).Result;

            //assert
            Assert.Empty(result);
        }


    }
}
