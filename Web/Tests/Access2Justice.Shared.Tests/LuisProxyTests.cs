using System;
using Xunit;
using NSubstitute;
using Access2Justice.Shared.Luis;
using System.Net.Http;
using System.Net;
using Access2Justice.Shared.Tests.TestData;

namespace Access2Justice.Shared.Tests
{
    public class LuisProxyTests
    {
        private readonly ILuisSettings luisSettings;
        private readonly IHttpClientService httpClientService;
        private readonly LuisProxy luisProxy;

        private readonly string expectedProperResult = "eviction";
        private readonly string expectedNoneResult = "None";

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
                Content = new StringContent(LuisTestData.ProperLuisResponse)
            };

            var luisResponse = httpClientService.GetAsync(luisSettings.Endpoint);
            luisResponse.Returns(httpResponseMessage);

            //act
            var result = luisProxy.GetIntents(LuisTestData.Query).Result;

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
                Content = new StringContent(LuisTestData.NoneLuisResponse)
            };

            var luisResponse = httpClientService.GetAsync(luisSettings.Endpoint);
            luisResponse.Returns(httpResponseMessage);

            //act
            var result = luisProxy.GetIntents(LuisTestData.Query).Result;

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
            var result = luisProxy.GetIntents(LuisTestData.Query).Result;

            //assert
            Assert.Empty(result);
        }


    }
}
