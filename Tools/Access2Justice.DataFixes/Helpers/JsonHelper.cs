using Newtonsoft.Json;
using System;

namespace Access2Justice.DataFixes.Helpers
{
    public class JsonHelper
    {
        public static T Deserialize<T>(dynamic dynamicObject)
        {
            if (dynamicObject == null || (dynamicObject.ToString() == "[]"))
            {
                return Activator.CreateInstance<T>();
            }
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(dynamicObject));
        }
    }
}
