using System;
using System.Collections.Generic;
using System.Text;
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
        private readonly dynamic topicId= Guid.Parse("e1fdbbc6-d66a-4275-9cd2-2be84d303e12");
        private readonly string planId= "86e693bd-c673-419f-95c4-9ee76cccab3c";
        private readonly dynamic answersDocId = Guid.Parse("b7cc8711-34d4-4170-bef1-9315dcc86000");

        private readonly dynamic generatedPersonalizedPlan = PersonalizedPlanTestData.personalizedPlan;
        private readonly dynamic topicDetails = PersonalizedPlanTestData.topicDetails;
        private readonly dynamic planSteps = PersonalizedPlanTestData.planSteps;
        private readonly dynamic dynamicObject = PersonalizedPlanTestData.dynamicObject;
        private readonly dynamic convertedPersonalizedPlanSteps = PersonalizedPlanTestData.convertedPersonalizedPlanSteps;
        //private readonly dynamic personalizedPlanView = PersonalizedPlanTestData.personalizedPlanView;
        private readonly dynamic quickLinks = PersonalizedPlanTestData.quickLinks;
        //private readonly dynamic resourceDetails = PersonalizedPlanTestData.resourceDetails;
        //private readonly dynamic planStepsWithResourceDetails = PersonalizedPlanTestData.planStepsWithResourceDetails;
        private readonly dynamic resourceIds = PersonalizedPlanTestData.resourceIds;
        private readonly dynamic resourceDetailsForResourceIds = PersonalizedPlanTestData.resourceDetailsForResourceIds;
        private readonly dynamic planStepsByTopicId = PersonalizedPlanTestData.planStepsByTopicId;

        public PersonalizedPlanBussinessLogicTests()
        {
            dbSettings = Substitute.For<ICosmosDbSettings>();
            dbService = Substitute.For<IBackendDatabaseService>();
            dynamicQueries = Substitute.For<IDynamicQueries>();
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
            var curatedExperienceJson = JsonConvert.DeserializeObject<CuratedExperience>(
                CuratedExperienceTestData.CuratedExperienceSampleSchema);
            var answersDocId = this.answersDocId;
            //var expectedPersonalizedPlan = this.generatedPersonalizedPlan;
            // Act
            var actualPersonalizedPlan = personalizedPlan.GeneratePersonalizedPlan(curatedExperienceJson, answersDocId);

            // Assert  
            //Assert.Contains("id", JsonConvert.SerializeObject(actualPersonalizedPlan));
        }

        [Fact]
        public void BuildPersonalizedPlanFromPlanSteps()
        {
            //Arrange
            List<PersonalizedPlanStep> planSteps = JsonConvert.DeserializeObject<List<PersonalizedPlanStep>>(JsonConvert.SerializeObject(this.planSteps));
            //PersonalizedPlanSteps expectedPersonalizedPlan = JsonConvert.DeserializeObject<PersonalizedPlanStep>(this.personalizedPlan);

            //Act
            var actualPersonalizedPlan = personalizedPlan.BuildPersonalizedPlan(planSteps);

            //Assert
            //Assert.Equal(actualPersonalizedPlan, expectedPersonalizedPlan);
        }

        [Fact]
        public void GetPlanStepsByTopic()
        {
            //Arrange
            List<PersonalizedPlanStep> planSteps = JsonConvert.DeserializeObject<List<PersonalizedPlanStep>>(JsonConvert.SerializeObject(this.planSteps));
            var topic = this.topicId;
            List<PersonalizedPlanStep> expectedPersonalizedPlanSteps = JsonConvert.DeserializeObject<List<PersonalizedPlanStep>>(JsonConvert.SerializeObject(this.planStepsByTopicId));

            //Act
            var actualPersonalizedPlanSteps = personalizedPlan.GetPlanSteps(topic,planSteps);

            //Assert
            //Assert.Equal(actualPersonalizedPlanSteps, expectedPersonalizedPlanSteps);
        }

        [Fact]
        public void ConvertPersonalizedPlanStepsFromDynamicObject()
        {
            //Arrange
            var dynamicObject = this.dynamicObject;
            PersonalizedPlanSteps expectedConvertedPlanSteps = JsonConvert.DeserializeObject<PersonalizedPlanSteps>(JsonConvert.SerializeObject(this.convertedPersonalizedPlanSteps));

            //Act
            var actualConvertedPlanSteps = personalizedPlan.ConvertPersonalizedPlanSteps(dynamicObject);

            //Assert
            //Assert.Equal(actualConvertedPlanSteps, expectedConvertedPlanSteps);
        }

        [Fact]
        public void GetPlanDataAsyncByPlanId()
        {
            //Arrange
            var planId = this.planId;
            //PersonalizedActionPlanViewModel expectedPersonalizedActionPlanView = JsonConvert.DeserializeObject<PersonalizedActionPlanViewModel>(JsonConvert.SerializeObject(this.personalizedPlanView));

            //Act
            //PersonalizedActionPlanViewModel actualPersonalizedActionPlanView = personalizedPlan.GetPlanDataAsync(planId);

            //Assert
            //Assert.Equal(actualPersonalizedActionPlanView, expectedPersonalizedActionPlanView);
        }

        [Fact]
        public void GetQuickLinksForTopicByTopicId()
        {
            //Arrange
            var topicId = this.topicId;
            List<PlanQuickLink> expectedQuickLinks = JsonConvert.DeserializeObject<List<PlanQuickLink>>(JsonConvert.SerializeObject(this.quickLinks));
            
            //Act
            //List<PlanQuickLink> actualQuickLinks = personalizedPlan.GetQuickLinksForTopic(topicId,this.topicDetails);

            //Assert
            //Assert.Equal(actualQuickLinks, expectedQuickLinks);
        }

        [Fact]
        public void ConvertToPlanStepsWithResourceDetails()
        {
            //Arrange
            List<PersonalizedPlanStep> planSteps = JsonConvert.DeserializeObject<List<PersonalizedPlanStep>>(JsonConvert.SerializeObject(this.planSteps));
            //List<Resource> resourceDetails = JsonConvert.DeserializeObject<List<Resource>>(this.resourceDetails);
            //List<PlanStep> expectedPlanSteps = JsonConvert.DeserializeObject<List<PlanStep>>(this.planStepsWithResourceDetails);

            //Act
            //List<PlanStep> actualPlanSteps = personalizedPlan.ConvertToPlanSteps(planSteps, this.resourceDetails);

            //Assert
            //Assert.Equal(actualPlanSteps, expectedPlanSteps);
        }

        [Fact]
        public void GetResourceDetailsForResourceIds()
        {
            //Arrange
            List<Guid> resourceIds = JsonConvert.DeserializeObject<List<Guid>>(JsonConvert.SerializeObject(this.resourceIds));
            //List<Resource> resourceDetails = JsonConvert.DeserializeObject<List<Resource>>(this.resourceDetails);
            //List<Resource> expectedResourceDetails = JsonConvert.DeserializeObject<List<Resource>>(this.resourceDetailsForResourceIds);

            //Act
            //List<Resource> actualResourceDetails = personalizedPlan.GetResources(resourceIds, this.resourceDetails);

            //Assert
            //Assert.Equal(actualResourceDetails, expectedResourceDetails);
        }

        [Fact]
        public void GetPersonalizedPlanByPlanId()
        {
            //Arrange
            string planId = this.planId;
            //List<PersonalizedPlanSteps> expectedPlan = JsonConvert.DeserializeObject<List<PersonalizedPlanSteps>>(this.generatedPersonalizedPlan);

            //Act
            //List<PersonalizedPlanSteps> actualPlan = personalizedPlan.GetPersonalizedPlan(planId);

            //Assert
            //Assert.Equal(expectedPlan, expectedPlan);
        }

        [Fact]
        public void UpdatePersonalizedPlanWithUpdatedPlan()
        {
            //Arrange
            //PersonalizedPlanSteps updatedplan= JsonConvert.DeserializeObject<List<PersonalizedPlanSteps>>(this.generatedPersonalizedPlan);
            //PersonalizedActionPlanViewModel expectedUpdatedPlanView = JsonConvert.DeserializeObject<PersonalizedActionPlanViewModel>(this.personalizedPlanView);

            //Act
            //PersonalizedActionPlanViewModel actualUpdatedPlanView = personalizedPlan.UpdatePersonalizedPlan(updatedplan);

            //Assert
            //Assert.Equal(expectedPlan, expectedPlan);
        }
    }
}
