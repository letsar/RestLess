using System;

namespace RestLess
{
    /// <summary>
    /// Identifies a request that will use the HTTP GET method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class GetAttribute : Attribute
    {
        /// <summary>
        /// Creates a new <see cref="GetAttribute"/>.
        /// </summary>
        /// <param name="path">The relative path to the resource.</param>
        public GetAttribute(string path) { }
    }
}
