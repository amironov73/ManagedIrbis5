// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using ManagedIrbis.Catalog;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Catalog;

[TestClass]
public sealed class RecordStateTest
{
    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void RecordState_Construction_1()
    {
        var state = new RecordState();
        Assert.AreEqual (0, state.Id);
        Assert.AreEqual (0, state.Mfn);
        Assert.AreEqual (0, state.Version);
    }
}