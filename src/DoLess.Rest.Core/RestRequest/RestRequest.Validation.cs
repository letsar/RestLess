using System;
using DoLess.Rest.Exceptions;

namespace DoLess.Rest.Generated
{
    public sealed partial class RestRequest
    {
        private void EnsureAllIsSetBeforeSendingTheRequest()
        {
            this.EnsureRequestUriIsSet();
        }

        private void EnsureRequestUriIsSet()
        {
            if (this.httpRequestMessage.RequestUri == null)
            {
                this.httpRequestMessage.RequestUri = this.BuildUri();
            }
        }

        private T EnsureDefaultValueSet<T>(RestSettingStore<T> store)
            where T : class
        {
            return store.Default ?? throw new ArgumentException($"You must provide a default value for the property '{store.Name}'.");
        }
    }
}
