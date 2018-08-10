using Access2Justice.Api.BusinessLogic;
using Access2Justice.Api.Tests.TestData;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using NSubstitute;
using System;
using System.Linq;
using Xunit;

namespace Access2Justice.Api.Tests.BusinessLogic
{
    public class CuratedExperienceBuisnessLogicTests
    {
        private readonly IBackendDatabaseService dbService;
        private readonly ICosmosDbSettings dbSettings;
        private readonly CuratedExperienceBuisnessLogic curatedExperience;

        public CuratedExperienceBuisnessLogicTests()
        {
            dbService = Substitute.For<IBackendDatabaseService>();
            dbSettings = Substitute.For<ICosmosDbSettings>();

            curatedExperience = new CuratedExperienceBuisnessLogic(dbSettings, dbService);
        }

        [Fact]
        public void FindDestinationComponentShouldReturnTheNextComponentInLine()
        {
            // Arrange
            var curatedExperienceJson = JsonConvert.DeserializeObject<CuratedExperience>(
                CuratedExperienceTestData.CuratedExperienceSampleSchema);
            var buttonId = Guid.Parse("2b92e07b-a555-48e8-ad7b-90b99ebc5c96");
            var expectedComponentName = "2-Gender";
            // Act
            var actualNextComponent = curatedExperience.FindDestinationComponent(curatedExperienceJson, buttonId);

            // Assert  
            Assert.Equal(expectedComponentName, actualNextComponent.Name);
        }

        [Fact]
        public void CalculateRemainingQuestionsShouldReturnLongestPossibleRoute()
        {
            // Arrange
            var curatedExperienceJson = JsonConvert.DeserializeObject<CuratedExperience>(
                CuratedExperienceTestData.CuratedExperienceSampleSchema);
            var component = curatedExperienceJson.Components.Where(
                x => x.ComponentId == Guid.Parse("4adec03b-4f9b-4bc9-bc44-27a8e84e30ae")).FirstOrDefault();
            var expectedRemainingQuestions = 8;
            
            // Act
            var actualRemainingQuestions = curatedExperience.CalculateRemainingQuestions(curatedExperienceJson, component);

            // Assert  
            Assert.Equal(expectedRemainingQuestions, actualRemainingQuestions);
        }
    }

}
