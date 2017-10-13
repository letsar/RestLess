using System;

namespace DoLess.Rest
{
    /// <summary>
    /// Identifies the UriTemplate prefix of all the REST methods inside this interface.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public sealed class UriTemplatePrefixAttribute : Attribute
    {
        /// <summary>
        /// Creates a new <see cref="UriTemplatePrefixAttribute"/>.
        /// </summary>
        /// <param name="uriTemplate">The uri template prefix.</param>
        public UriTemplatePrefixAttribute(string uriTemplate) { }
    }
}
