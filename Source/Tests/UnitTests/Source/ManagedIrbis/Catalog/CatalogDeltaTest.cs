// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using ManagedIrbis.Catalog;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Catalog;

[TestClass]
public sealed class CatalogDeltaTest
{
    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void CatalogDelta_Construction_1()
    {
        var state = new CatalogDelta();
        Assert.AreEqual (0, state.Id);
        Assert.IsNull (state.Database);
        Assert.AreEqual (default (DateTime), state.FirstDate);
        Assert.AreEqual (default (DateTime), state.SecondDate);
        Assert.IsNull (state.NewRecords);
        Assert.IsNull (state.DeletedRecords);
        Assert.IsNull (state.AlteredRecords);
    }
}
