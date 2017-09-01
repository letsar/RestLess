using System;
using System.Collections.Generic;
using System.Text;
using DoLess.Rest.Tasks;

namespace DoLess.Rest
{
    internal static class StringExtensions
    {
        public static string[] SplitOnFirstOccurrence(this string self, char[] separators)
        {
            return self?.Split(separators, 2, StringSplitOptions.RemoveEmptyEntries);
        }

        public static Arg ToIdentifier(this string self)
        {
            if (self == null)
            {
                return null;
            }

            return Arg.Identifier(self);
        }

        public static Arg ToLiteral(this string self)
        {
            if (self == null)
            {
                return null;
            }

            return Arg.Literal(self);
        }
    }
}
