using System;
using System.Collections.Generic;
using System.Text;

namespace DoLess.Rest
{
    public static class StringExtensions
    {
        private const char RelativePathStart = '/';

        internal static string EnsureRelativePath(this string self)
        {
            string result = self?.Trim();
            if (string.IsNullOrEmpty(result) || result[0] != RelativePathStart)
            {
                result = RelativePathStart + result;
            }
            return result;
        }
    }
}
