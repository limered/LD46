using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils.DotNet
{
    public static class LinqExtensions
    {
        public static T NthElement<T>(this IEnumerable<T> coll, int n)
        {
            return coll.OrderBy(x => x).Skip(n - 1).FirstOrDefault();
        }

        public static T RandomElement<T>(this T[] collection)
        {
            return collection[(int)(Random.value * collection.Length)];
        }

        public static T RandomElement<T>(this List<T> collection)
        {
            return collection[(int)(Random.value * collection.Count)];
        }
    }
}
