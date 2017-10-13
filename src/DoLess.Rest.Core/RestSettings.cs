using System.Collections.Generic;
using DoLess.Rest.Helpers;
using DoLess.UriTemplates;

namespace DoLess.Rest
{
    /// <summary>
    /// Represents the settings used by this rest library.
    /// </summary>
    public partial class RestSettings
    {
        /// <summary>
        /// Creates a new <see cref="RestSettings"/>.
        /// </summary>
        public RestSettings()
        {
            this.MediaTypeFormatters = new RestSettingStore<IMediaTypeFormatter>(nameof(this.MediaTypeFormatters), new DefaultMediaFormatter());
            this.UrlParameterFormatters = new RestSettingStore<IValueFormatter>(nameof(this.UrlParameterFormatters), new DefaultValueFormatter());
            this.FormFormatters = new RestSettingStore<IFormFormatter>(nameof(this.FormFormatters), new DefaultFormFormatter());
            this.CustomParameters = new Dictionary<string, string>();
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

        /// <summary>
        /// Gets the custom parameters.
        /// </summary>
        public IDictionary<string, string> CustomParameters { get; }
    }
}
