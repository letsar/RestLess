using System;
using System.Net.Http;

namespace DoLess.Rest
{
    /// <summary>
    /// Identifies a request that will use the HTTP PUT method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class PutAttribute : HttpMethodAttribute
    {
        /// <summary>
        /// Creates a new <see cref="PutAttribute"/>.
        /// </summary>
        /// <param name="path">The relative path to the resource.</param>
        public PutAttribute(string path) : base(HttpMethod.Put, path) { }
    }
}
