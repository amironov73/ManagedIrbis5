// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable NotAccessedField.Local
// ReSharper disable StringLiteralTypo

using System;
using System.Collections.Generic;
using System.Text;

using AM.Text;

using BenchmarkDotNet.Attributes;

namespace CoreBenchmarks
{
    [MemoryDiagnoser]
    public class TextNavigatorBenchmark
    {
        private static readonly string _uPopaBylaSobaka = "У попа была собака, он ее любил";

        [Benchmark]
        public int ReadWord()
        {
            var navigator = new TextNavigator(_uPopaBylaSobaka);
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
}
