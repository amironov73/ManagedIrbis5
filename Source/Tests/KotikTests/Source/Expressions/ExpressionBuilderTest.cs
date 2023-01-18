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
public sealed class ExpressionBuilderTest
    : CommonParserTest
{
    [TestMethod]
    public void ExpressionBuilder_Build_1()
    {
        var state = _GetState ("(1 + 2) * (3 * 4 - 5)");
        var parser = ExpressionBuilder.Build
            (
                Parser.Literal,
                new[]
                {
                    new[] { "<<", ">>" },
                    new[] { "&", "|" },
                    new[] { "*", "/", "%" },
                    new[] { "+", "-" },
                },
                IntegerArithmetic
            )
            .End();
        var value = (int) parser.ParseOrThrow (state);
        Assert.AreEqual (21, value);

        state = _GetState ("1 | 2");
        value = (int) parser.ParseOrThrow (state);
        Assert.AreEqual (3, value);

        state = _GetState ("1 << 2");
        value = (int) parser.ParseOrThrow (state);
        Assert.AreEqual (4, value);
    }
}
