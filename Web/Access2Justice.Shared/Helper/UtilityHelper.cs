using Newtonsoft.Json;

namespace Access2Justice.Shared.Helper
{
    public class UtilityHelper
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
    }
}