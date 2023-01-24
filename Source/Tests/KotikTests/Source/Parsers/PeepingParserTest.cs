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
public sealed class PeepingParserTest
    : CommonParserTest
{
    [TestMethod]
    [Description ("Успешный парсинг")]
    public void PeepingParser_Parse_1()
    {
        var state = _GetState ("hello!");
        var bang = Parser.Term ("!");
        var identifier = Parser.Identifier;
        var parser = new PeepingParser<string, string>
            (
                bang,
                identifier
            )
            .End();
        var value = parser.ParseOrThrow (state);
        Assert.IsNotNull (value);
        Assert.AreEqual ("hello", value);
    }

    [TestMethod]
    [Description ("Успешный парсинг")]
    public void PeepingParser_Parse_2()
    {
        var state = _GetState ("hello world!");
        var bang = Parser.Term ("!");
        var identifier = Parser.Identifier;
        var parser = new PeepingParser<string, IEnumerable<string>>
            (
                bang,
                identifier.Repeated()
            )
            .End();
        var value = parser.ParseOrThrow (state).ToArray();
        Assert.IsNotNull (value);
        Assert.AreEqual (2, value.Length);
        Assert.AreEqual ("hello", value[0]);
        Assert.AreEqual ("world", value[1]);
    }

    [TestMethod]
    [Description ("Неуспешный парсинг")]
    public void PeepingParser_Parse_3()
    {
        var state = _GetState ("hello world!");
        var bang = Parser.Term ("!");
        var identifier = Parser.Identifier;
        var parser = new PeepingParser<string, string>
            (
                bang,
                identifier
            )
            .End();

        Assert.ThrowsException<SyntaxException>
            (
                () => parser.ParseOrThrow (state)
            );

        var state2 = _GetState ("hello?");
        Assert.ThrowsException<SyntaxException>
            (
                () => parser.ParseOrThrow (state2)
            );
    }
}
