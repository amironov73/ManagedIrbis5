// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo

#region Using directives

using System.IO;

using AM.Scripting.Barsik;

using BenchmarkDotNet.Attributes;

#endregion

namespace BarsikBenchmarks;

[MemoryDiagnoser]
// не надо делать sealed!
public class SimplestBenchmark
{
    /// <summary>
    /// Наша супер-программа.
    /// </summary>
    private const string SourceCode = "x = 1; y = 2; z = x + y; println (z);";

    /// <summary>
    /// Глобальный интерпретатор.
    /// </summary>
    private static readonly Interpreter _barsik = new (TextReader.Null, TextWriter.Null);

    [Benchmark]
    public int Interpreter_Execute_1()
    {
        var result = 0;
        for (var i = 0; i < 100; i++)
        {
            _barsik.Execute (SourceCode);
            var z = (int) (object) _barsik.Context.Variables["z"]!;
            result += z;
        }

        return result;
    }

}
