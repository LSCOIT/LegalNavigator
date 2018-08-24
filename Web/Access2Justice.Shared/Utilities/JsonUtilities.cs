using Newtonsoft.Json;

namespace Access2Justice.Shared.Utilities
{
    public class JsonUtilities
    {
        public static JsonSerializerSettings JSONSanitizer()
        {
            //Ignoring Null and Default values, Default values which are set at model.
            return new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };
        }

        public static T DeserializeDynamicObject<T>(dynamic dynamicObject)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(dynamicObject));
        }
    }
}
