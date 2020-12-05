// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global

using BenchmarkDotNet.Running;

namespace CoreBenchmarks
{
    class Program
    {
        static void Main()
        {
            BenchmarkRunner.Run<ListBenchmark>();
        }
    }
}