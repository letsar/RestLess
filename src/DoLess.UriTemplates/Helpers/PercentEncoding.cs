using System.Text;

namespace DoLess.UriTemplates.Helpers
{
    internal static class PercentEncoding
    {
        public const int PercentEncodeLength = 3;
        private static readonly char[] HexDigits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        public static string Encode(params byte[] bytes)
        {
            char[] chars = new char[PercentEncodeLength * bytes.Length];
            int index = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                byte b = bytes[i];
                chars[index++] = '%';
                chars[index++] = HexDigits[(b & 240) >> 4];
                chars[index++] = HexDigits[(b & 15)];
            }
            return new string(chars);
        }

        public static string Encode(char c)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(new[] { c });
            return Encode(bytes);
        }
    }
}
