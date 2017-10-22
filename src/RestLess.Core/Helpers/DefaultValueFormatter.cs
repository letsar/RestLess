using System;
using System.Globalization;
using DoLess.UriTemplates;

namespace RestLess.Helpers
{
    /// <summary>
    /// Default ValueFormatter.
    /// </summary>
    public class DefaultValueFormatter : IValueFormatter
    {
        /// <summary>
        /// Format the specified object into a string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public string Format(object value)
        {
            return Convert.ToString(value, CultureInfo.InvariantCulture);
        }
    }
}
