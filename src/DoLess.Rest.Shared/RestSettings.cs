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
        /// Gets or sets the object that is used to write into the body of a request.
        /// </summary>
        public IContentWriter ContentWriter { get; set; }

        /// <summary>
        /// Gets or sets the objet that is used to read from the body of a response.
        /// </summary>
        public IContentReader ContentReader { get; set; }
    }
}
