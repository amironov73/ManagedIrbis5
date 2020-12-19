// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global

using BenchmarkDotNet.Running;

namespace CoreBenchmarks
{
    class Program
    {
        static void BenchmarkRun()
        {
            //BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).RunAll();
            BenchmarkRunner.Run(typeof(Program).Assembly);

        } // method BenchmarkRun

        static void DebugRun()
        {
            var benchmark = new FastNumberBenchmark();
            for (var i = 0; i < 1000; ++i)
            {
                benchmark.FastNumber_Int32ToChars();
            }
        } // method DebugRun

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
        } // method Main

    } // class Program

} // namespace CoreBenchmarks