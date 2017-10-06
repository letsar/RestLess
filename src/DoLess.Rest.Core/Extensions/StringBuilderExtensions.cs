using System;
using System.Collections.Generic;
using System.Text;

namespace DoLess.Rest
{
    internal static class StringBuilderExtensions
    {
        public static void AppendUrlPart(this StringBuilder self, string delimiter, string part)
        {
            if (!string.IsNullOrWhiteSpace(part))
            {
                self.Append(delimiter);
                self.Append(part);
            }
        }
    }
}
