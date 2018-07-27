using Access2Justice.Api.BusinessLogic;
using Access2Justice.Api.Tests.TestData;
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
    public class StaticResourceBusinessLogicTests
    {
        private readonly IDynamicQueries dynamicQueries;
        private readonly ICosmosDbSettings cosmosDbSettings;
        private readonly IBackendDatabaseService backendDatabaseService;
        private readonly UserProfileBusinessLogic userProfileBusinessLogic;
        //Mocked input data
        private readonly JArray userProfile =
                   JArray.Parse(@"[{'id': '4589592f-3312-eca7-64ed-f3561bbb7398',
                    'oId': '709709e7t0r7t96', 'firstName': 'family1.2.1', 'lastName': 'family1.2.2','email': 'test@email.com','IsActive': 'Yes','CreatedBy': 'vn','CreatedTimeStamp':'01/01/2018 10:00:00','ModifiedBy': 'vn','ModifiedTimeStamp':'01/01/2018 10:00:00'}]");
        private readonly string expectedUserProfileId = "709709e7t0r7t96";
        private readonly JArray emptyData = JArray.Parse(@"[{}]");
        private readonly string createUserProfileObjOId = "709709e7t0r7123";
        private readonly string updateUserProfileObjOId = "99889789";
        private readonly string expectedUserId = "outlookoremailOId";

        public StaticResourceBusinessLogicTests()
        {
            dynamicQueries = Substitute.For<IDynamicQueries>();
            cosmosDbSettings = Substitute.For<ICosmosDbSettings>();
            backendDatabaseService = Substitute.For<IBackendDatabaseService>();
            userProfileBusinessLogic = new UserProfileBusinessLogic(dynamicQueries, cosmosDbSettings, backendDatabaseService);

            cosmosDbSettings.AuthKey.Returns("dummykey");
            cosmosDbSettings.Endpoint.Returns(new System.Uri("https://bing.com"));
            cosmosDbSettings.DatabaseId.Returns("dbname");
            cosmosDbSettings.TopicCollectionId.Returns("TopicCollection");
            cosmosDbSettings.ResourceCollectionId.Returns("ResourceCollection");
            cosmosDbSettings.UserProfileCollectionId.Returns("UserProfile");
            cosmosDbSettings.StaticResourceCollectionId.Returns("StaticResource");

            //mock user data
            userProfileObj.Id = "4589592f-3312-eca7-64ed-f3561bbb7398";
            userProfileObj.OId = "709709e7t0r7t96";
            userProfileObj.FirstName = "family1.2.1";
            userProfileObj.LastName = " family1.2.2";
            userProfileObj.EMail = "test@testmail.com";
            userProfileObj.IsActive = "Yes";
            userProfileObj.CreatedBy = "vn";
            userProfileObj.CreatedTimeStamp = "01/01/2018 10:00:00";
            userProfileObj.ModifiedBy = "vn";
            userProfileObj.ModifiedTimeStamp = "01/01/2018 10:00:00";
        }

        [Fact]
        public void GetUserProfileDataAsyncShouldReturnEmptyData()
        {
            //arrange      
            var dbResponse = dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.UserProfileCollectionId, "oId", expectedUserProfileId);
            dbResponse.ReturnsForAnyArgs<dynamic>(emptyData);

            //act
            var response = userProfileBusinessLogic.GetUserProfileDataAsync(expectedUserProfileId);
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.DoesNotContain(expectedUserProfileId, result, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void GetUserProfileDataAsyncTestsShouldReturnProperData()
        {
            //arrange      
            var dbResponse = dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.UserProfileCollectionId, "oId", expectedUserProfileId);
            dbResponse.ReturnsForAnyArgs<dynamic>(userProfile);

            //act
            var response = userProfileBusinessLogic.GetUserProfileDataAsync(expectedUserProfileId);
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains(expectedUserProfileId, result, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
