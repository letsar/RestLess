using System;

namespace DoLess.Rest
{
    /// <summary>
    /// Identifies a new name for the specified parameter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
    public sealed class NameAttribute : Attribute
    {
        /// <summary>
        /// Creates a new <see cref="NameAttribute"/>.
        /// </summary>
        /// <param name="name">The new name of the parameter.</param>
        public NameAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// The parameter's name.
        /// </summary>
        public string Name { get; }
    }
}
