using System;

namespace RestLess
{
    /// <summary>
    /// Identifies which UrlParameterFormatter to provide to the request.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class UrlParameterFormatterAttribute : Attribute
    {
        /// <summary>
        /// Creates a new <see cref="UrlParameterFormatterAttribute"/>.
        /// </summary>
        /// <param name="name">The name of the formatter specified in the <see cref="IRestClient.Settings"/></param>
        public UrlParameterFormatterAttribute(string name) { }
    }
}
