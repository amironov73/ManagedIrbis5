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
public sealed class OptionalParserTest
    : CommonParserTest
{
    [TestMethod]
    [Description ("Успешный опциональный разбор")]
    public void BetweenParser_Parse_1()
    {
        var state = _GetState ("hello");
        var identifierParser = Parser.Identifier;
        var parser = new OptionalParser<string> (identifierParser).End();
        var value = parser.ParseOrThrow (state);
        Assert.IsNotNull (value);
        Assert.AreEqual ("hello", value);
    }

    [TestMethod]
    [Description ("Успешный опциональный разбор")]
    public void BetweenParser_Parse_2()
    {
        var state = _GetState ("1");
        var identifierParser = Parser.Identifier;
        var parser = new OptionalParser<string> (identifierParser);
        var value = parser.ParseOrThrow (state);
        Assert.IsNull (value);
    }

    [TestMethod]
    [ExpectedException (typeof (SyntaxException))]
    [Description ("Неуспешный опциональный разбор")]
    public void BetweenParser_Parse_3()
    {
        var state = _GetState ("1");
        var identifierParser = Parser.Identifier;
        var parser = new OptionalParser<string> (identifierParser).End();
        parser.ParseOrThrow (state);
    }
}
