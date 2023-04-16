// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Magazines;

using Moq;

#endregion

#nullable enable

namespace UnitTests.ManagedIrbis.Magazines;

[TestClass]
public sealed class BindingManagerTest
    : CommonMagazineTest
{
    /*

    [TestMethod]
    public void BindingManager_Construction_1()
    {
        var mock = new Mock<IIrbisConnection>();
        var connection = mock.Object;

        var manager = new BindingManager(connection);
        Assert.AreSame(connection, manager.Connection);
    }

    */
}
