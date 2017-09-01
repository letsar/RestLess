using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoLess.Rest
{
    internal static class IEnumerableExtensions
    {
        public static string Concatenate(this IEnumerable<string> self)
        {
            return self.Aggregate(new StringBuilder(), (builder, s) => builder.Append(s), builder => builder.ToString());
        }

        public static string ToMultilineSting(this IEnumerable<string> self)
        {
            return self.Aggregate(new StringBuilder(), (builder, s) => builder.AppendLine(s), builder => builder.ToString());
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> self)
        {
            return new HashSet<T>(self);
        }

        public static HashSet<string> ToAttributeNamesHashSet(this IEnumerable<string> self)
        {
            return self.Select(x => x.Replace(nameof(Attribute), string.Empty))
                       .ToHashSet();
        }

        public static void ForEach<T>(this IEnumerable<T> self, Action<T> action)
        {
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
    }
}
