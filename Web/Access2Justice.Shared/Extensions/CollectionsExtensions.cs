using System.Collections.Generic;

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
    }
}
