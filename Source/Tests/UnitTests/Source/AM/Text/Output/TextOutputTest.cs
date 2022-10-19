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
public sealed class TextOutputTest
{
    [TestMethod]
    public void TextOutput_Construction_1()
    {
        var output = new TextOutput();

        Assert.IsFalse (output.HaveError);
    }

    [TestMethod]
    public void TextOutput_ToString_1()
    {
        const string expected = "Quick brown fox";

        var output = new TextOutput();
        output.Write (expected);

        var actual = output.ToString();

        Assert.AreEqual (expected, actual);
    }
}
