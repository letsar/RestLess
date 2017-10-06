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
            this.MediaTypeFormatters = new RestSettingStore<IMediaTypeFormatter>(nameof(this.MediaTypeFormatters), new DefaultMediaFormatter());
            this.UrlParameterFormatters = new RestSettingStore<IValueFormatter>(nameof(this.UrlParameterFormatters), new DefaultValueFormatter());
            this.FormFormatters = new RestSettingStore<IFormFormatter>(nameof(this.FormFormatters), new DefaultFormFormatter());
        }

        /// <summary>
        /// Gets the formatters used to write/read objects to/from the http stream.
        /// </summary>
        public RestSettingStore<IMediaTypeFormatter> MediaTypeFormatters { get; }

        /// <summary>
        /// Gets formatters used to transform an <see cref="object"/> in <see cref="string"/> in the url.
        /// </summary>
        public RestSettingStore<IValueFormatter> UrlParameterFormatters { get; }

        /// <summary>
        /// Gets the formatters used when the body is FormUrlEncoded.
        /// </summary>
        public RestSettingStore<IFormFormatter> FormFormatters { get; }
    }
}
