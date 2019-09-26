using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Access2Justice.Shared.Extensions
{
    public static class DictionaryExtensions
    {
        public static List<T> AddIfNotNull<T>(this List<T> list, T objectToAdd)
        {
            if (objectToAdd != null)
            {
                list.Add(objectToAdd);
            }

            return list;
        }

        public static OrderedDictionary AddDistinctKeyValue(this OrderedDictionary dictionary, string key, string value)
        {
            if (!dictionary.Contains(key))
            {
                dictionary.Add(key, value);
            }
            else
            {
                dictionary[key] = value;
            }

            return dictionary;
        }

        public static Dictionary<T, T> AddDistinctRange<T>(this Dictionary<T, T> originalDic, Dictionary<T, T> newDic)
        {
            if (originalDic == null)
            {
                originalDic = new Dictionary<T, T>();
            }
            if (newDic == null)
            {
                newDic = new Dictionary<T, T>();
            }

            foreach (var item in newDic)
            {
                if (!originalDic.Keys.Contains(item.Key))
                {
                    originalDic.Add(item.Key, item.Value);
                }
            }

            return originalDic;
        }
    }
}