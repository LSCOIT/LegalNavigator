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

        /// <summary>
        /// Add new values to dictionary, if not exists
        /// </summary>
        /// <returns>Added values</returns>
        public static Dictionary<TKey, TValue> AddDistinctRange<TKey, TValue>(this Dictionary<TKey, TValue> originalDic, Dictionary<TKey, TValue> newDic)
        {
            if (originalDic == null)
            {
                originalDic = new Dictionary<TKey, TValue>();
            }
            if (newDic == null)
            {
                newDic = new Dictionary<TKey, TValue>();
            }

            var newValues = new Dictionary<TKey, TValue>();
            foreach (var item in newDic)
            {
                if (!originalDic.Keys.Contains(item.Key))
                {
                    originalDic.Add(item.Key, item.Value);
                    newValues[item.Key] = item.Value;
                }
            }

            return newValues;
        }
    }
}