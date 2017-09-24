using System;
using System.Collections;
using System.Collections.Generic;

namespace DoLess.Rest
{
    internal static partial  class IEnumerableExtensions
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

        public static void ForEachSpecialized(this IEnumerable self, Action<object> action)
        {
            if (self == null)
            {
                return;
            }

            if (self is IList list)
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
    }
}
