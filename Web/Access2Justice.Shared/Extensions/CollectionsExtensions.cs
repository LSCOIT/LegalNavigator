using System;
using System.Collections.Generic;
using System.Linq;

namespace Access2Justice.Shared.Extensions
{
    public static class CollectionsExtensions
    {
        public static List<T> AddIfNotNull<T>(this List<T> list, T objectToAdd)
        {
            if(objectToAdd != null)
            {
                list.Add(objectToAdd);
            }

            return list;
        }

        public static Dictionary<T, T> AddRange<T>(this Dictionary<T, T> originalDic, Dictionary<T, T> newDic)
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
