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

			dbSettings.UserProfileCollectionId.Returns("UserProfile");
			dbSettings.UserResourceCollectionId.Returns("UserResource");
			dbShareSettings.PermaLinkMaxLength.Returns(7);
		}

		[Theory]
		[MemberData(nameof(ShareTestData.ShareInputData), MemberType = typeof(ShareTestData))]
		public void CheckPermaLinkDataAsyncShouldValidate(ShareInput shareInput, dynamic expectedResult)
		{
			var profileResponse = userProfileBusinessLogic.GetUserProfileDataAsync(shareInput.UserId);
			profileResponse.ReturnsForAnyArgs<UserProfile>(ShareTestData.UserProfileWithSharedResourceData);
			var dbResponse = dynamicQueries.FindItemsWhereAsync(dbSettings.UserResourceCollectionId, "SharedResourceId", "0568B88C-3866-4CCA-97C8-B8E3F3D1FF3C");
			dbResponse.ReturnsForAnyArgs(ShareTestData.sharedResourcesData);

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

			dbService.CreateItemAsync<SharedResources>(
			   Arg.Any<SharedResources>(),
			   Arg.Any<string>()).ReturnsForAnyArgs<Document>(updatedDocument);

			//act
			var result = shareBusinessLogic.ShareResourceDataAsync(shareInput).Result;
			//assert
			Assert.Equal(expectedResult.PermaLink.Length, result.PermaLink.Length);
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
			var dbResponse = dynamicQueries.FindItemsWhereAsync(dbSettings.UserResourceCollectionId, "SharedResourceId", "0568B88C-3866-4CCA-97C8-B8E3F3D1FF3C");
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
		public void GetPermaLinkDataAsyncShouldValidate(string permaLinkInput, JArray shareProfileResponse, dynamic expectedResult)
		{
			var dbResponse = dynamicQueries.FindFieldWhereArrayContainsAsync(dbSettings.UserProfileCollectionId, Constants.SharedResource,
						Constants.PermaLink, permaLinkInput, Constants.ExpirationDate);
			dbResponse.ReturnsForAnyArgs<dynamic>(shareProfileResponse);

			var response = shareBusinessLogic.GetPermaLinkDataAsync(permaLinkInput);
			expectedResult = JsonConvert.SerializeObject(expectedResult);
			var actualResult = JsonConvert.SerializeObject(response.Result);
			//assert
			Assert.Equal(expectedResult, actualResult);
		}

		[Theory]
		[MemberData(nameof(ShareTestData.UpdatePersonalizedPlanData), MemberType = typeof(ShareTestData))]
		public void UpdatePersonalizedPlanShouldValidate(string planId, bool isShared, dynamic expectedResult)
		{
			var personalizedPlan = personalizedPlanBusinessLogic.GetPersonalizedPlan(planId);
			UserPersonalizedPlan userPlan = new UserPersonalizedPlan
			{
				OId="GFGDG8674"
			};
			var updatedPersonalizedPlan = personalizedPlanBusinessLogic.UpdatePersonalizedPlan(userPlan);

			var response = shareBusinessLogic.UpdatePersonalizedPlan(planId, isShared);
			//assert
			Assert.Equal(expectedResult, isShared);
		}

	}

}
