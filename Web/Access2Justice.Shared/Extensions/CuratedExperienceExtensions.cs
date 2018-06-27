using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Access2Justice.Shared.Extensions
{
    public static class CuratedExperienceExtensions
    {
        public static string GetValue(this IEnumerable<JProperty> jProperties, string propertyName)
        {
            return jProperties.Where(x => x.Name == propertyName).FirstOrDefault()?.Value.ToString();
        }

        public static IEnumerable<JToken> GetArrayValue(this IEnumerable<JProperty> jProperties, string propertyName)
        {
            return jProperties.Where(x => x.Name == propertyName).FirstOrDefault()?.ToList();
        }
    }
}
