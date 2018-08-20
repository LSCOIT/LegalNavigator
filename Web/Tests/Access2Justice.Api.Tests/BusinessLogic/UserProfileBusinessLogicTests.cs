using Access2Justice.Api.BusinessLogic;
using Access2Justice.Api.Tests.TestData;
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
                    'oId': '709709e7t0r7t96', 'firstName': 'family1.2.1', 'lastName': 'family1.2.2','email': 'test@email.com','IsActive': 'Yes','CreatedBy': 'vn','CreatedTimeStamp':'01/01/2018 10:00:00','ModifiedBy': 'vn','ModifiedTimeStamp':'01/01/2018 10:00:00'}]");
        private readonly string expectedUserProfileId = "709709e7t0r7t96";
        private readonly JArray emptyData = JArray.Parse(@"[{}]");
        private readonly string updateUserProfileObjOId = "99889789";
        private readonly string expectedUserId = "outlookoremailOId";

        private readonly JArray userProfilePersonalizedPlanData = UserPersonalizedPlanTestData.userProfilePersonalizedPlanData;
        private readonly JArray expectedUserProfilePersonalizedPlanData = UserPersonalizedPlanTestData.expectedUserProfilePersonalizedPlanData;
        private readonly JArray expectedUserProfilePersonalizedPlanUpdateData = UserPersonalizedPlanTestData.expectedUserProfilePersonalizedPlanUpdateData;
        private readonly JArray userProfileSavedResourcesData = UserPersonalizedPlanTestData.userProfileSavedResourcesData;
        private readonly JArray expectedUserProfileSavedResourcesData = UserPersonalizedPlanTestData.expectedUserProfileSavedResourcesData;
        private readonly JArray expectedUserProfileSavedResourcesUpdateData = UserPersonalizedPlanTestData.expectedUserProfileSavedResourcesUpdateData;
        private readonly JArray userPlanData = UserPersonalizedPlanTestData.userPlanData;
        private readonly JArray expectedUserPlanData = UserPersonalizedPlanTestData.expectedUserPlanData;
        private readonly JArray expectedUserPlanUpdateData = UserPersonalizedPlanTestData.expectedUserPlanUpdateData;

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
            cosmosDbSettings.UserProfileCollectionId.Returns("UserProfile");

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

        private object ResourceDeserialized(UserProfile userProfile)
        {
            var serializedResult = JsonConvert.SerializeObject(userProfile);
            return JsonConvert.DeserializeObject<object>(serializedResult);
        }
        
        [Fact]
        public void CreateUserSavedResourcesAsyncTestsShouldReturnProperData()
        {
            //arrange
            var userProfileSavedResources = this.userProfileSavedResourcesData;
            var resource = JsonConvert.DeserializeObject<ProfileResources>(JsonConvert.SerializeObject(this.userProfileSavedResourcesData.First));
            Document document = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(userProfileSavedResources[0].ToString()));
            document.LoadFrom(reader);
            dynamic actualUserSavedResourcesData = null;
            var dbResponse = backendDatabaseService.CreateItemAsync<dynamic>(userProfileSavedResources, cosmosDbSettings.ResourceCollectionId).ReturnsForAnyArgs(document);

            //act
            actualUserSavedResourcesData = userProfileBusinessLogic.CreateUserSavedResourcesAsync(resource).Result;

            //assert
            Assert.Equal(expectedUserProfileSavedResourcesData[0].ToString(), actualUserSavedResourcesData.ToString());
        }

        [Fact]
        public void UpdateUserSavedResourcesAsyncTestsShouldReturnProperData()
        {
            //arrange
            var userProfileSavedResources = this.userProfileSavedResourcesData;
            var resource = JsonConvert.SerializeObject(this.userProfileSavedResourcesData.First);
            var userUIDocument = JsonConvert.DeserializeObject<dynamic>(resource);
            var inputJson = userUIDocument;
            inputJson = JsonConvert.DeserializeObject<ProfileResources>(JsonConvert.SerializeObject(inputJson));
            string id = userUIDocument.id;
            string oId = userUIDocument.oId;
            string type = userUIDocument.type;
            List<string> resourcesPropertyNames = new List<string>() { Constants.OId, Constants.Type };
            List<string> resourcesValues = new List<string>() { oId, type };
            Document document = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(expectedUserProfileSavedResourcesUpdateData[0].ToString()));
            document.LoadFrom(reader);
            dynamic actualResult = null;
            dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.ResourceCollectionId, resourcesPropertyNames, resourcesValues).ReturnsForAnyArgs(expectedUserProfileSavedResourcesUpdateData);
            backendDatabaseService.UpdateItemAsync<dynamic>(id, document, cosmosDbSettings.ResourceCollectionId).ReturnsForAnyArgs(document);

            //act
            actualResult = userProfileBusinessLogic.UpdateUserSavedResourcesAsync(Guid.Parse(id), inputJson).Result;

            //assert
            Assert.Equal(expectedUserProfileSavedResourcesUpdateData[0].ToString(), actualResult.ToString());
        }

        [Fact]
        public void GetUserResourceProfileDataAsyncWithProperData()
        {
            //arrange
            var dbResponse = dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.ResourceCollectionId, "oId", expectedUserId);
            dbResponse.ReturnsForAnyArgs<dynamic>(userProfilePersonalizedPlanData);

            //act
            var response = userProfileBusinessLogic.GetUserResourceProfileDataAsync(expectedUserProfileId,"plan");
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains(expectedUserId, result, StringComparison.InvariantCulture);
        }

        [Fact]
        public void GetUserResourceProfileDataAsyncEmptyData()
        {
            //arrange
            var dbResponse = dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.ResourceCollectionId, "oId", expectedUserProfileId);
            dbResponse.ReturnsForAnyArgs<dynamic>(emptyData);

            //act
            var response = userProfileBusinessLogic.GetUserResourceProfileDataAsync(string.Empty, "resources");
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains("", result, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
