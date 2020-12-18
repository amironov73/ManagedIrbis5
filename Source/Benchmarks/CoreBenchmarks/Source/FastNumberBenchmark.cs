// ReSharper disable CheckNamespace
// ReSharper disable NotAccessedField.Local

using System;
using System.Collections.Generic;
using System.Text;

using AM;

using BenchmarkDotNet.Attributes;

namespace CoreBenchmarks
{
    [MemoryDiagnoser]
    public class FastNumberBenchmark
    {
        private List<string> _lines;
        private int _data;
        private string _data2;

        [GlobalSetup]
        public void Setup()
        {
            _lines = new List<string>(100000);
            for (int i = 0; i < _lines.Capacity; i++)
            {
                _lines.Add(i.ToInvariantString());
            }
        }

        [Benchmark(Baseline = true)]
        public void Int32_Parse()
        {
            foreach (var line in _lines)
            {
                _data = int.Parse(line);
            }
        }

        [Benchmark]
        public void FastNumber_ParseInt32_String()
        {
            foreach (var line in _lines)
            {
                _data = FastNumber.ParseInt32(line);
            }
        }

        [Benchmark]
        public unsafe void FastNumber_ParseInt32_Pointer()
        {
            foreach (var line in _lines)
            {
                fixed (char* pointer = line)
                {
                    _data = FastNumber.ParseInt32(pointer, line.Length);
                }
            }
        }

        [Benchmark]
        public void Int32_ToString()
        {
            for (var i = 0; i < 10000; i++)
            {
                _data2 = i.ToInvariantString();
            }
        }

        [Benchmark]
        public void FastNumber_Int32ToString()
        {
            for (var i = 0; i < 10000; i++)
            {
                _data2 = FastNumber.Int32ToString(i);
            }
        }

        [Benchmark]
        public unsafe void FastNumber_Int32ToChars()
        {
            Span<char> buffer = stackalloc char[10];

            for (var i = 0; i < 10000; i++)
            {
                FastNumber.Int32ToChars(i, buffer);
            }
        }
    }
}
