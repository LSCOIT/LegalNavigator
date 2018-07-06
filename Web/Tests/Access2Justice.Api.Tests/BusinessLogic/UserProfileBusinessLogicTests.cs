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
using Access2Justice.Api.Tests.TestData;

namespace Access2Justice.Api.Tests.BusinessLogic
{
    public class UserProfileBusinessLogicTests
    {
        private readonly IDynamicQueries dynamicQueries;
        private readonly ICosmosDbSettings cosmosDbSettings;
        private readonly IBackendDatabaseService backendDatabaseService;
        private readonly UserProfileBusinessLogic userProfileBusinessLogic;

        //Mocked input data
        private readonly JArray userProfile = UserPersonalizedPlanTestData.userProfile;
        private readonly string expectedUserProfileId = "709709e7t0r7t96";
        private readonly JArray emptyData = JArray.Parse(@"[{}]");
        private readonly JArray userProfilePersonalizedPlanData = UserPersonalizedPlanTestData.userProfilePersonalizedPlanData;
        private readonly JArray expectedUserProfilePersonalizedPlanData = UserPersonalizedPlanTestData.expectedUserProfilePersonalizedPlanData;
        private readonly JArray expectedUserProfilePersonalizedPlanUpdateData = UserPersonalizedPlanTestData.expectedUserProfilePersonalizedPlanUpdateData;
        private readonly JArray userProfileSavedResourcesData = UserPersonalizedPlanTestData.userProfileSavedResourcesData;
        private readonly JArray expectedUserProfileSavedResourcesData = UserPersonalizedPlanTestData.expectedUserProfileSavedResourcesData;
        private readonly JArray expectedUserProfileSavedResourcesUpdateData = UserPersonalizedPlanTestData.expectedUserProfileSavedResourcesUpdateData;

        public UserProfileBusinessLogicTests()
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
            actualResult = userProfileBusinessLogic.UpdateUserPersonalizedPlanAsync(id,inputJson).Result;

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
    }
}
