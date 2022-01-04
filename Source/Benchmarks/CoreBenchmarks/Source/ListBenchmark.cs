// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace

#region Using directives

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using AM.Collections;

using BenchmarkDotNet.Attributes;

#endregion

#nullable enable

namespace CoreBenchmarks;

[MemoryDiagnoser]
// не надо делать sealed!
public class ListBenchmark
{
    [Benchmark (Baseline = true)]
    public int UseStandardList()
    {
        int result = 0;
        List<int> list = new List<int>();

        unchecked
        {
            for (int i = 0; i < 1000; i++)
            {
                list.Add (i);
            }

            result += list.Count;
        }

        return result;
    }

    [Benchmark]
    public int UseLocalList()
    {
        int result = 0;
        Span<int> initial = stackalloc int[1024];
        LocalList<int> list = new LocalList<int> (initial);

        unchecked
        {
            for (int i = 0; i < 1000; i++)
            {
                list.Add (i);
            }

            result += list.Count;
        }

        list.Dispose();

        return result;
    }

    [Benchmark]
    [SkipLocalsInit]
    public int UseSkipLocalsInit()
    {
        int result = 0;
        Span<int> initial = stackalloc int[1024];
        LocalList<int> list = new LocalList<int> (initial);

        unchecked
        {
            for (int i = 0; i < 1000; i++)
            {
                list.Add (i);
            }

            result += list.Count;
        }

        list.Dispose();

        return result;
    }
}
