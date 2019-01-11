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
    }
}
