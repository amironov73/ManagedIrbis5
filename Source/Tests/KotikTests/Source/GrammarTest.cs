// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM.Kotik;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace KotikTests;

[TestClass]
public sealed class GrammarTest
{
    private ParseState _GetState
        (
            string text
        )
    {
        var tokenizer = new Tokenizer();
        var tokens = tokenizer.Tokenize (text);

        return new ParseState (tokens);
    }

    [TestMethod]
    [Description ("null")]
    public void Grammar_Literal_1()
    {
        var state = _GetState (" null hello ");
        var literal = Grammar.Literal;
        Assert.IsNull (literal.ParseOrThrow (state, true));
        Assert.ThrowsException<SyntaxException>
            (
                () => literal.ParseOrThrow (state, true)
            );
    }

    [TestMethod]
    [Description ("bool")]
    public void Grammar_Literal_2()
    {
        var state = _GetState (" true false hello ");
        var literal = Grammar.Literal;
        Assert.AreEqual (true, literal.ParseOrThrow (state, true));
        Assert.AreEqual (false, literal.ParseOrThrow (state, true));
        Assert.ThrowsException<SyntaxException>
            (
                () => literal.ParseOrThrow (state, true)
            );
    }

    [TestMethod]
    [Description ("Int32")]
    public void Grammar_Literal_3()
    {
        var state = _GetState (" 1, -1 hello ");
        var literal = Grammar.Literal;
        Assert.AreEqual (1, literal.ParseOrThrow (state, true));
        state.Advance();
        Assert.AreEqual (-1, literal.ParseOrThrow (state, true));
        Assert.ThrowsException<SyntaxException>
            (
                () => literal.ParseOrThrow (state, true)
            );
    }

    [TestMethod]
    [Description ("Int64")]
    public void Grammar_Literal_4()
    {
        var state = _GetState (" 1l, -1L hello ");
        var literal = Grammar.Literal;
        Assert.AreEqual (1L, literal.ParseOrThrow (state, true));
        state.Advance();
        Assert.AreEqual (-1L, literal.ParseOrThrow (state, true));
        Assert.ThrowsException<SyntaxException>
            (
                () => literal.ParseOrThrow (state, true)
            );
    }
}
