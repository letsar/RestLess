using System;

namespace DoLess.Rest
{
    /// <summary>
    /// Identifies which <see cref="IFormFormatter"/> to provide to the request.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class FormFormatterAttribute : Attribute
    {
        /// <summary>
        /// Creates a new <see cref="FormFormatterAttribute"/>.
        /// </summary>
        /// <param name="name">The name of the formatter specified in the <see cref="IRestClient.Settings"/></param>
        public FormFormatterAttribute(string name) { }
    }
}
