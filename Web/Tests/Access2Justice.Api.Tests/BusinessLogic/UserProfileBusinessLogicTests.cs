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
        private readonly string createUserProfileObjOId = "709709e7t0r7123";
        private readonly string updateUserProfileObjOId = "99889789";

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

        [Fact]
        public void CreateUserProfileDataAsyncTestsShouldCreateProperData()
        {
            var userprofiles = new List<dynamic>();
            var userprofiles2 = new List<dynamic>();

            //arrange
            userProfileObj.OId = createUserProfileObjOId;
            var result = backendDatabaseService.CreateItemAsync(ResourceDeserialized(userProfileObj), cosmosDbSettings.UserProfileCollectionId);
            userprofiles.Add(result);

            //act         
            var response = userProfileBusinessLogic.CreateUserProfileDataAsync(userProfileObj);            
            userprofiles2.Add(response);

            //assert
            Assert.Equal(userprofiles.Count, userprofiles2.Count);
        }

        [Fact]
        public void CreateUserProfileDataAsyncTestsShouldNotCreateDuplicateData()
        {
            var userprofiles = new List<dynamic>();
            var userprofiles2 = new List<dynamic>();

            //arrange            
            var result = backendDatabaseService.CreateItemAsync(ResourceDeserialized(userProfileObj), cosmosDbSettings.UserProfileCollectionId);
            userprofiles.Add(result);

            //act         
            var response = userProfileBusinessLogic.CreateUserProfileDataAsync(userProfileObj);
            userprofiles2.Add(response);

            //assert            
            Assert.Equal(userprofiles.Count, userprofiles2.Count);
        }

        [Fact]
        public void UpdateUserProfileDataAsyncTestsShouldUpdateProperData()
        {
            var userprofiles = new List<dynamic>();
            var userprofiles2 = new List<dynamic>();

            //arrange
            var result = backendDatabaseService.UpdateItemAsync(userProfileObj.Id, ResourceDeserialized(userProfileObj), cosmosDbSettings.UserProfileCollectionId);
            userprofiles.Add(result);

            //act         
            var response = userProfileBusinessLogic.UpdateUserProfileDataAsync(userProfileObj, userProfileObj.Id);
            userprofiles2.Add(response);

            //assert
            Assert.Equal(userprofiles.ToString(), userprofiles2.ToString());
        }

        [Fact]
        public void UpdateUserProfileDataAsyncTestsShouldNotUpdateData()
        {
            var userprofiles = new List<dynamic>();
            var userprofiles2 = new List<dynamic>();
            
            //arrange
            userProfileObj.OId = updateUserProfileObjOId; // Id is new, so should not update the data for this id
            var result = backendDatabaseService.UpdateItemAsync(userProfileObj.Id, ResourceDeserialized(userProfileObj), cosmosDbSettings.UserProfileCollectionId);
            userprofiles.Add(result);

            //act
            var response = userProfileBusinessLogic.UpdateUserProfileDataAsync(userProfileObj, userProfileObj.Id);
            userprofiles2.Add(response);

            //assert
            Assert.Equal(userprofiles.ToString(), userprofiles2.ToString());
        }
        private object ResourceDeserialized(UserProfile userProfile)
        {
            var serializedResult = JsonConvert.SerializeObject(userProfile);
            return JsonConvert.DeserializeObject<object>(serializedResult);
        }

        [Fact]
        public void UpsertUserPersonalizedPlanAsyncTestsShouldReturnProperData()
        {
            //arrange
            var userProfilePersonalizedPlan = this.userProfilePersonalizedPlanData;
            var resource = JsonConvert.SerializeObject(userProfilePersonalizedPlan);
            var userUIDocument = JsonConvert.DeserializeObject<dynamic>(resource);
            var inputJson = userUIDocument[0];
            string id = userUIDocument[0].id;
            string oId = userUIDocument[0].oId;
            string planId = userUIDocument[0].planId;
            List<string> propertyNames = new List<string>() { Constants.OId, Constants.PlanId };
            List<string> values = new List<string>() { oId, planId };
            dynamic actualResult = null;
            Document document = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(expectedUserProfilePersonalizedPlanUpdateData[0].ToString()));
            document.LoadFrom(reader);
            dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.ResourceCollectionId, propertyNames, values).ReturnsForAnyArgs(expectedUserProfilePersonalizedPlanUpdateData);
            backendDatabaseService.UpdateItemAsync<dynamic>(id, document, cosmosDbSettings.ResourceCollectionId).ReturnsForAnyArgs(document);

            //act
            actualResult = userProfileBusinessLogic.UpsertUserPersonalizedPlanAsync(inputJson).Result;
            string result = JsonConvert.SerializeObject(actualResult.Result);
            var response = JsonConvert.DeserializeObject<dynamic>(result);

            //assert
            Assert.Contains(planId, response.ToString());
        }

        [Fact]
        public void CreateUserPersonalizedPlanAsyncTestsShouldReturnProperData()
        {
            //arrange
            var userProfilePersonalizedPlan = this.userProfilePersonalizedPlanData;
            var resource = JsonConvert.SerializeObject(userProfilePersonalizedPlan);
            Document document = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(userProfilePersonalizedPlan[0].ToString()));
            document.LoadFrom(reader);
            dynamic actualUserPersonalizedPlanData = null;
            var dbResponse = backendDatabaseService.CreateItemAsync<dynamic>(userProfilePersonalizedPlan, cosmosDbSettings.ResourceCollectionId).ReturnsForAnyArgs(document);

            //act
            actualUserPersonalizedPlanData = userProfileBusinessLogic.CreateUserPersonalizedPlanAsync(resource).Result;

            //assert
            Assert.Equal(expectedUserProfilePersonalizedPlanData[0].ToString(), actualUserPersonalizedPlanData.ToString());
        }

        [Fact]
        public void UpdateUserPersonalizedPlanAsyncTestsShouldReturnProperData()
        {
            //arrange
            var userProfilePersonalizedPlan = this.userProfilePersonalizedPlanData;
            var resource = JsonConvert.SerializeObject(userProfilePersonalizedPlan);
            var userUIDocument = JsonConvert.DeserializeObject<dynamic>(resource);
            var inputJson = userUIDocument[0];
            string id = userUIDocument[0].id;
            string oId = userUIDocument[0].oId;
            string planId = userUIDocument[0].planId;
            List<string> propertyNames = new List<string>() { Constants.OId, Constants.PlanId };
            List<string> values = new List<string>() { oId, planId };
            Document document = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(expectedUserProfilePersonalizedPlanUpdateData[0].ToString()));
            document.LoadFrom(reader);
            dynamic actualResult = null;
            dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.ResourceCollectionId, propertyNames, values).ReturnsForAnyArgs(expectedUserProfilePersonalizedPlanUpdateData);
            backendDatabaseService.UpdateItemAsync<dynamic>(id, document, cosmosDbSettings.ResourceCollectionId).ReturnsForAnyArgs(document);

            //act
            actualResult = userProfileBusinessLogic.UpdateUserPersonalizedPlanAsync(id, inputJson).Result;

            //assert
            Assert.Equal(expectedUserProfilePersonalizedPlanUpdateData[0].ToString(), actualResult.ToString());
        }

        [Fact]
        public void CreateUserSavedResourcesAsyncTestsShouldReturnProperData()
        {
            //arrange
            var userProfileSavedResources = this.userProfileSavedResourcesData;
            var resource = JsonConvert.SerializeObject(userProfileSavedResources);
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
            var resource = JsonConvert.SerializeObject(userProfileSavedResources);
            var userUIDocument = JsonConvert.DeserializeObject<dynamic>(resource);
            var inputJson = userUIDocument[0];
            string id = userUIDocument[0].id;
            string oId = userUIDocument[0].oId;
            string type = userUIDocument[0].type;
            List<string> resourcesPropertyNames = new List<string>() { Constants.OId, Constants.Type };
            List<string> resourcesValues = new List<string>() { oId, type };
            Document document = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(expectedUserProfileSavedResourcesUpdateData[0].ToString()));
            document.LoadFrom(reader);
            dynamic actualResult = null;
            dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.ResourceCollectionId, resourcesPropertyNames, resourcesValues).ReturnsForAnyArgs(expectedUserProfileSavedResourcesUpdateData);
            backendDatabaseService.UpdateItemAsync<dynamic>(id, document, cosmosDbSettings.ResourceCollectionId).ReturnsForAnyArgs(document);

            //act
            actualResult = userProfileBusinessLogic.UpdateUserSavedResourcesAsync(id, inputJson).Result;

            //assert
            Assert.Equal(expectedUserProfileSavedResourcesUpdateData[0].ToString(), actualResult.ToString());
        }

        [Fact]
        public void UpsertUserPlanAsyncTestsShouldReturnProperData()
        {
            //arrange
            var userProfilePlan = this.userPlanData;
            var resource = JsonConvert.SerializeObject(userProfilePlan);
            var userUIDocument = JsonConvert.DeserializeObject<dynamic>(resource);
            var inputJson = userUIDocument[0];
            string id = userUIDocument[0].id;
            string oId = userUIDocument[0].oId;
            List<string> propertyNames = new List<string>() { Constants.OId, Constants.Id };
            List<string> values = new List<string>() { oId, id };
            dynamic actualResult = null;
            Document document = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(expectedUserPlanUpdateData[0].ToString()));
            document.LoadFrom(reader);
            dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.ResourceCollectionId, propertyNames, values).ReturnsForAnyArgs(expectedUserPlanUpdateData);
            backendDatabaseService.UpdateItemAsync<dynamic>(id, document, cosmosDbSettings.ResourceCollectionId).ReturnsForAnyArgs(document);

            //act
            actualResult = userProfileBusinessLogic.UpsertUserPlanAsync(inputJson).Result;
            string result = JsonConvert.SerializeObject(actualResult.Result);
            var response = JsonConvert.DeserializeObject<dynamic>(result);

            //assert
            Assert.Contains(id, response.ToString());
        }

        [Fact]
        public void CreateUserPlanAsyncTestsShouldReturnProperData()
        {
            //arrange
            var userProfilePlan = this.userPlanData;
            var resource = JsonConvert.SerializeObject(userProfilePlan);
            Document document = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(userProfilePlan[0].ToString()));
            document.LoadFrom(reader);
            dynamic actualUserPlanData = null;
            var dbResponse = backendDatabaseService.CreateItemAsync<dynamic>(userProfilePlan, cosmosDbSettings.ResourceCollectionId).ReturnsForAnyArgs(document);

            //act
            actualUserPlanData = userProfileBusinessLogic.CreateUserPlanAsync(resource).Result;

            //assert
            Assert.Equal(expectedUserPlanData[0].ToString(), actualUserPlanData.ToString());
        }

        [Fact]
        public void UpdateUserPlanAsyncTestsShouldReturnProperData()
        {
            //arrange
            var userProfilePlan = this.userPlanData;
            var resource = JsonConvert.SerializeObject(userProfilePlan);
            var userUIDocument = JsonConvert.DeserializeObject<dynamic>(resource);
            var inputJson = userUIDocument[0];
            string id = userUIDocument[0].id;
            string oId = userUIDocument[0].oId;
            List<string> propertyNames = new List<string>() { Constants.OId, Constants.Id };
            List<string> values = new List<string>() { oId, id };
            Document document = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(expectedUserPlanUpdateData[0].ToString()));
            document.LoadFrom(reader);
            dynamic actualResult = null;
            dynamicQueries.FindItemsWhereAsync(cosmosDbSettings.ResourceCollectionId, propertyNames, values).ReturnsForAnyArgs(expectedUserPlanUpdateData);
            backendDatabaseService.UpdateItemAsync<dynamic>(id, document, cosmosDbSettings.ResourceCollectionId).ReturnsForAnyArgs(document);

            //act
            actualResult = userProfileBusinessLogic.UpdateUserPlanAsync(id, inputJson).Result;

            //assert
            Assert.Equal(expectedUserPlanUpdateData[0].ToString(), actualResult.ToString());
        }
    }
}
