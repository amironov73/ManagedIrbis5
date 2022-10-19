// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text.Output;

#endregion

#nullable enable

namespace UnitTests.AM.Text.Output;

[TestClass]
public sealed class OutputWriterTest
{
    [TestMethod]
    public void OutputWriter_WriteLine_1()
    {
        const string expected = "Quick brown fox";

        var output = new TextOutput();
        var writer = new OutputWriter (output);
        writer.WriteLine (expected);

        var actual = output.ToString()
            .TrimEnd();

        Assert.AreEqual (expected, actual);
    }

    [TestMethod]
    public void OutputWriter_WriteLine_2()
    {
        const int value = 235;
        var expected = value.ToString();

        var output = new TextOutput();
        var writer = new OutputWriter (output);

        writer.WriteLine (value);

        var actual = output.ToString()
            .TrimEnd();

        Assert.AreEqual (expected, actual);
    }
}
