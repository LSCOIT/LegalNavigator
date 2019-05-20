using Access2Justice.Api.BusinessLogic;
using Access2Justice.Api.Interfaces;
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
using System.Collections.Generic;
using System.Globalization;
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
        private readonly IPersonalizedPlanBusinessLogic personalizedPlanBusinessLogic;
        private readonly ShareBusinessLogic shareBusinessLogic;

        public ShareBuisnessLogicTests()
        {
            dbService = Substitute.For<IBackendDatabaseService>();
            dbSettings = Substitute.For<ICosmosDbSettings>();
            dynamicQueries = Substitute.For<IDynamicQueries>();
            dbShareSettings = Substitute.For<IShareSettings>();
            userProfileBusinessLogic = Substitute.For<IUserProfileBusinessLogic>();
            personalizedPlanBusinessLogic = Substitute.For<IPersonalizedPlanBusinessLogic>();
            shareBusinessLogic = new ShareBusinessLogic(dynamicQueries, dbSettings, dbService, dbShareSettings, userProfileBusinessLogic, personalizedPlanBusinessLogic);

            dbSettings.ProfilesCollectionId.Returns("UserProfile");
            dbSettings.UserResourcesCollectionId.Returns("UserResource");
            dbShareSettings.PermaLinkMaxLength.Returns(7);
        }

        [Theory]
        [MemberData(nameof(ShareTestData.ShareInputData), MemberType = typeof(ShareTestData))]
        public void CheckPermaLinkDataAsyncShouldValidate(ShareInput shareInput, dynamic expectedResult)
        {
            dynamic profileResponse = userProfileBusinessLogic.GetUserProfileDataAsync(shareInput.UserId, false).Returns<dynamic>(ShareTestData.UserProfileWithSharedResourceData);
            var dbResponse = dynamicQueries.FindItemsWhereAsync(dbSettings.UserResourcesCollectionId, "SharedResourceId", "0568B88C-3866-4CCA-97C8-B8E3F3D1FF3C");
            dbResponse.ReturnsForAnyArgs(ShareTestData.sharedResourcesData);

            //act
            var response = shareBusinessLogic.CheckPermaLinkDataAsync(shareInput);
            expectedResult = JsonConvert.SerializeObject(expectedResult);
            var actualResult = JsonConvert.SerializeObject(response.Result);
            //assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [MemberData(nameof(ShareTestData.ShareInputData), MemberType = typeof(ShareTestData))]
        public void CheckPermaLinkDataAsyncShouldValidateNull(ShareInput shareInput, dynamic expectedResult)
        {
            shareInput.UserId = null;
            shareInput.Url = null;
            //act
            var response = shareBusinessLogic.CheckPermaLinkDataAsync(shareInput);
            expectedResult = JsonConvert.SerializeObject(null);
            var actualResult = JsonConvert.SerializeObject(response.Result);
            //assert
            Assert.Contains(expectedResult, actualResult);
        }

        [Theory]
        [MemberData(nameof(ShareTestData.ShareGenerateInputData), MemberType = typeof(ShareTestData))]
        public void ShareResourceDataAsyncShouldReturnResourceUrl(ShareInput shareInput, int permaLinkOutputLength, dynamic expectedResult)
        {
            dynamic profileResponse = userProfileBusinessLogic.GetUserProfileDataAsync(shareInput.UserId).Returns<dynamic>(ShareTestData.UserProfileWithoutSharedResourceData);
            dbShareSettings.PermaLinkMaxLength.Returns(permaLinkOutputLength);

            Document updatedDocument = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(ShareTestData.userProfileWithSharedResource));
            updatedDocument.LoadFrom(reader);

            dbService.UpdateItemAsync<UserProfile>(
               Arg.Any<string>(),
               Arg.Any<UserProfile>(),
               Arg.Any<string>()).ReturnsForAnyArgs<Document>(updatedDocument);

            dbService.CreateItemAsync<SharedResources>(
               Arg.Any<SharedResources>(),
               Arg.Any<string>()).ReturnsForAnyArgs<Document>(updatedDocument);

            //act
            var result = shareBusinessLogic.ShareResourceDataAsync(shareInput).Result;
            //assert
            Assert.Equal(expectedResult.PermaLink.Length, result.PermaLink.Length);
        }

        [Theory]
        [MemberData(nameof(ShareTestData.ShareGenerateInputDataNull), MemberType = typeof(ShareTestData))]
        public void ShareResourceDataAsyncShouldReturnResourceUserIdNull(ShareInput shareInput, int permaLinkOutputLength, dynamic expectedResult)
        {
            dynamic profileResponse = userProfileBusinessLogic.GetUserProfileDataAsync(shareInput.UserId).Returns<dynamic>(ShareTestData.UserProfileWithoutSharedResourceData);
            dbShareSettings.PermaLinkMaxLength.Returns(permaLinkOutputLength);

            Document updatedDocument = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(ShareTestData.userProfileWithSharedResource));
            updatedDocument.LoadFrom(reader);

            dbService.UpdateItemAsync<UserProfile>(
               Arg.Any<string>(),
               Arg.Any<UserProfile>(),
               Arg.Any<string>()).ReturnsForAnyArgs<Document>(updatedDocument);

            dbService.CreateItemAsync<SharedResources>(
               Arg.Any<SharedResources>(),
               Arg.Any<string>()).ReturnsForAnyArgs<Document>(updatedDocument);

            //act
            var result = shareBusinessLogic.ShareResourceDataAsync(shareInput).Result;
            //assert
            Assert.Equal(expectedResult.PermaLink, result);
        }

        [Theory]
        [MemberData(nameof(ShareTestData.UnShareInputData), MemberType = typeof(ShareTestData))]
        public void UnshareResourceDataAsyncShouldReturnTrue(ShareInput unShareInput, dynamic expectedResult)
        {
            var profileResponse = userProfileBusinessLogic.GetUserProfileDataAsync(unShareInput.UserId);
            profileResponse.ReturnsForAnyArgs(ShareTestData.UserProfileWithSharedResourceData);

            Document updatedDocument = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(ShareTestData.updatedSharedResourcesData));
            updatedDocument.LoadFrom(reader);

            dbService.UpdateItemAsync<SharedResources>(
               Arg.Any<string>(),
               Arg.Any<SharedResources>(),
               Arg.Any<string>()).ReturnsForAnyArgs<Document>(updatedDocument);

            userProfileBusinessLogic
                .DeleteUserSharedResource(Arg.Is<ShareInput>(x => x.Url == null))
                .Returns((Document)null);

            userProfileBusinessLogic
                .DeleteUserSharedResource(Arg.Is<ShareInput>(x => x.Url != null))
                .Returns(updatedDocument);

            var dbResponse = dynamicQueries.FindItemsWhereAsync(dbSettings.UserResourcesCollectionId, "SharedResourceId", "0568B88C-3866-4CCA-97C8-B8E3F3D1FF3C");
            dbResponse.ReturnsForAnyArgs(ShareTestData.sharedResourcesData);

            //act
            var result = shareBusinessLogic.UnshareResourceDataAsync(unShareInput).Result;
            expectedResult = JsonConvert.SerializeObject(expectedResult);
            var actualResult = JsonConvert.SerializeObject(result);
            //assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [MemberData(nameof(ShareTestData.ShareProfileResponseData), MemberType = typeof(ShareTestData))]
        public void GetPermaLinkDataAsyncShouldValidate(string permaLinkInput, string planId, JArray shareProfileDetails, JArray shareProfileResponse, dynamic expectedResult)
        {
            var dbResponse = dynamicQueries.FindFieldWhereArrayContainsAsync(dbSettings.ProfilesCollectionId, Constants.SharedResource,
                        Constants.PermaLink, permaLinkInput, Constants.ExpirationDate);
            dbResponse.ReturnsForAnyArgs(shareProfileDetails);

            var profileResponse = dynamicQueries.FindFieldWhereArrayContainsAsync(dbSettings.ProfilesCollectionId, Constants.sharedResourceId, planId);
            profileResponse.ReturnsForAnyArgs(shareProfileResponse);

            var response = shareBusinessLogic.GetPermaLinkDataAsync(permaLinkInput);
            expectedResult = JsonConvert.SerializeObject(expectedResult);
            var actualResult = JsonConvert.SerializeObject(response.Result);
            //assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [MemberData(nameof(ShareTestData.UserProfileWithSharedResourceDataForUpdate), MemberType = typeof(ShareTestData))]
        public void UpsertSharedResourceValidate(UserProfile userProfile, SharedResource sharedResource, JArray sharedResourceResults, dynamic expectedResult)
        {
            //arrange            
            var dbResponse = dynamicQueries.FindItemsWhereAsync(dbSettings.UserResourcesCollectionId, Constants.Id, Convert.ToString(userProfile.SharedResourceId, CultureInfo.InvariantCulture));
            dbResponse.ReturnsForAnyArgs(sharedResourceResults);

            Document updatedDocument = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(ShareTestData.upsertSharedResource));
            updatedDocument.LoadFrom(reader);

            dbService.UpdateItemAsync<SharedResources>(
               Arg.Any<string>(),
               Arg.Any<SharedResources>(),
               Arg.Any<string>()).ReturnsForAnyArgs<Document>(updatedDocument);

            dbService.CreateItemAsync<SharedResources>(
               Arg.Any<SharedResources>(),
               Arg.Any<string>()).ReturnsForAnyArgs<Document>(updatedDocument);

            //act
            var response = shareBusinessLogic.UpsertSharedResource(userProfile, sharedResource);
            var actualResult = JsonConvert.SerializeObject(response.Result);

            //assert
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
