using System;

namespace DoLess.Rest
{
    /// <summary>
    /// Identifies a request that will use the HTTP TRACE method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class TraceAttribute : Attribute
    {
        /// <summary>
        /// Creates a new <see cref="TraceAttribute"/>.
        /// </summary>
        /// <param name="path">The relative path to the resource.</param>
        public TraceAttribute(string path) { }
    }
}
