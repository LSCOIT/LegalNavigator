using Xunit;
using NSubstitute;
using System.Collections.Generic;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Api;
using Newtonsoft.Json.Linq;
using Access2Justice.Shared.Models;
using System;

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
        private readonly string properLuisResponse =
                   "{\r\n  \"query\": \"child abuse\",\r\n  \"topScoringIntent\": {\r\n    " +
                   "\"intent\": \"eviction\",\r\n    \"score\": 0.919329442\r\n  },\r\n  \"intents\": [\r\n    {\r\n     " +
                   " \"intent\": \"eviction\",\r\n      \"score\": 0.239329442\r\n    },\r\n    {\r\n      " +
                   "\"intent\": \"child abuse\",\r\n      \"score\": 0.09217278\r\n    },\r\n    {\r\n      " +
                   "\"intent\": \"child\",\r\n      \"score\": 0.06267241\r\n    },\r\n    {\r\n      " +
                   "\"intent\": \"divorce\",\r\n      \"score\": 0.00997853652\r\n    },\r\n    {\r\n     " +
                   " \"intent\": \"None\",\r\n      \"score\": 0.00248154555\r\n    }\r\n  ],\r\n  \"entities\": []\r\n}";
        private readonly string lowScoreLuisResponse =
                   "{\r\n  \"query\": \"child abuse\",\r\n  \"topScoringIntent\": {\r\n    " +
                   "\"intent\": \"eviction\",\r\n    \"score\": 0.269329442\r\n  },\r\n  \"intents\": [\r\n    {\r\n     " +
                   " \"intent\": \"eviction\",\r\n      \"score\": 0.239329442\r\n    },\r\n    {\r\n      " +
                   "\"intent\": \"child abuse\",\r\n      \"score\": 0.09217278\r\n    },\r\n    {\r\n      " +
                   "\"intent\": \"child\",\r\n      \"score\": 0.06267241\r\n    },\r\n    {\r\n      " +
                   "\"intent\": \"divorce\",\r\n      \"score\": 0.00997853652\r\n    },\r\n    {\r\n     " +
                   " \"intent\": \"None\",\r\n      \"score\": 0.00248154555\r\n    }\r\n  ],\r\n  \"entities\": []\r\n}";
        private readonly string meduimScoreLuisResponse =
                   "{\r\n  \"query\": \"good bye\",\r\n  \"topScoringIntent\": {\r\n    " +
                   "\"intent\": \"child abuse\",\r\n    \"score\": 0.7257252\r\n  },\r\n  " +
                   "\"intents\": [\r\n    {\r\n      \"intent\": \"None\",\r\n      " +
                   "\"score\": 0.06429157\r\n    },\r\n    {\r\n      \"intent\": \"Divorce\",\r\n      " +
                   "\"score\": 0.05946025\r\n    },\r\n    {\r\n      \"intent\": \"Eviction\",\r\n     " +
                   "\"score\": 4.371685E-05\r\n    }\r\n  ],\r\n  \"entities\": []\r\n}";
        private readonly string emptyLuisResponse = "";
        private readonly string noneLuisResponse =
                   "{\r\n  \"query\": \"good bye\",\r\n  \"topScoringIntent\": {\r\n    " +
                   "\"intent\": \"None\",\r\n    \"score\": 0.7257252\r\n  },\r\n  " +
                   "\"intents\": [\r\n    {\r\n      \"intent\": \"None\",\r\n      " +
                   "\"score\": 0.06429157\r\n    },\r\n    {\r\n      \"intent\": \"Divorce\",\r\n      " +
                   "\"score\": 0.05946025\r\n    },\r\n    {\r\n      \"intent\": \"Eviction\",\r\n     " +
                   "\"score\": 4.371685E-05\r\n    }\r\n  ],\r\n  \"entities\": []\r\n}";
        private readonly string keyword = "eviction";
        private readonly JArray topicsData =
                   JArray.Parse(@"[{'id':'addf41e9-1a27-4aeb-bcbb-7959f95094ba','name':'Family',
                   'parentTopicID':'','keywords':'eviction','location':[{'state':'Hawaii','county':'Kalawao County','city':'Kalawao',
                    'zipCode':'96742'},{'zipCode':'96741'},{'state':'Hawaii','county':'Honolulu County','city':'Honolulu'},{'state':
                   'Hawaii','city':'Hawaiian Beaches'},{'state':'Hawaii','city':'Haiku-Pauwela'},{'state':'Alaska'}],'jsonContent':'',
                   'icon':'./assets/images/topics/topic14.png','createdBy':'','createdTimeStamp':'','modifiedBy':'','modifiedTimeStamp':'
                   ','_rid':'mwoSALHtpAEBAAAAAAAAAA==','_self':'dbs/mwoSAA==/colls/mwoSALHtpAE=/docs/mwoSALHtpAEBAAAAAAAAAA==/',
                    '_etag':'\'05008e57-0000-0000-0000-5b0797c10000\'','_attachments':'attachments/','_ts':1527224257},
                    {'id':'3aa3a1be-8291-42b1-85c2-252f756febbc','name':'Family',
                   'parentTopicID':'','keywords':'eviction','location':[{'state':'Hawaii','county':'Kalawao County','city':'Kalawao',
                    'zipCode':'96742'},{'zipCode':'96741'},{'state':'Hawaii','county':'Honolulu County','city':'Honolulu'},{'state':
                   'Hawaii','city':'Hawaiian Beaches'},{'state':'Hawaii','city':'Haiku-Pauwela'},{'state':'Alaska'}],'jsonContent':'',
                   'icon':'./assets/images/topics/topic14.png','createdBy':'','createdTimeStamp':'','modifiedBy':'','modifiedTimeStamp':'
                   ','_rid':'mwoSALHtpAEBAAAAAAAAAA==','_self':'dbs/mwoSAA==/colls/mwoSALHtpAE=/docs/mwoSALHtpAEBAAAAAAAAAA==/',
                    '_etag':'\'05008e57-0000-0000-0000-5b0797c10000\'','_attachments':'attachments/','_ts':1527224257}]");
        private readonly JArray resourcesData =
                    JArray.Parse(@"[{'id':'77d301e7-6df2-612e-4704-c04edf271806','name':'Tenant Action Plan 
                    for Eviction','description':'This action plan is for tenants who are facing eviction and have experienced the following:',
                    'resourceType':'Action','externalUrl':'','url':'','topicTags':[{'id':'f102bfae-362d-4659-aaef-956c391f79de'},
                    {'id':'2c0cc7b8-62b1-4efb-8568-b1f767f879bc'},{'id':'3aa3a1be-8291-42b1-85c2-252f756febbc'}],'location':[{'state':'Hawaii',
                    'city':'Kalawao','zipCode':'96742'},{'zipCode':'96741'},{'state':'Alaska'}],'icon':'./assets/images/resources/resource.png',
                    'createdBy':'','createdTimeStamp':'','modifiedBy':'','modifiedTimeStamp':'','_rid':'mwoSAJdNlwIBAAAAAAAAAA==',
                    '_self':'dbs/mwoSAA==/colls/mwoSAJdNlwI=/docs/mwoSAJdNlwIBAAAAAAAAAA==/','_etag':'\'040007b5-0000-0000-0000-5b0792260000\'',
                    '_attachments':'attachments/','_ts':1527222822},{'id':'19a02209-ca38-4b74-bd67-6ea941d41518','name':'Legal Help Organization',
                    'description':'This action plan is for tenants who are facing eviction and have experienced the following:','resourceType':'Organization'
                    ,'externalUrl':'','url':'','topicTags':[{'id':'f102bfae-362d-4659-aaef-956c391f79de'},{'id':'3aa3a1be-8291-42b1-85c2-252f756febbc'}],
                    'location':[{'state':'Hawaii','city':'Kalawao','zipCode':'96742'}],'icon':'./assets/images/resources/resource.png','createdBy':'',
                    'createdTimeStamp':'','modifiedBy':'','modifiedTimeStamp':'','_rid':'mwoSAJdNlwIBAAAAAAAAAA==','_self':
                    'dbs/mwoSAA==/colls/mwoSAJdNlwI=/docs/mwoSAJdNlwIBAAAAAAAAAA==/','_etag':'\'040007b5-0000-0000-0000-5b0792260000\'',
                    '_attachments':'attachments/','_ts':1527222822}]");
        private readonly JObject emptyTopicObject = JObject.Parse(@"{}");
        private readonly JObject emptyResourceObject = JObject.Parse(@"{}");
        private readonly string searchText = "i'm getting kicked out";
        private readonly string webData =
                   "{\r\n  \"webResources\": {\r\n    \"_type\": \"SearchResponse\",\r\n    \"instrumentation\": {\r\n      \"_type\": " +
                   "\"ResponseInstrumentation\",\r\n      \"pingUrlBase\": \"https://www.bingapis.com/api/ping?IG=1965C084847D497DB211864D4160BBAD&CID=0F6A141A98C863D31AD41FE799436276&ID=\"," +
                   "\r\n      \"pageLoadPingUrl\": \"https://www.bingapis.com/api/ping/pageload?IG=1965C084847D497DB211864D4160BBAD&CID=0F6A141A98C863D31AD41FE799436276&Type=Event.CPT&DATA=0\"" +
                   "\r\n    },\r\n    \"queryContext\": {\r\n      \"originalQuery\": \"getting kicked out\"\r\n    },\r\n    \"webPages\": " +
                   "{\r\n      \"webSearchUrl\": \"https://www.bing.com/search?q=getting+kicked+out\",\r\n      \"webSearchUrlPingSuffix\": " +
                   "\"DevEx,5388.1\",\r\n      \"totalEstimatedMatches\": 6,\r\n      \"value\": [\r\n        {\r\n          \"id\": " +
                   "\"https://api.cognitive.microsoft.com/api/v7/#WebPages.0\",\r\n          \"name\": \"Mukesh and Another v State for NCT of Delhi and " +
                   "Others - Lawnotes.in\",\r\n          \"url\": \"http://www.lawnotes.in/Mukesh_and_Another_v_State_for_NCT_of_Delhi_and_Others\"," +
                   "\r\n          \"urlPingSuffix\": \"DevEx,5076.1\",\r\n          \"isFamilyFriendly\": true,\r\n          \"displayUrl\": " +
                   "\"www.lawnotes.in/Mukesh_and_Another_v_State_for_NCT_of_Delhi_and_Others\",\r\n          \"snippet\": \"Mukesh and Another v State " +
                   "for NCT of Delhi and Others. From Lawnotes.in. ... kicked on her abdomen and bitten over lips, cheek, breast and vulval region. " +
                   "The prosecutrix remembered intercourse two times and rectal penetration also ... and his disclosure Ex.PW-60/H was also recorded. " +
                   "He pointed out the Munirka bus stand from where the victims were picked up vide memo Ex.PW-68/I and he also pointed out Mahipalpur Flyover, " +
                   "the place where the victims were thrown out of the moving bus ...\",\r\n          \"deepLinks\": [\r\n            {\r\n              " +
                   "\"name\": \"K Srinivas Rao v D A Deepa\",\r\n              \"url\": \"http://www.lawnotes.in/K_Srinivas_Rao_v_D_A_Deepa\"," +
                   "\r\n              \"urlPingSuffix\": \"DevEx,5065.1\",\r\n              \"snippet\": \"K abc Roo v D A Deep. From " +
                   "Lawnotes.in. Jump to:navigation, search. ... the appellant-husband beat her mother and kicked her on her stomach. Both of " +
                   "them received injuries. She, therefore, ... must ensure that this exercise does not lead to the erring spouse using mediation " +
                   "process to get out of clutches of the law.\"\r\n            },\r\n            {\r\n              \"name\": \"LawNotes.\",\r" +
                   "\n              \"url\": \"http://www.lawnotes.in/B_D_Khunte_v_Union_of_India_and_Others\",\r\n              \"urlPingSuffix" +
                   "\": \"DevEx,5066.1\",\r\n              \"snippet\": \"B D Khunte v Union of India and Others. From Lawnotes.in. Jump to:" +
                   "navigation, search. ... the deceased punched him and kicked him repeatedly and asked him to put up his hand and hold the side " +
                   "beams of the top berth of the double bunk in the store room. The appellant’s further case is that the deceased thereafter made " +
                   "unwelcome and improper ... We must at the threshold point out that there is no challenge to the finding that it was the " +
                   "appellant who had shot the deceased using the weapon ...\"\r\n            },\r\n            {\r\n              \"name\": " +
                   "\"C K Dasegowda And Others v State of Karnataka - LawNotes.\",\r\n              \"url\": \"https://www.lawnotes.in/C_K_Dasegowda_and_Others_v_State_of_Karnataka\"," +
                   "\r\n              \"urlPingSuffix\": \"DevEx,5067.1\",\r\n              \"snippet\": \"C K Dasegowda and Others v State of " +
                   "Karnataka. From Lawnotes.in. Jump to:navigation, search. ... Necessary relevant facts are stated hereunder to appreciate the " +
                   "case of the appellants and also to find out whether they are entitled to the relief as prayed for in ... A-6, A-8 and A-10 " +
                   "kicked PW-1. A-5 and A-7 assaulted Bhagyamma- PW-6 with iron blade of plough and A-9 kicked her. 4. A complaint (Ex.-P1) was " +
                   "lodged on 11.8.1999 at 9:00 a.m. before the police. The Crime Case No. CC 728 of 2000 ...\"\r\n            },\r\n            {\r\n" +
                   "   \"name\": \"Geeta Mehrotra And Another Vs State of U P And Another - Lawnotes.\",\r\n              \"url\": " +
                   "\"https://www.lawnotes.in/Geeta_Mehrotra_and_Another_Vs_State_of_U_P_and_Another\",\r\n              \"urlPingSuffix\": \"DevEx,5068.1" +
                   "\",\r\n              \"snippet\": \"Geeta Mehrotra and Another Vs State of U P and Another. From Lawnotes.in. Jump to:navigation, " +
                   "search. Home Indian Law Supreme Court of India Supreme Court of India Cases 2012 Supreme Court of India Cases October 2012. " +
                   "REPORTABLE IN THE SUPREME COURT OF INDIA ... Geeta Mehrotra regarding the complainant using bad words and it was said that if " +
                   "her daughter came there she will be kicked out.\"\r\n            },\r\n            {\r\n              \"name\": \"B Thirumal " +
                   "v Ananda Sivakumar And Others - LawNotes.\",\r\n              \"url\": \"https://www.lawnotes.in/B_Thirumal_v_Ananda_Sivakumar_and_Others\",\r\n" +
                   "       \"urlPingSuffix\": \"DevEx,5069.1\",\r\n              \"snippet\": \"B Thirumal v Ananda Sivakumar and Others. From Lawnotes.in. Jump to:navigation," +
                   " ... Leave granted. 2. These appeals arise out of a judgment and order dated 4th August, 2009 whereby a Division Bench of the " +
                   "High Court of Judicature at Madras has allowed Writ Appeals No. 1155, ... such a person receives higher salary, but when he is " +
                   "compulsorily “kicked upstairs” (if we may permitted to observe so) the Diploma-holder Junior Engineer, ...\"\r\n            }" +
                   "\r\n          ],\r\n          \"dateLastCrawled\": \"2018-05-19T08:31:00Z\",\r\n          \"fixedPosition\": false,\r\n          " +
                   "\"language\": \"en\"\r\n        }\r\n      ]\r\n    },\r\n    \"rankingResponse\": {\r\n      \"mainline\": {\r\n        " +
                   "\"items\": [\r\n          {\r\n            \"answerType\": \"WebPages\",\r\n            \"resultIndex\": 0,\r\n            " +
                   "\"value\": {\r\n              \"id\": \"https://api.cognitive.microsoft.com/api/v7/#WebPages.0\"\r\n            }\r\n          " +
                   "}\r\n        ]\r\n      }\r\n    }\r\n  }\r\n}";        
        private readonly Location location = new Location();
        private readonly LuisInput luisInput = new LuisInput();
        private readonly ResourceFilter resourceFilter = new ResourceFilter { TopicIds = new List<string> { "addf41e9-1a27-4aeb-bcbb-7959f95094ba" }, PageNumber = 0, ResourceType = "ALL", Location = new Location() };
        private readonly List<dynamic> allResourcesCount = new List<dynamic> {  new {  ResourceName = "All", ResourceCount = 10     },
            new {  ResourceName = "Action Plans", ResourceCount = 2 },
            new {  ResourceName = "Articles", ResourceCount = 6 },
            new {   ResourceName = "Forms", ResourceCount = 2 } };
        private readonly List<string> topicIds = new List<string> { "addf41e9-1a27-4aeb-bcbb-7959f95094ba" };
    #endregion

        #region Mocked Output Data         
        private readonly string expectedLuisNoneIntent = "None";
        private readonly int expectedLowerthreshold = 0;
        private readonly string expectedLuisTopIntent = "eviction";
        private readonly string expectedTopicId = "addf41e9-1a27-4aeb-bcbb-7959f95094ba";        
        private readonly string expectedInternalResponse = "topics";
        private readonly string expectedWebResponse = "webResources";
        private readonly string expectedEmptyInternalResponse = "{\r\n  \"topics\": [],\r\n  \"resources\": [],\r\n  \"continuationToken\": [],\r\n  " +
                   "\"topicIds\": [],\r\n  \"resourceTypeFilter\": [],\r\n  \"topIntent\": \"eviction\"\r\n}";
        
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
            luisSettings.UpperThreshold.Returns(0.9M);
            luisSettings.LowerThreshold.Returns(0.6M);
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
                Score = 0.96M,
                TopScoringIntent = "eviction",
                TopNIntents = topNIntents
            };
            int expectedUpperthreshold = 2;

            //act 
            int threshold = luisBusinessLogic.ApplyThreshold(intentWithScore);

            //assert
            Assert.Equal(expectedUpperthreshold, threshold);
        }

        [Fact]
        public void ApplyThresholdTestsShouldMatchMediumthreshold()
        {
            // arrange
            List<string> topNIntents = new List<string> { "eviction", "child abuse", "traffic ticket", "divorce" };
            IntentWithScore intentWithScore = new IntentWithScore
            {
                IsSuccessful = true,
                Score = 0.81M,
                TopScoringIntent = "eviction",
                TopNIntents = topNIntents
            };
            int expectedMediumthreshold = 1;


            //act 
            int threshold = luisBusinessLogic.ApplyThreshold(intentWithScore);

            //assert            
            Assert.Equal(expectedMediumthreshold, threshold);
        }

        [Fact]
        public void ApplyThresholdTestsShouldMatchLowerthreshold()
        {
            // arrange
            List<string> topNIntents = new List<string> { "eviction", "child abuse", "traffic ticket", "divorce" };
            IntentWithScore intentWithScore = new IntentWithScore
            {
                IsSuccessful = true,
                Score = 0.59M,
                TopScoringIntent = "eviction",
                TopNIntents = topNIntents
            };

            //act 
            int threshold = luisBusinessLogic.ApplyThreshold(intentWithScore);

            //assert            
            Assert.Equal(expectedLowerthreshold, threshold);
        }

        [Fact]
        public void ApplyThresholdTestsShouldWithEmptyObject()
        {
            // arrange
            List<string> topNIntents = new List<string> { "" };
            IntentWithScore intentWithScore = new IntentWithScore
            {
                IsSuccessful = false,
                Score = 0M,
                TopScoringIntent = "",
                TopNIntents = topNIntents
            };

            //act 
            int threshold = luisBusinessLogic.ApplyThreshold(intentWithScore);

            //assert
            Assert.Equal(expectedLowerthreshold, threshold);
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
            var result = luisBusinessLogic.GetInternalResourcesAsync(keyword, location).Result;

            //assert            
            Assert.Contains(expectedTopicId, result);
        }

        [Fact]
        public void GetInternalResourcesAsyncTestsShouldReturnEmptyTopics()
        {
            //arrange
            PagedResources pagedResources = new PagedResources() { Results = resourcesData, ContinuationToken = "[]", TopicIds = topicIds };
            var topicResponse = topicsResourcesBusinessLogic.GetTopicsAsync(keyword, location);
            topicResponse.Returns(emptyTopicObject);

            //act
            var result = luisBusinessLogic.GetInternalResourcesAsync(keyword, location).Result;

            //assert            
            Assert.Equal(expectedEmptyInternalResponse, result);
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
            var result = luisBusinessLogic.GetInternalResourcesAsync(keyword, location).Result;

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
            var result = luisBusinessLogic.GetInternalResourcesAsync(keyword, location).Result;

            //assert            
            Assert.Contains(keyword, result);
        }
                
        [Fact]
        public void GetResourceBasedOnThresholdAsyncTestsShouldValidateLowScore()
        {
            //arrange
            luisInput.Sentence = searchText;
            var luisResponse = luisProxy.GetIntents(searchText);
            luisResponse.Returns(lowScoreLuisResponse);

            var webResponse = webSearchBusinessLogic.SearchWebResourcesAsync(bingSettings.BingSearchUrl);
            webResponse.ReturnsForAnyArgs<dynamic>(webData);

            //act
            var result = luisBusinessLogic.GetResourceBasedOnThresholdAsync(luisInput).Result;

            //assert
            Assert.Contains(expectedWebResponse, result);
        }

        [Fact]
        public void GetResourceBasedOnThresholdAsyncTestsShouldValidateMediumScore()
        {
            //arrange
            luisInput.Sentence = searchText;
            var luisResponse = luisProxy.GetIntents(searchText);
            luisResponse.ReturnsForAnyArgs(meduimScoreLuisResponse);
            
            var webResponse = webSearchBusinessLogic.SearchWebResourcesAsync(bingSettings.BingSearchUrl);
            webResponse.ReturnsForAnyArgs<dynamic>(webData);

            //act
            var response = luisBusinessLogic.GetResourceBasedOnThresholdAsync(luisInput).Result;

            //assert
            Assert.Contains(expectedWebResponse, response);
        }

        [Fact]
        public void GetResourceBasedOnThresholdAsyncTestsShouldValidateUpperScore()
        {
            //arrange
            luisInput.Sentence = searchText;
            var luisResponse = luisProxy.GetIntents(searchText);
            luisResponse.ReturnsForAnyArgs(properLuisResponse);

            var topicResponse = topicsResourcesBusinessLogic.GetTopicsAsync(keyword, location);
            topicResponse.ReturnsForAnyArgs<dynamic>(emptyTopicObject);

            var internalResponse = luis.GetInternalResourcesAsync(keyword, location);
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

            var internalResponse = luis.GetInternalResourcesAsync(keyword, location);
            internalResponse.ReturnsForAnyArgs<dynamic>(internalResponse);

            //act
            var result = luisBusinessLogic.GetResourceBasedOnThresholdAsync(luisInput).Result;

            //assert
            Assert.Contains(expectedInternalResponse, result);
        }

    }
}
