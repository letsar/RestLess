using System;
using System.Net.Http;

namespace DoLess.Rest
{
    /// <summary>
    /// Identifies a request that will use the HTTP POST method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class PostAttribute : HttpMethodAttribute
    {
        /// <summary>
        /// Creates a new <see cref="PostAttribute"/>.
        /// </summary>
        /// <param name="path">The relative path to the resource.</param>
        public PostAttribute(string path) : base(HttpMethod.Post, path) { }
    }
}
