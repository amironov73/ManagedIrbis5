// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text.Output;

using Moq;

#endregion

#nullable enable

namespace UnitTests.AM.Text.Output;

[TestClass]
public sealed class DummyOutputTest
{
    private AbstractOutput GetMock()
    {
        var mock = new Mock<AbstractOutput>();

        return mock.Object;
    }

    [TestMethod]
    public void DummyOutput_Construction_1()
    {
        var inner = GetMock();
        var outer = new DummyOutput (inner);

        Assert.IsFalse (outer.HaveError);
    }
}
