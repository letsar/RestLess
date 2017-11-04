using System;
using System.Collections.Generic;
using System.Text;

namespace Benchmarks
{
    public class BenchmarkResult
    {
        public BenchmarkResult(string libName, long elapsedMilliseconds)
        {
            this.LibName = libName;
            this.ElapsedMilliseconds = elapsedMilliseconds;
        }

        public string LibName { get; }

        public long ElapsedMilliseconds { get; }
    }
}
