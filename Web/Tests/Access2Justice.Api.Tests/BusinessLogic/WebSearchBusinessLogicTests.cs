
using System;
using Xunit;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using NSubstitute;
using Access2Justice.Api.BusinessLogic;
using System.Net.Http;
using System.Net;

namespace Access2Justice.Api.Tests.BusinessLogic
{
    public class WebSearchBusinessLogicTests
    {

        private readonly IBingSettings _bingSettings;
        private readonly IHttpClientService _httpClientService;
        private readonly WebSearchBusinessLogic _webSearchBusinessLogic;


        public WebSearchBusinessLogicTests()
        {
            _bingSettings = Substitute.For<IBingSettings>();
            _httpClientService = Substitute.For<IHttpClientService>();
            _webSearchBusinessLogic = new WebSearchBusinessLogic(_httpClientService, _bingSettings);

            _bingSettings.BingSearchUrl.Returns(new Uri("https://www.bing.com"));
            _bingSettings.SubscriptionKey.Returns("456sdf56sd4f56d44546565");
            _bingSettings.CustomConfigId.Returns("2425415097");
        }

        [Fact]
        public void SearchWebResourcesAsyncSearchTextFound()
        {
            var responseq = new HttpResponseMessage();
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"_type\": \"SearchResponse\", \"instrumentation\": {\"_type\": \"ResponseInstrumentation\", \"pingUrlBase\": \"https:\\/\\/www.bingapis.com\\/api\\/ping?IG=428B8CFC22EA4711A146283DEF6F2821&CID=3ED5BEDB376A69A72CE9B52336E1689C&ID=\", \"pageLoadPingUrl\": \"https:\\/\\/www.bingapis.com\\/api\\/ping\\/pageload?IG=428B8CFC22EA4711A146283DEF6F2821&CID=3ED5BEDB376A69A72CE9B52336E1689C&Type=Event.CPT&DATA=0\"}, \"queryContext\": {\"originalQuery\": \"microsoft\"}, \"webPages\": {\"webSearchUrl\": \"https:\\/\\/www.bing.com\\/search?q=microsoft\", \"webSearchUrlPingSuffix\": \"DevEx,5232.1\", \"totalEstimatedMatches\": 5, \"value\": [{\"id\": \"https:\\/\\/api.cognitive.microsoft.com\\/api\\/v7\\/#WebPages.0\", \"name\": \"Offer - Indian Contract Act, 1872 - Lawnotes.in\", \"url\": \"http:\\/\\/www.lawnotes.in\\/Offer_-_Indian_Contract_Act,_1872\", \"urlPingSuffix\": \"DevEx,5071.1\", \"about\": [{\"name\": \"Indian Contract Act, 1872\"}], \"isFamilyFriendly\": true, \"displayUrl\": \"www.lawnotes.in\\/Offer_-_Inan_Contract_Act,_1872\", \"snippet\": \"Main Article : Indian Contract Act, 1872. Offer is one of the essential elements of a contract as defined in Section 10 of the Indian Contract Act, 1872.\", \"deepLinks\": [{\"name\": \"Section 245 of Income-Tax Act, 1961\", \"url\": \"http:\\/\\/www.lawnotes.in\\/Section_245_of_Income-Tax_Act,_1961\", \"urlPingSuffix\": \"DevEx,5062.1\", \"snippet\": \"Section 245 of Income-Tax Act, 1961 deals with the topic of Set off of refunds against tax remaining payable\"}, {\"name\": \"11-CV-01846-LHK\", \"url\": \"https:\\/\\/www.lawnotes.in\\/Apple_Inc_vs_Samsung_Electronics_Co_Ltd_et_al,_No._11-CV-01846-LHK\", \"urlPingSuffix\": \"DevEx,5063.1\", \"snippet\": \"Apple Inc vs Samsung Electronics Co Ltd et al, No. 11-CV-01846-LHK. From Lawnotes.in. Jump to:navigation, search. UNITED STATES DISTRICT COURT NORTHERN DISTRICT OF CALIFORNIA SAN JOSE DIVISION Case No.: 11-CV-01846-LHK APPLE, INC., a California corporation, Plaintiff and Counterdefendant, v. ... Uniloc USA, Inc. v. Micosoft Corp., 632 F.3d 1292, 1302 (Fed. Cir. 2011): ...\"}, {\"name\": \"Ramesh Chandra Shah And Others v Anil Joshi And Others - Lawnotes.\", \"url\": \"https:\\/\\/www.lawnotes.in\\/Ramesh_Chandra_Shah_and_others_v_Anil_Joshi_and_others\", \"urlPingSuffix\": \"DevEx,5064.1\", \"snippet\": \"Ramesh Chandra Shah and others … Appellants versus Anil Joshi and others … Respondents J U D G M E N T ... with regard to basic knowledge of computer operation would be tested at the time of interview for which knowledge of Microsoft Operating System and Microsoft Office operation would be essential. In the call letter also which was sent to the appellant at the time of calling him for interview, ...\"}], \"dateLastCrawled\": \"2018-05-16T17:14:00.0000000Z\", \"fixedPosition\": false, \"language\": \"en\"}, {\"id\": \"https:\\/\\/api.cognitive.microsoft.com\\/api\\/v7\\/#WebPages.1\", \"name\": \"Book Review- Balance of Justice\", \"url\": \"http:\\/\\/accesstojustice-ng.org\\/Review_of_the_Balance_of_Justice.pdf\", \"urlSuffix\": \"DevEx,5084.1\", \"isFamilyFriendly\": true, \"displayUrl\": \"accesstojustice-ng.org\\/Review_of_the_Balance_of_Justice.pdf\", \"snippet\": \"The Balance of Justice presents factual insights into how judges of the Federal High Court, Lagos are perceived at work by users of the court system, and comprises of fact-based narratives, inventories, and analyses on the manner judges managed their time, the ... Microsoft Word - Book Review- Balance of Justice.doc Author: techniques Created Date:\", \"dateLastCrawled\": \"2018-04-07T12:11:00.0000000Z\", \"fixedPosition\": false, \"language\": \"en\"}]}, \"rankingResponse\": {\"mainline\": {\"items\": [{\"answerType\": \"WebPages\", \"resultIndex\": 0, \"value\": {\"id\": \"https:\\/\\/api.cognitive.microsoft.com\\/api\\/v7\\/#WebPages.0\"}}, {\"answerType\": \"WebPages\", \"resultIndex\": 1, \"value\": {\"id\": \"https:\\/\\/api.cognitive.microsoft.com\\/api\\/v7\\/#WebPages.1\"}}]}}}")
            };
            var response = _httpClientService.GetDataAsync(_bingSettings.BingSearchUrl, _bingSettings.SubscriptionKey);
            response.Returns(httpResponseMessage);

            var responseContent = _webSearchBusinessLogic.SearchWebResourcesAsync("microsoft").Result;

            Assert.Contains("microsoft", responseContent);
        }

        [Fact]
        public void SearchWebResourcesSearchTextNotFound()
        {
            var responseq = new HttpResponseMessage();
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"_type\": \"SearchResponse\", \"instrumentation\": {\"_type\": \"ResponseInstrumentation\", \"pingUrlBase\": \"https:\\/\\/www.bingapis.com\\/api\\/ping?IG=428B8CFC22EA4711A146283DEF6F2821&CID=3ED5BEDB376A69A72CE9B52336E1689C&ID=\", \"pageLoadPingUrl\": \"https:\\/\\/www.bingapis.com\\/api\\/ping\\/pageload?IG=428B8CFC22EA4711A146283DEF6F2821&CID=3ED5BEDB376A69A72CE9B52336E1689C&Type=Event.CPT&DATA=0\"}, \"queryContext\": {\"originalQuery\": \"microsoft\"}, \"webPages\": {\"webSearchUrl\": \"https:\\/\\/www.bing.com\\/search?q=microsoft\", \"webSearchUrlPingSuffix\": \"DevEx,5232.1\", \"totalEstimatedMatches\": 5, \"value\": [{\"id\": \"https:\\/\\/api.cognitive.microsoft.com\\/api\\/v7\\/#WebPages.0\", \"name\": \"Offer - Indian Contract Act, 1872 - Lawnotes.in\", \"url\": \"http:\\/\\/www.lawnotes.in\\/Offer_-_Indian_Contract_Act,_1872\", \"urlPingSuffix\": \"DevEx,5071.1\", \"about\": [{\"name\": \"Indian Contract Act, 1872\"}], \"isFamilyFriendly\": true, \"displayUrl\": \"www.lawnotes.in\\/Offer_-_Inan_Contract_Act,_1872\", \"snippet\": \"Main Article : Indian Contract Act, 1872. Offer is one of the essential elements of a contract as defined in Section 10 of the Indian Contract Act, 1872.\", \"deepLinks\": [{\"name\": \"Section 245 of Income-Tax Act, 1961\", \"url\": \"http:\\/\\/www.lawnotes.in\\/Section_245_of_Income-Tax_Act,_1961\", \"urlPingSuffix\": \"DevEx,5062.1\", \"snippet\": \"Section 245 of Income-Tax Act, 1961 deals with the topic of Set off of refunds against tax remaining payable\"}, {\"name\": \"11-CV-01846-LHK\", \"url\": \"https:\\/\\/www.lawnotes.in\\/Apple_Inc_vs_Samsung_Electronics_Co_Ltd_et_al,_No._11-CV-01846-LHK\", \"urlPingSuffix\": \"DevEx,5063.1\", \"snippet\": \"Apple Inc vs Samsung Electronics Co Ltd et al, No. 11-CV-01846-LHK. From Lawnotes.in. Jump to:navigation, search. UNITED STATES DISTRICT COURT NORTHERN DISTRICT OF CALIFORNIA SAN JOSE DIVISION Case No.: 11-CV-01846-LHK APPLE, INC., a California corporation, Plaintiff and Counterdefendant, v. ... Uniloc USA, Inc. v. Micosoft Corp., 632 F.3d 1292, 1302 (Fed. Cir. 2011): ...\"}, {\"name\": \"Ramesh Chandra Shah And Others v Anil Joshi And Others - Lawnotes.\", \"url\": \"https:\\/\\/www.lawnotes.in\\/Ramesh_Chandra_Shah_and_others_v_Anil_Joshi_and_others\", \"urlPingSuffix\": \"DevEx,5064.1\", \"snippet\": \"Ramesh Chandra Shah and others … Appellants versus Anil Joshi and others … Respondents J U D G M E N T ... with regard to basic knowledge of computer operation would be tested at the time of interview for which knowledge of Microsoft Operating System and Microsoft Office operation would be essential. In the call letter also which was sent to the appellant at the time of calling him for interview, ...\"}], \"dateLastCrawled\": \"2018-05-16T17:14:00.0000000Z\", \"fixedPosition\": false, \"language\": \"en\"}, {\"id\": \"https:\\/\\/api.cognitive.microsoft.com\\/api\\/v7\\/#WebPages.1\", \"name\": \"Book Review- Balance of Justice\", \"url\": \"http:\\/\\/accesstojustice-ng.org\\/Review_of_the_Balance_of_Justice.pdf\", \"urlSuffix\": \"DevEx,5084.1\", \"isFamilyFriendly\": true, \"displayUrl\": \"accesstojustice-ng.org\\/Review_of_the_Balance_of_Justice.pdf\", \"snippet\": \"The Balance of Justice presents factual insights into how judges of the Federal High Court, Lagos are perceived at work by users of the court system, and comprises of fact-based narratives, inventories, and analyses on the manner judges managed their time, the ... Microsoft Word - Book Review- Balance of Justice.doc Author: techniques Created Date:\", \"dateLastCrawled\": \"2018-04-07T12:11:00.0000000Z\", \"fixedPosition\": false, \"language\": \"en\"}]}, \"rankingResponse\": {\"mainline\": {\"items\": [{\"answerType\": \"WebPages\", \"resultIndex\": 0, \"value\": {\"id\": \"https:\\/\\/api.cognitive.microsoft.com\\/api\\/v7\\/#WebPages.0\"}}, {\"answerType\": \"WebPages\", \"resultIndex\": 1, \"value\": {\"id\": \"https:\\/\\/api.cognitive.microsoft.com\\/api\\/v7\\/#WebPages.1\"}}]}}}")
            };
            var response = _httpClientService.GetDataAsync(_bingSettings.BingSearchUrl, _bingSettings.SubscriptionKey);
            response.Returns(httpResponseMessage);

            var responseContent = _webSearchBusinessLogic.SearchWebResourcesAsync("microsoft").Result;

            Assert.DoesNotContain("No results found", responseContent);
        }

        [Fact]
        public void SearchWebResourcesSubscriptionKeyMissingValidation()
        {
            var responseq = new HttpResponseMessage();
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"_type\": \"SearchResponse\", \"instrumentation\": {\"_type\": \"ResponseInstrumentation\", \"pingUrlBase\": \"https:\\/\\/www.bingapis.com\\/api\\/ping?IG=428B8CFC22EA4711A146283DEF6F2821&CID=3ED5BEDB376A69A72CE9B52336E1689C&ID=\", \"pageLoadPingUrl\": \"https:\\/\\/www.bingapis.com\\/api\\/ping\\/pageload?IG=428B8CFC22EA4711A146283DEF6F2821&CID=3ED5BEDB376A69A72CE9B52336E1689C&Type=Event.CPT&DATA=0\"}, \"queryContext\": {\"originalQuery\": \"microsoft\"}, \"webPages\": {\"webSearchUrl\": \"https:\\/\\/www.bing.com\\/search?q=microsoft\", \"webSearchUrlPingSuffix\": \"DevEx,5232.1\", \"totalEstimatedMatches\": 5, \"value\": [{\"id\": \"https:\\/\\/api.cognitive.microsoft.com\\/api\\/v7\\/#WebPages.0\", \"name\": \"Offer - Indian Contract Act, 1872 - Lawnotes.in\", \"url\": \"http:\\/\\/www.lawnotes.in\\/Offer_-_Indian_Contract_Act,_1872\", \"urlPingSuffix\": \"DevEx,5071.1\", \"about\": [{\"name\": \"Indian Contract Act, 1872\"}], \"isFamilyFriendly\": true, \"displayUrl\": \"www.lawnotes.in\\/Offer_-_Inan_Contract_Act,_1872\", \"snippet\": \"Main Article : Indian Contract Act, 1872. Offer is one of the essential elements of a contract as defined in Section 10 of the Indian Contract Act, 1872.\", \"deepLinks\": [{\"name\": \"Section 245 of Income-Tax Act, 1961\", \"url\": \"http:\\/\\/www.lawnotes.in\\/Section_245_of_Income-Tax_Act,_1961\", \"urlPingSuffix\": \"DevEx,5062.1\", \"snippet\": \"Section 245 of Income-Tax Act, 1961 deals with the topic of Set off of refunds against tax remaining payable\"}, {\"name\": \"11-CV-01846-LHK\", \"url\": \"https:\\/\\/www.lawnotes.in\\/Apple_Inc_vs_Samsung_Electronics_Co_Ltd_et_al,_No._11-CV-01846-LHK\", \"urlPingSuffix\": \"DevEx,5063.1\", \"snippet\": \"Apple Inc vs Samsung Electronics Co Ltd et al, No. 11-CV-01846-LHK. From Lawnotes.in. Jump to:navigation, search. UNITED STATES DISTRICT COURT NORTHERN DISTRICT OF CALIFORNIA SAN JOSE DIVISION Case No.: 11-CV-01846-LHK APPLE, INC., a California corporation, Plaintiff and Counterdefendant, v. ... Uniloc USA, Inc. v. Micosoft Corp., 632 F.3d 1292, 1302 (Fed. Cir. 2011): ...\"}, {\"name\": \"Ramesh Chandra Shah And Others v Anil Joshi And Others - Lawnotes.\", \"url\": \"https:\\/\\/www.lawnotes.in\\/Ramesh_Chandra_Shah_and_others_v_Anil_Joshi_and_others\", \"urlPingSuffix\": \"DevEx,5064.1\", \"snippet\": \"Ramesh Chandra Shah and others … Appellants versus Anil Joshi and others … Respondents J U D G M E N T ... with regard to basic knowledge of computer operation would be tested at the time of interview for which knowledge of Microsoft Operating System and Microsoft Office operation would be essential. In the call letter also which was sent to the appellant at the time of calling him for interview, ...\"}], \"dateLastCrawled\": \"2018-05-16T17:14:00.0000000Z\", \"fixedPosition\": false, \"language\": \"en\"}, {\"id\": \"https:\\/\\/api.cognitive.microsoft.com\\/api\\/v7\\/#WebPages.1\", \"name\": \"Book Review- Balance of Justice\", \"url\": \"http:\\/\\/accesstojustice-ng.org\\/Review_of_the_Balance_of_Justice.pdf\", \"urlSuffix\": \"DevEx,5084.1\", \"isFamilyFriendly\": true, \"displayUrl\": \"accesstojustice-ng.org\\/Review_of_the_Balance_of_Justice.pdf\", \"snippet\": \"The Balance of Justice presents factual insights into how judges of the Federal High Court, Lagos are perceived at work by users of the court system, and comprises of fact-based narratives, inventories, and analyses on the manner judges managed their time, the ... Microsoft Word - Book Review- Balance of Justice.doc Author: techniques Created Date:\", \"dateLastCrawled\": \"2018-04-07T12:11:00.0000000Z\", \"fixedPosition\": false, \"language\": \"en\"}]}, \"rankingResponse\": {\"mainline\": {\"items\": [{\"answerType\": \"WebPages\", \"resultIndex\": 0, \"value\": {\"id\": \"https:\\/\\/api.cognitive.microsoft.com\\/api\\/v7\\/#WebPages.0\"}}, {\"answerType\": \"WebPages\", \"resultIndex\": 1, \"value\": {\"id\": \"https:\\/\\/api.cognitive.microsoft.com\\/api\\/v7\\/#WebPages.1\"}}]}}}")
            };
            var response = _httpClientService.GetDataAsync(_bingSettings.BingSearchUrl, _bingSettings.SubscriptionKey);
            response.Returns(httpResponseMessage);

            var responseContent = _webSearchBusinessLogic.SearchWebResourcesAsync("microsoft").Result;

            Assert.DoesNotContain("Access denied due to missing subscription key. Make sure to include subscription key when making requests to an API.", responseContent);
        }

    }
}