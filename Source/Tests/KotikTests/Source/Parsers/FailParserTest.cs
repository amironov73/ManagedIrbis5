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
public sealed class FailParserTest
    : CommonParserTest
{
    [TestMethod]
    public void FailParser_TryParse_1()
    {
        var state = _GetState ("hello world");
        var parser = new FailParser<Unit> ("Some message");
        Assert.IsFalse (parser.TryParse (state, out _));
    }
}
