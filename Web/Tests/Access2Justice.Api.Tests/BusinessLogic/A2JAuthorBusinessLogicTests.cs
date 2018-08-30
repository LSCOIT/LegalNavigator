using Access2Justice.Api.BusinessLogic;
using Access2Justice.Api.Tests.TestData;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSubstitute;
using System;
using System.Linq;
using Xunit;

namespace Access2Justice.Api.Tests.BusinessLogic
{
    public class A2JAuthorBusinessLogicTests
    {
        private readonly ICosmosDbSettings dbSettings;
        private readonly IBackendDatabaseService dbService;
        private readonly IA2JAuthorBusinessLogic a2jAuthor;
        private readonly IPersonalizedPlanEngine personalizedPlanEngine;

        public A2JAuthorBusinessLogicTests()
        {
            dbSettings = Substitute.For<ICosmosDbSettings>();
            dbService = Substitute.For<IBackendDatabaseService>();
            personalizedPlanEngine = Substitute.For<IPersonalizedPlanEngine>();
            a2jAuthor = new A2JAuthorBusinessLogic(dbSettings, dbService, personalizedPlanEngine);
        }

        [Fact]
        public void ConvertA2JAuthorToCuratedExperienceShouldConstructValidJson()
        {
            // Arrange
            var a2j = A2JAuthorTestData.A2JAuthorSampleSchema;
            var a2jJson = (JObject)JsonConvert.DeserializeObject(a2j);
            var curatedExperienceJson = a2jAuthor.ConvertA2JAuthorToCuratedExperience(a2jJson);

            var expectedComponantText = "Enter your name.";
            var expectedButtonLabel = "Continue";
            var expectedFieldLabel = "First";

            // Act
            var componentFromA2jJson = curatedExperienceJson.Components.Where(x => x.Name == "1-Name").First();
            var buttonFromA2jJson = componentFromA2jJson.Buttons.First();
            var fieldFromA2jJson = componentFromA2jJson.Fields.First();

            // Assert  
            Assert.Equal(expectedButtonLabel, buttonFromA2jJson.Label);
            Assert.Contains(expectedFieldLabel, fieldFromA2jJson.Label, StringComparison.InvariantCultureIgnoreCase);
            Assert.Contains(expectedComponantText, componentFromA2jJson.Text, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
