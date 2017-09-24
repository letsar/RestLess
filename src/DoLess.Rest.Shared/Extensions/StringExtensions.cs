namespace DoLess.Rest
{
    internal static partial class StringExtensions
    {
        private const char RelativePathStart = '/';

        public static string EnsureRelativePath(this string self)
        {
            string result = self?.Trim();
            if (string.IsNullOrEmpty(result) || result[0] != RelativePathStart)
            {
                result = RelativePathStart + result;
            }
            return result;
        }

        public static bool HasContent(this string self)
        {
            return !string.IsNullOrEmpty(self);
        }

        public static bool IsNullOrEmpty(this string self)
        {
            return string.IsNullOrEmpty(self);
        }

        public static bool IsNullOrWhiteSpace(this string self)
        {
            return string.IsNullOrWhiteSpace(self);
        }
    }
}
