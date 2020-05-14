using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TunZhou.Core
{
    public static class EnumerableExtension
    {
        public static bool HasItems<T>(this IEnumerable<T> collection) => collection != null && collection.Any();

        public static bool Empty<T>(this IEnumerable<T> collection) => HasItems(collection) == false;
    }
}
