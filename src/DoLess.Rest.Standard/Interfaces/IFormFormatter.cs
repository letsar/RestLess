using System.Collections.Generic;

namespace DoLess.Rest
{
    /// <summary>
    /// Represents an object that can transforms an <see cref="object"/> into a named-value enumerable.
    /// </summary>
    public interface IFormFormatter
    {
        /// <summary>
        /// Formats the <paramref name="value"/> into its named-value enumerable representation.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The object.</param>
        /// <returns></returns>
        IEnumerable<KeyValuePair<string, string>> Format<T>(T value);
    }
}
