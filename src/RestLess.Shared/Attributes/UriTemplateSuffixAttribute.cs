using System;

namespace RestLess
{
    /// <summary>
    /// Identifies the UriTemplate prefix of all the REST methods inside this interface.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public sealed class UriTemplateSuffixAttribute : Attribute
    {
        /// <summary>
        /// Creates a new <see cref="UriTemplatePrefixAttribute"/>.
        /// </summary>
        /// <param name="uriTemplate">The uri temmplate suffix.</param>
        public UriTemplateSuffixAttribute(string uriTemplate) { }
    }
}
