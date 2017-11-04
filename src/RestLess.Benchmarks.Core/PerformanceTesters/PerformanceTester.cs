using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarks
{
    public abstract class PerformanceTester
    {
        public const string Path = "/v1";
        public const string Query = "all";
        protected readonly HttpClient httpClient;
        protected readonly Dictionary<string, object> dictionary;

        protected PerformanceTester(string libName, HttpClient httpClient)
        {
            this.LibName = libName;
            this.httpClient = httpClient;
            this.dictionary = new Dictionary<string, object>
            {
                ["order"] = "desc",
                ["limit"] = "1",
                ["skip"] = "100",
            };
        }

        public string LibName { get; }

        public async Task StartupAndOneRequestAsync()
        {
            this.CreateImplementation(this.httpClient);
            await this.ExecuteOneSimpleRequestAsync();
        }

        public Task MultipleSimpleRequestsAsync(int requestCount)
        {
            return this.MultipleRequestAsync(requestCount, this.ExecuteOneSimpleRequestAsync);
        }

        public Task MultipleDictionaryRequestsAsync(int requestCount)
        {
            return this.MultipleRequestAsync(requestCount, this.ExecuteOneDictionaryRequestAsync);
        }

        private async Task MultipleRequestAsync(int requestCount, Func<Task> methodToExecute)
        {
            for (int i = 0; i < requestCount; i++)
            {
                await methodToExecute();
            }
        }

        protected abstract void CreateImplementation(HttpClient httpClient);

        protected abstract Task ExecuteOneSimpleRequestAsync();

        protected abstract Task ExecuteOneDictionaryRequestAsync();
    }
}
