using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Access2Justice.Api.BusinessLogic;
using Access2Justice.Api.Interfaces;
using Access2Justice.Api.Tests.TestData;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Utilities;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSubstitute;
using Xunit;

namespace Access2Justice.Api.Tests.BusinessLogic
{
    //Todo this entire class must be revisited after the new personalized plan logic is in place.
    public class PersonalizedPlanBussinessLogicTests
    {
        private readonly IBackendDatabaseService dbService;
        private readonly ICosmosDbSettings dbSettings;
        private readonly IDynamicQueries dynamicQueries;
        private readonly IUserProfileBusinessLogic userProfileBusinessLogic;
        private readonly IPersonalizedPlanEngine personalizedPlanEngine;
        private readonly IPersonalizedPlanViewModelMapper personalizedPlanViewModelMapper;
        private readonly IPersonalizedPlanBusinessLogic personalizedPlanBusinessLogicSettings;
        private readonly ITopicsResourcesBusinessLogic topicsResourcesBusinessLogic;

        //Mocked input data
        private readonly dynamic topicId = Guid.Parse("e1fdbbc6-d66a-4275-9cd2-2be84d303e12");
        private readonly dynamic answersDocId = Guid.Parse("288af4da-06bb-4655-aa91-41314e248d6b");
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
        private readonly PersonalizedPlanBusinessLogic personalizedPlanBusinessLogic;
        private readonly PersonalizedPlanViewModelMapper personalizedPlanBusinessLogicViewModelMapper;
        public PersonalizedPlanBussinessLogicTests()
        {
            dbService = Substitute.For<IBackendDatabaseService>();
            dbSettings = Substitute.For<ICosmosDbSettings>();
            dynamicQueries = Substitute.For<IDynamicQueries>();
            userProfileBusinessLogic = Substitute.For<IUserProfileBusinessLogic>();
            personalizedPlanBusinessLogicSettings = Substitute.For<IPersonalizedPlanBusinessLogic>();
            topicsResourcesBusinessLogic = Substitute.For<ITopicsResourcesBusinessLogic>();
            personalizedPlanBusinessLogic = new PersonalizedPlanBusinessLogic(dbSettings, dbService, dynamicQueries, userProfileBusinessLogic, personalizedPlanEngine, personalizedPlanViewModelMapper);
            personalizedPlanBusinessLogicViewModelMapper = new PersonalizedPlanViewModelMapper(dbSettings, dynamicQueries, topicsResourcesBusinessLogic, dbService);
            dbSettings.ActionPlansCollectionId.Returns("ActionPlans");
            dbSettings.A2JAuthorDocsCollectionId.Returns("A2JAuthorDocs");
            dbSettings.GuidedAssistantAnswersCollectionId.Returns("GuidedAssistantAnswers");
            dbSettings.ProfilesCollectionId.Returns("Profiles");
            dbSettings.ResourcesCollectionId.Returns("Resources");
        }

        [Theory]
        [MemberData(nameof(PersonalizedPlanTestData.PersonalizedPlanData), MemberType = typeof(PersonalizedPlanTestData))]
        public void GetPersonalizedPlanAsyncValidate(PersonalizedPlanViewModel personalizedPlan, dynamic expectedData)
        {
            //arrange
            var dbResponse = dbService.GetItemAsync<PersonalizedPlanViewModel>(personalizedPlan.PersonalizedPlanId.ToString(), dbSettings.ActionPlansCollectionId);
            dbResponse.ReturnsForAnyArgs<PersonalizedPlanViewModel>(personalizedPlan);
            var response = personalizedPlanBusinessLogic.GetPersonalizedPlanAsync(personalizedPlan.PersonalizedPlanId);

            //act
            var actualResult = JsonConvert.SerializeObject(response.Result);
            var expectedResult = JsonConvert.SerializeObject(expectedData);

            //assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [MemberData(nameof(PersonalizedPlanTestData.CuratedExperienceAnswersData), MemberType = typeof(PersonalizedPlanTestData))]
        public void GeneratePersonalizedPlanAsyncValidate(CuratedExperience curatedExperience, CuratedExperienceAnswers expectedData, JObject personalizedPlan)
        {
            //arrange            
            var a2jPersonalizedPlan = dynamicQueries.FindItemWhereAsync<JObject>(dbSettings.A2JAuthorDocsCollectionId, Constants.Id, curatedExperience.A2jPersonalizedPlanId.ToString());
            a2jPersonalizedPlan.ReturnsForAnyArgs<JObject>(personalizedPlan);

            var dbResponse = dbService.GetItemAsync<dynamic>(answersDocId.ToString(), dbSettings.GuidedAssistantAnswersCollectionId);
            var response = personalizedPlanBusinessLogic.GeneratePersonalizedPlanAsync(curatedExperience, answersDocId);

            //act
            var actualResult = JsonConvert.SerializeObject(response.Result);
            var expectedResult = JsonConvert.SerializeObject(expectedData);

            //assert
            Assert.Equal("null", actualResult);
        }

        [Theory]
        [MemberData(nameof(PersonalizedPlanTestData.UserPlanData), MemberType = typeof(PersonalizedPlanTestData))]
        public void UpsertPersonalizedPlanAsyncValidate(UserPlan userPlan)
        {
            //arrange            
            dynamic expectedResult;
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
            var result = personalizedPlanBusinessLogic.UpsertPersonalizedPlanAsync(userPlan).Result;
            expectedResult = null;

            //assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [MemberData(nameof(PersonalizedPlanTestData.ProfileToPlan), MemberType = typeof(PersonalizedPlanTestData))]
        public void UpdatePlanToProfileValidate(Guid planId, string oId, PersonalizedPlanViewModel personalizedPlan)
        {
            //arrange
            var profileResponse = userProfileBusinessLogic.GetUserProfileDataAsync(oId);
            profileResponse.ReturnsForAnyArgs(ShareTestData.UserProfileWithoutPlanId);
            var dbResponse = dbService.GetItemAsync<PersonalizedPlanViewModel>(personalizedPlan.PersonalizedPlanId.ToString(), dbSettings.ActionPlansCollectionId);
            dbResponse.ReturnsForAnyArgs<PersonalizedPlanViewModel>(personalizedPlan);
            var response1 = personalizedPlanBusinessLogic.GetPersonalizedPlanAsync(personalizedPlan.PersonalizedPlanId);

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
            dynamic response = personalizedPlanBusinessLogic.UpdatePlanToProfile(planId, oId, personalizedPlan);

            //assert
            Assert.Equal("RanToCompletion", response.Status.ToString());
        }

        //[Theory]
        //[MemberData(nameof(PersonalizedPlanTestData.UnprocessedPersonalizedPlan), MemberType = typeof(PersonalizedPlanTestData))]
        //public void MapViewModelValidate(UnprocessedPersonalizedPlan unprocessedPersonalizedPlan, UnprocessedTopic unprocessedTopic)
        //{
        //    //arrange           
        //    IEnumerable<string> resourceValues2 = new string[] { "6a9f4c22-b1ab-46cf-8702-41a0b9522f82", "6a9f4c22-b1ab-46cf-8702-41a0b9522f12" };
        //    var resourceData = dynamicQueries.FindItemsWhereInClauseAsync(dbSettings.ResourcesCollectionId, Constants.Id, resourceValues2);
        //    resourceData.ReturnsForAnyArgs(ResourceObjects);
        //    var resourceDetails = JsonUtilities.DeserializeDynamicObject<List<Shared.Models.Resource>>(resourceData);

        //    //act
        //    var response = personalizedPlanBusinessLogicViewModelMapper.MapViewModel(unprocessedPersonalizedPlan);

        //    //assert
        //}
        //public static string ResourceObjects = "{\"name\":\"Form1\",\"type\":\"form\",\"description\":\"Subhead lorem ipsum solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem\",\"resourceType\":\"Forms\",\"externalUrl\":\"www.youtube.com\",\"url\":\"access2justice.com\",\"topicTags\":[{\"id\":\"aaa085ef-96fb-4fd0-bcd0-0472ede66512\"},{\"id\":\"2c0cc7b8-62b1-4efb-8568-b1f767f879bc\"}],\"organizationalUnit\":\"Alaska\",\"location\":[{\"state\":\"Hawaii\",\"city\":\"Haiku-Pauwela\"},{\"state\":\"Alaska\"}],\"overview\":\"Form1\",\"fullDescription\":\"Below is the form you will need if you are looking to settle your child custody dispute in court. We have included helpful tips to guide you along the way.\",\"createdBy\":\"API\",\"createdTimeStamp\":\"\",\"modifiedBy\":\"API\",\"modifiedTimeStamp\":\"\"},{\"name\":\"Form2\",\"type\":\"form\",\"description\":\"Subhead lorem ipsum solor sit amet bibodem consecuter orem ipsum solor sit amet bibodem\",\"resourceType\":\"Forms\",\"externalUrl\":\"\",\"url\":\"\",\"topicTags\":[{\"id\":\"aaa085ef-96fb-4fd0-bcd0-0472ede66512\"},{\"id\":\"2c0cc7b8-62b1-4efb-8568-b1f767f879bc\"}],\"organizationalUnit\":\"Alaska\",\"location\":[{\"state\":\"Hawaii\",\"city\":\"Haiku-Pauwela\"},{\"state\":\"Hawaii\"}],\"overview\":\"Form2\",\"fullDescription\":\"Below is the form you will need if you are looking to settle your child custody dispute in court. We have included helpful tips to guide you along the way.\",\"createdBy\":\"\",\"createdTimeStamp\":\"\",\"modifiedBy\":\"\",\"modifiedTimeStamp\":\"\"}";
        //public static Shared.Models.Resource ResourceData =>
        //     new Shared.Models.Resource { Name = "Form1", ResourceType = "Forms", ResourceId = Guid.Parse("19a02209-ca38-4b74-bd67-6ea941d41518"), Url = "access2justice.com", TopicTags = { new TopicTag { TopicTags = { "aaa085ef-96fb-4fd0-bcd0-0472ede66512", "2c0cc7b8-62b1-4efb-8568-b1f767f879bc" } } }, ResourceCategory = "test", OrganizationalUnit = "Alaska", Location = { new Location { State = "AL", City = "Alaska", County = "test", ZipCode = "test" } }, CreatedBy = "test", Description = "test", ModifiedBy = "test", CreatedTimeStamp = DateTime.UtcNow, ModifiedTimeStamp = DateTime.UtcNow };
        //[Fact]
        //public void GetTopicNameByTopicId()
        //{
        //    //Arrange
        //    var topicId = this.topicId;
        //    var isTopicName = true;
        //    var topicDetails = JsonConvert.DeserializeObject<List<TopicDetails>>(JsonConvert.SerializeObject(this.topicDetails));
        //    string expectedTopicName = "Family";

        //    //Act
        //    var actualTopicName = personalizedPlan.g.GetByTopicId(topicId, topicDetails, isTopicName);

        //    //Assert
        //    Assert.Equal(actualTopicName, expectedTopicName);
        //}

        //[Fact]
        //public void GetTopicIconByTopicId()
        //{
        //    //Arrange
        //    var topicId = this.topicId;
        //    var isTopicName = false;
        //    var topicDetails = JsonConvert.DeserializeObject<List<TopicDetails>>(JsonConvert.SerializeObject(this.topicDetails));
        //    string expectedTopicIcon = "https://cs4892808efec24x447cx944.blob.core.windows.net/static-resource/assets/images/categories/family.svg";

        //    //Act
        //    var actualTopicIcon = personalizedPlan.GetByTopicId(topicId, topicDetails, isTopicName);

        //    //Assert
        //    Assert.Equal(actualTopicIcon, expectedTopicIcon);
        //}

        //[Fact]
        //public void GeneratePersonalizedPlanFromCuratedExperienceAnswers()
        //{
        //    // Arrange
        //    var curatedExperience = this.curatedExperience;
        //    Microsoft.Azure.Documents.Document document = new Microsoft.Azure.Documents.Document();
        //    JsonTextReader reader = new JsonTextReader(new StringReader(curatedExperience[0].ToString()));
        //    document.LoadFrom(reader);

        //    //var curatedExperienceAsnwersJson = JsonConvert.DeserializeObject<CuratedExperienceAnswers>(CuratedExperienceTestData.CuratedExperienceAnswersSchema);            
        //    //var userAnswers = dbService.GetItemAsync<CuratedExperienceAnswers>(answersDocId.ToString(), dbSettings.CuratedExperienceAnswersCollectionId).ReturnsForAnyArgs(curatedExperienceAsnwersJson);
        //    //var dbResponse = dbService.CreateItemAsync<dynamic>(curatedExperience, dbSettings.PersonalizedActionPlanCollectionId).ReturnsForAnyArgs(document);
        //    //var curatedExperienceJson = JsonConvert.DeserializeObject<CuratedExperience>(CuratedExperienceTestData.CuratedExperienceSampleSchema);

        //    //// Act
        //    //var actualPersonalizedPlan = personalizedPlan.GeneratePersonalizedPlan(curatedExperienceJson, answersDocId).Returns(userAnswers);

        //    ////Assert
        //    //Assert.True(actualPersonalizedPlan.count > 1);
        //}


        //[Fact]
        //public void ConvertPersonalizedPlanStepsFromDynamicObject()
        //{
        //    //Arrange
        //    var dynamicObject = this.dynamicObject;
        //    PersonalizedPlanSteps expectedConvertedPlanSteps = JsonConvert.DeserializeObject<PersonalizedPlanSteps>(JsonConvert.SerializeObject(this.convertedPersonalizedPlanSteps.First));

        //    //Act
        //    var actualConvertedPlanSteps = personalizedPlan.ConvertPersonalizedPlanSteps(dynamicObject);

        //    //Assert
        //    Assert.Equal(actualConvertedPlanSteps.Topics.Count, expectedConvertedPlanSteps.Topics.Count);
        //}

        //[Fact]
        //public void GetQuickLinksForTopicByTopicId()
        //{
        //    //Arrange
        //    var topicId = this.topicId;
        //    List<PlanQuickLink> expectedQuickLinks = JsonConvert.DeserializeObject<List<PlanQuickLink>>(JsonConvert.SerializeObject(this.quickLinks));
        //    var topicDetails = JsonConvert.DeserializeObject<List<TopicDetails>>(JsonConvert.SerializeObject(this.topicDetails));

        //    //Act
        //    List<PlanQuickLink> actualQuickLinks = personalizedPlan.GetQuickLinksForTopic(topicId, topicDetails);

        //    //Assert
        //    var obj1Str = JsonConvert.SerializeObject(expectedQuickLinks);
        //    var obj2Str = JsonConvert.SerializeObject(actualQuickLinks);

        //    Assert.Equal(obj1Str, obj2Str);
        //}

        //[Fact]
        //public void ConvertToPlanStepsWithResourceDetails()
        //{
        //    //Arrange
        //    List<PersonalizedPlanStep> planSteps = JsonConvert.DeserializeObject<List<PersonalizedPlanStep>>(JsonConvert.SerializeObject(this.planSteps));
        //    List<Resource> resourceDetails = JsonConvert.DeserializeObject<List<Resource>>(JsonConvert.SerializeObject(this.resourceDetails));
        //    List<PlanStep> expectedPlanSteps = JsonConvert.DeserializeObject<List<PlanStep>>(JsonConvert.SerializeObject(this.planStepsWithResourceDetails));

        //    //Act
        //    List<PlanStep> actualPlanSteps = personalizedPlan.ConvertToPlanSteps(planSteps, resourceDetails);

        //    //Assert         
        //    Assert.Equal(expectedPlanSteps.Count, actualPlanSteps.Count);
        //}

        //[Fact]
        //public void GetResourceDetailsForResourceIds()
        //{
        //    //Arrange
        //    List<Guid> resourceIds = JsonConvert.DeserializeObject<List<Guid>>(JsonConvert.SerializeObject(this.resourceIds));
        //    List<Resource> resourceDetails = JsonConvert.DeserializeObject<List<Resource>>(JsonConvert.SerializeObject(this.resourceDetails));
        //    List<Resource> expectedResourceDetails = JsonConvert.DeserializeObject<List<Resource>>(JsonConvert.SerializeObject(this.resourceDetails));

        //    //Act
        //    List<Resource> actualResourceDetails = personalizedPlan.GetResources(resourceIds, resourceDetails);

        //    //Assert
        //    Assert.Equal(expectedResourceDetails.Count, actualResourceDetails.Count);
        //}

        //[Fact]
        //public void GetPersonalizedPlanByPlanId()
        //{
        //    //Arrange
        //    string planId = this.mockPlanId1;
        //    var dbResponse = dynamicQueries.FindItemsWhereAsync(dbSettings.PersonalizedActionPlanCollectionId, "id", planId);
        //    List<PersonalizedPlanSteps> expectedPlan = JsonConvert.DeserializeObject<List<PersonalizedPlanSteps>>(JsonConvert.SerializeObject(this.generatedPersonalizedPlan));
        //    dbResponse.ReturnsForAnyArgs<dynamic>(expectedPlan);

        //    //Act         
        //    var actualPlan = personalizedPlan.GetPersonalizedPlan(planId);

        //    string result = JsonConvert.SerializeObject(actualPlan);

        //    //assert
        //    Assert.Contains(mockPlanId1, result, StringComparison.InvariantCultureIgnoreCase);

        //}

        //[Fact]
        //public void UpdatePersonalizedPlanWithUpdatedPlan()
        //{
        //    //Arrange
        //    var userpersonalizedPlanView = this.personalizedPlanView;
        //    var resource = JsonConvert.SerializeObject(userpersonalizedPlanView);
        //    var userUIDocument = JsonConvert.DeserializeObject<dynamic>(resource);
        //    string id = userUIDocument[0].id;
        //    string planId = userUIDocument[0].PlanId;
        //    string type = userUIDocument[0].type;
        //    dbSettings.PersonalizedActionPlanCollectionId.Returns("PersonalizedActionPlan");
        //    List<string> resourcesPropertyNames = new List<string>() { Constants.PlanId, Constants.Type };
        //    List<string> resourcesValues = new List<string>() { planId, type };
        //    Microsoft.Azure.Documents.Document document = new Microsoft.Azure.Documents.Document();
        //    JsonTextReader reader = new JsonTextReader(new StringReader(personalizedPlanView[0].ToString()));
        //    document.LoadFrom(reader);
        //    dynamic actualResult = null;

        //    dynamicQueries.FindItemsWhereAsync(dbSettings.PersonalizedActionPlanCollectionId, resourcesPropertyNames, resourcesValues).ReturnsForAnyArgs(expectedConvertedPersonalizedPlanSteps);
        //    dbService.UpdateItemAsync<dynamic>(id, document, dbSettings.PersonalizedActionPlanCollectionId).ReturnsForAnyArgs(document);

        //    //Act
        //    PersonalizedPlanSteps expectedConvertedPlanSteps = JsonConvert.DeserializeObject<PersonalizedPlanSteps>(JsonConvert.SerializeObject(this.convertedPersonalizedPlanSteps.First));
        //    //actualResult = personalizedPlan.UpdatePersonalizedPlan(expectedConvertedPlanSteps);

        //    ////Assert
        //    //Assert.NotEmpty(actualResult);
        //}

        #region Old Personalized Plan Tests
        //[Fact]
        //public void GeneratePersonalizedPlanFromCuratedExperienceShouldNotGenerate()
        //{
        //    // Arrange
        //    var curatedExperience = this.curatedExperience;
        //    Microsoft.Azure.Documents.Document document = new Microsoft.Azure.Documents.Document();
        //    JsonTextReader reader = new JsonTextReader(new StringReader(curatedExperience[0].ToString()));
        //    document.LoadFrom(reader);

        //    var dbResponse = dbService.CreateItemAsync<dynamic>(curatedExperience, dbSettings.PersonalizedActionPlanCollectionId);
        //    var curatedExperienceJson = JsonConvert.DeserializeObject<CuratedExperience>(CuratedExperienceTestData.CuratedExperienceSampleSchema);

        //    // Act
        //    var actualPersonalizedPlan = personalizedPlan.GeneratePersonalizedPlan(curatedExperienceJson, answersDocId);

        //    //Assert
        //    //Assert.Equal(actualPersonalizedPlan.Result,"Id = 21, Status = Faulted, Method = "{null}", Result = "{Not yet computed}"");
        //}

        //[Fact]
        //public void BuildPersonalizedPlanFromPlanSteps()
        //{
        //    //Arrange            
        //    List<PersonalizedPlanStep> planSteps = JsonConvert.DeserializeObject<List<PersonalizedPlanStep>>(JsonConvert.SerializeObject(this.planSteps));

        //    //Act
        //    var actualPersonalizedPlan = personalizedPlan.BuildPersonalizedPlan(planSteps);

        //    //Assert
        //    Assert.NotNull(actualPersonalizedPlan.PersonalizedPlanId.ToString());
        //}

        ////[JsonIgnore] [JsonProperty(PropertyName = "topicIds")] JsonIgnore property is making the test case fail. Need to find the solution.
        //[Fact]
        //public void GetPlanStepsByTopic()
        //{
        //    //Arrange
        //    List<PersonalizedPlanStep> planSteps = JsonConvert.DeserializeObject<List<PersonalizedPlanStep>>(JsonConvert.SerializeObject(this.planStepsByTopicId));
        //    var topic = this.topicId;
        //    List<PersonalizedPlanStep> expectedPersonalizedPlanSteps = JsonConvert.DeserializeObject<List<PersonalizedPlanStep>>(JsonConvert.SerializeObject(this.planStepsByTopicId));

        //    //Act
        //    var actualPersonalizedPlanSteps = personalizedPlan.GetPlanSteps(topic, planSteps);

        //    //Assert
        //    // Assert.Equal(actualPersonalizedPlanSteps.Count, expectedPersonalizedPlanSteps.Count);
        //}

        //[Fact]
        //public void GetPlanDataAsyncByPlanId()
        //{
        //    //Arrange         
        //    var dbResponse = dynamicQueries.FindItemsWhereAsync(dbSettings.PersonalizedActionPlanCollectionId, "id", mockPlanId);

        //    List<PersonalizedPlanStep> planSteps = JsonConvert.DeserializeObject<List<PersonalizedPlanStep>>(JsonConvert.SerializeObject(this.planSteps));
        //    dbResponse.ReturnsForAnyArgs<dynamic>(planSteps);

        //    //Act
        //    var response = personalizedPlan.GetPlanDataAsync(mockPlanId);
        //    string result = JsonConvert.SerializeObject(response);

        //    //Assert
        //    Assert.NotEmpty(result);
        //}
        #endregion
    }
}
