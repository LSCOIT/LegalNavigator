using System.Linq;
using System.Reflection;
using Microsoft.Azure.Documents;
using Microsoft.Rest.Serialization;
using Newtonsoft.Json;

namespace Access2Justice.CosmosDb
{
    public static class DataExtensions
    {
        public static T Convert<T>(this Document document) where T : class, new()
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(document));

            //var result = new T();
            //foreach (var propertyInfo in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            //{
            //    var jsonName = (JsonPropertyAttribute)propertyInfo.GetCustomAttributes(typeof(JsonPropertyAttribute), false).FirstOrDefault();
            //    if (jsonName == null)
            //    {
            //        continue;
            //    }
            //    propertyInfo.SetValue(result, document.GetPropertyValue<object>(jsonName.PropertyName));
            //}

            //return result;
        }
    }
}
