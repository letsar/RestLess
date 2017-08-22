using System;
using System.Net.Http;

namespace DoLess.Rest
{
    /// <summary>
    /// Identifies a request that will use the HTTP PATCH method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class PatchAttribute : HttpMethodAttribute
    {
        private static readonly HttpMethod Patch = new HttpMethod("PATCH");

        /// <summary>
        /// Creates a new <see cref="PatchAttribute"/>.
        /// </summary>
        /// <param name="path">The relative path to the resource.</param>
        public PatchAttribute(string path) : base(Patch, path) { }
    }
}
