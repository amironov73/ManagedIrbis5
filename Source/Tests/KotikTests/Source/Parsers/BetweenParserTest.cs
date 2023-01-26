// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using System.Collections.Generic;
using System.Linq;

using AM.Kotik;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace KotikTests;

[TestClass]
public sealed class BetweenParserTest
    : CommonParserTest
{
    [TestMethod]
    [Description ("Успешный разбор токена между двумя другими")]
    public void BetweenParser_Parse_1()
    {
        var state = _GetState (" ( hello ) ");
        var brackets = Parser.Term ("(", ")");
        var identifier = Parser.Identifier;
        var parser = new BetweenParser<string, string, string>
            (
                brackets,
                identifier,
                brackets
            )
            .End();
        var value = parser.ParseOrThrow (state);
        Assert.IsNotNull (value);
        Assert.AreEqual ("hello", value);
    }

    [TestMethod]
    [Description ("Успешный разбор последовательности токенов между двумя термами")]
    public void BetweenParser_Parse_2()
    {
        var state = _GetState (" ( hello, world ) ");
        var brackets = Parser.Term ("(", ")");
        var comma = Parser.Term (",");
        var identifier = Parser.Identifier;
        var identifiers = new SeparatedParser<string, string, string>
            (
                identifier,
                comma
            );
        var parser = new BetweenParser<string, IList<string>, string>
            (
                brackets,
                identifiers,
                brackets
            )
            .End();
        var tokens = parser.ParseOrThrow (state).ToArray();
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Length);
        Assert.AreEqual ("hello", tokens[0]);
        Assert.AreEqual ("world", tokens[1]);
    }

    [TestMethod]
    [ExpectedException (typeof (SyntaxException))]
    [Description ("Неуспешный разбор токена между двумя другими")]
    public void BetweenParser_Parse_3()
    {
        var state = _GetState (" ( using ) ");
        var brackets = Parser.Term ("(", ")");
        var identifier = Parser.Identifier;
        var parser = new BetweenParser<string, string, string>
            (
                brackets,
                identifier,
                brackets
            )
            .End();
        parser.ParseOrThrow (state);
    }

    [TestMethod]
    [ExpectedException (typeof (SyntaxException))]
    [Description ("Неуспешный разбор токена между двумя другими")]
    public void BetweenParser_Parse_4()
    {
        var state = _GetState (" + hello - ");
        var brackets = Parser.Term ("(", ")");
        var identifier = Parser.Identifier;
        var parser = new BetweenParser<string, string, string>
            (
                brackets,
                identifier,
                brackets
            )
            .End();
        parser.ParseOrThrow (state);
    }
}
