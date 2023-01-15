// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Kotik;

#endregion

#nullable enable

namespace KotikTests;

[TestClass]
public sealed class ParseStateTest
{
    [TestMethod]
    public void ParseState_Construction_1()
    {
        var tokens = Array.Empty<Token>();
        var state = new ParseState (tokens);
        Assert.IsFalse (state.HasCurrent);
        Assert.AreEqual (0, state.Location);
        Assert.IsFalse (state.Advance ());
        Assert.AreEqual (1, state.Location);
    }

    [TestMethod]
    public void ParseState_Advance_1()
    {
        var tokens = new Token[]
        {
            new (TokenKind.Char, "a"),
            new (TokenKind.String, "b"),
        };
        var state = new ParseState (tokens);

        Assert.IsTrue (state.HasCurrent);
        var current = state.Current;
        Assert.IsNotNull (current);
        Assert.AreEqual (TokenKind.Char, current.Kind);
        Assert.AreEqual ("a", current.Value);
        Assert.AreEqual (0, state.Location);

        Assert.IsTrue (state.Advance ());
        Assert.IsTrue (state.HasCurrent);
        current = state.Current;
        Assert.AreEqual (TokenKind.String, current.Kind);
        Assert.AreEqual ("b", current.Value);
        Assert.AreEqual (1, state.Location);

        Assert.IsFalse (state.Advance ());
        Assert.IsFalse (state.HasCurrent);
        Assert.AreEqual (2, state.Location);
    }

    [TestMethod]
    public void ParseState_LookAhead_2()
    {
        var tokens = new Token[]
        {
            new (TokenKind.Char, "a"),
            new (TokenKind.String, "b"),
        };
        var state = new ParseState (tokens);

        var value = state.LookAhead (1);
        Assert.IsNotNull (value);
        Assert.AreEqual (TokenKind.String, value.Kind);
        Assert.AreEqual ("b", value.Value);

        value = state.LookAhead (2);
        Assert.IsNull (value);
    }
}
