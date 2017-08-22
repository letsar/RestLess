using System;
using System.Net.Http;

namespace DoLess.Rest
{
    /// <summary>
    /// Identifies a request that will use the HTTP DELETE method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class DeleteAttribute : HttpMethodAttribute
    {
        /// <summary>
        /// Creates a new <see cref="DeleteAttribute"/>.
        /// </summary>
        /// <param name="path">The relative path to the resource.</param>
        public DeleteAttribute(string path) : base(HttpMethod.Delete, path) { }
    }
}
