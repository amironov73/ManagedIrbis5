// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using ManagedIrbis.Catalog;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Catalog;

[TestClass]
public sealed class CatalogStateTest
{
    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void CatalogState_Construction_1()
    {
        var state = new CatalogState();
        Assert.AreEqual (0, state.Id);
        Assert.IsNull (state.Database);
        Assert.AreEqual (default (DateTime), state.Date);
        Assert.AreEqual (0, state.MaxMfn);
        Assert.IsNull (state.Records);
        Assert.IsNull (state.LogicallyDeleted);
    }
}