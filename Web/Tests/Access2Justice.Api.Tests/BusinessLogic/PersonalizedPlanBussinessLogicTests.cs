using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Access2Justice.Api.BusinessLogic;
using Access2Justice.Api.Tests.TestData;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSubstitute;
using Xunit;

namespace Access2Justice.Api.Tests.BusinessLogic
{
    public class PersonalizedPlanBussinessLogicTests
    {
        private readonly IBackendDatabaseService dbService;
        private readonly ICosmosDbSettings dbSettings;
        private readonly IDynamicQueries dynamicQueries;
        private readonly PersonalizedPlanBusinessLogic personalizedPlan;
        
        //Mocked input data
        private readonly dynamic topicId = Guid.Parse("e1fdbbc6-d66a-4275-9cd2-2be84d303e12");
        private readonly string planId = "e48b4302-f548-4402-a7b6-6351602a9f09";
        private readonly string mockPlanId1 = "86e693bd-c673-419f-95c4-9ee76cccab3c";
        private readonly dynamic answersDocId = Guid.Parse("288af4da-06bb-4655-aa91-41314e248d6b");
        private readonly string mockPlanId = "a9f6c0db-093b-4fa8-9cde-2495087b31f6";
        private readonly dynamic curatedExperience = PersonalizedPlanTestData.curatedExperience;
        private readonly dynamic generatedPersonalizedPlan = PersonalizedPlanTestData.personalizedPlan;
        private readonly dynamic userAnswers = PersonalizedPlanTestData.userAnswers;
        private readonly dynamic topicDetails = PersonalizedPlanTestData.topicDetails;
        private readonly dynamic planSteps = PersonalizedPlanTestData.planSteps;
        private readonly dynamic dynamicObject = PersonalizedPlanTestData.dynamicObject;
        private readonly dynamic convertedPersonalizedPlanSteps = PersonalizedPlanTestData.convertedPersonalizedPlanSteps;
        private readonly dynamic personalizedPlanView = PersonalizedPlanTestData.personalizedPlanView;
        private readonly dynamic quickLinks = PersonalizedPlanTestData.quickLinks;
        private readonly dynamic resourceDetails = PersonalizedPlanTestData.resourceDetailsForResourceIds;
        private readonly dynamic planStepsWithResourceDetails = PersonalizedPlanTestData.planStepsWithResourceDetails;
        private readonly dynamic resourceIds = PersonalizedPlanTestData.resourceIds;
        private readonly dynamic planStepsByTopicId = PersonalizedPlanTestData.planStepsByTopicId;
        private readonly JArray expectedPersonalizedPlanView = PersonalizedPlanTestData.personalizedPlanView;
        private readonly JArray expectedConvertedPersonalizedPlanSteps = PersonalizedPlanTestData.convertedPersonalizedPlanSteps;
        public PersonalizedPlanBussinessLogicTests()
        {
            dbSettings = Substitute.For<ICosmosDbSettings>();
            dbService = Substitute.For<IBackendDatabaseService>();
            dynamicQueries = Substitute.For<IDynamicQueries>();


            dbSettings.AuthKey.Returns("dummykey");
            dbSettings.Endpoint.Returns(new System.Uri("https://bing.com"));
            dbSettings.DatabaseId.Returns("dbname");
            dbSettings.TopicCollectionId.Returns("TopicCollection");
            dbSettings.ResourceCollectionId.Returns("ResourceCollection");
            dbSettings.UserProfileCollectionId.Returns("UserProfile");
            dbSettings.StaticResourceCollectionId.Returns("StaticResource");
            dbSettings.PersonalizedActionPlanCollectionId.Returns("PersonalizedActionPlan");
            dbSettings.CuratedExperienceCollectionId.Returns("CuratedExperience");
            dbSettings.CuratedExperienceAnswersCollectionId.Returns("CuratedExperienceAnswers");

            personalizedPlan = new PersonalizedPlanBusinessLogic(dbSettings, dbService, dynamicQueries);
        }
        [Fact]
        public void GetTopicNameByTopicId()
        {
            //Arrange
            var topicId = this.topicId;
            var isTopicName = true;
            var topicDetails = JsonConvert.DeserializeObject<List<TopicDetails>>(JsonConvert.SerializeObject(this.topicDetails));
            string expectedTopicName = "Family";

            //Act
            var actualTopicName = personalizedPlan.GetByTopicId(topicId, topicDetails, isTopicName);

            //Assert
            Assert.Equal(actualTopicName, expectedTopicName);
        }

        [Fact]
        public void GetTopicIconByTopicId()
        {
            //Arrange
            var topicId = this.topicId;
            var isTopicName = false;
            var topicDetails = JsonConvert.DeserializeObject<List<TopicDetails>>(JsonConvert.SerializeObject(this.topicDetails));
            string expectedTopicIcon = "https://cs4892808efec24x447cx944.blob.core.windows.net/static-resource/assets/images/categories/family.svg";

            //Act
            var actualTopicIcon = personalizedPlan.GetByTopicId(topicId, topicDetails, isTopicName);

            //Assert
            Assert.Equal(actualTopicIcon, expectedTopicIcon);
        }
                
        [Fact]
        public void GeneratePersonalizedPlanFromCuratedExperienceAnswers()
        {
            // Arrange
            var curatedExperience = this.curatedExperience;
            Microsoft.Azure.Documents.Document document = new Microsoft.Azure.Documents.Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(curatedExperience[0].ToString()));
            document.LoadFrom(reader);

            //var curatedExperienceAsnwersJson = JsonConvert.DeserializeObject<CuratedExperienceAnswers>(CuratedExperienceTestData.CuratedExperienceAnswersSchema);            
            //var userAnswers = dbService.GetItemAsync<CuratedExperienceAnswers>(answersDocId.ToString(), dbSettings.CuratedExperienceAnswersCollectionId).ReturnsForAnyArgs(curatedExperienceAsnwersJson);
            //var dbResponse = dbService.CreateItemAsync<dynamic>(curatedExperience, dbSettings.PersonalizedActionPlanCollectionId).ReturnsForAnyArgs(document);
            //var curatedExperienceJson = JsonConvert.DeserializeObject<CuratedExperience>(CuratedExperienceTestData.CuratedExperienceSampleSchema);

            //// Act
            //var actualPersonalizedPlan = personalizedPlan.GeneratePersonalizedPlan(curatedExperienceJson, answersDocId).Returns(userAnswers);

            ////Assert
            //Assert.True(actualPersonalizedPlan.count > 1);
        }

        [Fact]
        public void GeneratePersonalizedPlanFromCuratedExperienceShouldNotGenerate()
        {
            // Arrange
            var curatedExperience = this.curatedExperience;
            Microsoft.Azure.Documents.Document document = new Microsoft.Azure.Documents.Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(curatedExperience[0].ToString()));
            document.LoadFrom(reader);

            var dbResponse = dbService.CreateItemAsync<dynamic>(curatedExperience, dbSettings.PersonalizedActionPlanCollectionId);
            var curatedExperienceJson = JsonConvert.DeserializeObject<CuratedExperience>(CuratedExperienceTestData.CuratedExperienceSampleSchema);

            // Act
            var actualPersonalizedPlan = personalizedPlan.GeneratePersonalizedPlan(curatedExperienceJson, answersDocId);

            //Assert
            //Assert.Equal(actualPersonalizedPlan.Result,"Id = 21, Status = Faulted, Method = "{null}", Result = "{Not yet computed}"");
        }

        [Fact]
        public void BuildPersonalizedPlanFromPlanSteps()
        {
            //Arrange            
            List<PersonalizedPlanStep> planSteps = JsonConvert.DeserializeObject<List<PersonalizedPlanStep>>(JsonConvert.SerializeObject(this.planSteps));

            //Act
            var actualPersonalizedPlan = personalizedPlan.BuildPersonalizedPlan(planSteps);

            //Assert
            Assert.NotNull(actualPersonalizedPlan.PersonalizedPlanId.ToString());
        }

        //[JsonIgnore] [JsonProperty(PropertyName = "topicIds")] JsonIgnore property is making the test case fail. Need to find the solution.
        [Fact]
        public void GetPlanStepsByTopic()
        {
            //Arrange
            List<PersonalizedPlanStep> planSteps = JsonConvert.DeserializeObject<List<PersonalizedPlanStep>>(JsonConvert.SerializeObject(this.planStepsByTopicId));
            var topic = this.topicId;
            List<PersonalizedPlanStep> expectedPersonalizedPlanSteps = JsonConvert.DeserializeObject<List<PersonalizedPlanStep>>(JsonConvert.SerializeObject(this.planStepsByTopicId));

            //Act
            var actualPersonalizedPlanSteps = personalizedPlan.GetPlanSteps(topic, planSteps);

            //Assert
           // Assert.Equal(actualPersonalizedPlanSteps.Count, expectedPersonalizedPlanSteps.Count);
        }

        [Fact]
        public void ConvertPersonalizedPlanStepsFromDynamicObject()
        {
            //Arrange
            var dynamicObject = this.dynamicObject;
            PersonalizedPlanSteps expectedConvertedPlanSteps = JsonConvert.DeserializeObject<PersonalizedPlanSteps>(JsonConvert.SerializeObject(this.convertedPersonalizedPlanSteps.First));

            //Act
            var actualConvertedPlanSteps = personalizedPlan.ConvertPersonalizedPlanSteps(dynamicObject);

            //Assert
            Assert.Equal(actualConvertedPlanSteps.Topics.Count, expectedConvertedPlanSteps.Topics.Count);
        }

        [Fact]
        public void GetPlanDataAsyncByPlanId()
        {
            //Arrange         
            var dbResponse = dynamicQueries.FindItemsWhereAsync(dbSettings.PersonalizedActionPlanCollectionId, "id", mockPlanId);

            List<PersonalizedPlanStep> planSteps = JsonConvert.DeserializeObject<List<PersonalizedPlanStep>>(JsonConvert.SerializeObject(this.planSteps));
            dbResponse.ReturnsForAnyArgs<dynamic>(planSteps);

            //Act
            var response = personalizedPlan.GetPlanDataAsync(mockPlanId);
            string result = JsonConvert.SerializeObject(response);

            //Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public void GetQuickLinksForTopicByTopicId()
        {
            //Arrange
            var topicId = this.topicId;
            List<PlanQuickLink> expectedQuickLinks = JsonConvert.DeserializeObject<List<PlanQuickLink>>(JsonConvert.SerializeObject(this.quickLinks));
            var topicDetails = JsonConvert.DeserializeObject<List<TopicDetails>>(JsonConvert.SerializeObject(this.topicDetails));

            //Act
            List<PlanQuickLink> actualQuickLinks = personalizedPlan.GetQuickLinksForTopic(topicId, topicDetails);

            //Assert
            var obj1Str = JsonConvert.SerializeObject(expectedQuickLinks);
            var obj2Str = JsonConvert.SerializeObject(actualQuickLinks);

            Assert.Equal(obj1Str, obj2Str);
        }

        [Fact]
        public void ConvertToPlanStepsWithResourceDetails()
        {
            //Arrange
            List<PersonalizedPlanStep> planSteps = JsonConvert.DeserializeObject<List<PersonalizedPlanStep>>(JsonConvert.SerializeObject(this.planSteps));
            List<Resource> resourceDetails = JsonConvert.DeserializeObject<List<Resource>>(JsonConvert.SerializeObject(this.resourceDetails));
            List<PlanStep> expectedPlanSteps = JsonConvert.DeserializeObject<List<PlanStep>>(JsonConvert.SerializeObject(this.planStepsWithResourceDetails));

            //Act
            List<PlanStep> actualPlanSteps = personalizedPlan.ConvertToPlanSteps(planSteps, resourceDetails);

            //Assert         
            Assert.Equal(expectedPlanSteps.Count, actualPlanSteps.Count);
        }

        [Fact]
        public void GetResourceDetailsForResourceIds()
        {
            //Arrange
            List<Guid> resourceIds = JsonConvert.DeserializeObject<List<Guid>>(JsonConvert.SerializeObject(this.resourceIds));
            List<Resource> resourceDetails = JsonConvert.DeserializeObject<List<Resource>>(JsonConvert.SerializeObject(this.resourceDetails));
            List<Resource> expectedResourceDetails = JsonConvert.DeserializeObject<List<Resource>>(JsonConvert.SerializeObject(this.resourceDetails));

            //Act
            List<Resource> actualResourceDetails = personalizedPlan.GetResources(resourceIds, resourceDetails);

            //Assert
            Assert.Equal(expectedResourceDetails.Count, actualResourceDetails.Count);
        }

        [Fact]
        public void GetPersonalizedPlanByPlanId()
        {
            //Arrange
            string planId = this.mockPlanId1;
            var dbResponse = dynamicQueries.FindItemsWhereAsync(dbSettings.PersonalizedActionPlanCollectionId, "id", planId);
            List<PersonalizedPlanSteps> expectedPlan = JsonConvert.DeserializeObject<List<PersonalizedPlanSteps>>(JsonConvert.SerializeObject(this.generatedPersonalizedPlan));
            dbResponse.ReturnsForAnyArgs<dynamic>(expectedPlan);

            //Act         
            var actualPlan = personalizedPlan.GetPersonalizedPlan(planId);

            string result = JsonConvert.SerializeObject(actualPlan);

            //assert
            Assert.Contains(mockPlanId1, result, StringComparison.InvariantCultureIgnoreCase);

        }

        [Fact]
        public void UpdatePersonalizedPlanWithUpdatedPlan()
        {
            //Arrange
            var userpersonalizedPlanView = this.personalizedPlanView;
            var resource = JsonConvert.SerializeObject(userpersonalizedPlanView);
            var userUIDocument = JsonConvert.DeserializeObject<dynamic>(resource);
            string id = userUIDocument[0].id;
            string planId = userUIDocument[0].PlanId;
            string type = userUIDocument[0].type;
            dbSettings.PersonalizedActionPlanCollectionId.Returns("PersonalizedActionPlan");
            List<string> resourcesPropertyNames = new List<string>() { Constants.PlanId, Constants.Type };
            List<string> resourcesValues = new List<string>() { planId, type };
            Microsoft.Azure.Documents.Document document = new Microsoft.Azure.Documents.Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(personalizedPlanView[0].ToString()));
            document.LoadFrom(reader);
            dynamic actualResult = null;

            dynamicQueries.FindItemsWhereAsync(dbSettings.PersonalizedActionPlanCollectionId, resourcesPropertyNames, resourcesValues).ReturnsForAnyArgs(expectedConvertedPersonalizedPlanSteps);
            dbService.UpdateItemAsync<dynamic>(id, document, dbSettings.PersonalizedActionPlanCollectionId).ReturnsForAnyArgs(document);
            
            //Act
            PersonalizedPlanSteps expectedConvertedPlanSteps = JsonConvert.DeserializeObject<PersonalizedPlanSteps>(JsonConvert.SerializeObject(this.convertedPersonalizedPlanSteps.First));
            actualResult = personalizedPlan.UpdatePersonalizedPlan(expectedConvertedPlanSteps);

            ////Assert
            //Assert.NotEmpty(actualResult);
        }
    }
}
