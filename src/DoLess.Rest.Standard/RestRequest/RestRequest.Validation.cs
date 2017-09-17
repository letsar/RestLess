using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Text;
using DoLess.Rest.Exceptions;

namespace DoLess.Rest
{
    public sealed partial class RestRequest
    {
        private void EnsureAllIsSetBeforeSendingTheRequest()
        {
            this.EnsureRequestUriIsSet();
        }

        private void EnsureMediaTypeFormatter()
        {
            if (this.client.Settings?.MediaTypeFormatter == null)
            {
                throw new RestClientException("In order to serialize/deserialize the http response from/into an object you need to provide an IMediaTypeFormatter to the RestSettings of the REST client");
            }
        }

        private void EnsureFormFormatter()
        {
            if (this.client.Settings?.FormFormatter == null)
            {
                throw new RestClientException("In order to url encode the body you need to provide an IFormFormatter to the RestSettings of the REST client");
            }
        }

        private void EnsureRequestUriIsSet()
        {
            if (this.httpRequestMessage.RequestUri == null)
            {
                this.httpRequestMessage.RequestUri = this.BuildUri();
            }
        }

        private void EnsureUrlParameterFormatter()
        {
            if(this.client.Settings?.UrlParameterFormatter == null)
            {
                throw new RestClientException("In order to format an objet parameter into a string, you need to provide an IUrlParameterFormatter to the RestSettings of the REST client");
            }
        }

    }
}
