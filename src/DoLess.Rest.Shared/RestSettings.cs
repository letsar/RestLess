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
        /// <summary>
        /// Gets or sets the serializer used in requests and responses.
        /// </summary>
        public IStringConverter Serializer { get; set; }
    }
}
