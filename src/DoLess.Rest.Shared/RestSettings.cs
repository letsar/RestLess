using System;
using System.Collections.Generic;
using System.Text;

namespace DoLess.Rest
{
    /// <summary>
    /// Represents the settings used by this rest library.
    /// </summary>
    public sealed class RestSettings
    {
        private Func<IReadOnlyList<string>, IReadOnlyList<string>> queryWithMultipleValuesTransformer;

        /// <summary>
        /// Gets or sets the serializer used in requests and responses.
        /// </summary>
        public ISerializer Serializer { get; set; }

        /// <summary>
        /// Gets or sets the method used to transform query with multiple values.
        /// </summary>
        public Func<IReadOnlyList<string>, IReadOnlyList<string>> QueryWithMultipleValuesTransformer
        {
            get { return this.queryWithMultipleValuesTransformer ?? MultipleValuesToOneValueCommaSeparated; }
            set { this.queryWithMultipleValuesTransformer = value; }
        }

        public static IReadOnlyList<string> MultipleValuesToOneValueCommaSeparated(IReadOnlyList<string> values)
        {
            return new string[] { string.Join(",", values) };
        }

        public static IReadOnlyList<string> MultipleValuesToMultipleValues(IReadOnlyList<string> values)
        {
            return values;
        }
    }
}
