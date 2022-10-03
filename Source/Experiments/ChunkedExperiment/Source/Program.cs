// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* BufferWriterUtility.cs -- методы расширения для IBufferWriter
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers;
using System.IO;
using System.Text;

#endregion

#nullable enable

namespace ChunkedExperiment;

internal static class Program
{
    private static void Dump
        (
            ReadOnlyMemory<byte> memory,
            TextWriter output
        )
    {
        if (memory.IsEmpty)
        {
            output.WriteLine ("-- empty --");
        }
        else
        {
            var counter = 0;
            foreach (var b in memory.Span)
            {
                if (counter % 16 == 0)
                {
                    if (counter != 0)
                    {
                        output.WriteLine();
                    }

                    output.Write ($"{counter:x8}>");
                }

                output.Write ($" {b:x2}");
                ++counter;
            }

            output.WriteLine();
        }
    }

    private static void Test001()
    {
        using var writer = new ArrayPoolWriter (ArrayPool<byte>.Shared, 24);
        writer.Write (new byte[]
        {
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
            11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
            21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
            31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
            41, 42, 43, 44, 45, 46, 47, 48, 49, 50,
            51, 52
        });
        writer.Dump (Console.Out);
        Console.WriteLine();

        var reader = MemoryReader.FromWriter (writer);
        var bytes = reader.ReadBlock (12);
        Dump (bytes, Console.Out);
        for (var i = 0; i < 12; i++)
        {
            bytes = reader.ReadBlock (12);
            Dump (bytes, Console.Out);
        }
    }

    private static void Test002()
    {
        const string text = "У попа была собака, он ее любил, " +
                            "она съела кусок мяса, он ее убил. В землю закопал " +
                            "и надпись написал.";
        using var writer = new ArrayPoolWriter (ArrayPool<byte>.Shared, 16);
        writer.Write (text, Encoding.UTF8);
        writer.Dump (Console.Out);
        Console.WriteLine();

        var reader = MemoryReader.FromWriter (writer);
        var bytes = reader.ReadBlock (12);
        Dump (bytes, Console.Out);
        for (var i = 0; i < 30; i++)
        {
            bytes = reader.ReadBlock (12);
            Dump (bytes, Console.Out);
        }
    }

    private static void Main()
    {
        Test001();
        Console.WriteLine (new string ('=', 80));

        Test002();
        Console.WriteLine (new string ('=', 80));
    }
}
