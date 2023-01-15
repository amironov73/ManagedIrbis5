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
public sealed class ReturnParserTest
    : CommonParserTest
{
    [TestMethod]
    [Description ("Успешный разбор")]
    public void ReturnParser_Parse_1()
    {
        var state = _GetState ("hello");
        var parser = new ReturnParser<string> ("world");
        var value = parser.ParseOrThrow (state);
        Assert.AreEqual ("world", value);
    }
}
