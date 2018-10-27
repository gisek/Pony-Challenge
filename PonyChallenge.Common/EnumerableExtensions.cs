using System.Collections.Generic;

namespace PonyChallenge.Common
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> ToEnumerable<T>(this T item)
        {
            return new List<T> {item};
        }
    }
}