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
public sealed class MapParserTest
    : CommonParserTest
{
    [TestMethod]
    [Description ("Успешная трансформация")]
    public void MapParser_Parse_1()
    {
        var state = _GetState ("hello");
        var parser = Parser.Identifier
            .Map (x => new ValueHolder<int> (x.Length)).End();
        var value = parser.ParseOrThrow (state).Value;
        Assert.AreEqual (5, value);
    }

    [TestMethod]
    [ExpectedException (typeof (SyntaxException))]
    [Description ("Неуспешная трансформация")]
    public void MapParser_Parse_2()
    {
        var state = _GetState ("1");
        var parser = Parser.Identifier
            .Map (x => new ValueHolder<int> (x.Length)).End();
        parser.ParseOrThrow (state);
    }
}
