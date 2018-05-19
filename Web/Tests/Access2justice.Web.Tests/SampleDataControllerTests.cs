using Xunit;

namespace Access2Justice.Web.Tests
{
    public static class SampleDataControllerTests
    {

        [Fact]
        public static void WeatherForecastsReturnsResults()
        {
            // arrange 
            var controller = new Access2Justice.Web.Controllers.SampleDataController();
            var expectedReturnValue = "Hello World!";

            // act
            var returnValue = controller.WeatherForecasts();

            // assert
            Assert.Equal(returnValue, expectedReturnValue);
        }
    }

}
