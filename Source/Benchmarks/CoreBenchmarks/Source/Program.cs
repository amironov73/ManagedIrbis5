// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global

#region Using directives

using BenchmarkDotNet.Running;

#endregion

#nullable enable

namespace CoreBenchmarks;

class Program
{
    static void BenchmarkRun()
    {
        //BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).RunAll();
        BenchmarkRunner.Run (typeof (Program).Assembly);
    }

    static void DebugRun()
    {
        var benchmark = new FastNumberBenchmark();
        for (var i = 0; i < 1000; ++i)
        {
            benchmark.FastNumber_Int32ToChars();
        }
    }

    static void Main (string[] args)
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
