// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable IdentifierTypo

#region Using directives

using BenchmarkDotNet.Running;

#endregion

namespace BarsikBenchmarks;

static class Program
{
    static void BenchmarkRun()
    {
        //BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).RunAll();
        BenchmarkRunner.Run (typeof(Program).Assembly);
    }

    static void DebugRun()
    {
        var benchmark = new SimplestBenchmark();
        for (var i = 0; i < 100; ++i)
        {
            benchmark.Interpreter_Execute_1();
        }
    }

    static void Main
        (
            string[] args
        )
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
