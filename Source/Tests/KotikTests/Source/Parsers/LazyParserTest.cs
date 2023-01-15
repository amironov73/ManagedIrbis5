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
public sealed class LazyParserTest
    : CommonParserTest
{
    [TestMethod]
    [Description ("Успешный разбор")]
    public void LazyParser_Parse_1()
    {
        var state = _GetState ("hello world");
        var parser = Parser.Lazy (() => Parser.Identifier.Repeat()).End();
        var tokens = parser.ParseOrThrow (state).ToArray();
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Length);
        Assert.AreEqual ("hello", tokens[0]);
        Assert.AreEqual ("world", tokens[1]);
    }

    [TestMethod]
    [Description ("Успешный разбор")]
    public void LazyParser_Parse_2()
    {
        var state = _GetState ("hello, world");
        var comma = Parser.Term (",");
        var parser = Parser.Lazy (() => Parser.Identifier.Separated(comma)).End();
        var tokens = parser.ParseOrThrow (state).ToArray();
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Length);
        Assert.AreEqual ("hello", tokens[0]);
        Assert.AreEqual ("world", tokens[1]);
    }
}
