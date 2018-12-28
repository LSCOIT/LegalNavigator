using Access2Justice.Api.BusinessLogic;
using Access2Justice.Api.Tests.TestData;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models.Integration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSubstitute;
using Xunit;

namespace Access2Justice.Api.Tests.BusinessLogic
{

    public class OnboardingInfoBusinessLogicTests
    {
        private readonly IHttpClientService httpClientService;
        private readonly IOnboardingInfoSettings onboardingInfoSettings;
        private readonly IDynamicQueries dynamicQueries;
        private readonly ICosmosDbSettings dbSettings;
        private readonly OnboardingInfoBusinessLogic onboardingInfo;
        public OnboardingInfoBusinessLogicTests()
        {
            httpClientService = Substitute.For<IHttpClientService>();
            onboardingInfoSettings = Substitute.For<IOnboardingInfoSettings>();
            dynamicQueries = Substitute.For<IDynamicQueries>();
            dbSettings = Substitute.For<ICosmosDbSettings>();
            dynamicQueries = Substitute.For<IDynamicQueries>();
            onboardingInfo = new OnboardingInfoBusinessLogic(httpClientService, onboardingInfoSettings, dynamicQueries, dbSettings);

            onboardingInfoSettings.HostAddress.Returns("http://smtpservice");
            onboardingInfoSettings.PortNumber.Returns("467");
            onboardingInfoSettings.UserName.Returns("Test");
            onboardingInfoSettings.Password.Returns("Test");
            onboardingInfoSettings.FromAddress.Returns("test@test.com");
            onboardingInfoSettings.FallbackToAddress.Returns("test@test.com");
            onboardingInfoSettings.Subject.Returns("Test Mail");
            onboardingInfoSettings.FallbackBodyMessage.Returns("Test Mail Body");
        }

        [Theory]
        [MemberData(nameof(OnboardingInfoBusinessLogicTestData.OnboardingInfoTestData), MemberType = typeof(OnboardingInfoBusinessLogicTestData))]
        public void GetOnboardingInfoShouldValidate(string organizationId, JObject onboarding, dynamic expectedResult)
        {
            var dbResponse = dynamicQueries.FindItemWhereAsync<JObject>(dbSettings.ResourcesCollectionId, Constants.Id, organizationId);
            dbResponse.ReturnsForAnyArgs(onboarding);

            //act
            var response = onboardingInfo.GetOnboardingInfo(organizationId);
            expectedResult = JsonConvert.SerializeObject(expectedResult);
            var actualResult = JsonConvert.SerializeObject(response.Result);

            //assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [MemberData(nameof(OnboardingInfoBusinessLogicTestData.PostOnboardingInfoTestData), MemberType = typeof(OnboardingInfoBusinessLogicTestData))]
        public void PostOnboardingInfoShouldValidate(OnboardingInfo onboarding, dynamic expectedResult)
        {
            //act
            var response = onboardingInfo.PostOnboardingInfo(onboarding);
            expectedResult = JsonConvert.SerializeObject(expectedResult);
            var actualResult = JsonConvert.SerializeObject(response.Result);
            //assert
            Assert.Equal(expectedResult, actualResult);
        }                 
    }
}

