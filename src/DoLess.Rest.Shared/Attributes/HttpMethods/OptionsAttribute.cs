using System;
using System.Net.Http;

namespace DoLess.Rest
{
    /// <summary>
    /// Identifies a request that will use the HTTP OPTIONS method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class OptionsAttribute : HttpMethodAttribute
    {
        /// <summary>
        /// Creates a new <see cref="OptionsAttribute"/>.
        /// </summary>
        /// <param name="path">The relative path to the resource.</param>
        public OptionsAttribute(string path) : base(HttpMethod.Options, path) { }
    }
}
