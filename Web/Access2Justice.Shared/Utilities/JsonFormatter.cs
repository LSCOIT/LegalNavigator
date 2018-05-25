using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Access2Justice.Shared.Utilities
{
    public class JsonFormatter
    {
        public static string SanitizeJson(string jsonString, string propPrefix)
        {

            var root = JToken.Parse(jsonString);
            if (root as JObject == null)
            {
                var temp = JArray.Parse(jsonString);
                temp.Descendants().OfType<JProperty>().Where(attr => attr.Name.StartsWith(propPrefix, true, CultureInfo.InvariantCulture)).ToList()
                   .ForEach(attr => attr.Remove()); // removing unwanted attributes
                return temp.ToString();
            }
            return "";
        }

        public static string FilterJson(string jsonString, IEnumerable<string> props)
        {
            var jsonObject = JToken.Parse(jsonString);
            JToken filteredJson = "";
            foreach (var prop in props)
            {
                if (jsonObject[prop] != null)
                {
                    filteredJson = jsonObject[prop];
                    jsonObject = jsonObject[prop];
                }
            }
            return filteredJson.ToString();
        }

    }

}