using System;

namespace RestLess
{
    /// <summary>
    /// Identifies a parameter as the content (or a part of the content) of the request.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ContentAttribute : Attribute
    {
        /// <summary>
        /// Creates a new <see cref="ContentAttribute"/>.
        /// </summary>
        /// <param name="fileName">The file name parameter (used for multipart content).</param>
        /// <param name="contentType">The content type.</param>
        public ContentAttribute(string fileName = null, string contentType = null)
        {
        }
    }
}
