using System;

namespace DoLess.Rest
{
    /// <summary>
    /// Identifies the parameter as a value for the specified header.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public sealed class HeaderValueAttribute : Attribute
    {
        /// <summary>
        /// Creates a new <see cref="HeaderValueAttribute"/>.
        /// </summary>
        /// <param name="name">The name of the header.</param>
        public HeaderValueAttribute(string name) { }
    }
}
