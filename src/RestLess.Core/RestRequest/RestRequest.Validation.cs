using System;
using RestLess.Exceptions;
using System.Net.Http;


namespace RestLess.Generated
{
    public sealed partial class RestRequest
    {
        private const string DoLessRestBoundary = "DoLessRestBoundary";

        private void EnsureAllIsSetBeforeSendingTheRequest()
        {
            this.EnsureRequestUriIsSet();
            this.EnsureRequestContentIsSet();
        }

        private void EnsureRequestUriIsSet()
        {
            if (this.httpRequestMessage.RequestUri == null)
            {
                this.httpRequestMessage.RequestUri = this.BuildUri();
            }
        }

        private void EnsureRequestContentIsSet()
        {
            int parts = this.contentParts.Count;
            if (parts > 0 && this.httpRequestMessage.Content == null)
            {
                if (parts == 1 && !this.contentParts[0].IsMultipartRequired)
                {
                    this.httpRequestMessage.Content = this.contentParts[0].Content;
                }
                else
                {
                    // Multipart.
                    var multipartContent = new MultipartFormDataContent(DoLessRestBoundary);
                    for (int i = 0; i < parts; i++)
                    {
                        var contentPart = this.contentParts[i];

                        if (contentPart.FileName != null)
                        {
                            multipartContent.Add(contentPart.Content, contentPart.Name, contentPart.FileName);
                        }
                        else
                        {
                            multipartContent.Add(contentPart.Content, contentPart.Name);
                        }
                    }

                    this.httpRequestMessage.Content = multipartContent;
                }
            }
        }

        private T EnsureDefaultValueSet<T>(RestSettingStore<T> store)
            where T : class
        {
            return store.Default ?? throw new ArgumentException($"You must provide a default value for the property '{store.Name}'.");
        }
    }
}
