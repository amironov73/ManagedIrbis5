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
public class TeeOutputTest
{
    [TestMethod]
    public void TeeOutput_Write_1()
    {
        const string text = "Hello, World!";
        var first = new TextOutput();
        var second = new TextOutput();
        var tee = new TeeOutput (new AbstractOutput[] { first, second });

        tee.Write (text);

        Assert.IsFalse (tee.HaveError);
        Assert.IsFalse (first.HaveError);
        Assert.AreEqual (text, first.ToString());
        Assert.IsFalse (second.HaveError);
        Assert.AreEqual (text, second.ToString());
    }

    [TestMethod]
    public void TeeOutput_WriteError_1()
    {
        const string text = "Hello, World!";
        var first = new TextOutput();
        var second = new TextOutput();
        var tee = new TeeOutput (new AbstractOutput[] { first, second });

        tee.WriteError (text);

        Assert.IsTrue (tee.HaveError);
        Assert.IsTrue (first.HaveError);
        Assert.AreEqual (text, first.ToString());
        Assert.IsTrue (second.HaveError);
        Assert.AreEqual (text, second.ToString());
    }
}
