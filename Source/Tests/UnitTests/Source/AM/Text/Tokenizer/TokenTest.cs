// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;
using AM.Text;
using AM.Text.Tokenizer;

#endregion

#nullable enable

namespace UnitTests.AM.Text.Tokenizer;

[TestClass]
public class TokenTest
{
    private void _TestSerialization
        (
            Token first
        )
    {
        var bytes = first.SaveToMemory();
        var second = bytes.RestoreObjectFromMemory<Token>();

        Assert.IsNotNull (second);
        Assert.AreEqual (first.Value, second.Value);
        Assert.AreEqual (first.Column, second.Column);
        Assert.AreEqual (first.Line, second.Line);
        Assert.AreEqual (first.Kind, second.Kind);
    }

    [TestMethod]
    public void Token_Serialization_1()
    {
        var token = new Token();
        _TestSerialization (token);

        token = new Token (TokenKind.Number, "123", 1, 2);
        _TestSerialization (token);
    }

    [TestMethod]
    public void Token_Implicit_1()
    {
        Token token = "Hello";
        Assert.AreEqual ("Hello", token.Value);

        string hello = token!;
        Assert.AreEqual ("Hello", hello);
    }

    [TestMethod]
    public void Token_ToString_1()
    {
        var token = new Token (TokenKind.Number, "123", 1, 2);
        var actual = token.ToString();
        Assert.AreEqual
            (
                "Kind: Number, Column: 2, Line: 1, Value: 123",
                actual
            );
    }

    [TestMethod]
    public void Token_IsEOF_1()
    {
        var token = new Token (TokenKind.Number, "123", 1, 2);
        Assert.IsFalse (token.IsEOF);

        token = new Token (TokenKind.EOF, null, 0, 0);
        Assert.IsTrue (token.IsEOF);
    }

    [TestMethod]
    public void Token_Convert_1()
    {
        string[] words = { "Hello", "Irbis", "Word" };
        var tokens = Token.Convert (words);
        Assert.AreEqual (words.Length, tokens.Length);
        for (var i = 0; i < words.Length; i++)
        {
            Assert.AreEqual (words[i], tokens[i].Value);
        }
    }

    [TestMethod]
    public void Token_FromNavigator_1()
    {
        const string text = "Hello, world!";
        var navigator = new TextNavigator (text);
        navigator.ReadChar();
        navigator.ReadChar();
        navigator.ReadChar();
        navigator.ReadChar();
        navigator.ReadChar();
        navigator.SkipWhitespaceAndPunctuation();
        var token = Token.FromNavigator
            (
                navigator,
                TokenKind.Word,
                navigator.GetRemainingText().ToString()
            );
        Assert.IsFalse (token.IsEOF);
        Assert.AreEqual (1, token.Line);
        Assert.AreEqual (8, token.Column);
        Assert.AreEqual (TokenKind.Word, token.Kind);
        Assert.AreEqual ("world!", token.Value);
    }

    [TestMethod]
    public void Token_FromNavigator_2()
    {
        const string text = "Hello, world!";
        var navigator = new TextNavigator (text);
        navigator.ReadChar();
        navigator.ReadChar();
        navigator.ReadChar();
        navigator.ReadChar();
        navigator.ReadChar();
        navigator.SkipWhitespaceAndPunctuation();
        var token = Token.FromNavigator
            (
                navigator,
                navigator.GetRemainingText().ToString()
            );
        Assert.IsFalse (token.IsEOF);
        Assert.AreEqual (1, token.Line);
        Assert.AreEqual (8, token.Column);
        Assert.AreEqual (TokenKind.Unknown, token.Kind);
        Assert.AreEqual ("world!", token.Value);
    }
}
