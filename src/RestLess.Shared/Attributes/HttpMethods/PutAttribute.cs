using System;

namespace RestLess
{
    /// <summary>
    /// Identifies a request that will use the HTTP PUT method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class PutAttribute : Attribute
    {
        /// <summary>
        /// Creates a new <see cref="PutAttribute"/>.
        /// </summary>
        /// <param name="path">The relative path to the resource.</param>
        public PutAttribute(string path) { }
    }
}
