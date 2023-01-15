// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using AM.Kotik;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using static AM.Kotik.Parser;

#endregion

#nullable enable

namespace KotikTests;

[TestClass]
public sealed class ChainParserTest
    : CommonParserTest
{
    [TestMethod]
    [Description ("Цепочка из двух парсеров - успешно")]
    public void ChainParser_Two_1()
    {
        var state = _GetState ("hello 1");
        var parser = Chain (Identifier, Literal,
            (x1, x2) => x1 + "_" + x2).End();
        Assert.AreEqual ("hello_1", parser.ParseOrThrow (state));
    }

    [TestMethod]
    [Description ("Цепочка из двух парсеров - неуспешно")]
    public void ChainParser_Two_2()
    {
        var state = _GetState ("hello using");
        var parser = Chain (Identifier, Literal,
            (x1, x2) => x1 + "_" + x2).End();
        Assert.ThrowsException<SyntaxException> (() => parser.ParseOrThrow (state));
    }

    [TestMethod]
    [Description ("Цепочка из двух парсеров - неуспешно")]
    public void ChainParser_Two_3()
    {
        var state = _GetState ("hello, 1");
        var parser = Chain (Identifier, Literal,
            (x1, x2) => x1 + "_" + x2).End();
        Assert.ThrowsException<SyntaxException> (() => parser.ParseOrThrow (state));
    }

    [TestMethod]
    [Description ("Цепочка из трех парсеров - успешно")]
    public void ChainParser_Three_1()
    {
        var state = _GetState ("hello 1 using");
        var reserved = Reserved ("using");
        var parser = Chain (Identifier, Literal, reserved,
            (x1, x2, x3) => x1 + "_" + x2 + "_" + x3).End();
        Assert.AreEqual ("hello_1_using", parser.ParseOrThrow (state));
    }

    [TestMethod]
    [Description ("Цепочка из трех парсеров - успешно")]
    public void ChainParser_Three_2()
    {
        var state = _GetState ("hello using", true);
        var reserved = Reserved ("using");
        var parser = Chain (Identifier, Literal.Optional(), reserved,
            (x1, x2, x3) => x1 + "_" + x2 + "_" + x3).End();
        Assert.AreEqual ("hello__using", parser.ParseOrThrow (state));
    }

    [TestMethod]
    [Description ("Цепочка из трех парсеров - неуспешно")]
    public void ChainParser_Three_3()
    {
        var state = _GetState ("hello using");
        var reserved = Reserved ("using");
        var parser = Chain (Identifier, Literal, reserved,
            (x1, x2, x3) => x1 + "_" + x2 + "_" + x3).End();
        Assert.ThrowsException<SyntaxException> (() => parser.ParseOrThrow (state));
    }

    [TestMethod]
    [Description ("Цепочка из трех парсеров - неуспешно")]
    public void ChainParser_Three_4()
    {
        var state = _GetState ("hello, 1");
        var reserved = Reserved ("using");
        var parser = Chain (Identifier, Literal, reserved,
            (x1, x2, x3) => x1 + "_" + x2 + "_" + x3).End();
        Assert.ThrowsException<SyntaxException> (() => parser.ParseOrThrow (state));
    }
}
