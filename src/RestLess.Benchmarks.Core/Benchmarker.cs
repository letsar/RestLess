using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Benchmarks.PerformanceTesters;

namespace Benchmarks
{
    public class Benchmarker
    {
        private readonly Stopwatch swatch;
        private readonly PerformanceTester[] performanceTesters;
        private readonly Benchmark[] benchmarks;
        private readonly HttpClient httpClient;

        private const int RequestCount = 1000;

        public Benchmarker()
        {
            this.swatch = new Stopwatch();
            this.httpClient = new HttpClient(new BenchmarkHttpMessageHandler())
            {
                BaseAddress = new Uri("http://www.benchmark.com")
            };

            this.performanceTesters = new PerformanceTester[]
            {
                new RefitPerformanceTester(this.httpClient),
                new RestLessPerformanceTester(this.httpClient)
            };

            this.benchmarks = new Benchmark[]
            {
                new Benchmark("Startup and one request","The startup time and the time to process one request.",x => x.StartupAndOneRequestAsync()),
                new Benchmark($"{RequestCount} requests",$"The time to process {RequestCount} requests",x => x.MultipleSimpleRequestsAsync(RequestCount)),
                new Benchmark($"{RequestCount} requests with dictionary",$"The time to process {RequestCount} requests with a dicitonary query parameter",x => x.MultipleDictionaryRequestsAsync(RequestCount))
            };
        }

        public async Task<IReadOnlyList<Benchmark>> RunAsync()
        {            
            for (int i = 0; i < this.benchmarks.Length; i++)
            {
                await this.ExecuteBenchmarkAsync(this.benchmarks[i]);
            }
            return this.benchmarks;
        }

        private async Task ExecuteBenchmarkAsync(Benchmark benchmark)
        {
            Console.WriteLine($"Benchmark: {benchmark.Title}");
            for (int i = 0; i < this.performanceTesters.Length; i++)
            {
                var benchmarkResult = await this.ExecuteMethodToBenchAsync(benchmark.MethodToBench, this.performanceTesters[i]);
                benchmark.Results.Add(benchmarkResult);
            }
            Console.WriteLine();
        }

        private async Task<BenchmarkResult> ExecuteMethodToBenchAsync(Func<PerformanceTester, Task> methodToBench, PerformanceTester performanceTester)
        {
            this.swatch.Restart();
            await methodToBench(performanceTester);
            this.swatch.Stop();
            Console.WriteLine($"{performanceTester.LibName}: {this.swatch.ElapsedMilliseconds}ms");
            return new BenchmarkResult(performanceTester.LibName, this.swatch.ElapsedMilliseconds);
        }
    }
}
