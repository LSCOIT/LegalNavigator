using Xunit;
using NSubstitute;
using System.Collections.Generic;
using System.Net.Http;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Api;
using Newtonsoft.Json.Linq;

namespace Access2Justice.Tests.ServiceUnitTestCases
{
    
    public class LuisBusinessLogicTests
    {
        private readonly ILuisProxy _luisProxy;
        private readonly ILuisSettings _luisSettings;
        private readonly ITopicsResourcesBusinessLogic _topicsResourcesBusinessLogic;
        private readonly IWebSearchBusinessLogic _webSearchBusinessLogic;
        private readonly ILuisBusinessLogic _luisBusinessLogic;
        private readonly LuisBusinessLogic luisBusinessLogic;


        public LuisBusinessLogicTests()
        {
            _luisProxy = Substitute.For<ILuisProxy>();
            _luisSettings = Substitute.For<ILuisSettings>();
            _topicsResourcesBusinessLogic = Substitute.For<ITopicsResourcesBusinessLogic>();
            _webSearchBusinessLogic = Substitute.For<IWebSearchBusinessLogic>();
            _luisBusinessLogic = Substitute.For<ILuisBusinessLogic>();
            luisBusinessLogic = new LuisBusinessLogic(_luisProxy, _luisSettings, _topicsResourcesBusinessLogic, _webSearchBusinessLogic);

            _luisSettings.Endpoint.Returns(new System.Uri("http://www.bing.com"));
            _luisSettings.TopIntentsCount.Returns("3");
            _luisSettings.UpperThreshold.Returns("0.9");
            _luisSettings.LowerThreshold.Returns("0.6");
        }

        [Fact]
        public void ParseLuisIntentWithProperIntent()
        {
            // arrange 
            string LuisResponse = "{\r\n  \"query\": \"child abuse\",\r\n  \"topScoringIntent\": {\r\n    \"intent\": \"ChildAbuse\",\r\n    \"score\": 0.239329442\r\n  },\r\n  \"intents\": [\r\n    {\r\n      \"intent\": \"ChildAbuse\",\r\n      \"score\": 0.239329442\r\n    },\r\n    {\r\n      \"intent\": \"Above18Age\",\r\n      \"score\": 0.09217278\r\n    },\r\n    {\r\n      \"intent\": \"Age\",\r\n      \"score\": 0.06267241\r\n    },\r\n    {\r\n      \"intent\": \"Greetings\",\r\n      \"score\": 0.00997853652\r\n    },\r\n    {\r\n      \"intent\": \"None\",\r\n      \"score\": 0.00248154555\r\n    }\r\n  ],\r\n  \"entities\": []\r\n}";

            // act
            IntentWithScore intentWithScore = luisBusinessLogic.ParseLuisIntent(LuisResponse);

            //assert            
            Assert.Equal("ChildAbuse", intentWithScore.TopScoringIntent);
        }

        [Fact]
        public void ParseLuisIntentWithEmptyObject()
        {
            // arrange 
            string LuisResponse = "";

            // act
            IntentWithScore intentWithScore = luisBusinessLogic.ParseLuisIntent(LuisResponse);

            //assert            
            Assert.Null(intentWithScore.TopScoringIntent);
        }

        [Fact]
        public void ParseLuisIntentWithNoneIntent()
        {
            // arrange 
            string LuisResponse = "{\r\n  \"query\": \"good bye\",\r\n  \"topScoringIntent\": {\r\n    \"intent\": \"None\",\r\n    \"score\": 0.7257252\r\n  },\r\n  \"intents\": [\r\n    {\r\n      \"intent\": \"None\",\r\n      \"score\": 0.7257252\r\n    },\r\n    {\r\n      \"intent\": \"Mobile home park tenants\",\r\n      \"score\": 0.355601132\r\n    },\r\n    {\r\n      \"intent\": \"Small Claims Court\",\r\n      \"score\": 0.1499888\r\n    },\r\n    {\r\n      \"intent\": \"Domestic Violence\",\r\n      \"score\": 0.144558728\r\n    },\r\n    {\r\n      \"intent\": \"Going to court\",\r\n      \"score\": 0.120800942\r\n    },\r\n    {\r\n      \"intent\": \"Child support\",\r\n      \"score\": 0.06429157\r\n    },\r\n    {\r\n      \"intent\": \"Divorce\",\r\n      \"score\": 0.05946025\r\n    },\r\n    {\r\n      \"intent\": \"Eviction\",\r\n      \"score\": 0.0429888479\r\n    },\r\n    {\r\n      \"intent\": \"Tenant's rights\",\r\n      \"score\": 0.0364453942\r\n    },\r\n    {\r\n      \"intent\": \"Unemployment\",\r\n      \"score\": 0.0278799217\r\n    },\r\n    {\r\n      \"intent\": \"Public & subsidized housing\",\r\n      \"score\": 0.0264688972\r\n    },\r\n    {\r\n      \"intent\": \"child custody\",\r\n      \"score\": 0.0251409784\r\n    },\r\n    {\r\n      \"intent\": \"Separation\",\r\n      \"score\": 0.02284297\r\n    },\r\n    {\r\n      \"intent\": \"division of property\",\r\n      \"score\": 0.0187299736\r\n    },\r\n    {\r\n      \"intent\": \"Guardianship\",\r\n      \"score\": 0.01589121\r\n    },\r\n    {\r\n      \"intent\": \"Consumer Fraud\",\r\n      \"score\": 0.0157391261\r\n    },\r\n    {\r\n      \"intent\": \"Other Consumer\",\r\n      \"score\": 0.0153076174\r\n    },\r\n    {\r\n      \"intent\": \"Home buyers & owners\",\r\n      \"score\": 0.0150051648\r\n    },\r\n    {\r\n      \"intent\": \"Debt\",\r\n      \"score\": 0.013652849\r\n    },\r\n    {\r\n      \"intent\": \"Car issues\",\r\n      \"score\": 0.01349422\r\n    },\r\n    {\r\n      \"intent\": \"Elder Abuse\",\r\n      \"score\": 0.0122769\r\n    },\r\n    {\r\n      \"intent\": \"Contracts\",\r\n      \"score\": 0.0104453657\r\n    },\r\n    {\r\n      \"intent\": \"Managing money\",\r\n      \"score\": 0.009631508\r\n    },\r\n    {\r\n      \"intent\": \"Utilities & phones\",\r\n      \"score\": 0.008403383\r\n    },\r\n    {\r\n      \"intent\": \"marriage\",\r\n      \"score\": 0.00816951\r\n    },\r\n    {\r\n      \"intent\": \"Marriage equality\",\r\n      \"score\": 0.007255214\r\n    },\r\n    {\r\n      \"intent\": \"Sexual Assault\",\r\n      \"score\": 0.006860437\r\n    },\r\n    {\r\n      \"intent\": \"The child protection system\",\r\n      \"score\": 0.006541669\r\n    },\r\n    {\r\n      \"intent\": \"Employment Discrimination\",\r\n      \"score\": 0.0049147536\r\n    },\r\n    {\r\n      \"intent\": \"Contempt of court\",\r\n      \"score\": 0.004551603\r\n    },\r\n    {\r\n      \"intent\": \"Legal financial obligation\",\r\n      \"score\": 0.00444463268\r\n    },\r\n    {\r\n      \"intent\": \"Custody\",\r\n      \"score\": 0.00298966654\r\n    },\r\n    {\r\n      \"intent\": \"alimony\",\r\n      \"score\": 0.002650723\r\n    },\r\n    {\r\n      \"intent\": \"Emergency shelter & assistance\",\r\n      \"score\": 0.00261214585\r\n    },\r\n    {\r\n      \"intent\": \"Credit problems\",\r\n      \"score\": 0.0026004985\r\n    },\r\n    {\r\n      \"intent\": \"Paternity/Parentage\",\r\n      \"score\": 0.00255779\r\n    },\r\n    {\r\n      \"intent\": \"Driver & Professional licenses\",\r\n      \"score\": 0.00251075416\r\n    },\r\n    {\r\n      \"intent\": \"Parenting Plans/Custody\",\r\n      \"score\": 0.00225709751\r\n    },\r\n    {\r\n      \"intent\": \"Housing discrimination\",\r\n      \"score\": 0.002157124\r\n    },\r\n    {\r\n      \"intent\": \"Adoption\",\r\n      \"score\": 0.00167205709\r\n    },\r\n    {\r\n      \"intent\": \"Utilities and telecommunications\",\r\n      \"score\": 0.00149995193\r\n    },\r\n    {\r\n      \"intent\": \"Identity\",\r\n      \"score\": 0.00148085481\r\n    },\r\n    {\r\n      \"intent\": \"Divorce Contested\",\r\n      \"score\": 0.00137153524\r\n    },\r\n    {\r\n      \"intent\": \"Unmarried couples\",\r\n      \"score\": 0.0013140107\r\n    },\r\n    {\r\n      \"intent\": \"Medical bills\",\r\n      \"score\": 0.00116409734\r\n    },\r\n    {\r\n      \"intent\": \"Student loans\",\r\n      \"score\": 0.00110878225\r\n    },\r\n    {\r\n      \"intent\": \"Other family\",\r\n      \"score\": 0.0009938785\r\n    },\r\n    {\r\n      \"intent\": \"Divorce Legal Separation\",\r\n      \"score\": 0.0009852285\r\n    },\r\n    {\r\n      \"intent\": \"Divorce Annulment\",\r\n      \"score\": 0.000906408\r\n    },\r\n    {\r\n      \"intent\": \"More family court procedures\",\r\n      \"score\": 0.000795052\r\n    },\r\n    {\r\n      \"intent\": \"Divorce Uncontested\",\r\n      \"score\": 0.000634081371\r\n    },\r\n    {\r\n      \"intent\": \"Foreclosure\",\r\n      \"score\": 0.000577257\r\n    },\r\n    {\r\n      \"intent\": \"Non-parents caring for children\",\r\n      \"score\": 0.0004945614\r\n    },\r\n    {\r\n      \"intent\": \"Veteran/Military Families\",\r\n      \"score\": 0.000434205169\r\n    },\r\n    {\r\n      \"intent\": \"Bankrucptcy\",\r\n      \"score\": 0.000325363\r\n    },\r\n    {\r\n      \"intent\": \"Relocation\",\r\n      \"score\": 0.000226491058\r\n    },\r\n    {\r\n      \"intent\": \"Non-parent custody\",\r\n      \"score\": 4.371685E-05\r\n    }\r\n  ],\r\n  \"entities\": []\r\n}";

            // act
            IntentWithScore intentWithScore = luisBusinessLogic.ParseLuisIntent(LuisResponse);

            //assert            
            Assert.Equal("None", intentWithScore.TopScoringIntent);
        }

        [Fact]
        public void ApplyThresholdToMatchWithUpperthreshold()
        {
            // arrange
            List<string> topNIntents = new List<string> { "eviction" , "child abuse", "traffic ticket", "divorce"};
            IntentWithScore intentWithScore = new IntentWithScore { IsSuccessful = true, Score = 0.96M, TopScoringIntent = "eviction",TopNIntents = topNIntents };

            //act 
            int threshold = luisBusinessLogic.ApplyThreshold(intentWithScore);

            //assert
            Assert.Equal(2, threshold);
        }

        [Fact]
        public void ApplyThresholdToMatchWithMediumthreshold()
        {
            // arrange
            List<string> topNIntents = new List<string> { "eviction", "child abuse", "traffic ticket", "divorce" };
            IntentWithScore intentWithScore = new IntentWithScore { IsSuccessful = true, Score = 0.81M, TopScoringIntent = "eviction", TopNIntents = topNIntents };

            //act 
            int threshold = luisBusinessLogic.ApplyThreshold(intentWithScore);

            //assert
            Assert.Equal(1, threshold);
        }

        [Fact]
        public void ApplyThresholdToMatchWithLowerthreshold()
        {
            // arrange
            List<string> topNIntents = new List<string> { "eviction", "child abuse", "traffic ticket", "divorce" };
            IntentWithScore intentWithScore = new IntentWithScore { IsSuccessful = true, Score = 0.59M, TopScoringIntent = "eviction", TopNIntents = topNIntents };

            //act 
            int threshold = luisBusinessLogic.ApplyThreshold(intentWithScore);

            //assert
            Assert.Equal(0, threshold);
        }

        [Fact]
        public void ApplyThresholdWithEmptyObject()
        {
            // arrange
            List<string> topNIntents = new List<string> { "" };
            IntentWithScore intentWithScore = new IntentWithScore { IsSuccessful = false, Score = 0M , TopScoringIntent = "", TopNIntents = topNIntents };

            //act 
            int threshold = luisBusinessLogic.ApplyThreshold(intentWithScore);

            //assert
            Assert.Equal(0, threshold);
        }        

        [Fact]
        public void GetWebResourcesAsyncWithProperData()
        {
            //arrange
            string searchText = "Access2Justice";
            string validResponse = "{\r\n  \"webResources\": \"[\\r\\n  {\\r\\n    \\\"id\\\": \\\"https://api.cognitive.microsoft.com/api/v7/#WebPages.0\\\",\\r\\n    \\\"name\\\": \\\"Offer - Indian Contract Act, 1872 - Lawnotes.in\\\",\\r\\n    \\\"url\\\": \\\"http://www.lawnotes.in/Offer_-_Indian_Contract_Act,_1872\\\",\\r\\n    \\\"urlPingSuffix\\\": \\\"DevEx,5071.1\\\",\\r\\n    \\\"about\\\": [\\r\\n      {\\r\\n        \\\"name\\\": \\\"Indian Contract Act, 1872\\\"\\r\\n      }\\r\\n    ],\\r\\n    \\\"isFamilyFriendly\\\": true,\\r\\n    \\\"displayUrl\\\": \\\"www.lawnotes.in/Offer_-_Inan_Contract_Act,_1872\\\",\\r\\n    \\\"snippet\\\": \\\"Main Article : Indian Contract Act, 1872. Offer is one of the essential elements of a contract as defined in Section 10 of the Indian Contract Act, 1872.\\\",\\r\\n    \\\"deepLinks\\\": [\\r\\n      {\\r\\n        \\\"name\\\": \\\"Section 245 of Income-Tax Act, 1961\\\",\\r\\n        \\\"url\\\": \\\"http://www.lawnotes.in/Section_245_of_Income-Tax_Act,_1961\\\",\\r\\n        \\\"urlPingSuffix\\\": \\\"DevEx,5062.1\\\",\\r\\n        \\\"snippet\\\": \\\"Section 245 of Income-Tax Act, 1961 deals with the topic of Set off of refunds against tax remaining payable\\\"\\r\\n      },\\r\\n      {\\r\\n        \\\"name\\\": \\\"11-CV-01846-LHK\\\",\\r\\n        \\\"url\\\": \\\"https://www.lawnotes.in/Apple_Inc_vs_Samsung_Electronics_Co_Ltd_et_al,_No._11-CV-01846-LHK\\\",\\r\\n        \\\"urlPingSuffix\\\": \\\"DevEx,5063.1\\\",\\r\\n        \\\"snippet\\\": \\\"Apple Inc vs Samsung Electronics Co Ltd et al, No. 11-CV-01846-LHK. From Lawnotes.in. Jump to:navigation, search. UNITED STATES DISTRICT COURT NORTHERN DISTRICT OF CALIFORNIA SAN JOSE DIVISION Case No.: 11-CV-01846-LHK APPLE, INC., a California corporation, Plaintiff and Counterdefendant, v. ... Uniloc USA, Inc. v. Micosoft Corp., 632 F.3d 1292, 1302 (Fed. Cir. 2011): ...\\\"\\r\\n      },\\r\\n      {\\r\\n        \\\"name\\\": \\\"Ramesh Chandra Shah And Others v Anil Joshi And Others - Lawnotes.\\\",\\r\\n        \\\"url\\\": \\\"https://www.lawnotes.in/Ramesh_Chandra_Shah_and_others_v_Anil_Joshi_and_others\\\",\\r\\n        \\\"urlPingSuffix\\\": \\\"DevEx,5064.1\\\",\\r\\n        \\\"snippet\\\": \\\"Ramesh Chandra Shah and others … Appellants versus Anil Joshi and others … Respondents J U D G M E N T ... with regard to basic knowledge of computer operation would be tested at the time of interview for which knowledge of Microsoft Operating System and Microsoft Office operation would be essential. In the call letter also which was sent to the appellant at the time of calling him for interview, ...\\\"\\r\\n      }\\r\\n    ],\\r\\n    \\\"dateLastCrawled\\\": \\\"2018-05-16T17:14:00Z\\\",\\r\\n    \\\"fixedPosition\\\": false,\\r\\n    \\\"language\\\": \\\"en\\\"\\r\\n  },\\r\\n  {\\r\\n    \\\"id\\\": \\\"https://api.cognitive.microsoft.com/api/v7/#WebPages.1\\\",\\r\\n    \\\"name\\\": \\\"Book Review- Balance of Justice\\\",\\r\\n    \\\"url\\\": \\\"http://accesstojustice-ng.org/Review_of_the_Balance_of_Justice.pdf\\\",\\r\\n    \\\"urlSuffix\\\": \\\"DevEx,5084.1\\\",\\r\\n    \\\"isFamilyFriendly\\\": true,\\r\\n    \\\"displayUrl\\\": \\\"accesstojustice-ng.org/Review_of_the_Balance_of_Justice.pdf\\\",\\r\\n    \\\"snippet\\\": \\\"The Balance of Justice presents factual insights into how judges of the Federal High Court, Lagos are perceived at work by users of the court system, and comprises of fact-based narratives, inventories, and analyses on the manner judges managed their time, the ... Microsoft Word - Book Review- Balance of Justice.doc Author: techniques Created Date:\\\",\\r\\n    \\\"dateLastCrawled\\\": \\\"2018-04-07T12:11:00Z\\\",\\r\\n    \\\"fixedPosition\\\": false,\\r\\n    \\\"language\\\": \\\"en\\\"\\r\\n  }\\r\\n]\"\r\n}";
            var responseq = new HttpResponseMessage();
            var ResponseMessage = "{\"_type\": \"SearchResponse\", \"instrumentation\": {\"_type\": \"ResponseInstrumentation\", \"pingUrlBase\": \"https:\\/\\/www.bingapis.com\\/api\\/ping?IG=428B8CFC22EA4711A146283DEF6F2821&CID=3ED5BEDB376A69A72CE9B52336E1689C&ID=\", \"pageLoadPingUrl\": \"https:\\/\\/www.bingapis.com\\/api\\/ping\\/pageload?IG=428B8CFC22EA4711A146283DEF6F2821&CID=3ED5BEDB376A69A72CE9B52336E1689C&Type=Event.CPT&DATA=0\"}, \"queryContext\": {\"originalQuery\": \"microsoft\"}, \"webPages\": {\"webSearchUrl\": \"https:\\/\\/www.bing.com\\/search?q=microsoft\", \"webSearchUrlPingSuffix\": \"DevEx,5232.1\", \"totalEstimatedMatches\": 5, \"value\": [{\"id\": \"https:\\/\\/api.cognitive.microsoft.com\\/api\\/v7\\/#WebPages.0\", \"name\": \"Offer - Indian Contract Act, 1872 - Lawnotes.in\", \"url\": \"http:\\/\\/www.lawnotes.in\\/Offer_-_Indian_Contract_Act,_1872\", \"urlPingSuffix\": \"DevEx,5071.1\", \"about\": [{\"name\": \"Indian Contract Act, 1872\"}], \"isFamilyFriendly\": true, \"displayUrl\": \"www.lawnotes.in\\/Offer_-_Inan_Contract_Act,_1872\", \"snippet\": \"Main Article : Indian Contract Act, 1872. Offer is one of the essential elements of a contract as defined in Section 10 of the Indian Contract Act, 1872.\", \"deepLinks\": [{\"name\": \"Section 245 of Income-Tax Act, 1961\", \"url\": \"http:\\/\\/www.lawnotes.in\\/Section_245_of_Income-Tax_Act,_1961\", \"urlPingSuffix\": \"DevEx,5062.1\", \"snippet\": \"Section 245 of Income-Tax Act, 1961 deals with the topic of Set off of refunds against tax remaining payable\"}, {\"name\": \"11-CV-01846-LHK\", \"url\": \"https:\\/\\/www.lawnotes.in\\/Apple_Inc_vs_Samsung_Electronics_Co_Ltd_et_al,_No._11-CV-01846-LHK\", \"urlPingSuffix\": \"DevEx,5063.1\", \"snippet\": \"Apple Inc vs Samsung Electronics Co Ltd et al, No. 11-CV-01846-LHK. From Lawnotes.in. Jump to:navigation, search. UNITED STATES DISTRICT COURT NORTHERN DISTRICT OF CALIFORNIA SAN JOSE DIVISION Case No.: 11-CV-01846-LHK APPLE, INC., a California corporation, Plaintiff and Counterdefendant, v. ... Uniloc USA, Inc. v. Micosoft Corp., 632 F.3d 1292, 1302 (Fed. Cir. 2011): ...\"}, {\"name\": \"Ramesh Chandra Shah And Others v Anil Joshi And Others - Lawnotes.\", \"url\": \"https:\\/\\/www.lawnotes.in\\/Ramesh_Chandra_Shah_and_others_v_Anil_Joshi_and_others\", \"urlPingSuffix\": \"DevEx,5064.1\", \"snippet\": \"Ramesh Chandra Shah and others … Appellants versus Anil Joshi and others … Respondents J U D G M E N T ... with regard to basic knowledge of computer operation would be tested at the time of interview for which knowledge of Microsoft Operating System and Microsoft Office operation would be essential. In the call letter also which was sent to the appellant at the time of calling him for interview, ...\"}], \"dateLastCrawled\": \"2018-05-16T17:14:00.0000000Z\", \"fixedPosition\": false, \"language\": \"en\"}, {\"id\": \"https:\\/\\/api.cognitive.microsoft.com\\/api\\/v7\\/#WebPages.1\", \"name\": \"Book Review- Balance of Justice\", \"url\": \"http:\\/\\/accesstojustice-ng.org\\/Review_of_the_Balance_of_Justice.pdf\", \"urlSuffix\": \"DevEx,5084.1\", \"isFamilyFriendly\": true, \"displayUrl\": \"accesstojustice-ng.org\\/Review_of_the_Balance_of_Justice.pdf\", \"snippet\": \"The Balance of Justice presents factual insights into how judges of the Federal High Court, Lagos are perceived at work by users of the court system, and comprises of fact-based narratives, inventories, and analyses on the manner judges managed their time, the ... Microsoft Word - Book Review- Balance of Justice.doc Author: techniques Created Date:\", \"dateLastCrawled\": \"2018-04-07T12:11:00.0000000Z\", \"fixedPosition\": false, \"language\": \"en\"}]}, \"rankingResponse\": {\"mainline\": {\"items\": [{\"answerType\": \"WebPages\", \"resultIndex\": 0, \"value\": {\"id\": \"https:\\/\\/api.cognitive.microsoft.com\\/api\\/v7\\/#WebPages.0\"}}, {\"answerType\": \"WebPages\", \"resultIndex\": 1, \"value\": {\"id\": \"https:\\/\\/api.cognitive.microsoft.com\\/api\\/v7\\/#WebPages.1\"}}]}}}";           

            var webResponse = _webSearchBusinessLogic.SearchWebResourcesAsync(searchText);
            webResponse.Returns(ResponseMessage);

            //act
            var response =  luisBusinessLogic.GetWebResourcesAsync(searchText).Result;
            
            //assert
            Assert.Equal(validResponse, response);
        }

        [Fact]
        public void GetWebResourcesAsyncWithEmptyValueObject()
        {
            //arrange
            string searchText = "Access2Justice";
            var responseq = new HttpResponseMessage();
            var ResponseMessage = "{\"_type\": \"SearchResponse\", \"instrumentation\": {}, \"queryContext\": {\"originalQuery\": \"microsoft\"}, \"webPages\": {\"webSearchUrl\": \"https:\\/\\/www.bing.com\\/search?q=microsoft\", \"webSearchUrlPingSuffix\": \"DevEx,5232.1\", \"totalEstimatedMatches\": 5, \"value\": [{}]}}";

            var webResponse = _webSearchBusinessLogic.SearchWebResourcesAsync(searchText);
            webResponse.Returns(ResponseMessage);

            //act
            var response = luisBusinessLogic.GetWebResourcesAsync(searchText).Result;

            //assert
            Assert.Contains("{\r\n  \"webResources\": \"[\\r\\n  {}\\r\\n]\"\r\n}", response);
        }

        [Fact]
        public void GetWebResourcesAsyncWithoutwebPageObject()
        {
            //arrange
            string searchText = "Access2Justice";
            var responseq = new HttpResponseMessage();
            var ResponseMessage = "{\"_type\": \"SearchResponse\", \"instrumentation\": {}, \"queryContext\": {\"originalQuery\": \"microsoft\"}}";

            var webResponse = _webSearchBusinessLogic.SearchWebResourcesAsync(searchText);
            webResponse.Returns(ResponseMessage);

            //act
            var response = luisBusinessLogic.GetWebResourcesAsync(searchText).Result;

            //assert
            Assert.Equal("{\r\n  \"webResources\": \"\"\r\n}", response);
        }

        [Fact]
        public void GetWebResourcesAsyncWithEmptyObject()
        {
            //arrange
            string searchText = "Access2Justice";
            var responseq = new HttpResponseMessage();
            var ResponseMessage = "";

            var webResponse = _webSearchBusinessLogic.SearchWebResourcesAsync(searchText);
            webResponse.Returns(ResponseMessage);

            //act
            var response = luisBusinessLogic.GetWebResourcesAsync(searchText).Result;

            //assert
            Assert.Equal("{\r\n  \"webResources\": \"\"\r\n}", response);
        }

        [Fact]
        public void GetInternalResourcesAsyncWithProperKeyword()
        {
            //arrange
            string keywords = "eviction";            
            JArray topicsData = JArray.Parse(@"[{'id':'addf41e9-1a27-4aeb-bcbb-7959f95094ba','name':'Family','parentTopicID':'','keywords':'eviction','location':[{'state':'Hawaii','county':'Kalawao County','city':'Kalawao','zipCode':'96742'},{'zipCode':'96741'},{'state':'Hawaii','county':'Honolulu County','city':'Honolulu'},{'state':'Hawaii','city':'Hawaiian Beaches'},{'state':'Hawaii','city':'Haiku-Pauwela'},{'state':'Alaska'}],'jsonContent':'','icon':'./assets/images/topics/topic14.png','createdBy':'','createdTimeStamp':'','modifiedBy':'','modifiedTimeStamp':'','_rid':'mwoSALHtpAEBAAAAAAAAAA==','_self':'dbs/mwoSAA==/colls/mwoSALHtpAE=/docs/mwoSALHtpAEBAAAAAAAAAA==/','_etag':'\'05008e57-0000-0000-0000-5b0797c10000\'','_attachments':'attachments/','_ts':1527224257},{'id':'3e278591-ec50-479d-8e38-ae9a9d4cabd9','name':'Eviction','parentTopicID':'','keywords':'eviction','location':[{'state':'Hawaii','county':'Kalawao County','city':'Kalawao','zipCode':'96742'},{'zipCode':'96741'},{'state':'Hawaii','county':'Honolulu County','city':'Honolulu'},{'state':'Hawaii','city':'Hawaiian Beaches'},{'state':'Hawaii','city':'Haiku-Pauwela'},{'state':'Alaska'}],'jsonContent':'','icon':'./assets/images/topics/topic14.png','createdBy':'','createdTimeStamp':'','modifiedBy':'','modifiedTimeStamp':'','_rid':'mwoSALHtpAEBAAAAAAAAAA==','_self':'dbs/mwoSAA==/colls/mwoSALHtpAE=/docs/mwoSALHtpAEBAAAAAAAAAA==/','_etag':'\'05008e57-0000-0000-0000-5b0797c10000\'','_attachments':'attachments/','_ts':1527224257},{'id':'3aa3a1be-8291-42b1-85c2-252f756febbc','name':'Housing','parentTopicID':'','keywords':'eviction','location':[{'state':'Hawaii','county':'Kalawao County','city':'Kalawao','zipCode':'96742'},{'zipCode':'96741'},{'state':'Hawaii','county':'Honolulu County','city':'Honolulu'},{'state':'Hawaii','city':'Hawaiian Beaches'},{'state':'Hawaii','city':'Haiku-Pauwela'},{'state':'Alaska'}],'jsonContent':'','icon':'./assets/images/topics/topic14.png','createdBy':'','createdTimeStamp':'','modifiedBy':'','modifiedTimeStamp':'','_rid':'mwoSALHtpAEBAAAAAAAAAA==','_self':'dbs/mwoSAA==/colls/mwoSALHtpAE=/docs/mwoSALHtpAEBAAAAAAAAAA==/','_etag':'\'05008e57-0000-0000-0000-5b0797c10000\'','_attachments':'attachments/','_ts':1527224257}]");
            JArray resourcesData = JArray.Parse(@"[{'id':'77d301e7-6df2-612e-4704-c04edf271806','name':'Tenant Action Plan for Eviction','description':'This action plan is for tenants who are facing eviction and have experienced the following:','resourceType':'Action','externalUrl':'','url':'','topicTags':[{'id':'f102bfae-362d-4659-aaef-956c391f79de'},{'id':'2c0cc7b8-62b1-4efb-8568-b1f767f879bc'},{'id':'3aa3a1be-8291-42b1-85c2-252f756febbc'}],'location':[{'state':'Hawaii','city':'Kalawao','zipCode':'96742'},{'zipCode':'96741'},{'state':'Hawaii','city':'Honolulu'},{'state':'Hawaii','city':'Hawaiian Beaches'},{'state':'Hawaii','city':'Haiku-Pauwela'},{'state':'Alaska'}],'icon':'./assets/images/resources/resource.png','createdBy':'','createdTimeStamp':'','modifiedBy':'','modifiedTimeStamp':'','_rid':'mwoSAJdNlwIBAAAAAAAAAA==','_self':'dbs/mwoSAA==/colls/mwoSAJdNlwI=/docs/mwoSAJdNlwIBAAAAAAAAAA==/','_etag':'\'040007b5-0000-0000-0000-5b0792260000\'','_attachments':'attachments/','_ts':1527222822},{'id':'19a02209-ca38-4b74-bd67-6ea941d41518','name':'Legal Help Organization','description':'This action plan is for tenants who are facing eviction and have experienced the following:','resourceType':'Organization','externalUrl':'','url':'','topicTags':[{'id':'f102bfae-362d-4659-aaef-956c391f79de'},{'id':'2c0cc7b8-62b1-4efb-8568-b1f767f879bc'},{'id':'3aa3a1be-8291-42b1-85c2-252f756febbc'}],'location':[{'state':'Hawaii','city':'Kalawao','zipCode':'96742'},{'zipCode':'96741'},{'state':'Hawaii','city':'Honolulu'},{'state':'Hawaii','city':'Hawaiian Beaches'},{'state':'Hawaii','city':'Haiku-Pauwela'},{'state':'Alaska'}],'icon':'./assets/images/resources/resource.png','createdBy':'','createdTimeStamp':'','modifiedBy':'','modifiedTimeStamp':'','_rid':'mwoSAJdNlwIBAAAAAAAAAA==','_self':'dbs/mwoSAA==/colls/mwoSAJdNlwI=/docs/mwoSAJdNlwIBAAAAAAAAAA==/','_etag':'\'040007b5-0000-0000-0000-5b0792260000\'','_attachments':'attachments/','_ts':1527222822},{'id':'2225cb29-2442-42ac-a4f5-9e88a7aabc4a','name':'Landlord Action Plan','description':'This action plan is for land lord who are facing eviction and have experienced the following:','resourceType':'Action','externalUrl':'','url':'','topicTags':[{'id':'f102bfae-362d-4659-aaef-956c391f79de'},{'id':'2c0cc7b8-62b1-4efb-8568-b1f767f879bc'},{'id':'3aa3a1be-8291-42b1-85c2-252f756febbc'}],'location':[{'state':'Hawaii','city':'Kalawao','zipCode':'96742'},{'zipCode':'96741'},{'state':'Hawaii','city':'Honolulu'},{'state':'Hawaii','city':'Hawaiian Beaches'},{'state':'Hawaii','city':'Haiku-Pauwela'},{'state':'Alaska'}],'icon':'./assets/images/resources/resource.png','createdBy':'','createdTimeStamp':'','modifiedBy':'','modifiedTimeStamp':'','_rid':'mwoSAJdNlwIBAAAAAAAAAA==','_self':'dbs/mwoSAA==/colls/mwoSAJdNlwI=/docs/mwoSAJdNlwIBAAAAAAAAAA==/','_etag':'\'040007b5-0000-0000-0000-5b0792260000\'','_attachments':'attachments/','_ts':1527222822}]");
                        
            _topicsResourcesBusinessLogic.GetTopicAsync(Arg.Any<string>()).Returns(topicsData);            

            _topicsResourcesBusinessLogic.GetResourcesAsync(Arg.Any<string>()).Returns(resourcesData);
            

            //act
            var result = luisBusinessLogic.GetInternalResourcesAsync(keywords).Result;


            //assert
            Assert.Contains("77d301e7-6df2-612e-4704-c04edf271806", result);
            Assert.DoesNotContain("_ts", result);
        }

        [Fact]
        public void GetInternalResourcesAsyncWithEmptyTopic()
        {
            //arrange
            string keywords = "eviction";
            JObject topicsData = JObject.Parse(@"{}");

            _topicsResourcesBusinessLogic.GetTopicAsync(Arg.Any<string>()).Returns(topicsData);

            //act
            var result = luisBusinessLogic.GetInternalResourcesAsync(keywords).Result;

            //assert
            Assert.Equal("{\r\n  \"topics\": \"\",\r\n  \"resources\": \"\"\r\n}", result);            
        }

        [Fact]
        public void GetInternalResourcesAsyncWithEmptyResource()
        {
            //arrange
            string keywords = "eviction";
            JArray topicsData = JArray.Parse(@"[{'id':'addf41e9-1a27-4aeb-bcbb-7959f95094ba','name':'Family','parentTopicID':'','keywords':'eviction','location':[{'state':'Hawaii','county':'Kalawao County','city':'Kalawao','zipCode':'96742'},{'zipCode':'96741'},{'state':'Hawaii','county':'Honolulu County','city':'Honolulu'},{'state':'Hawaii','city':'Hawaiian Beaches'},{'state':'Hawaii','city':'Haiku-Pauwela'},{'state':'Alaska'}],'jsonContent':'','icon':'./assets/images/topics/topic14.png','createdBy':'','createdTimeStamp':'','modifiedBy':'','modifiedTimeStamp':'','_rid':'mwoSALHtpAEBAAAAAAAAAA==','_self':'dbs/mwoSAA==/colls/mwoSALHtpAE=/docs/mwoSALHtpAEBAAAAAAAAAA==/','_etag':'\'05008e57-0000-0000-0000-5b0797c10000\'','_attachments':'attachments/','_ts':1527224257},{'id':'3e278591-ec50-479d-8e38-ae9a9d4cabd9','name':'Eviction','parentTopicID':'','keywords':'eviction','location':[{'state':'Hawaii','county':'Kalawao County','city':'Kalawao','zipCode':'96742'},{'zipCode':'96741'},{'state':'Hawaii','county':'Honolulu County','city':'Honolulu'},{'state':'Hawaii','city':'Hawaiian Beaches'},{'state':'Hawaii','city':'Haiku-Pauwela'},{'state':'Alaska'}],'jsonContent':'','icon':'./assets/images/topics/topic14.png','createdBy':'','createdTimeStamp':'','modifiedBy':'','modifiedTimeStamp':'','_rid':'mwoSALHtpAEBAAAAAAAAAA==','_self':'dbs/mwoSAA==/colls/mwoSALHtpAE=/docs/mwoSALHtpAEBAAAAAAAAAA==/','_etag':'\'05008e57-0000-0000-0000-5b0797c10000\'','_attachments':'attachments/','_ts':1527224257},{'id':'3aa3a1be-8291-42b1-85c2-252f756febbc','name':'Housing','parentTopicID':'','keywords':'eviction','location':[{'state':'Hawaii','county':'Kalawao County','city':'Kalawao','zipCode':'96742'},{'zipCode':'96741'},{'state':'Hawaii','county':'Honolulu County','city':'Honolulu'},{'state':'Hawaii','city':'Hawaiian Beaches'},{'state':'Hawaii','city':'Haiku-Pauwela'},{'state':'Alaska'}],'jsonContent':'','icon':'./assets/images/topics/topic14.png','createdBy':'','createdTimeStamp':'','modifiedBy':'','modifiedTimeStamp':'','_rid':'mwoSALHtpAEBAAAAAAAAAA==','_self':'dbs/mwoSAA==/colls/mwoSALHtpAE=/docs/mwoSALHtpAEBAAAAAAAAAA==/','_etag':'\'05008e57-0000-0000-0000-5b0797c10000\'','_attachments':'attachments/','_ts':1527224257}]");
            JObject resourcesData = JObject.Parse(@"{}");            

            _topicsResourcesBusinessLogic.GetTopicAsync(Arg.Any<string>()).Returns(topicsData);

            _topicsResourcesBusinessLogic.GetResourcesAsync(Arg.Any<string>()).Returns(resourcesData);

            //act
            var result = luisBusinessLogic.GetInternalResourcesAsync(keywords).Result;

            //assert
            Assert.Contains("addf41e9-1a27-4aeb-bcbb-7959f95094ba", result);
            Assert.Contains("resources\": \"\"\r\n}", result);
        }

        [Fact]
        public void GetInternalResourcesAsyncWithTopicResource()
        {
            //arrange
            string keywords = "eviction";
            JArray topicsData = JArray.Parse(@"[{'id':'addf41e9-1a27-4aeb-bcbb-7959f95094ba','name':'Family','parentTopicID':'','keywords':'eviction','location':[{'state':'Hawaii','county':'Kalawao County','city':'Kalawao','zipCode':'96742'},{'zipCode':'96741'},{'state':'Hawaii','county':'Honolulu County','city':'Honolulu'},{'state':'Hawaii','city':'Hawaiian Beaches'},{'state':'Hawaii','city':'Haiku-Pauwela'},{'state':'Alaska'}],'jsonContent':'','icon':'./assets/images/topics/topic14.png','createdBy':'','createdTimeStamp':'','modifiedBy':'','modifiedTimeStamp':'','_rid':'mwoSALHtpAEBAAAAAAAAAA==','_self':'dbs/mwoSAA==/colls/mwoSALHtpAE=/docs/mwoSALHtpAEBAAAAAAAAAA==/','_etag':'\'05008e57-0000-0000-0000-5b0797c10000\'','_attachments':'attachments/','_ts':1527224257}]");
            JArray resourcesData = JArray.Parse(@"[{'id':'77d301e7-6df2-612e-4704-c04edf271806','name':'Tenant Action Plan for Eviction','description':'This action plan is for tenants who are facing eviction and have experienced the following:','resourceType':'Action','externalUrl':'','url':'','topicTags':[{'id':'f102bfae-362d-4659-aaef-956c391f79de'},{'id':'2c0cc7b8-62b1-4efb-8568-b1f767f879bc'},{'id':'3aa3a1be-8291-42b1-85c2-252f756febbc'}],'location':[{'state':'Hawaii','city':'Kalawao','zipCode':'96742'},{'zipCode':'96741'},{'state':'Hawaii','city':'Honolulu'},{'state':'Hawaii','city':'Hawaiian Beaches'},{'state':'Hawaii','city':'Haiku-Pauwela'},{'state':'Alaska'}],'icon':'./assets/images/resources/resource.png','createdBy':'','createdTimeStamp':'','modifiedBy':'','modifiedTimeStamp':'','_rid':'mwoSAJdNlwIBAAAAAAAAAA==','_self':'dbs/mwoSAA==/colls/mwoSAJdNlwI=/docs/mwoSAJdNlwIBAAAAAAAAAA==/','_etag':'\'040007b5-0000-0000-0000-5b0792260000\'','_attachments':'attachments/','_ts':1527222822}]");

            _topicsResourcesBusinessLogic.GetTopicAsync(Arg.Any<string>()).Returns(topicsData);

            _topicsResourcesBusinessLogic.GetResourcesAsync(Arg.Any<string>()).Returns(resourcesData);

            //act
            var result = luisBusinessLogic.GetInternalResourcesAsync(keywords).Result;

            //assert
            Assert.Contains("addf41e9-1a27-4aeb-bcbb-7959f95094ba", result);
            Assert.Contains("77d301e7-6df2-612e-4704-c04edf271806", result);
        }                

        [Fact]
        public void GetResourceBasedOnThresholdAsyncWithLowScore()
        {
            string searchText = "i'm getting kicked out";
            var luisResponse = _luisProxy.GetIntents(searchText);
            luisResponse.Returns("{\r\n  \"query\": \"eviction\",\r\n  \"topScoringIntent\": {\r\n    \"intent\": \"Eviction\",\r\n    \"score\": 0.37\r\n  },\r\n  \"intents\": [\r\n    {\r\n      \"intent\": \"Eviction\",\r\n      \"score\": 0.998598337\r\n    },\r\n    {\r\n      \"intent\": \"None\",\r\n      \"score\": 0.3924698\r\n    },\r\n    {\r\n      \"intent\": \"Small Claims Court\",\r\n      \"score\": 0.374458015\r\n    },\r\n    {\r\n      \"intent\": \"Mobile home park tenants\",\r\n      \"score\": 0.352736056\r\n    },\r\n    {\r\n      \"intent\": \"Going to court\",\r\n      \"score\": 0.131461561\r\n    },\r\n    {\r\n      \"intent\": \"Domestic Violence\",\r\n      \"score\": 0.0386172235\r\n    },\r\n    {\r\n      \"intent\": \"Separation\",\r\n      \"score\": 0.035968408\r\n    },\r\n    {\r\n      \"intent\": \"Foreclosure\",\r\n      \"score\": 0.0271484256\r\n    },\r\n    {\r\n      \"intent\": \"division of property\",\r\n      \"score\": 0.0246635266\r\n    },\r\n    {\r\n      \"intent\": \"Child support\",\r\n      \"score\": 0.0197562873\r\n    },\r\n    {\r\n      \"intent\": \"child custody\",\r\n      \"score\": 0.0193109587\r\n    },\r\n    {\r\n      \"intent\": \"Other Consumer\",\r\n      \"score\": 0.0174270831\r\n    },\r\n    {\r\n      \"intent\": \"Unemployment\",\r\n      \"score\": 0.0172820911\r\n    },\r\n    {\r\n      \"intent\": \"Guardianship\",\r\n      \"score\": 0.0149135459\r\n    },\r\n    {\r\n      \"intent\": \"Consumer Fraud\",\r\n      \"score\": 0.0109810177\r\n    },\r\n    {\r\n      \"intent\": \"Contracts\",\r\n      \"score\": 0.0105354656\r\n    },\r\n    {\r\n      \"intent\": \"Elder Abuse\",\r\n      \"score\": 0.0100716464\r\n    },\r\n    {\r\n      \"intent\": \"Divorce\",\r\n      \"score\": 0.009231578\r\n    },\r\n    {\r\n      \"intent\": \"Managing money\",\r\n      \"score\": 0.00913277548\r\n    },\r\n    {\r\n      \"intent\": \"Sexual Assault\",\r\n      \"score\": 0.007721106\r\n    },\r\n    {\r\n      \"intent\": \"Home buyers & owners\",\r\n      \"score\": 0.00558177941\r\n    },\r\n    {\r\n      \"intent\": \"Marriage equality\",\r\n      \"score\": 0.005541543\r\n    },\r\n    {\r\n      \"intent\": \"Utilities & phones\",\r\n      \"score\": 0.00534641044\r\n    },\r\n    {\r\n      \"intent\": \"Public & subsidized housing\",\r\n      \"score\": 0.005005484\r\n    },\r\n    {\r\n      \"intent\": \"Contempt of court\",\r\n      \"score\": 0.00479589263\r\n    },\r\n    {\r\n      \"intent\": \"Tenant's rights\",\r\n      \"score\": 0.00391642563\r\n    },\r\n    {\r\n      \"intent\": \"marriage\",\r\n      \"score\": 0.00364745827\r\n    },\r\n    {\r\n      \"intent\": \"The child protection system\",\r\n      \"score\": 0.00345990737\r\n    },\r\n    {\r\n      \"intent\": \"Legal financial obligation\",\r\n      \"score\": 0.00318646478\r\n    },\r\n    {\r\n      \"intent\": \"Custody\",\r\n      \"score\": 0.002302651\r\n    },\r\n    {\r\n      \"intent\": \"Car issues\",\r\n      \"score\": 0.00218277727\r\n    },\r\n    {\r\n      \"intent\": \"Emergency shelter & assistance\",\r\n      \"score\": 0.00181414338\r\n    },\r\n    {\r\n      \"intent\": \"Driver & Professional licenses\",\r\n      \"score\": 0.00177037634\r\n    },\r\n    {\r\n      \"intent\": \"Employment Discrimination\",\r\n      \"score\": 0.00176623627\r\n    },\r\n    {\r\n      \"intent\": \"Credit problems\",\r\n      \"score\": 0.00128342363\r\n    },\r\n    {\r\n      \"intent\": \"alimony\",\r\n      \"score\": 0.00127891626\r\n    },\r\n    {\r\n      \"intent\": \"Parenting Plans/Custody\",\r\n      \"score\": 0.0012039718\r\n    },\r\n    {\r\n      \"intent\": \"Adoption\",\r\n      \"score\": 0.00105760642\r\n    },\r\n    {\r\n      \"intent\": \"Paternity/Parentage\",\r\n      \"score\": 0.00103898533\r\n    },\r\n    {\r\n      \"intent\": \"Divorce Contested\",\r\n      \"score\": 0.00100662454\r\n    },\r\n    {\r\n      \"intent\": \"Debt\",\r\n      \"score\": 0.0007743759\r\n    },\r\n    {\r\n      \"intent\": \"Identity\",\r\n      \"score\": 0.000727297564\r\n    },\r\n    {\r\n      \"intent\": \"Divorce Legal Separation\",\r\n      \"score\": 0.000590754149\r\n    },\r\n    {\r\n      \"intent\": \"Utilities and telecommunications\",\r\n      \"score\": 0.000529284531\r\n    },\r\n    {\r\n      \"intent\": \"Medical bills\",\r\n      \"score\": 0.000471992535\r\n    },\r\n    {\r\n      \"intent\": \"Student loans\",\r\n      \"score\": 0.0004546414\r\n    },\r\n    {\r\n      \"intent\": \"Divorce Annulment\",\r\n      \"score\": 0.000423705118\r\n    },\r\n    {\r\n      \"intent\": \"Other family\",\r\n      \"score\": 0.00041898893\r\n    },\r\n    {\r\n      \"intent\": \"Divorce Uncontested\",\r\n      \"score\": 0.0003876439\r\n    },\r\n    {\r\n      \"intent\": \"Unmarried couples\",\r\n      \"score\": 0.000380990357\r\n    },\r\n    {\r\n      \"intent\": \"More family court procedures\",\r\n      \"score\": 0.000221170354\r\n    },\r\n    {\r\n      \"intent\": \"Housing discrimination\",\r\n      \"score\": 0.00018730732\r\n    },\r\n    {\r\n      \"intent\": \"Veteran/Military Families\",\r\n      \"score\": 0.000181092473\r\n    },\r\n    {\r\n      \"intent\": \"Bankrucptcy\",\r\n      \"score\": 0.000131298162\r\n    },\r\n    {\r\n      \"intent\": \"Non-parents caring for children\",\r\n      \"score\": 7.45657E-05\r\n    },\r\n    {\r\n      \"intent\": \"Relocation\",\r\n      \"score\": 6.903267E-05\r\n    },\r\n    {\r\n      \"intent\": \"Non-parent custody\",\r\n      \"score\": 1.08726072E-05\r\n    }\r\n  ],\r\n  \"entities\": []\r\n}");

            var webResponse = _luisBusinessLogic.GetWebResourcesAsync(searchText);
            webResponse.Returns("{\r\n  \"webResources\": \"[\\r\\n  {\\r\\n    \\\"id\\\": \\\"https://api.cognitive.microsoft.com/api/v7/#WebPages.0\\\",\\r\\n    \\\"name\\\": \\\"Offer - Indian Contract Act, 1872 - Lawnotes.in\\\",\\r\\n    \\\"url\\\": \\\"http://www.lawnotes.in/Offer_-_Indian_Contract_Act,_1872\\\",\\r\\n    \\\"urlPingSuffix\\\": \\\"DevEx,5072.1\\\",\\r\\n    \\\"about\\\": [\\r\\n      {\\r\\n        \\\"name\\\": \\\"Indian Contract Act, 1872\\\"\\r\\n      }\\r\\n    ],\\r\\n    \\\"isFamilyFriendly\\\": true,\\r\\n    \\\"displayUrl\\\": \\\"www.lawnotes.in/Offer_-_Indian_Contract_Act,_1872\\\",\\r\\n    \\\"snippet\\\": \\\"Main Article : Indian Contract Act, 1872. Offer is one of the essential elements of a contract as defined in Section 10 of the Indian Contract Act, 1872.\\\",\\r\\n    \\\"deepLinks\\\": [\\r\\n      {\\r\\n        \\\"name\\\": \\\"Section 245 of Income-Tax Act, 1961\\\",\\r\\n        \\\"url\\\": \\\"http://www.lawnotes.in/Section_245_of_Income-Tax_Act,_1961\\\",\\r\\n        \\\"urlPingSuffix\\\": \\\"DevEx,5063.1\\\",\\r\\n        \\\"snippet\\\": \\\"Section 245 of Income-Tax Act, 1961 deals with the topic of Set off of refunds against tax remaining payable\\\"\\r\\n      },\\r\\n      {\\r\\n        \\\"name\\\": \\\"11-CV-01846-LHK\\\",\\r\\n        \\\"url\\\": \\\"https://www.lawnotes.in/Apple_Inc_vs_Samsung_Electronics_Co_Ltd_et_al,_No._11-CV-01846-LHK\\\",\\r\\n        \\\"urlPingSuffix\\\": \\\"DevEx,5064.1\\\",\\r\\n        \\\"snippet\\\": \\\"Apple Inc vs Samsung Electronics Co Ltd et al, No. 11-CV-01846-LHK. From Lawnotes.in. Jump to:navigation, search. UNITED STATES DISTRICT COURT NORTHERN DISTRICT OF CALIFORNIA SAN JOSE DIVISION Case No.: 11-CV-01846-LHK APPLE, INC., a California corporation, Plaintiff and Counterdefendant, v. ... Uniloc USA, Inc. v. Microsoft Corp., 632 F.3d 1292, 1302 (Fed. Cir. 2011): ...\\\"\\r\\n      },\\r\\n      {\\r\\n        \\\"name\\\": \\\"Ramesh Chandra Shah And Others v Anil Joshi And Others - Lawnotes.\\\",\\r\\n        \\\"url\\\": \\\"https://www.lawnotes.in/Ramesh_Chandra_Shah_and_others_v_Anil_Joshi_and_others\\\",\\r\\n        \\\"urlPingSuffix\\\": \\\"DevEx,5065.1\\\",\\r\\n        \\\"snippet\\\": \\\"Ramesh Chandra Shah and others … Appellants versus Anil Joshi and others … Respondents J U D G M E N T ... with regard to basic knowledge of computer operation would be tested at the time of interview for which knowledge of Microsoft Operating System and Microsoft Office operation would be essential. In the call letter also which was sent to the appellant at the time of calling him for interview, ...\\\"\\r\\n      }\\r\\n    ],\\r\\n    \\\"dateLastCrawled\\\": \\\"2018-05-19T11:27:00Z\\\",\\r\\n    \\\"fixedPosition\\\": false,\\r\\n    \\\"language\\\": \\\"en\\\"\\r\\n  },\\r\\n  {\\r\\n    \\\"id\\\": \\\"https://api.cognitive.microsoft.com/api/v7/#WebPages.1\\\",\\r\\n    \\\"name\\\": \\\"Book Review- Balance of Justice\\\",\\r\\n    \\\"url\\\": \\\"http://accesstojustice-ng.org/Review_of_the_Balance_of_Justice.pdf\\\",\\r\\n    \\\"urlPingSuffix\\\": \\\"DevEx,5085.1\\\",\\r\\n    \\\"isFamilyFriendly\\\": true,\\r\\n    \\\"displayUrl\\\": \\\"accesstojustice-ng.org/Review_of_the_Balance_of_Justice.pdf\\\",\\r\\n    \\\"snippet\\\": \\\"Access to Justice’ Report Put Federal High Court Judges in the Performance Spotlight The Report The Balance of Justice is a performance-related\\\",\\r\\n    \\\"dateLastCrawled\\\": \\\"2018-04-07T12:11:00Z\\\",\\r\\n    \\\"fixedPosition\\\": false,\\r\\n    \\\"language\\\": \\\"en\\\"\\r\\n  }\\r\\n]\"\r\n}");

            var result = luisBusinessLogic.GetResourceBasedOnThresholdAsync(searchText).Result;


        }

        [Fact]
        public void GetResourceBasedOnThresholdAsyncWithMediumScore()
        {
            //arrange
            string searchText = "i'm getting kicked out";            
            var luisResponse = _luisProxy.GetIntents(searchText);
            luisResponse.Returns("{\r\n  \"query\": \"eviction\",\r\n  \"topScoringIntent\": {\r\n    \"intent\": \"Eviction\",\r\n    \"score\": 0.78\r\n  },\r\n  \"intents\": [\r\n    {\r\n      \"intent\": \"Eviction\",\r\n      \"score\": 0.998598337\r\n    },\r\n    {\r\n      \"intent\": \"None\",\r\n      \"score\": 0.3924698\r\n    },\r\n    {\r\n      \"intent\": \"Small Claims Court\",\r\n      \"score\": 0.374458015\r\n    },\r\n    {\r\n      \"intent\": \"Mobile home park tenants\",\r\n      \"score\": 0.352736056\r\n    },\r\n    {\r\n      \"intent\": \"Going to court\",\r\n      \"score\": 0.131461561\r\n    },\r\n    {\r\n      \"intent\": \"Domestic Violence\",\r\n      \"score\": 0.0386172235\r\n    },\r\n    {\r\n      \"intent\": \"Separation\",\r\n      \"score\": 0.035968408\r\n    },\r\n    {\r\n      \"intent\": \"Foreclosure\",\r\n      \"score\": 0.0271484256\r\n    },\r\n    {\r\n      \"intent\": \"division of property\",\r\n      \"score\": 0.0246635266\r\n    },\r\n    {\r\n      \"intent\": \"Child support\",\r\n      \"score\": 0.0197562873\r\n    },\r\n    {\r\n      \"intent\": \"child custody\",\r\n      \"score\": 0.0193109587\r\n    },\r\n    {\r\n      \"intent\": \"Other Consumer\",\r\n      \"score\": 0.0174270831\r\n    },\r\n    {\r\n      \"intent\": \"Unemployment\",\r\n      \"score\": 0.0172820911\r\n    },\r\n    {\r\n      \"intent\": \"Guardianship\",\r\n      \"score\": 0.0149135459\r\n    },\r\n    {\r\n      \"intent\": \"Consumer Fraud\",\r\n      \"score\": 0.0109810177\r\n    },\r\n    {\r\n      \"intent\": \"Contracts\",\r\n      \"score\": 0.0105354656\r\n    },\r\n    {\r\n      \"intent\": \"Elder Abuse\",\r\n      \"score\": 0.0100716464\r\n    },\r\n    {\r\n      \"intent\": \"Divorce\",\r\n      \"score\": 0.009231578\r\n    },\r\n    {\r\n      \"intent\": \"Managing money\",\r\n      \"score\": 0.00913277548\r\n    },\r\n    {\r\n      \"intent\": \"Sexual Assault\",\r\n      \"score\": 0.007721106\r\n    },\r\n    {\r\n      \"intent\": \"Home buyers & owners\",\r\n      \"score\": 0.00558177941\r\n    },\r\n    {\r\n      \"intent\": \"Marriage equality\",\r\n      \"score\": 0.005541543\r\n    },\r\n    {\r\n      \"intent\": \"Utilities & phones\",\r\n      \"score\": 0.00534641044\r\n    },\r\n    {\r\n      \"intent\": \"Public & subsidized housing\",\r\n      \"score\": 0.005005484\r\n    },\r\n    {\r\n      \"intent\": \"Contempt of court\",\r\n      \"score\": 0.00479589263\r\n    },\r\n    {\r\n      \"intent\": \"Tenant's rights\",\r\n      \"score\": 0.00391642563\r\n    },\r\n    {\r\n      \"intent\": \"marriage\",\r\n      \"score\": 0.00364745827\r\n    },\r\n    {\r\n      \"intent\": \"The child protection system\",\r\n      \"score\": 0.00345990737\r\n    },\r\n    {\r\n      \"intent\": \"Legal financial obligation\",\r\n      \"score\": 0.00318646478\r\n    },\r\n    {\r\n      \"intent\": \"Custody\",\r\n      \"score\": 0.002302651\r\n    },\r\n    {\r\n      \"intent\": \"Car issues\",\r\n      \"score\": 0.00218277727\r\n    },\r\n    {\r\n      \"intent\": \"Emergency shelter & assistance\",\r\n      \"score\": 0.00181414338\r\n    },\r\n    {\r\n      \"intent\": \"Driver & Professional licenses\",\r\n      \"score\": 0.00177037634\r\n    },\r\n    {\r\n      \"intent\": \"Employment Discrimination\",\r\n      \"score\": 0.00176623627\r\n    },\r\n    {\r\n      \"intent\": \"Credit problems\",\r\n      \"score\": 0.00128342363\r\n    },\r\n    {\r\n      \"intent\": \"alimony\",\r\n      \"score\": 0.00127891626\r\n    },\r\n    {\r\n      \"intent\": \"Parenting Plans/Custody\",\r\n      \"score\": 0.0012039718\r\n    },\r\n    {\r\n      \"intent\": \"Adoption\",\r\n      \"score\": 0.00105760642\r\n    },\r\n    {\r\n      \"intent\": \"Paternity/Parentage\",\r\n      \"score\": 0.00103898533\r\n    },\r\n    {\r\n      \"intent\": \"Divorce Contested\",\r\n      \"score\": 0.00100662454\r\n    },\r\n    {\r\n      \"intent\": \"Debt\",\r\n      \"score\": 0.0007743759\r\n    },\r\n    {\r\n      \"intent\": \"Identity\",\r\n      \"score\": 0.000727297564\r\n    },\r\n    {\r\n      \"intent\": \"Divorce Legal Separation\",\r\n      \"score\": 0.000590754149\r\n    },\r\n    {\r\n      \"intent\": \"Utilities and telecommunications\",\r\n      \"score\": 0.000529284531\r\n    },\r\n    {\r\n      \"intent\": \"Medical bills\",\r\n      \"score\": 0.000471992535\r\n    },\r\n    {\r\n      \"intent\": \"Student loans\",\r\n      \"score\": 0.0004546414\r\n    },\r\n    {\r\n      \"intent\": \"Divorce Annulment\",\r\n      \"score\": 0.000423705118\r\n    },\r\n    {\r\n      \"intent\": \"Other family\",\r\n      \"score\": 0.00041898893\r\n    },\r\n    {\r\n      \"intent\": \"Divorce Uncontested\",\r\n      \"score\": 0.0003876439\r\n    },\r\n    {\r\n      \"intent\": \"Unmarried couples\",\r\n      \"score\": 0.000380990357\r\n    },\r\n    {\r\n      \"intent\": \"More family court procedures\",\r\n      \"score\": 0.000221170354\r\n    },\r\n    {\r\n      \"intent\": \"Housing discrimination\",\r\n      \"score\": 0.00018730732\r\n    },\r\n    {\r\n      \"intent\": \"Veteran/Military Families\",\r\n      \"score\": 0.000181092473\r\n    },\r\n    {\r\n      \"intent\": \"Bankrucptcy\",\r\n      \"score\": 0.000131298162\r\n    },\r\n    {\r\n      \"intent\": \"Non-parents caring for children\",\r\n      \"score\": 7.45657E-05\r\n    },\r\n    {\r\n      \"intent\": \"Relocation\",\r\n      \"score\": 6.903267E-05\r\n    },\r\n    {\r\n      \"intent\": \"Non-parent custody\",\r\n      \"score\": 1.08726072E-05\r\n    }\r\n  ],\r\n  \"entities\": []\r\n}");
            
            //act
            var response = luisBusinessLogic.GetResourceBasedOnThresholdAsync(searchText).Result;

            //assert
            Assert.Contains("luisResponse", response);
        }

        //[Fact]
        //public void GetResourceBasedOnThresholdAsyncTest()
        //{
        //    string query = "i'm getting kicked out";
        //    var internalResponse = "{\r\n  \"topics\": \"[\\r\\n  {\\r\\n    \\\"id\\\": \\\"addf41e9-1a27-4aeb-bcbb-7959f95094ba\\\",\\r\\n    \\\"name\\\": \\\"Family\\\",\\r\\n    \\\"parentTopicID\\\": \\\"\\\",\\r\\n    \\\"keywords\\\": \\\"eviction\\\",\\r\\n    \\\"location\\\": [\\r\\n      {\\r\\n        \\\"state\\\": \\\"Hawaii\\\",\\r\\n        \\\"county\\\": \\\"Kalawao County\\\",\\r\\n        \\\"city\\\": \\\"Kalawao\\\",\\r\\n        \\\"zipCode\\\": \\\"96742\\\"\\r\\n      },\\r\\n      {\\r\\n        \\\"zipCode\\\": \\\"96741\\\"\\r\\n      },\\r\\n      {\\r\\n        \\\"state\\\": \\\"Hawaii\\\",\\r\\n        \\\"county\\\": \\\"Honolulu County\\\",\\r\\n        \\\"city\\\": \\\"Honolulu\\\"\\r\\n      },\\r\\n      {\\r\\n        \\\"state\\\": \\\"Hawaii\\\",\\r\\n        \\\"city\\\": \\\"Hawaiian Beaches\\\"\\r\\n      },\\r\\n      {\\r\\n        \\\"state\\\": \\\"Hawaii\\\",\\r\\n        \\\"city\\\": \\\"Haiku-Pauwela\\\"\\r\\n      },\\r\\n      {\\r\\n        \\\"state\\\": \\\"Alaska\\\"\\r\\n      }\\r\\n    ],\\r\\n    \\\"jsonContent\\\": \\\"\\\",\\r\\n    \\\"icon\\\": \\\"./assets/images/topics/topic14.png\\\",\\r\\n    \\\"createdBy\\\": \\\"\\\",\\r\\n    \\\"createdTimeStamp\\\": \\\"\\\",\\r\\n    \\\"modifiedBy\\\": \\\"\\\",\\r\\n    \\\"modifiedTimeStamp\\\": \\\"\\\"\\r\\n  },\\r\\n  {\\r\\n    \\\"id\\\": \\\"3aa3a1be-8291-42b1-85c2-252f756febbc\\\",\\r\\n    \\\"name\\\": \\\"Housing\\\",\\r\\n    \\\"parentTopicID\\\": \\\"\\\",\\r\\n    \\\"keywords\\\": \\\"eviction|housing|tenant\\\",\\r\\n    \\\"location\\\": [\\r\\n      {\\r\\n        \\\"state\\\": \\\"Hawaii\\\"\\r\\n      },\\r\\n      {\\r\\n        \\\"state\\\": \\\"Alaska\\\"\\r\\n      }\\r\\n    ],\\r\\n    \\\"jsonContent\\\": \\\"\\\",\\r\\n    \\\"icon\\\": \\\"./assets/images/topics/topic14.png\\\",\\r\\n    \\\"createdBy\\\": \\\"\\\",\\r\\n    \\\"createdTimeStamp\\\": \\\"\\\",\\r\\n    \\\"modifiedBy\\\": \\\"\\\",\\r\\n    \\\"modifiedTimeStamp\\\": \\\"\\\"\\r\\n  },\\r\\n  {\\r\\n    \\\"id\\\": \\\"3e278591-ec50-479d-8e38-ae9a9d4cabd9\\\",\\r\\n    \\\"name\\\": \\\"Eviction\\\",\\r\\n    \\\"parentTopicID\\\": \\\"3aa3a1be-8291-42b1-85c2-252f756febbc\\\",\\r\\n    \\\"keywords\\\": \\\"eviction|kicked out|children\\\",\\r\\n    \\\"location\\\": [\\r\\n      {\\r\\n        \\\"state\\\": \\\"Hawaii\\\",\\r\\n        \\\"city\\\": \\\"Kalawao\\\"\\r\\n      },\\r\\n      {\\r\\n        \\\"zipCode\\\": \\\"96741\\\"\\r\\n      },\\r\\n      {\\r\\n        \\\"state\\\": \\\"Hawaii\\\",\\r\\n        \\\"city\\\": \\\"Honolulu\\\"\\r\\n      },\\r\\n      {\\r\\n        \\\"state\\\": \\\"Hawaii\\\",\\r\\n        \\\"city\\\": \\\"Hawaiian Beaches\\\"\\r\\n      },\\r\\n      {\\r\\n        \\\"state\\\": \\\"Hawaii\\\",\\r\\n        \\\"city\\\": \\\"Haiku-Pauwela\\\"\\r\\n      },\\r\\n      {\\r\\n        \\\"state\\\": \\\"Alaska\\\"\\r\\n      }\\r\\n    ],\\r\\n    \\\"jsonContent\\\": \\\"\\\",\\r\\n    \\\"icon\\\": \\\"./assets/images/topics/topic14.png\\\",\\r\\n    \\\"createdBy\\\": \\\"\\\",\\r\\n    \\\"createdTimeStamp\\\": \\\"\\\",\\r\\n    \\\"modifiedBy\\\": \\\"\\\",\\r\\n    \\\"modifiedTimeStamp\\\": \\\"\\\"\\r\\n  }\\r\\n]\",\r\n  \"resources\": \"[\\r\\n  {\\r\\n    \\\"id\\\": \\\"77d301e7-6df2-612e-4704-c04edf271806\\\",\\r\\n    \\\"name\\\": \\\"Tenant Action Plan for Eviction\\\",\\r\\n    \\\"description\\\": \\\"This action plan is for tenants who are facing eviction and have experienced the following:\\\",\\r\\n    \\\"resourceType\\\": \\\"Action\\\",\\r\\n    \\\"externalUrl\\\": \\\"\\\",\\r\\n    \\\"url\\\": \\\"\\\",\\r\\n    \\\"topicTags\\\": [\\r\\n      {\\r\\n        \\\"id\\\": \\\"addf41e9-1a27-4aeb-bcbb-7959f95094ba\\\"\\r\\n      },\\r\\n      {\\r\\n        \\\"id\\\": \\\"2c0cc7b8-62b1-4efb-8568-b1f767f879bc\\\"\\r\\n      },\\r\\n      {\\r\\n        \\\"id\\\": \\\"3aa3a1be-8291-42b1-85c2-252f756febbc\\\"\\r\\n      }\\r\\n    ],\\r\\n    \\\"location\\\": \\\"Hawaii, Honolulu, 96812 |Hawaii, Hawaiian Beaches |Hawaii, Haiku-Pauwela | Alaska\\\",\\r\\n    \\\"icon\\\": \\\"./assets/images/resources/resource.png\\\",\\r\\n    \\\"createdBy\\\": \\\"\\\",\\r\\n    \\\"createdTimeStamp\\\": \\\"\\\",\\r\\n    \\\"modifiedBy\\\": \\\"\\\",\\r\\n    \\\"modifiedTimeStamp\\\": \\\"\\\"\\r\\n  }\\r\\n]\"\r\n}";

        //    var luisResponse = _luisProxy.GetIntents(query);
        //    luisResponse.Returns("{\r\n  \"query\": \"eviction\",\r\n  \"topScoringIntent\": {\r\n    \"intent\": \"Eviction\",\r\n    \"score\": 0.998598337\r\n  },\r\n  \"intents\": [\r\n    {\r\n      \"intent\": \"Eviction\",\r\n      \"score\": 0.998598337\r\n    },\r\n    {\r\n      \"intent\": \"None\",\r\n      \"score\": 0.3924698\r\n    },\r\n    {\r\n      \"intent\": \"Small Claims Court\",\r\n      \"score\": 0.374458015\r\n    },\r\n    {\r\n      \"intent\": \"Mobile home park tenants\",\r\n      \"score\": 0.352736056\r\n    },\r\n    {\r\n      \"intent\": \"Going to court\",\r\n      \"score\": 0.131461561\r\n    },\r\n    {\r\n      \"intent\": \"Domestic Violence\",\r\n      \"score\": 0.0386172235\r\n    },\r\n    {\r\n      \"intent\": \"Separation\",\r\n      \"score\": 0.035968408\r\n    },\r\n    {\r\n      \"intent\": \"Foreclosure\",\r\n      \"score\": 0.0271484256\r\n    },\r\n    {\r\n      \"intent\": \"division of property\",\r\n      \"score\": 0.0246635266\r\n    },\r\n    {\r\n      \"intent\": \"Child support\",\r\n      \"score\": 0.0197562873\r\n    },\r\n    {\r\n      \"intent\": \"child custody\",\r\n      \"score\": 0.0193109587\r\n    },\r\n    {\r\n      \"intent\": \"Other Consumer\",\r\n      \"score\": 0.0174270831\r\n    },\r\n    {\r\n      \"intent\": \"Unemployment\",\r\n      \"score\": 0.0172820911\r\n    },\r\n    {\r\n      \"intent\": \"Guardianship\",\r\n      \"score\": 0.0149135459\r\n    },\r\n    {\r\n      \"intent\": \"Consumer Fraud\",\r\n      \"score\": 0.0109810177\r\n    },\r\n    {\r\n      \"intent\": \"Contracts\",\r\n      \"score\": 0.0105354656\r\n    },\r\n    {\r\n      \"intent\": \"Elder Abuse\",\r\n      \"score\": 0.0100716464\r\n    },\r\n    {\r\n      \"intent\": \"Divorce\",\r\n      \"score\": 0.009231578\r\n    },\r\n    {\r\n      \"intent\": \"Managing money\",\r\n      \"score\": 0.00913277548\r\n    },\r\n    {\r\n      \"intent\": \"Sexual Assault\",\r\n      \"score\": 0.007721106\r\n    },\r\n    {\r\n      \"intent\": \"Home buyers & owners\",\r\n      \"score\": 0.00558177941\r\n    },\r\n    {\r\n      \"intent\": \"Marriage equality\",\r\n      \"score\": 0.005541543\r\n    },\r\n    {\r\n      \"intent\": \"Utilities & phones\",\r\n      \"score\": 0.00534641044\r\n    },\r\n    {\r\n      \"intent\": \"Public & subsidized housing\",\r\n      \"score\": 0.005005484\r\n    },\r\n    {\r\n      \"intent\": \"Contempt of court\",\r\n      \"score\": 0.00479589263\r\n    },\r\n    {\r\n      \"intent\": \"Tenant's rights\",\r\n      \"score\": 0.00391642563\r\n    },\r\n    {\r\n      \"intent\": \"marriage\",\r\n      \"score\": 0.00364745827\r\n    },\r\n    {\r\n      \"intent\": \"The child protection system\",\r\n      \"score\": 0.00345990737\r\n    },\r\n    {\r\n      \"intent\": \"Legal financial obligation\",\r\n      \"score\": 0.00318646478\r\n    },\r\n    {\r\n      \"intent\": \"Custody\",\r\n      \"score\": 0.002302651\r\n    },\r\n    {\r\n      \"intent\": \"Car issues\",\r\n      \"score\": 0.00218277727\r\n    },\r\n    {\r\n      \"intent\": \"Emergency shelter & assistance\",\r\n      \"score\": 0.00181414338\r\n    },\r\n    {\r\n      \"intent\": \"Driver & Professional licenses\",\r\n      \"score\": 0.00177037634\r\n    },\r\n    {\r\n      \"intent\": \"Employment Discrimination\",\r\n      \"score\": 0.00176623627\r\n    },\r\n    {\r\n      \"intent\": \"Credit problems\",\r\n      \"score\": 0.00128342363\r\n    },\r\n    {\r\n      \"intent\": \"alimony\",\r\n      \"score\": 0.00127891626\r\n    },\r\n    {\r\n      \"intent\": \"Parenting Plans/Custody\",\r\n      \"score\": 0.0012039718\r\n    },\r\n    {\r\n      \"intent\": \"Adoption\",\r\n      \"score\": 0.00105760642\r\n    },\r\n    {\r\n      \"intent\": \"Paternity/Parentage\",\r\n      \"score\": 0.00103898533\r\n    },\r\n    {\r\n      \"intent\": \"Divorce Contested\",\r\n      \"score\": 0.00100662454\r\n    },\r\n    {\r\n      \"intent\": \"Debt\",\r\n      \"score\": 0.0007743759\r\n    },\r\n    {\r\n      \"intent\": \"Identity\",\r\n      \"score\": 0.000727297564\r\n    },\r\n    {\r\n      \"intent\": \"Divorce Legal Separation\",\r\n      \"score\": 0.000590754149\r\n    },\r\n    {\r\n      \"intent\": \"Utilities and telecommunications\",\r\n      \"score\": 0.000529284531\r\n    },\r\n    {\r\n      \"intent\": \"Medical bills\",\r\n      \"score\": 0.000471992535\r\n    },\r\n    {\r\n      \"intent\": \"Student loans\",\r\n      \"score\": 0.0004546414\r\n    },\r\n    {\r\n      \"intent\": \"Divorce Annulment\",\r\n      \"score\": 0.000423705118\r\n    },\r\n    {\r\n      \"intent\": \"Other family\",\r\n      \"score\": 0.00041898893\r\n    },\r\n    {\r\n      \"intent\": \"Divorce Uncontested\",\r\n      \"score\": 0.0003876439\r\n    },\r\n    {\r\n      \"intent\": \"Unmarried couples\",\r\n      \"score\": 0.000380990357\r\n    },\r\n    {\r\n      \"intent\": \"More family court procedures\",\r\n      \"score\": 0.000221170354\r\n    },\r\n    {\r\n      \"intent\": \"Housing discrimination\",\r\n      \"score\": 0.00018730732\r\n    },\r\n    {\r\n      \"intent\": \"Veteran/Military Families\",\r\n      \"score\": 0.000181092473\r\n    },\r\n    {\r\n      \"intent\": \"Bankrucptcy\",\r\n      \"score\": 0.000131298162\r\n    },\r\n    {\r\n      \"intent\": \"Non-parents caring for children\",\r\n      \"score\": 7.45657E-05\r\n    },\r\n    {\r\n      \"intent\": \"Relocation\",\r\n      \"score\": 6.903267E-05\r\n    },\r\n    {\r\n      \"intent\": \"Non-parent custody\",\r\n      \"score\": 1.08726072E-05\r\n    }\r\n  ],\r\n  \"entities\": []\r\n}");

        //    _luisBusinessLogic.GetInternalResourcesAsync(Arg.Any<string>()).Returns(internalResponse);



        //    var result = luisBusinessLogic.GetResourceBasedOnThresholdAsync(query).Result;


        //}
        
    }
}
