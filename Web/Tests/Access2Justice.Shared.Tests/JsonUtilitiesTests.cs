using Access2Justice.Shared.Utilities;
using Xunit;

namespace Access2Justice.Shared.Tests
{
    public class JsonUtilitiesTests
    {
        [Fact]
        public void DeserializeDynamicObjectShouldSerializeToStringAndDeserializeBackToValidJson()
        {
            dynamic expectedJsonString = "{\r\n \"id\":\"49779468-1fe0-4183-850b-ff365e05893e\",\r\n \"name\": \"Alaska Native Justice Center\"\r\n}";

            string actualJsonString = JsonUtilities.DeserializeDynamicObject<string>(expectedJsonString);

            Assert.Equal(expectedJsonString, actualJsonString);
        }
    }
}
