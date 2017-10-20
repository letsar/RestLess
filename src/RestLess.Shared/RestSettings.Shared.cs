using System;
using System.Net.Http;

namespace RestLess
{
    public partial class RestSettings
    {
        /// <summary>
        /// Gets or sets the factory used to create a <see cref="HttpMessageHandler"/> when creating a RestClient from <see cref="Uri"/> or <see cref="string"/>.
        /// </summary>
        public Func<HttpMessageHandler> HttpMessageHandlerFactory { get; set; }
    }
}
