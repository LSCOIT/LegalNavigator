using Access2Justice.Api.BusinessLogic;
using Access2Justice.Api.Tests.TestData;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private readonly UserRoleBusinessLogic userRoleBusinessLogic;
        public UserRoleBusinessLogicTests()
        {
            dbClient = Substitute.For<IDynamicQueries>();
            dbSettings = Substitute.For<ICosmosDbSettings>();
            dbUserProfile = Substitute.For<IUserProfileBusinessLogic>();
            httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            userRoleBusinessLogic = new UserRoleBusinessLogic(dbClient,dbSettings, dbUserProfile, httpContextAccessor);
            
            dbSettings.UserRoleCollectionId.Returns("UserRole");
            
        }

        [Theory]
        [MemberData(nameof(UserRoleTestData.GetPermissionData), MemberType = typeof(UserRoleTestData))]
        public void GetUserRoleDataAsyncShouldValidate(UserProfile userProfile,JArray roleResponse, dynamic expectedResult)
        {
            dynamic profileResponse = dbUserProfile.GetUserProfileDataAsync(userProfile.OId).Returns<dynamic>(userProfile);
            var dbResponse = dbClient.FindItemsWhereAsync(dbSettings.UserRoleCollectionId, Constants.Id, userProfile.RoleInformationId.ToString());
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
        public void ValidateOUForRoleShouldValidate(string roleInformationId, string ou, JArray roleResponse, dynamic expectedResult)
        {
            var dbResponse = dbClient.FindItemsWhereAsync(dbSettings.UserRoleCollectionId, Constants.Id, roleInformationId);
            dbResponse.ReturnsForAnyArgs(roleResponse);

            //act
            var response = userRoleBusinessLogic.ValidateOUForRole(roleInformationId,ou);
            expectedResult = JsonConvert.SerializeObject(expectedResult);
            var actualResult = JsonConvert.SerializeObject(response.Result);

            //assert
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
