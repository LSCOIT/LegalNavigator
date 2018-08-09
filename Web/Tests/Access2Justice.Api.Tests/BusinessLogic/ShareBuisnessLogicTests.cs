using Access2Justice.Api.BusinessLogic;
using Access2Justice.Api.Tests.TestData;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using NSubstitute;
using System.IO;
using Xunit;

namespace Access2Justice.Api.Tests.BusinessLogic
{
    public class ShareBuisnessLogicTests
    {
        private readonly IBackendDatabaseService dbService;
        private readonly ICosmosDbSettings dbSettings;
        private readonly IDynamicQueries dynamicQueries;
        private readonly IShareSettings dbShareSettings;
        private readonly IUserProfileBusinessLogic userProfileBusinessLogic;
        private readonly ShareBusinessLogic shareBusinessLogic;

        public ShareBuisnessLogicTests()
        {
            dbService = Substitute.For<IBackendDatabaseService>();
            dbSettings = Substitute.For<ICosmosDbSettings>();
            dynamicQueries = Substitute.For<IDynamicQueries>();
            dbShareSettings = Substitute.For<IShareSettings>();
            userProfileBusinessLogic = Substitute.For<IUserProfileBusinessLogic>();
            shareBusinessLogic = new ShareBusinessLogic(dynamicQueries, dbSettings, dbService, dbShareSettings, userProfileBusinessLogic);

            dbSettings.UserProfileCollectionId.Returns("UserProfile");
            dbShareSettings.PermaLinkMaxLength.Returns(7);
        }

        [Theory]
        [MemberData(nameof(ShareTestData.ShareInputData), MemberType = typeof(ShareTestData))]
        public void CheckPermaLinkDataAsyncShouldValidate(ShareInput shareInput, dynamic expectedResult)
        {
            var profileResponse = userProfileBusinessLogic.GetUserProfileDataAsync(shareInput.UserId);
            profileResponse.ReturnsForAnyArgs<UserProfile>(ShareTestData.UserProfileWithSharedResourceData);
            //act
            var response = shareBusinessLogic.CheckPermaLinkDataAsync(shareInput);
            expectedResult = JsonConvert.SerializeObject(expectedResult);
            var actualResult = JsonConvert.SerializeObject(response.Result);
            //assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [MemberData(nameof(ShareTestData.ShareGenerateInputData), MemberType = typeof(ShareTestData))]
        public void ShareResourceDataAsyncShouldReturnResourceUrl(ShareInput shareInput, int permaLinkOutputLength, dynamic expectedResult)
        {
            var profileResponse = userProfileBusinessLogic.GetUserProfileDataAsync(shareInput.UserId);
            profileResponse.ReturnsForAnyArgs<UserProfile>(ShareTestData.UserProfileWithoutSharedResourceData);

            dbShareSettings.PermaLinkMaxLength.Returns(permaLinkOutputLength);

            Document updatedDocument = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(ShareTestData.userProfileWithSharedResource));
            updatedDocument.LoadFrom(reader);

            dbService.UpdateItemAsync<UserProfile>(
               Arg.Any<string>(),
               Arg.Any<UserProfile>(),
               Arg.Any<string>()).ReturnsForAnyArgs<Document>(updatedDocument);

            //act
            var result = shareBusinessLogic.ShareResourceDataAsync(shareInput).Result;
            //assert
            Assert.Equal(expectedResult.PermaLink.Length, result.PermaLink.Length);
        }

        [Theory]
        [MemberData(nameof(ShareTestData.UnShareInputData), MemberType = typeof(ShareTestData))]
        public void UnshareResourceDataAsyncShouldReturnTrue(UnShareInput unShareInput, dynamic expectedResult)
        {
            var profileResponse = userProfileBusinessLogic.GetUserProfileDataAsync(unShareInput.UserId);
            profileResponse.ReturnsForAnyArgs<UserProfile>(ShareTestData.UserProfileWithSharedResourceData);

            Document updatedDocument = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(ShareTestData.userProfile));
            updatedDocument.LoadFrom(reader);

            dbService.UpdateItemAsync<UserProfile>(
               Arg.Any<string>(),
               Arg.Any<UserProfile>(),
               Arg.Any<string>()).ReturnsForAnyArgs<Document>(updatedDocument);

            //act
            var result = shareBusinessLogic.UnshareResourceDataAsync(unShareInput).Result;
            expectedResult = JsonConvert.SerializeObject(expectedResult);
            var actualResult = JsonConvert.SerializeObject(result);
            //assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GetPermaLinkDataAsyncShouldValidate()
        {
            var dbResponse = dynamicQueries.FindFieldWhereArrayContainsAsync(dbSettings.UserProfileCollectionId, Constants.SharedResource,
                        Constants.Url, Constants.PermaLink, ShareTestData.permalinkInputData, Constants.ExpirationDate);
            dbResponse.ReturnsForAnyArgs<dynamic>(ShareTestData.ExpectedResourceData);

            var response = shareBusinessLogic.GetPermaLinkDataAsync(ShareTestData.permalinkInputData);
            //assert
            Assert.Equal(ShareTestData.ExpectedResourceData, response.Result);
        }
        
    }

}
