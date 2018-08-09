using Access2Justice.Api.BusinessLogic;
using Access2Justice.Api.Tests.TestData;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSubstitute;
using System;
using System.IO;
using System.Linq;
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

        [Fact]
        public void ShareResourceDataAsyncShouldReturnResourceUrl()
        {
            Document document = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(ShareTestData.userProfile));
            document.LoadFrom(reader);

            Document updatedDocument = new Document();
            reader = new JsonTextReader(new StringReader(ShareTestData.userProfileWithSharedResource));
            updatedDocument.LoadFrom(reader);

            var profileResponse = userProfileBusinessLogic.GetUserProfileDataAsync(ShareTestData.oId);
            profileResponse.ReturnsForAnyArgs<UserProfile>(ShareTestData.UserProfileWithoutSharedResourceData);

            dbService.UpdateItemAsync<UserProfile>(
                ShareTestData.id,
                ShareTestData.UserProfileWithoutSharedResourceData,
                dbSettings.UserProfileCollectionId).ReturnsForAnyArgs<Document>(updatedDocument);

            //act
            var response = shareBusinessLogic.ShareResourceDataAsync(ShareTestData.ShareInputSingleData);
            var expectedResult = JsonConvert.SerializeObject(ShareTestData.UserProfileWithSharedResourceData);
            var actualResult = JsonConvert.SerializeObject(response.Result);
            //assert
            Assert.Equal(expectedResult, actualResult);
        }
        [Fact]
        public void UnshareResourceDataAsyncShouldReturnTrue()
        {

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
        [Fact]
        public void UpdateUserProfileDataAsyncShouldUpdateData()
        {

        }
    }

}
