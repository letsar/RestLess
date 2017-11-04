using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Benchmarks.Interfaces;
using Refit;

namespace Benchmarks.PerformanceTesters
{
    public class RefitPerformanceTester : PerformanceTester
    {
        private IRefitApi api;

        public RefitPerformanceTester(HttpClient httpClient) : base("Refit", httpClient)
        {
        }

        protected override void CreateImplementation(HttpClient httpClient)
        {
            this.api = RestService.For<IRefitApi>(httpClient, new RefitSettings { UrlParameterFormatter = new UrlParameterFormatter() });
        }

        protected override Task ExecuteOneDictionaryRequestAsync()
        {
            return this.api.GetStringWithDictionaryAsync(this.dictionary);
        }

        protected override Task ExecuteOneSimpleRequestAsync()
        {
            return this.api.GetString00Async(Path, Query);
        }

        private class UrlParameterFormatter : IUrlParameterFormatter
        {
            public string Format(object value, ParameterInfo parameterInfo)
            {
                if (value is IEnumerable<KeyValuePair<string, object>> dictionary)
                {
                    StringBuilder str = new StringBuilder();
                    foreach (var item in dictionary)
                    {
                        str.Append($"{item.Key}={item.Value.ToString()}&");
                    }
                    str.Remove(str.Length - 1, 1);
                    return str.ToString();
                }
                else
                {
                    return value?.ToString();
                }
            }
        }
    }
}
