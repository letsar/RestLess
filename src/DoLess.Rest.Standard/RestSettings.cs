using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using DoLess.Rest.Helpers;
using DoLess.UriTemplates;

namespace DoLess.Rest
{
    /// <summary>
    /// Represents the settings used by this rest library.
    /// </summary>
    public partial class RestSettings
    {
        public RestSettings()
        {
            this.FormFormatter = new DefaultFormFormatter();
        }

        /// <summary>
        /// Gets or sets the formatter used to write/read objects to/from the http stream.
        /// </summary>
        public IMediaTypeFormatter MediaTypeFormatter { get; set; }

        /// <summary>
        /// Gets or sets the formatter used to transform an <see cref="object"/> in <see cref="string"/>.
        /// </summary>
        public IValueFormatter UrlParameterFormatter { get; set; }

        /// <summary>
        /// Gets or sets the formatter used when the body is FormUrlEncoded.
        /// </summary>
        public IFormFormatter FormFormatter { get; set; }
    }
}
