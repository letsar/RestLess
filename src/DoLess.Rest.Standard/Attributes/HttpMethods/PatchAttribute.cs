using System;

namespace DoLess.Rest
{
    /// <summary>
    /// Identifies a request that will use the HTTP PATCH method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class PatchAttribute : Attribute
    {
        /// <summary>
        /// Creates a new <see cref="PatchAttribute"/>.
        /// </summary>
        /// <param name="path">The relative path to the resource.</param>
        public PatchAttribute(string path) { }
    }
}
