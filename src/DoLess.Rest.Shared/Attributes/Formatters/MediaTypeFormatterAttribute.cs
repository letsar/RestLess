using System;

namespace DoLess.Rest
{
    /// <summary>
    /// Identifies which <see cref="IMediaTypeFormatter"/> to provide to the request.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class MediaTypeFormatterAttribute : Attribute
    {
        /// <summary>
        /// Creates a new <see cref="MediaTypeFormatterAttribute"/>.
        /// </summary>
        /// <param name="name">The name of the formatter specified in the <see cref="IRestClient.Settings"/></param>
        public MediaTypeFormatterAttribute(string name) { }
    }
}
