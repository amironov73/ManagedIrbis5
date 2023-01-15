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
public sealed class OneOfParserTest
    : CommonParserTest
{
    [TestMethod]
    [Description ("Успешный разбор смеси токенов")]
    public void OneOfParser_Parse_1()
    {
        var state = _GetState ("hello ++ world --");
        var parser = new OneOfParser<string> (Parser.Identifier,
            Parser.Term ("++", "--"));
        var value = parser.ParseOrThrow (state);
        Assert.IsNotNull (value);
        Assert.AreEqual ("hello", value);
    }

    [TestMethod]
    [ExpectedException (typeof (SyntaxException))]
    [Description ("Неуспешный разбор смеси токенов")]
    public void OneOfParser_Parse_2()
    {
        var state = _GetState ("using ++ world --");
        var parser = new OneOfParser<string> (Parser.Identifier,
            Parser.Term ("++", "--"));
        parser.ParseOrThrow (state);
    }
}
