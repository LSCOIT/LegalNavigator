using Access2Justice.CosmosDb.Interfaces;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Api.BusinessLogic;
using System;
using NSubstitute;
using Xunit;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.Azure.Documents;
using System.IO;
using System.Collections.Generic;
using Access2Justice.Shared.Models;

namespace Access2Justice.Api.Tests.BusinessLogic
{
    public class UserProfileBusinessLogicTests
    {
        private readonly IDynamicQueries dynamicQueries;
        private readonly ICosmosDbSettings cosmosDbSettings;
        private readonly IBackendDatabaseService backendDatabaseService;
        private readonly UserProfileBusinessLogic userProfileBusinessLogic;
        private readonly UserProfile userProfileObj;
        //Mocked input data
        private readonly JArray userProfile =
                   JArray.Parse(@"[{'id': '4589592f-3312-eca7-64ed-f3561bbb7398',
                    'oId': '709709e7t0r7t96', 'firstName': 'family1.2.1', 'lastName': '5c035d27-2fdb-9776-6236-70983a918431','email': 'f102bfae-362d-4659-aaef-956c391f79de'}]");
        private readonly string expectedUserProfileId = "709709e7t0r7t96";
        private readonly JArray emptyData = JArray.Parse(@"[{}]");

        public UserProfileBusinessLogicTests()
        {
            dynamicQueries = Substitute.For<IDynamicQueries>();
            cosmosDbSettings = Substitute.For<ICosmosDbSettings>();
            backendDatabaseService = Substitute.For<IBackendDatabaseService>();
            userProfileBusinessLogic = new UserProfileBusinessLogic(dynamicQueries, cosmosDbSettings, backendDatabaseService);
            userProfileObj = new UserProfile();

            cosmosDbSettings.AuthKey.Returns("dummykey");
            cosmosDbSettings.Endpoint.Returns(new System.Uri("https://bing.com"));
            cosmosDbSettings.DatabaseId.Returns("dbname");
            cosmosDbSettings.TopicCollectionId.Returns("TopicCollection");
            cosmosDbSettings.ResourceCollectionId.Returns("ResourceCollection");

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
            Assert.Contains("[{}]", result, StringComparison.InvariantCultureIgnoreCase);
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

        [Fact]
        public void CreateUserProfileDataAsyncTestsShouldCreateProperData()
        {
            List<dynamic> userprofiles = new List<dynamic>();
            List<dynamic> userprofiles2 = new List<dynamic>();

            //arrange
            userProfileObj.OId = "709709e7t0r7123";
            var serializedResult = JsonConvert.SerializeObject(userProfileObj);
            var userDeserialisedObjects = JsonConvert.DeserializeObject<object>(serializedResult);
            var result = backendDatabaseService.CreateItemAsync(userDeserialisedObjects, cosmosDbSettings.UserProfileCollectionId);
            userprofiles.Add(result);

            ////act         
            var response = userProfileBusinessLogic.CreateUserProfileDataAsync(userProfileObj);
            userprofiles2.Add(response);
            // string results = JsonConvert.SerializeObject(response);

            //assert
            Assert.Equal(userprofiles.Count, userprofiles2.Count);
        }

        [Fact]
        public void CreateUserProfileDataAsyncTestsShouldNotCreateDuplicateData()
        {
            List<dynamic> userprofiles = new List<dynamic>();
            List<dynamic> userprofiles2 = new List<dynamic>();

            //arrange            
            var serializedResult = JsonConvert.SerializeObject(userProfileObj);
            var userDeserialisedObjects = JsonConvert.DeserializeObject<object>(serializedResult);
            var result = backendDatabaseService.CreateItemAsync(userDeserialisedObjects, cosmosDbSettings.UserProfileCollectionId);
            userprofiles.Add(result);

            ////act         
            var response = userProfileBusinessLogic.CreateUserProfileDataAsync(userProfileObj);
            userprofiles2.Add(response);

            //assert            
            Assert.Equal(userprofiles.Count, userprofiles2.Count);
        }

        [Fact]
        public void UpdateUserProfileDataAsyncTestsShouldUpdateProperData()
        {
            List<dynamic> userprofiles = new List<dynamic>();
            List<dynamic> userprofiles2 = new List<dynamic>();

            //arrange
            var serializedResult = JsonConvert.SerializeObject(userProfileObj);
            var userDeserialisedObjects = JsonConvert.DeserializeObject<object>(serializedResult);
            var result = backendDatabaseService.UpdateItemAsync(userProfileObj.Id, userDeserialisedObjects, cosmosDbSettings.UserProfileCollectionId);

            userprofiles.Add(result);

            ////act         
            var response = userProfileBusinessLogic.UpdateUserProfileDataAsync(userProfileObj, userProfileObj.Id);
            userprofiles2.Add(response);

            //assert
            Assert.Equal(userprofiles.ToString(), userprofiles2.ToString());            
        }

        [Fact]
        public void UpdateUserProfileDataAsyncTestsShouldNotUpdateData()
        {
            List<dynamic> userprofiles = new List<dynamic>();
            List<dynamic> userprofiles2 = new List<dynamic>();

            //arrange
            userProfileObj.Id = "99889789"; // Id is new, so should not update the data for this id
            var resultUP = userProfileBusinessLogic.GetUserProfileDataAsync(userProfileObj.Id);
            
            var serializedResult = JsonConvert.SerializeObject(userProfileObj);
            var userDeserialisedObjects = JsonConvert.DeserializeObject<object>(serializedResult);
            var result = backendDatabaseService.UpdateItemAsync(userProfileObj.Id, userDeserialisedObjects, cosmosDbSettings.UserProfileCollectionId);
            userprofiles.Add(result);

            ////act         
            var response = userProfileBusinessLogic.UpdateUserProfileDataAsync(userProfileObj, userProfileObj.Id);
            userprofiles2.Add(response);

            //assert
            Assert.NotEqual(userprofiles.ToString(), userprofiles2.ToString());
        }
    }
}
