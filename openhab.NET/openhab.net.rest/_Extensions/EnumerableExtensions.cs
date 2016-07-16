using System;
using System.Collections;
using System.Collections.Generic;

namespace openhab.net.rest
{
    public static class EnumerableExtensions
    {
        public static void AddRange<T>(this IEnumerable<T> source, IEnumerable<T> target)
        {
            var list = source as IList<T>;
            if (list != null) {
                list.AddRange(target);
            }
            else {
                var collection = source as ICollection<T>;
                if (collection != null) {
                    target?.ForEach(x => collection.Add(x));
                }
            }
        }

        public static object ItemAt(this IEnumerable source, int index)
        {
            if (source != null && index >= 0) {
                int i = 0;
                var enumerator = source.GetEnumerator();
                while (enumerator.MoveNext() && i++ < index) ;
                return enumerator?.Current;
            }
            return null;
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            ForEach(source, action);
        }

        public static void ForEach(this IEnumerable enumeration, Action<object> action)
        {
            if (enumeration != null) {
                foreach (var item in enumeration)
                {
                    action?.Invoke(item);
                }
            }
        }
    }
}
