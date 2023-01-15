// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

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
        var termParser = Parser.Term ("(", ")");
        var identifierParser = Parser.Identifier;
        var parser = new BetweenParser<string, string, string>
            (
                termParser,
                identifierParser,
                termParser
            )
            .End();
        var value = parser.ParseOrThrow (state);
        Assert.IsNotNull (value);
        Assert.AreEqual ("hello", value);
    }

    [TestMethod]
    [ExpectedException (typeof (SyntaxException))]
    [Description ("Неспешный разбор токена между двумя другими")]
    public void BetweenParser_Parse_2()
    {
        var state = _GetState (" ( using ) ");
        var termParser = Parser.Term ("(", ")");
        var identifierParser = Parser.Identifier;
        var parser = new BetweenParser<string, string, string>
            (
                termParser,
                identifierParser,
                termParser
            )
            .End();
        parser.ParseOrThrow (state);
    }

    [TestMethod]
    [ExpectedException (typeof (SyntaxException))]
    [Description ("Неспешный разбор токена между двумя другими")]
    public void BetweenParser_Parse_3()
    {
        var state = _GetState (" + hello - ");
        var termParser = Parser.Term ("(", ")");
        var identifierParser = Parser.Identifier;
        var parser = new BetweenParser<string, string, string>
            (
                termParser,
                identifierParser,
                termParser
            )
            .End();
        parser.ParseOrThrow (state);
    }
}
