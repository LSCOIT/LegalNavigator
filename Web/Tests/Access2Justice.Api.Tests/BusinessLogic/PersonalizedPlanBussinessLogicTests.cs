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
        private readonly dynamic generatedPersonalizedPlan = PersonalizedPlanTestData.personalizedPlan;
        private readonly dynamic topicDetails = PersonalizedPlanTestData.topicDetails;
        public PersonalizedPlanBussinessLogicTests()
        {
            dbSettings = Substitute.For<ICosmosDbSettings>();
            dbService = Substitute.For<IBackendDatabaseService>();
            dynamicQueries = Substitute.For<IDynamicQueries>();
            personalizedPlan = new PersonalizedPlanBusinessLogic(dbSettings, dbService, dynamicQueries);
        }

        [Fact]
        public void GeneratePersonalizedPlanFromCuratedExperienceAnswers()
        {
            // Arrange
            var curatedExperienceJson = JsonConvert.DeserializeObject<CuratedExperience>(
                CuratedExperienceTestData.CuratedExperienceSampleSchema);
            var answersDocId = Guid.Parse("2b45fg5b-a555-48e8-ad7b-90b99ebc5c96");
            var expectedPersonalizedPlan = this.generatedPersonalizedPlan;
            // Act
             var actualPersonalizedPlan = personalizedPlan.GeneratePersonalizedPlan(curatedExperienceJson, answersDocId);

            // Assert  
             //Assert.Equal(expectedPersonalizedPlan, actualPersonalizedPlan);
        }

        [Fact]
        public void GetTopicNameByTopicId()
        {
            //Arrange
            var topicId = Guid.Parse("addf41e9-1a27-4aeb-bcbb-7959f95094ba");
            var isTopicName = true;
            var topicDetails = JsonConvert.DeserializeObject<List<TopicDetails>>(JsonConvert.SerializeObject(this.topicDetails));
            var expectedTopicName = "Family";

            //Act
            var actualTopicName = personalizedPlan.GetByTopicId(topicId, topicDetails, isTopicName);

            //Assert
            Assert.Equal(actualTopicName, expectedTopicName);
        }
    }
}
