using System.Text;
using DoLess.UriTemplates.Helpers;

namespace DoLess.UriTemplates
{
    internal static class StringBuilderExtensions
    {
        public static void AppendEncoded(this StringBuilder self, char value, bool allowReserved)
        {
            var doNotEncode = (value.IsUnreserved()) ||
                              (allowReserved && value.IsUnreserved());

            if (doNotEncode)
            {
                self.Append(value);
            }
            else
            {
                self.Append(PercentEncoding.Encode(value));
            }
        }

        public static void AppendEncoded(this StringBuilder self, string value, bool allowReserved)
        {
            for (int i = 0; i < value.Length; i++)
            {
                self.AppendEncoded(value[i], allowReserved);
            }
        }

        public static void RemoveLastChar(this StringBuilder self)
        {
            self.Remove(self.Length - 1, 1);
        }
    }
}
