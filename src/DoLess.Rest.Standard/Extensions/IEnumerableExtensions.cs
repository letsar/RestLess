using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoLess.Rest
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
    }
}
