namespace DoLess.Rest
{
    /// <summary>
    /// Represents a generic serializer.
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Serializes the value.
        /// </summary>
        /// <typeparam name="T">the type of the value.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>A string representing the value.</returns>
        string Serialize<T>(T value);

        /// <summary>
        /// Deserializes the string value into the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the data.</typeparam>
        /// <param name="value">The string representation of the data.</param>
        /// <returns></returns>
        T Deserialize<T>(string value);
    }
}
