// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using System.Linq;

using AM.Kotik;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace KotikTests;

[TestClass]
public sealed class RepeatParserTest
    : CommonParserTest
{
    [TestMethod]
    [Description ("Успешный разбор последовательности токенов")]
    public void RepeatParser_Parse_1()
    {
        var state = _GetState (" у попа была собака ");
        var identifierParser = Parser.Identifier;
        var parser = new RepeatParser<string> (identifierParser).End();
        var tokens = parser.ParseOrThrow (state).ToArray();
        Assert.IsNotNull (tokens);
        Assert.AreEqual (4, tokens.Length);
        Assert.AreEqual ("у", tokens[0]);
        Assert.AreEqual ("попа", tokens[1]);
        Assert.AreEqual ("была", tokens[2]);
        Assert.AreEqual ("собака", tokens[3]);
    }

    [TestMethod]
    [Description ("Успешный разбор последовательности токенов")]
    public void RepeatParser_Parse_2()
    {
        var state = _GetState (" у попа была 1 собака ");
        var identifierParser = Parser.Identifier;
        var parser = new RepeatParser<string> (identifierParser);
        var tokens = parser.ParseOrThrow (state).ToArray();
        Assert.IsNotNull (tokens);
        Assert.AreEqual (3, tokens.Length);
        Assert.AreEqual ("у", tokens[0]);
        Assert.AreEqual ("попа", tokens[1]);
        Assert.AreEqual ("была", tokens[2]);
    }

    [TestMethod]
    [Description ("Успешный разбор последовательности токенов")]
    public void RepeatParser_Parse_3()
    {
        var state = _GetState (" 1 у 2 попа 3 была 4 собака ");
        var identifierParser = Parser.Identifier;
        var parser = new RepeatParser<string> (identifierParser);
        var tokens = parser.ParseOrThrow (state).ToArray();
        Assert.IsNotNull (tokens);
        Assert.AreEqual (0, tokens.Length);
    }

    [TestMethod]
    [Description ("Успешный разбор последовательности токенов")]
    public void RepeatParser_Parse_4()
    {
        var state = _GetState (" у попа была собака ");
        var identifierParser = Parser.Identifier;
        var parser = new RepeatParser<string> (identifierParser, maxCount:3);
        var tokens = parser.ParseOrThrow (state).ToArray();
        Assert.IsNotNull (tokens);
        Assert.AreEqual (3, tokens.Length);
        Assert.AreEqual ("у", tokens[0]);
        Assert.AreEqual ("попа", tokens[1]);
        Assert.AreEqual ("была", tokens[2]);
    }

    [TestMethod]
    [ExpectedException (typeof (SyntaxException))]
    [Description ("Неуспешный разбор последовательности токенов")]
    public void RepeatParser_Parse_5()
    {
        var state = _GetState (" у попа была собака ");
        var identifierParser = Parser.Identifier;
        var parser = new RepeatParser<string> (identifierParser, minCount:5);
        parser.ParseOrThrow (state);
    }

    [TestMethod]
    [ExpectedException (typeof (SyntaxException))]
    [Description ("Неуспешный разбор последовательности токенов")]
    public void RepeatParser_Parse_6()
    {
        var state = _GetState (" у 1 попа была собака ");
        var identifierParser = Parser.Identifier;
        var parser = new RepeatParser<string> (identifierParser).End();
        parser.ParseOrThrow (state);
    }
}
