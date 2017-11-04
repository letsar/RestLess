using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarks
{
    public class Benchmark
    {
        public Benchmark(string title, string description, Func<PerformanceTester, Task> methodToBench)
        {
            this.Title = title;
            this.Description = description;
            this.MethodToBench = methodToBench;
            this.Results = new List<BenchmarkResult>();
        }

        public string Title { get; }

        public string Description { get; }

        public Func<PerformanceTester, Task> MethodToBench{ get; }

        public List<BenchmarkResult> Results { get; }
    }
}
