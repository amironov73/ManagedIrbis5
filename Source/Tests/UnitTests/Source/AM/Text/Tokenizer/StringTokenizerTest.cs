// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable StringLiteralTypo

#region Using directives

using System.Collections;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text.Tokenizer;

#endregion

#nullable enable

namespace UnitTests.AM.Text.Tokenizer;

[TestClass]
public sealed class StringTokenizerTest
{
    [TestMethod]
    public void StringTokenizer_Construction_1()
    {
        const string text = "Hello, world!";
        var tokenizer = new StringTokenizer (text);
        Assert.IsNotNull (tokenizer.Settings);
    }

    [TestMethod]
    public void StringTokenizer_GetAllTokens_1()
    {
        const string text = "Hello, world!";
        var tokenizer = new StringTokenizer (text);
        var tokens = tokenizer.GetAllTokens();
        Assert.AreEqual (5, tokens.Length);
        Assert.AreEqual ("Hello", tokens[0].Value);
        Assert.AreEqual (",", tokens[1].Value);
        Assert.AreEqual ("world", tokens[2].Value);
        Assert.AreEqual ("!", tokens[3].Value);
        Assert.AreEqual (TokenKind.EOF, tokens[4].Kind);
    }

    [TestMethod]
    [ExpectedException (typeof (TokenizerException))]
    public void StringTokenizer_ReadNumber_Exception_1()
    {
        const string text = "Hello, 123EWorld!";
        var tokenizer = new StringTokenizer (text);
        tokenizer.GetAllTokens();
    }

    [TestMethod]
    public void StringTokenizer_NextToken_1()
    {
        const string text = "Hello\r\nWorld!";
        var tokenizer = new StringTokenizer (text);
        var tokens = tokenizer.GetAllTokens();
        Assert.AreEqual (4, tokens.Length);
    }

    [TestMethod]
    public void StringTokenizer_NextToken_2()
    {
        const string text = "Hello World!";
        var tokenizer = new StringTokenizer (text)
        {
            Settings =
            {
                IgnoreWhitespace = false
            }
        };
        var tokens = tokenizer.GetAllTokens();
        Assert.AreEqual (5, tokens.Length);
    }

    [TestMethod]
    public void StringTokenizer_ReadWhitespace()
    {
        const string text = "Hello  World!";
        var tokenizer = new StringTokenizer (text)
        {
            Settings =
            {
                IgnoreWhitespace = false
            }
        };
        var tokens = tokenizer.GetAllTokens();
        Assert.AreEqual (5, tokens.Length);
    }

    [TestMethod]
    public void StringTokenizer_ReadString()
    {
        const string text = "Hello\"\\x123\"World!";
        var tokenizer = new StringTokenizer (text);
        var tokens = tokenizer.GetAllTokens();

        // TODO: fix this!

        Assert.AreEqual (3, tokens.Length);
    }

    [TestMethod]
    public void StringTokenizer_ReadChar()
    {
        var tokenizer = new StringTokenizer ("");
        tokenizer.ReadChar();
        Assert.AreEqual ('\0', tokenizer.ReadChar());
    }

    [TestMethod]
    public void StringTokenizer_GetEnumerator()
    {
        const string text = "Hello, World!";
        IEnumerable tokenizer = new StringTokenizer (text);
        var count = 0;
        foreach (var _ in tokenizer)
        {
            count++;
        }

        Assert.AreEqual (5, count);
    }

    [TestMethod]
    public void StringTokenizer_IgnoreEOF()
    {
        const string text = "Hello, world!";
        var tokenizer = new StringTokenizer (text)
        {
            Settings =
            {
                IgnoreEOF = true
            }
        };
        var tokens = tokenizer.GetAllTokens();
        Assert.AreEqual (4, tokens.Length);
        Assert.AreEqual ("Hello", tokens[0].Value);
        Assert.AreEqual (",", tokens[1].Value);
        Assert.AreEqual ("world", tokens[2].Value);
        Assert.AreEqual ("!", tokens[3].Value);
    }
}
