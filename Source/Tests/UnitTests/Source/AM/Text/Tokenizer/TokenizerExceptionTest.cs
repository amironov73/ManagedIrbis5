// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text.Tokenizer;

#endregion

#nullable enable

namespace UnitTests.AM.Text.Tokenizer;

[TestClass]
public sealed class TokenizerExceptionTest
{
    [TestMethod]
    public void TokenizerException_Construction1()
    {
        var exception = new TokenizerException();
        Assert.IsNotNull (exception.Message);
    }

    [TestMethod]
    public void TokenizerException_Construction2()
    {
        const string expected = "Key";
        var exception = new TokenizerException (expected);
        Assert.AreEqual (expected, exception.Message);
    }

    [TestMethod]
    public void TokenizerException_Construction3()
    {
        var innerException = new Exception ("Message");
        const string expected = "Key";
        var exception = new TokenizerException
            (
                expected,
                innerException
            );
        Assert.AreEqual (expected, exception.Message);
        Assert.AreEqual (innerException, exception.InnerException);
    }
}
