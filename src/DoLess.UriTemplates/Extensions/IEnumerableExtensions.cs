using System;
using System.Collections;
using System.Collections.Generic;

namespace DoLess.UriTemplates
{
    internal static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> self, Action<T> action)
        {
            if (self == null)
            {
                return;
            }

            if (self is IReadOnlyList<T> list)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    action(list[i]);
                }
            }
            else
            {
                foreach (var item in self)
                {
                    action(item);
                }
            }
        }

        public static TReturn Do<T, TReturn>(this IEnumerable<T> self, Func<IEnumerable<T>, TReturn> func)
        {
            return func(self);
        }

        public static bool Any(this IEnumerable self)
        {
            switch (self)
            {
                case null:
                    return false;

                case ICollection collection:
                    return collection.Count > 0;

                default:
                    return self.GetEnumerator().MoveNext();
            }
        }

        public static void ForEachIEnumerable(this IEnumerable self, Action<object> action)
        {
            switch (self)
            {
                case null:
                    break;

                case IList list:
                    for (int i = 0; i < list.Count; i++)
                    {
                        action(list[i]);
                    }
                    break;

                default:
                    foreach (var item in self)
                    {
                        action(item);
                    }
                    break;
            }
        }       
    }
}
