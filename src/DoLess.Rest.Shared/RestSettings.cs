using System;
using System.Collections.Generic;
using System.Text;
using DoLess.Rest.Interfaces;

namespace DoLess.Rest
{
    /// <summary>
    /// Represents the settings used by this rest library.
    /// </summary>
    public class RestSettings
    {
        public RestSettings()
        {
            this.UrlParameterFormatter = new DefaultUrlParameterFormatter();
        }

        /// <summary>
        /// Gets or sets the formatter used to write/read objects to/from the http stream.
        /// </summary>
        public IMediaTypeFormatter MediaTypeFormatter { get; set; }

        /// <summary>
        /// Gets or sets the formatter used to transform an <see cref="object"/> in <see cref="string"/>.
        /// </summary>
        public IUrlParameterFormatter UrlParameterFormatter { get; set; }
    }
}
