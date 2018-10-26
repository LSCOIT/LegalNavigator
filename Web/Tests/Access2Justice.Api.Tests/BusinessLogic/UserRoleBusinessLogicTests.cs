using Access2Justice.Api.Authentication;
using Access2Justice.Api.BusinessLogic;
using Access2Justice.Api.Tests.TestData;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private readonly UserRoleBusinessLogic userRoleBusinessLogic;
        private readonly IOptions<AzureAdOptions> azureOptions;
        public UserRoleBusinessLogicTests()
        {
            dbClient = Substitute.For<IDynamicQueries>();
            dbSettings = Substitute.For<ICosmosDbSettings>();
            dbUserProfile = Substitute.For<IUserProfileBusinessLogic>();
            httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            azureOptions = Substitute.For<IOptions<AzureAdOptions>>();
            userRoleBusinessLogic = new UserRoleBusinessLogic(dbClient, dbSettings, dbUserProfile, httpContextAccessor, azureOptions);

            dbSettings.RolesCollectionId.Returns("UserRole");

        }

        [Theory]
        [MemberData(nameof(UserRoleTestData.GetPermissionData), MemberType = typeof(UserRoleTestData))]
        public void GetUserRoleDataAsyncShouldValidate(UserProfile userProfile, JArray roleResponse, dynamic expectedResult)
        {
            dynamic profileResponse = dbUserProfile.GetUserProfileDataAsync(userProfile.OId).Returns<dynamic>(userProfile);
            var ids = new List<string>() { "guid1" };
            var dbResponse = dbClient.FindItemsWhereInClauseAsync(dbSettings.RolesCollectionId, Constants.Id, ids);
            dbResponse.ReturnsForAnyArgs(roleResponse);

            //act
            var response = userRoleBusinessLogic.GetPermissionDataAsync(userProfile.OId);
            expectedResult = JsonConvert.SerializeObject(expectedResult);
            var actualResult = JsonConvert.SerializeObject(response.Result);

            //assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [MemberData(nameof(UserRoleTestData.ValidateOUForRole), MemberType = typeof(UserRoleTestData))]
        public void ValidateOUForRoleShouldValidate(List<string> roleInformationId, string ou, JArray roleResponse, dynamic expectedResult)
        {
            var ids = new List<string>() { "guid1" };
            var dbResponse = dbClient.FindItemsWhereInClauseAsync(dbSettings.RolesCollectionId, Constants.Id, ids);
            dbResponse.ReturnsForAnyArgs(roleResponse);

            //act
            var response = userRoleBusinessLogic.ValidateOUForRole(roleInformationId, ou);
            expectedResult = JsonConvert.SerializeObject(expectedResult);
            var actualResult = JsonConvert.SerializeObject(response.Result);

            //assert
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
