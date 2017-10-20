using System;

namespace RestLess
{
    /// <summary>
    /// Indicates to add the specified header to all the methods (in case it is defined at interface scope) or to the specified method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class HeaderAttribute : Attribute
    {
        /// <summary>
        /// Creates a new <see cref="HeaderAttribute"/>.
        /// </summary>
        /// <param name="name">The header name.</param>
        /// <param name="value">The header value.</param>
        /// <param name="isCustomParameter">Indicates that the value is the key of a custom parameter defined in the <see cref="RestSettings"/>.</param>
        public HeaderAttribute(string name, string value, bool isCustomParameter = false) { }
    }
}
