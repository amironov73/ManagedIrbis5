// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using System;
using System.Buffers;
using System.IO;
using System.Text;

using CommunityToolkit.HighPerformance.Buffers;

#endregion

namespace UnitTests.CommunityToolkit;

[TestClass]
public class ArrayPoolBufferWriterTest
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

                    output.Write ($"{counter:X8}>");
                }

                output.Write ($" {b:x2}");
                ++counter;
            }

            output.WriteLine();
        }
    }

    [TestMethod]
    public void ArrayPoolBufferWriter_Write_1()
    {
        const string text = "У попа была собака, он ее любил, " +
                            "она съела кусок мяса, он ее убил. В землю закопал " +
                            "и надпись написал.";

        var textBytes = Encoding.UTF8.GetBytes (text);

        using var writer = new ArrayPoolBufferWriter <byte>();
        writer.Write (textBytes);

        var memory = writer.WrittenMemory;
        Dump (memory, Console.Out);
        Console.WriteLine();
    }
}
