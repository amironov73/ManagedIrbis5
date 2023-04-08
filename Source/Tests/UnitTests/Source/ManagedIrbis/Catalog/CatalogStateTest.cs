// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using ManagedIrbis.Catalog;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

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
        Assert.AreEqual (default, state.Date);
        Assert.AreEqual (0, state.MaxMfn);
        Assert.IsNull (state.Records);
        Assert.IsNull (state.LogicallyDeleted);
    }
}
