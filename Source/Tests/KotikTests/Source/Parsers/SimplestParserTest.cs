// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using AM.Kotik;
using AM.Kotik.Parsers;
using AM.Kotik.Tokenizers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace KotikTests;

[TestClass]
public sealed class SimplestParserTest
{
    private ParseState _GetState
        (
            string text,
            string kind,
            string regex
        )
    {
        var tokenizer = new Tokenizer
        {
            Tokenizers =
            {
                new WhitespaceTokenizer(),
                new RegexTokenizer (kind, regex)
            }
        };
        var tokens = tokenizer.Tokenize (text);

        return new ParseState (tokens);
    }
    
    [TestMethod]
    [Description ("Успешный простой разбор одного токена")]
    public void SimplestParser_Parse_1()
    {
        var state = _GetState ("A", "char", "^[A-Z]$");
        var parser = new SimplestParser<string> ("char");
        var value = parser.ParseOrThrow (state);
        Assert.AreEqual ("A", value);
    }
    
    [TestMethod]
    [Description ("Успешный простой разбор пары токенов")]
    public void SimplestParser_Parse_2()
    {
        var state = _GetState ("A B", "char", "^[A-Z]$");
        var parser = new SimplestParser<string> ("char")
            .Repeated();
        var value = parser.ParseOrThrow (state);
        Assert.AreEqual (2, value.Count);
        Assert.AreEqual ("A", value[0]);
        Assert.AreEqual ("B", value[1]);
    }
    
    [TestMethod]
    [Description ("Успешный простой разбор трех токенов")]
    public void SimplestParser_Parse_3()
    {
        var state = _GetState ("A B C", "char", "^[A-Z]$");
        var parser = new SimplestParser<string> ("char")
            .Repeated();
        var value = parser.ParseOrThrow (state);
        Assert.AreEqual (3, value.Count);
        Assert.AreEqual ("A", value[0]);
        Assert.AreEqual ("B", value[1]);
        Assert.AreEqual ("C", value[2]);
    }
    
}
