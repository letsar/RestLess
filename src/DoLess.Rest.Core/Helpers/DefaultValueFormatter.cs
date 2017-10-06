using System;
using System.Globalization;
using DoLess.UriTemplates;

namespace DoLess.Rest.Helpers
{
    public class DefaultValueFormatter : IValueFormatter
    {
        public string Format(object value)
        {
            return Convert.ToString(value, CultureInfo.InvariantCulture);
        }
    }
}
