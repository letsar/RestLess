using System;

namespace DoLess.Rest
{
    /// <summary>
    /// Identifies a request that will use the HTTP POST method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class PostAttribute : Attribute
    {
        /// <summary>
        /// Creates a new <see cref="PostAttribute"/>.
        /// </summary>
        /// <param name="path">The relative path to the resource.</param>
        public PostAttribute(string path) { }
    }
}
