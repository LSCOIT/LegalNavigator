
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

        #region variables
        private readonly IBingSettings bingSettings;
        private readonly IHttpClientService httpClientService;
        private readonly WebSearchBusinessLogic webSearchBusinessLogic;
        #endregion

        #region Mocked Input Data
        private readonly string emptyWebData = "{}";
        private readonly string webData =
                   "{\r\n  \"webResources\": {\r\n    \"_type\": \"SearchResponse\",\r\n    \"instrumentation\": {\r\n      \"_type\": " +
                   "\"ResponseInstrumentation\",\r\n      \"pingUrlBase\": \"https://www.bingapis.com/api/ping?IG=1965C084847D497DB211864D4160BBAD&CID=0F6A141A98C863D31AD41FE799436276&ID=\"," +
                   "\r\n      \"pageLoadPingUrl\": \"https://www.bingapis.com/api/ping/pageload?IG=1965C084847D497DB211864D4160BBAD&CID=0F6A141A98C863D31AD41FE799436276&Type=Event.CPT&DATA=0\"" +
                   "\r\n    },\r\n    \"queryContext\": {\r\n      \"originalQuery\": \"Microsoft\"\r\n    },\r\n    \"webPages\": " +
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
        #endregion

        #region Mocked Output Data        
        private readonly string expectedWebResponse = "Microsoft";
        private readonly string expectedEmptyWebResponse = "{}";
        #endregion 

        public WebSearchBusinessLogicTests()
        {
            bingSettings = Substitute.For<IBingSettings>();
            httpClientService = Substitute.For<IHttpClientService>();
            webSearchBusinessLogic = new WebSearchBusinessLogic(httpClientService, bingSettings);

            bingSettings.BingSearchUrl.Returns(new Uri("http://www.bing.com?{0}{1}{2}"));
            bingSettings.SubscriptionKey.Returns("subscriptionKey");
            bingSettings.CustomConfigId.Returns("0");
            bingSettings.PageResultsCount.Returns((short)10);
            bingSettings.PageOffsetValue.Returns((short)1);
        }

        [Fact]
        public void SearchWebResourcesAsyncSearchTextFound()
        {
            var responseq = new HttpResponseMessage();
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(webData)
            };
            var response = httpClientService.GetDataAsync(bingSettings.BingSearchUrl, bingSettings.SubscriptionKey);
            response.Returns(httpResponseMessage);

            var responseContent = webSearchBusinessLogic.SearchWebResourcesAsync(bingSettings.BingSearchUrl).Result;

            Assert.Contains(expectedWebResponse, responseContent);
        }

        [Fact]
        public void SearchWebResourcesSearchTextNotFound()
        {
            var responseq = new HttpResponseMessage();
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(emptyWebData)
            };
            var response = httpClientService.GetDataAsync(bingSettings.BingSearchUrl, bingSettings.SubscriptionKey);
            response.Returns(httpResponseMessage);

            var responseContent = webSearchBusinessLogic.SearchWebResourcesAsync(bingSettings.BingSearchUrl).Result;

            Assert.Contains(expectedEmptyWebResponse, responseContent);
        }

    }
}