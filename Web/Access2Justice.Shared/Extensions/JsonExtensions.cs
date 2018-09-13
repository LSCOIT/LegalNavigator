using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Access2Justice.Shared.Extensions
{
    public static class JsonExtensions
    {
        public static string GetValue(this IEnumerable<JProperty> jProperties, string propertyName)
        {
            return jProperties.Where(x => x.Name == propertyName).FirstOrDefault()?.Value.ToString();
        }

        public static string GetValue(this JToken jToken, string propertyName)
        {
            return ((JObject)jToken).Properties().GetValue(propertyName);
        }

        public static JArray GetValueAsArray(this JToken jToken, string propertyName)
        {
            return (JArray)JsonConvert.DeserializeObject(jToken.GetValue(propertyName));
        }

        public static IEnumerable<JToken> GetArrayValue(this IEnumerable<JProperty> jProperties, string propertyName)
        {
            return jProperties.Where(x => x.Name == propertyName).FirstOrDefault()?.ToList();
        }

        public static IEnumerable<JToken> GetArrayValue(this JToken jToken, string propertyName)
        {
            return ((JObject)jToken).Properties().GetArrayValue(propertyName);
        }

        public static DateTime? GetDateOrNull(this IEnumerable<JProperty> jProperties, string propertyName)
        {
            var dateString = jProperties.Where(x => x.Name == propertyName).FirstOrDefault()?.Value.ToString();

            var date = new DateTime();
            if (DateTime.TryParse(dateString, out date))
            {
                return date;
            }
            return null;
        }
    }
}
