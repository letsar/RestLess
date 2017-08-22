using System;
using System.Net.Http;

namespace DoLess.Rest
{
    /// <summary>
    /// Identifies a request that will use the HTTP GET method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class GetAttribute : HttpMethodAttribute
    {
        /// <summary>
        /// Creates a new <see cref="GetAttribute"/>.
        /// </summary>
        /// <param name="path">The relative path to the resource.</param>
        public GetAttribute(string path) : base(HttpMethod.Get, path) { }
    }
}
