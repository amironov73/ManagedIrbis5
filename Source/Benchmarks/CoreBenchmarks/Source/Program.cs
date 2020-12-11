// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global

using BenchmarkDotNet.Running;

namespace CoreBenchmarks
{
    class Program
    {
        static void BenchmarkRun()
        {
            BenchmarkRunner.Run<ListBenchmark>();
            BenchmarkRunner.Run<FastNumberBenchmark>();
            BenchmarkRunner.Run<TextNavigatorBenchmark>();
        }

        static void DebugRun()
        {
            var benchmark = new ListBenchmark();
            for (var i = 0; i < 1000; ++i)
            {
                benchmark.UseSkipLocalsInit();
            }
        }

        static void Main(string[] args)
        {
            if (args.Length == 1 && args[0] == "debug")
            {
                DebugRun();
            }
            else
            {
                BenchmarkRun();
            }
        }
    }
}