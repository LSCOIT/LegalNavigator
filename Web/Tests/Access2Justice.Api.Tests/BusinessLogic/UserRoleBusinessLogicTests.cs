using Access2Justice.Api.Tests.TestData;
using Access2Justice.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Access2Justice.Api.Tests.BusinessLogic
{
    public class UserRoleBusinessLogicTests
    {
        private readonly IDynamicQueries dbClient;
        private readonly ICosmosDbSettings dbSettings;
        private readonly IUserProfileBusinessLogic dbUserProfile;
        private readonly IHttpContextAccessor httpContextAccessor;
        public UserRoleBusinessLogicTests()
        {
            dbClient = Substitute.For<IDynamicQueries>();
            dbSettings = Substitute.For<ICosmosDbSettings>();
            dbUserProfile = Substitute.For<IUserProfileBusinessLogic>();
            httpContextAccessor = Substitute.For<IHttpContextAccessor>();

            dbSettings.UserRoleCollectionId.Returns("UserRole");
        }

        [Theory]
        [MemberData(nameof(UserRoleTestData.GetPermissionData), MemberType = typeof(UserRoleTestData))]
        public void GetUserRoleDataAsyncShouldValidate(string roleInformationId, dynamic expectedResult)
        {
			var profileResponse = dbUserProfile.GetUserProfileDataAsync(roleInformationId);
			profileResponse.ReturnsForAnyArgs(ShareTestData.UserProfileWithSharedResourceData);

			var dbResponse = dbClient.FindItemsWhereAsync(dbSettings.UserRoleCollectionId, Constants.Id, roleInformationId);
            //dbResponse.ReturnsForAnyArgs(expectedResult);

            //act

            //assert
            //Assert.Equal(expectedResult, actualResult);
        }
    }
}
