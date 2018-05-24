using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Access2Justice.Shared.Utilities
{
    public class JsonFormatter
    {
        public static string SanitizeJson(string jsonString, string propPrefix)
        {
            var root = (JContainer)JToken.Parse(jsonString);

            var temp = JArray.Parse(jsonString);
            temp.Descendants().OfType<JProperty>().Where(attr => attr.Name.StartsWith(propPrefix)).ToList()
               .ForEach(attr => attr.Remove()); // removing unwanted attributes
            return temp.ToString();
        }

        public static string FilterJson(string jsonString, IEnumerable<string> props)
        {
            var jsonObject = JToken.Parse(jsonString);            
            foreach (var prop in props)
            {
                if (jsonObject[prop] != null)
                {
                    jsonObject = jsonObject[prop];
                }
            }
            return jsonObject.ToString();
        }

    }

}