using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Benchmarks.Interfaces;

namespace Benchmarks
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var benchmarker = new Benchmarker();
            await benchmarker.RunAsync();
            Console.WriteLine("Benchmarks executed!");
            Console.Read();
        }        
    }
}
