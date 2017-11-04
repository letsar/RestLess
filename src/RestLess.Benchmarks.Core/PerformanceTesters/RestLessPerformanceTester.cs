using System.Net.Http;
using System.Threading.Tasks;
using Benchmarks.Interfaces;
using RestLess;

namespace Benchmarks.PerformanceTesters
{
    public class RestLessPerformanceTester : PerformanceTester
    {
        private IRestLessApi api;

        public RestLessPerformanceTester(HttpClient httpClient) : base("RestLess", httpClient)
        {
        }

        protected override void CreateImplementation(HttpClient httpClient)
        {
            this.api = RestClient.For<IRestLessApi>(httpClient);
        }

        protected override Task ExecuteOneDictionaryRequestAsync()
        {
            return this.api.GetStringWithDictionaryAsync(this.dictionary);
        }

        protected override Task ExecuteOneSimpleRequestAsync()
        {
            return this.api.GetString00Async(Path, Query);
        }
    }
}
