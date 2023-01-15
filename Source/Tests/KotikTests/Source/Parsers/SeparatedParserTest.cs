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
public sealed class SeparatedParserTest
    : CommonParserTest
{
    [TestMethod]
    [Description ("Успешный разбор последовательности токенов")]
    public void SeparatedParserTest_Parse_1()
    {
        var state = _GetState (" у, попа, была, собака ");
        var identifier = Parser.Identifier;
        var comma = Parser.Term (",");
        var parser = new SeparatedParser<string, string, string>
            (identifier, comma).End();
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
    public void SeparatedParserTest_Parse_2()
    {
        var state = _GetState (" у, попа, была, собака, ");
        var identifier = Parser.Identifier;
        var comma = Parser.Term (",");
        var parser = new SeparatedParser<string, string, string>
            (identifier, comma).End();
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
    public void SeparatedParserTest_Parse_3()
    {
        var state = _GetState (" у, попа, была, собака 1");
        var identifier = Parser.Identifier;
        var comma = Parser.Term (",");
        var parser = new SeparatedParser<string, string, string>
            (identifier, comma);
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
    public void SeparatedParserTest_Parse_4()
    {
        var state = _GetState (" у, попа, была, собака, 1");
        var identifier = Parser.Identifier;
        var comma = Parser.Term (",");
        var parser = new SeparatedParser<string, string, string>
            (identifier, comma);
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
    public void SeparatedParserTest_Parse_5()
    {
        var state = _GetState (" у, попа, была, собака,,");
        var identifier = Parser.Identifier;
        var comma = Parser.Term (",");
        var parser = new SeparatedParser<string, string, string>
            (identifier, comma);
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
    public void SeparatedParserTest_Parse_6()
    {
        var state = _GetState (" у, попа, была, собака");
        var identifier = Parser.Identifier;
        var comma = Parser.Term (",");
        var parser = new SeparatedParser<string, string, string>
            (identifier, comma, maximum:3);
        var tokens = parser.ParseOrThrow (state).ToArray();
        Assert.IsNotNull (tokens);
        Assert.AreEqual (3, tokens.Length);
        Assert.AreEqual ("у", tokens[0]);
        Assert.AreEqual ("попа", tokens[1]);
        Assert.AreEqual ("была", tokens[2]);
    }

    [TestMethod]
    [Description ("Успешный разбор последовательности токенов")]
    public void SeparatedParserTest_Parse_7()
    {
        var state = _GetState (" у, попа, была; собака");
        var identifier = Parser.Identifier;
        var comma = Parser.Term (",");
        var semicolon = Parser.Term (";");
        var parser = new SeparatedParser<string, string, string>
            (identifier, comma, semicolon);
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
    public void SeparatedParserTest_Parse_8()
    {
        var state = _GetState (" у, попа, была; собака");
        var identifier = Parser.Identifier;
        var comma = Parser.Term (",");
        var semicolon = Parser.Term (";");
        var parser = new SeparatedParser<string, string, string>
            (identifier, comma, semicolon,
                mininum:4);
        parser.ParseOrThrow (state);
    }
}
