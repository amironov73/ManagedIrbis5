// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using ManagedIrbis;
using ManagedIrbis.BibTex;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace UnitTests.ManagedIrbis.BibTex;

[TestClass]
public sealed class TokenTest
{
    private Token _GetToken()
    {
        return new Token (TokenKind.Comma, ",");
    }

    [TestMethod]
    [Description ("Конструктор")]
    public void Token_Construction_1()
    {
        var token = _GetToken();
        Assert.AreEqual (TokenKind.Comma, token.Kind);
        Assert.AreEqual (",", token.Value);
    }

    [TestMethod]
    [Description ("Требование, чтобы токен был определенного типа")]
    [ExpectedException (typeof (IrbisException))]
    public void Token_MustBe_1()
    {
        var token = _GetToken();
        token.MustBe (TokenKind.Comment);
    }

    [TestMethod]
    [Description ("Требование, чтобы токен был определенного типа")]
    [ExpectedException (typeof (IrbisException))]
    public void Token_MustBe_2()
    {
        var token = _GetToken();
        token.MustBe (TokenKind.Comment, TokenKind.Equals);
    }

    [TestMethod]
    [Description ("Плоское текстовое представление")]
    public void Token_ToString_1()
    {
        var token = _GetToken();
        Assert.AreEqual ("Comma: ,", token.ToString());
    }

}
