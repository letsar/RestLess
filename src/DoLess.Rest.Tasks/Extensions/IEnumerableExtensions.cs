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

        public static TSource ZeroOrSingle<TSource, TException>(this IEnumerable<TSource> self, Func<TException> getException)
            where TException : Exception
        {
            if (self == null)
            {
                return default(TSource);
            }

            if (self is IList<TSource> list)
            {
                switch (list.Count)
                {
                    case 0: return default(TSource);
                    case 1: return list[0];
                }
            }
            else
            {
                using (IEnumerator<TSource> e = self.GetEnumerator())
                {
                    if (!e.MoveNext())
                    {
                        return default(TSource);
                    }
                    TSource result = e.Current;
                    if (!e.MoveNext())
                    {
                        return result;
                    }
                }
            }

            throw getException();
        }
    }
}
