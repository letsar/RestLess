using System;
using System.Net.Http;

namespace DoLess.Rest
{
    /// <summary>
    /// Identifies a request that will use a specific HTTP method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class HttpMethodAttribute : Attribute
    {
        /// <summary>
        /// Creates a new <see cref="HttpMethodAttribute"/>.
        /// </summary>
        /// <param name="method">The http method to use.</param>
        /// <param name="path">The relative path to the resource.</param>
        public HttpMethodAttribute(HttpMethod method, string path)
        {
            this.Method = method;
            this.Path = path.EnsureRelativePath();
        }

        /// <summary>
        /// Gets the http method used.
        /// </summary>
        public HttpMethod Method { get; }

        /// <summary>
        /// Gets the relative path to the resource.
        /// </summary>
        public string Path { get; }
    }
}
