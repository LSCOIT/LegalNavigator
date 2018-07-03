using Access2Justice.Api.BusinessLogic;
using Access2Justice.Api.Tests.TestData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using Xunit;

namespace Access2Justice.Api.Tests.BusinessLogic
{
    public class CuratedExperienceBuisnessLogicTests
    {
        private readonly CuratedExperienceBuisnessLogic curatedExperience;
        public CuratedExperienceBuisnessLogicTests()
        {
            curatedExperience = new CuratedExperienceBuisnessLogic();
        }

        [Fact]
        public void ConvertA2JAuthorToCuratedExperienceShouldConstructValidJson()
        {
            // Arrange
            var a2j = CuratedExperienceTestData.A2JAuthorSampleSchema;
            var a2jJson = (JObject)JsonConvert.DeserializeObject(a2j);
            var curatedExperienceJson = curatedExperience.ConvertA2JAuthorToCuratedExperience(a2jJson);

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
