namespace Access2Justice.Tests.ServiceUnitTestCases
{
    using Access2Justice.CognitiveServices;
    using NUnit.Framework;    
    using NSubstitute;
    using Microsoft.Extensions.Options;
    using System.Net.Http;
    using System.Net;
    using Access2Justice.Shared;    

    [TestFixture]
    public class LuisHelperTests
    {        
        private IOptions<App> options;        
        private IHttpClientService httpClientService;
        private LuisHelper luisHelper;
        private App app;


        [SetUp]
        public void SetUp() {
            
            options = Substitute.For<IOptions<App>>();
            httpClientService = Substitute.For<IHttpClientService>();
            luisHelper = new LuisHelper(options, httpClientService);

            //setting  configuration values.
            app = new App();
            app.LuisUrl = "http://www.bing.com";
            app.TopIntentsCount = 3;
            options.Value.Returns(app);
        }

        [Test]
        public void GetLuisIntentWithProperResponseFromLuis()
        {            
            LuisInput luisInput = new LuisInput();

            var responseq = new HttpResponseMessage(); 
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\r\n  \"query\": \"child abuse\",\r\n  \"topScoringIntent\": {\r\n    \"intent\": \"ChildAbuse\",\r\n    \"score\": 0.239329442\r\n  },\r\n  \"intents\": [\r\n    {\r\n      \"intent\": \"ChildAbuse\",\r\n      \"score\": 0.239329442\r\n    },\r\n    {\r\n      \"intent\": \"Above18Age\",\r\n      \"score\": 0.09217278\r\n    },\r\n    {\r\n      \"intent\": \"Age\",\r\n      \"score\": 0.06267241\r\n    },\r\n    {\r\n      \"intent\": \"Greetings\",\r\n      \"score\": 0.00997853652\r\n    },\r\n    {\r\n      \"intent\": \"None\",\r\n      \"score\": 0.00248154555\r\n    }\r\n  ],\r\n  \"entities\": []\r\n}")
            };            

            var luisResponse = httpClientService.GetAsync(app.LuisUrl);
            luisResponse.Returns(httpResponseMessage);

            //Act
            IntentWithScore intentWithScore = luisHelper.GetLuisIntent(luisInput).Result;          

            //Assert
            Assert.AreEqual(true, intentWithScore.IsSuccessful);
        }

        [Test]
        public void GetLuisIntentWithNullObjectResponseFromLuis()
        {
            LuisInput luisInput = new LuisInput();

            var responseq = new HttpResponseMessage();
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\r\n  \"query\":null,\"intents\":[],\"entities\":[]}")
            };

            var luisResponse = httpClientService.GetAsync(app.LuisUrl);
            luisResponse.Returns(httpResponseMessage);
            //Act
            IntentWithScore intentWithScore = luisHelper.GetLuisIntent(luisInput).Result;

            //Assert
            Assert.AreEqual(true, intentWithScore.IsSuccessful);
        }

        [Test]
        public void GetLuisIntentWithEmptyResponseFromLuis()
        {
            LuisInput luisInput = new LuisInput();

            var responseq = new HttpResponseMessage();
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("")
            };

            var luisResponse = httpClientService.GetAsync(app.LuisUrl);
            luisResponse.Returns(httpResponseMessage);
            //Act
            IntentWithScore intentWithScore = luisHelper.GetLuisIntent(luisInput).Result;

            //Assert
            Assert.AreEqual(true, intentWithScore.IsSuccessful);
        }
    }
}
