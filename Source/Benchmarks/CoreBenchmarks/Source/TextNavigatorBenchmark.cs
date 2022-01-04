// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable NotAccessedField.Local
// ReSharper disable StringLiteralTypo

#region Using directives

using AM.Text;

using BenchmarkDotNet.Attributes;

#endregion

#nullable enable

namespace CoreBenchmarks;

[MemoryDiagnoser]
// не надо делать sealed!
public class TextNavigatorBenchmark
{
    private static readonly string _uPopaBylaSobaka
        = "У попа была собака, он ее любил";

    [Benchmark (Baseline = true)]
    public int TN_ReadWord()
    {
        var navigator = new TextNavigator (_uPopaBylaSobaka);
        var result = 0;

        while (true)
        {
            var word = navigator.ReadWord();
            if (word.IsEmpty)
            {
                break;
            }

            result += word.Length;
        }

        return result;
    }

    [Benchmark]
    public int VTN_ReadWord()
    {
        var navigator = new ValueTextNavigator (_uPopaBylaSobaka);
        var result = 0;

        while (true)
        {
            var word = navigator.ReadWord();
            if (word.IsEmpty)
            {
                break;
            }

            result += word.Length;
        }

        return result;
    }

    [Benchmark]
    public int UTN_ReadWord()
    {
        var navigator = new UnsafeTextNavigator (_uPopaBylaSobaka);
        var result = 0;

        while (true)
        {
            var word = navigator.ReadWord();
            if (word.IsEmpty)
            {
                break;
            }

            result += word.Length;
        }

        return result;
    }
}
