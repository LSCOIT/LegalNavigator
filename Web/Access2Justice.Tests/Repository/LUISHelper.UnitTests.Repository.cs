namespace Access2Justice.Tests.ServiceUnitTestCases
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Access2Justice.Repository;
    using NUnit.Framework;    
    using NSubstitute;
    using Microsoft.Extensions.Options;
    using System.Net.Http;
    using System.Net;
    using Newtonsoft.Json;

    [TestFixture]
    public class LUISHelper_Repository_UnitTests
    {
        private ILUISHelper _luisHelper;
        private IOptions<App> _options;        
        private IHttpClientService _httpClientService;


        [SetUp]
        public void SetUp() {
            _luisHelper = Substitute.For<ILUISHelper>();
            _options = Substitute.For<IOptions<App>>();
            _httpClientService = Substitute.For<IHttpClientService>();
        }

        [Test]
        public void GetLUISIntentWithProperData()
        {

            LUISHelper luisHelper = new LUISHelper(_options, _httpClientService);
            LUISInput luisInput = new LUISInput();
            var app = new App();
            app.LuisUrl = "http://www.bing.com";
            _options.Value.Returns(app);            
            

            var responseq = new HttpResponseMessage(); 
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\r\n  \"query\": \"child abuse\",\r\n  \"topScoringIntent\": {\r\n    \"intent\": \"ChildAbuse\",\r\n    \"score\": 0.239329442\r\n  },\r\n  \"intents\": [\r\n    {\r\n      \"intent\": \"ChildAbuse\",\r\n      \"score\": 0.239329442\r\n    },\r\n    {\r\n      \"intent\": \"Above18Age\",\r\n      \"score\": 0.09217278\r\n    },\r\n    {\r\n      \"intent\": \"Age\",\r\n      \"score\": 0.06267241\r\n    },\r\n    {\r\n      \"intent\": \"Greetings\",\r\n      \"score\": 0.00997853652\r\n    },\r\n    {\r\n      \"intent\": \"None\",\r\n      \"score\": 0.00248154555\r\n    }\r\n  ],\r\n  \"entities\": []\r\n}")
            };            

            var luisResponse = _httpClientService.GetAsync(app.LuisUrl);
            luisResponse.Returns(httpResponseMessage);

            //Act
            IntentWithScore intentWithScore = luisHelper.GetLUISIntent(luisInput).Result;          

            //Assert
            Assert.AreEqual(true, intentWithScore.IsSuccessful);
        }

        [Test]
        public void GetLUISIntentWithEmptyData()
        {

            LUISHelper luisHelper = new LUISHelper(_options, _httpClientService);
            LUISInput luisInput = new LUISInput();
            var app = new App();
            app.LuisUrl = "http://www.bing.com";
            _options.Value.Returns(app);


            var responseq = new HttpResponseMessage();
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\r\n  \"query\":null,\"intents\":[],\"entities\":[]}")
            };

            var luisResponse = _httpClientService.GetAsync(app.LuisUrl);
            luisResponse.Returns(httpResponseMessage);
            //Act
            IntentWithScore intentWithScore = luisHelper.GetLUISIntent(luisInput).Result;

            //Assert
            Assert.AreEqual(true, intentWithScore.IsSuccessful);
        }

        [Test]
        public void GetLUISIntentWithAnEmptyObject()
        {

            LUISHelper luisHelper = new LUISHelper(_options, _httpClientService);
            LUISInput luisInput = new LUISInput();
            var app = new App();
            app.LuisUrl = "http://www.bing.com";
            _options.Value.Returns(app);


            var responseq = new HttpResponseMessage();
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("")
            };

            var luisResponse = _httpClientService.GetAsync(app.LuisUrl);
            luisResponse.Returns(httpResponseMessage);
            //Act
            IntentWithScore intentWithScore = luisHelper.GetLUISIntent(luisInput).Result;

            //Assert
            Assert.AreEqual(true, intentWithScore.IsSuccessful);
        }
    }
}
