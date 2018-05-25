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
        private readonly ILuisSettings _luisSettings;
        private readonly IHttpClientService _httpClientService;
        private readonly LuisProxy luisProxy;

        public LuisProxyTests()
        {
            _luisSettings = Substitute.For<ILuisSettings>();
            _httpClientService = Substitute.For<IHttpClientService>();
            luisProxy = new LuisProxy(_httpClientService, _luisSettings);

            _luisSettings.Endpoint.Returns(new Uri("https://www.luis.ai/home"));
            _luisSettings.TopIntentsCount.Returns("3");
            _luisSettings.UpperThreshold.Returns("0.9");
            _luisSettings.LowerThreshold.Returns("0.6");
        }

        [Fact]
        public void GetIntentsWithProperIntent()
        {
            //arrange
            string query = "eviction";
            var responseq = new HttpResponseMessage();
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\r\n  \"query\": \"eviction\",\r\n  \"topScoringIntent\": {\r\n    \"intent\": \"Eviction\",\r\n    \"score\": 0.998598337\r\n  },\r\n  \"intents\": [\r\n    {\r\n      \"intent\": \"Eviction\",\r\n      \"score\": 0.998598337\r\n    },\r\n    {\r\n      \"intent\": \"None\",\r\n      \"score\": 0.3924698\r\n    },\r\n    {\r\n      \"intent\": \"Small Claims Court\",\r\n      \"score\": 0.374458015\r\n    },\r\n    {\r\n      \"intent\": \"Mobile home park tenants\",\r\n      \"score\": 0.352736056\r\n    },\r\n    {\r\n      \"intent\": \"Going to court\",\r\n      \"score\": 0.131461561\r\n    },\r\n    {\r\n      \"intent\": \"Domestic Violence\",\r\n      \"score\": 0.0386172235\r\n    },\r\n    {\r\n      \"intent\": \"Separation\",\r\n      \"score\": 0.035968408\r\n    },\r\n    {\r\n      \"intent\": \"Foreclosure\",\r\n      \"score\": 0.0271484256\r\n    },\r\n    {\r\n      \"intent\": \"division of property\",\r\n      \"score\": 0.0246635266\r\n    },\r\n    {\r\n      \"intent\": \"Child support\",\r\n      \"score\": 0.0197562873\r\n    },\r\n    {\r\n      \"intent\": \"child custody\",\r\n      \"score\": 0.0193109587\r\n    },\r\n    {\r\n      \"intent\": \"Other Consumer\",\r\n      \"score\": 0.0174270831\r\n    },\r\n    {\r\n      \"intent\": \"Unemployment\",\r\n      \"score\": 0.0172820911\r\n    },\r\n    {\r\n      \"intent\": \"Guardianship\",\r\n      \"score\": 0.0149135459\r\n    },\r\n    {\r\n      \"intent\": \"Consumer Fraud\",\r\n      \"score\": 0.0109810177\r\n    },\r\n    {\r\n      \"intent\": \"Contracts\",\r\n      \"score\": 0.0105354656\r\n    },\r\n    {\r\n      \"intent\": \"Elder Abuse\",\r\n      \"score\": 0.0100716464\r\n    },\r\n    {\r\n      \"intent\": \"Divorce\",\r\n      \"score\": 0.009231578\r\n    },\r\n    {\r\n      \"intent\": \"Managing money\",\r\n      \"score\": 0.00913277548\r\n    },\r\n    {\r\n      \"intent\": \"Sexual Assault\",\r\n      \"score\": 0.007721106\r\n    },\r\n    {\r\n      \"intent\": \"Home buyers & owners\",\r\n      \"score\": 0.00558177941\r\n    },\r\n    {\r\n      \"intent\": \"Marriage equality\",\r\n      \"score\": 0.005541543\r\n    },\r\n    {\r\n      \"intent\": \"Utilities & phones\",\r\n      \"score\": 0.00534641044\r\n    },\r\n    {\r\n      \"intent\": \"Public & subsidized housing\",\r\n      \"score\": 0.005005484\r\n    },\r\n    {\r\n      \"intent\": \"Contempt of court\",\r\n      \"score\": 0.00479589263\r\n    },\r\n    {\r\n      \"intent\": \"Tenant's rights\",\r\n      \"score\": 0.00391642563\r\n    },\r\n    {\r\n      \"intent\": \"marriage\",\r\n      \"score\": 0.00364745827\r\n    },\r\n    {\r\n      \"intent\": \"The child protection system\",\r\n      \"score\": 0.00345990737\r\n    },\r\n    {\r\n      \"intent\": \"Legal financial obligation\",\r\n      \"score\": 0.00318646478\r\n    },\r\n    {\r\n      \"intent\": \"Custody\",\r\n      \"score\": 0.002302651\r\n    },\r\n    {\r\n      \"intent\": \"Car issues\",\r\n      \"score\": 0.00218277727\r\n    },\r\n    {\r\n      \"intent\": \"Emergency shelter & assistance\",\r\n      \"score\": 0.00181414338\r\n    },\r\n    {\r\n      \"intent\": \"Driver & Professional licenses\",\r\n      \"score\": 0.00177037634\r\n    },\r\n    {\r\n      \"intent\": \"Employment Discrimination\",\r\n      \"score\": 0.00176623627\r\n    },\r\n    {\r\n      \"intent\": \"Credit problems\",\r\n      \"score\": 0.00128342363\r\n    },\r\n    {\r\n      \"intent\": \"alimony\",\r\n      \"score\": 0.00127891626\r\n    },\r\n    {\r\n      \"intent\": \"Parenting Plans/Custody\",\r\n      \"score\": 0.0012039718\r\n    },\r\n    {\r\n      \"intent\": \"Adoption\",\r\n      \"score\": 0.00105760642\r\n    },\r\n    {\r\n      \"intent\": \"Paternity/Parentage\",\r\n      \"score\": 0.00103898533\r\n    },\r\n    {\r\n      \"intent\": \"Divorce Contested\",\r\n      \"score\": 0.00100662454\r\n    },\r\n    {\r\n      \"intent\": \"Debt\",\r\n      \"score\": 0.0007743759\r\n    },\r\n    {\r\n      \"intent\": \"Identity\",\r\n      \"score\": 0.000727297564\r\n    },\r\n    {\r\n      \"intent\": \"Divorce Legal Separation\",\r\n      \"score\": 0.000590754149\r\n    },\r\n    {\r\n      \"intent\": \"Utilities and telecommunications\",\r\n      \"score\": 0.000529284531\r\n    },\r\n    {\r\n      \"intent\": \"Medical bills\",\r\n      \"score\": 0.000471992535\r\n    },\r\n    {\r\n      \"intent\": \"Student loans\",\r\n      \"score\": 0.0004546414\r\n    },\r\n    {\r\n      \"intent\": \"Divorce Annulment\",\r\n      \"score\": 0.000423705118\r\n    },\r\n    {\r\n      \"intent\": \"Other family\",\r\n      \"score\": 0.00041898893\r\n    },\r\n    {\r\n      \"intent\": \"Divorce Uncontested\",\r\n      \"score\": 0.0003876439\r\n    },\r\n    {\r\n      \"intent\": \"Unmarried couples\",\r\n      \"score\": 0.000380990357\r\n    },\r\n    {\r\n      \"intent\": \"More family court procedures\",\r\n      \"score\": 0.000221170354\r\n    },\r\n    {\r\n      \"intent\": \"Housing discrimination\",\r\n      \"score\": 0.00018730732\r\n    },\r\n    {\r\n      \"intent\": \"Veteran/Military Families\",\r\n      \"score\": 0.000181092473\r\n    },\r\n    {\r\n      \"intent\": \"Bankrucptcy\",\r\n      \"score\": 0.000131298162\r\n    },\r\n    {\r\n      \"intent\": \"Non-parents caring for children\",\r\n      \"score\": 7.45657E-05\r\n    },\r\n    {\r\n      \"intent\": \"Relocation\",\r\n      \"score\": 6.903267E-05\r\n    },\r\n    {\r\n      \"intent\": \"Non-parent custody\",\r\n      \"score\": 1.08726072E-05\r\n    }\r\n  ],\r\n  \"entities\": []\r\n}")
            };

            var luisResponse = _httpClientService.GetAsync(_luisSettings.Endpoint);
            luisResponse.Returns(httpResponseMessage);

            //act
            var result = luisProxy.GetIntents(query).Result;

            //assert
            Assert.Contains("eviction", result,StringComparison.InvariantCulture);
        }

        [Fact]
        public void GetIntentsWithNoneIntent()
        {
            //arrange
            string query = "Access2Justice";
            var responseq = new HttpResponseMessage();
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\r\n  \"query\": \"Access2Justice\",\r\n  \"topScoringIntent\": {\r\n    \"intent\": \"None\",\r\n    \"score\": 0.998598337\r\n  },\r\n  \"intents\": [\r\n    {\r\n      \"intent\": \"None\",\r\n      \"score\": 0.998598337\r\n    },\r\n    {\r\n      \"intent\": \"None\",\r\n      \"score\": 0.3924698\r\n    },\r\n    {\r\n      \"intent\": \"Small Claims Court\",\r\n      \"score\": 0.374458015\r\n    },\r\n    {\r\n      \"intent\": \"Mobile home park tenants\",\r\n      \"score\": 0.352736056\r\n    },\r\n    {\r\n      \"intent\": \"Going to court\",\r\n      \"score\": 0.131461561\r\n    },\r\n    {\r\n      \"intent\": \"Domestic Violence\",\r\n      \"score\": 0.0386172235\r\n    },\r\n    {\r\n      \"intent\": \"Separation\",\r\n      \"score\": 0.035968408\r\n    },\r\n    {\r\n      \"intent\": \"Foreclosure\",\r\n      \"score\": 0.0271484256\r\n    },\r\n    {\r\n      \"intent\": \"division of property\",\r\n      \"score\": 0.0246635266\r\n    },\r\n    {\r\n      \"intent\": \"Child support\",\r\n      \"score\": 0.0197562873\r\n    },\r\n    {\r\n      \"intent\": \"child custody\",\r\n      \"score\": 0.0193109587\r\n    },\r\n    {\r\n      \"intent\": \"Other Consumer\",\r\n      \"score\": 0.0174270831\r\n    },\r\n    {\r\n      \"intent\": \"Unemployment\",\r\n      \"score\": 0.0172820911\r\n    },\r\n    {\r\n      \"intent\": \"Guardianship\",\r\n      \"score\": 0.0149135459\r\n    },\r\n    {\r\n      \"intent\": \"Consumer Fraud\",\r\n      \"score\": 0.0109810177\r\n    },\r\n    {\r\n      \"intent\": \"Contracts\",\r\n      \"score\": 0.0105354656\r\n    },\r\n    {\r\n      \"intent\": \"Elder Abuse\",\r\n      \"score\": 0.0100716464\r\n    },\r\n    {\r\n      \"intent\": \"Divorce\",\r\n      \"score\": 0.009231578\r\n    },\r\n    {\r\n      \"intent\": \"Managing money\",\r\n      \"score\": 0.00913277548\r\n    },\r\n    {\r\n      \"intent\": \"Sexual Assault\",\r\n      \"score\": 0.007721106\r\n    },\r\n    {\r\n      \"intent\": \"Home buyers & owners\",\r\n      \"score\": 0.00558177941\r\n    },\r\n    {\r\n      \"intent\": \"Marriage equality\",\r\n      \"score\": 0.005541543\r\n    },\r\n    {\r\n      \"intent\": \"Utilities & phones\",\r\n      \"score\": 0.00534641044\r\n    },\r\n    {\r\n      \"intent\": \"Public & subsidized housing\",\r\n      \"score\": 0.005005484\r\n    },\r\n    {\r\n      \"intent\": \"Contempt of court\",\r\n      \"score\": 0.00479589263\r\n    },\r\n    {\r\n      \"intent\": \"Tenant's rights\",\r\n      \"score\": 0.00391642563\r\n    },\r\n    {\r\n      \"intent\": \"marriage\",\r\n      \"score\": 0.00364745827\r\n    },\r\n    {\r\n      \"intent\": \"The child protection system\",\r\n      \"score\": 0.00345990737\r\n    },\r\n    {\r\n      \"intent\": \"Legal financial obligation\",\r\n      \"score\": 0.00318646478\r\n    },\r\n    {\r\n      \"intent\": \"Custody\",\r\n      \"score\": 0.002302651\r\n    },\r\n    {\r\n      \"intent\": \"Car issues\",\r\n      \"score\": 0.00218277727\r\n    },\r\n    {\r\n      \"intent\": \"Emergency shelter & assistance\",\r\n      \"score\": 0.00181414338\r\n    },\r\n    {\r\n      \"intent\": \"Driver & Professional licenses\",\r\n      \"score\": 0.00177037634\r\n    },\r\n    {\r\n      \"intent\": \"Employment Discrimination\",\r\n      \"score\": 0.00176623627\r\n    },\r\n    {\r\n      \"intent\": \"Credit problems\",\r\n      \"score\": 0.00128342363\r\n    },\r\n    {\r\n      \"intent\": \"alimony\",\r\n      \"score\": 0.00127891626\r\n    },\r\n    {\r\n      \"intent\": \"Parenting Plans/Custody\",\r\n      \"score\": 0.0012039718\r\n    },\r\n    {\r\n      \"intent\": \"Adoption\",\r\n      \"score\": 0.00105760642\r\n    },\r\n    {\r\n      \"intent\": \"Paternity/Parentage\",\r\n      \"score\": 0.00103898533\r\n    },\r\n    {\r\n      \"intent\": \"Divorce Contested\",\r\n      \"score\": 0.00100662454\r\n    },\r\n    {\r\n      \"intent\": \"Debt\",\r\n      \"score\": 0.0007743759\r\n    },\r\n    {\r\n      \"intent\": \"Identity\",\r\n      \"score\": 0.000727297564\r\n    },\r\n    {\r\n      \"intent\": \"Divorce Legal Separation\",\r\n      \"score\": 0.000590754149\r\n    },\r\n    {\r\n      \"intent\": \"Utilities and telecommunications\",\r\n      \"score\": 0.000529284531\r\n    },\r\n    {\r\n      \"intent\": \"Medical bills\",\r\n      \"score\": 0.000471992535\r\n    },\r\n    {\r\n      \"intent\": \"Student loans\",\r\n      \"score\": 0.0004546414\r\n    },\r\n    {\r\n      \"intent\": \"Divorce Annulment\",\r\n      \"score\": 0.000423705118\r\n    },\r\n    {\r\n      \"intent\": \"Other family\",\r\n      \"score\": 0.00041898893\r\n    },\r\n    {\r\n      \"intent\": \"Divorce Uncontested\",\r\n      \"score\": 0.0003876439\r\n    },\r\n    {\r\n      \"intent\": \"Unmarried couples\",\r\n      \"score\": 0.000380990357\r\n    },\r\n    {\r\n      \"intent\": \"More family court procedures\",\r\n      \"score\": 0.000221170354\r\n    },\r\n    {\r\n      \"intent\": \"Housing discrimination\",\r\n      \"score\": 0.00018730732\r\n    },\r\n    {\r\n      \"intent\": \"Veteran/Military Families\",\r\n      \"score\": 0.000181092473\r\n    },\r\n    {\r\n      \"intent\": \"Bankrucptcy\",\r\n      \"score\": 0.000131298162\r\n    },\r\n    {\r\n      \"intent\": \"Non-parents caring for children\",\r\n      \"score\": 7.45657E-05\r\n    },\r\n    {\r\n      \"intent\": \"Relocation\",\r\n      \"score\": 6.903267E-05\r\n    },\r\n    {\r\n      \"intent\": \"Non-parent custody\",\r\n      \"score\": 1.08726072E-05\r\n    }\r\n  ],\r\n  \"entities\": []\r\n}")
            };

            var luisResponse = _httpClientService.GetAsync(_luisSettings.Endpoint);
            luisResponse.Returns(httpResponseMessage);

            //act
            var result = luisProxy.GetIntents(query).Result;

            //assert
            Assert.Contains("none", result, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void GetIntentsWithEmptyObject()
        {
            //arrange
            string query = "Access2Justice";
            var responseq = new HttpResponseMessage();
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("")
            };

            var luisResponse = _httpClientService.GetAsync(_luisSettings.Endpoint);
            luisResponse.Returns(httpResponseMessage);

            //act
            var result = luisProxy.GetIntents(query).Result;

            //assert
            Assert.Empty(result);
        }


    }
}
