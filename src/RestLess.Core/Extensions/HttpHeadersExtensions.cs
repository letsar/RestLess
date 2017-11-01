using System.Collections.Generic;
using System.Linq;

namespace System.Net.Http.Headers
{
    /// <summary>
    /// Contains extensions for <see cref="HttpHeaders"/>.
    /// </summary>
    public static class HttpHeadersExtensions
    {
        /// <summary>
        /// Try to get the first value from the specified header name.
        /// </summary>
        /// <param name="self">The headers.</param>
        /// <param name="name">The header's name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool TryGetValue(this HttpHeaders self, string name, out string value)
        {
            value = null;
            return self.TryGetValues(name, out IEnumerable<string> values) &&
                   (value = values?.FirstOrDefault()) != null;
        }

        /// <summary>
        /// Try to get the first value from the specified header name and converts it into the specified type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="self">The headers.</param>
        /// <param name="name">The header's name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool TryGetValue<T>(this HttpHeaders self, string name, out T value)
        {
            value = default;
            if (self.TryGetValue(name, out string stringValue))
            {
                try
                {
                    value = (T)Convert.ChangeType(stringValue, typeof(T));
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
