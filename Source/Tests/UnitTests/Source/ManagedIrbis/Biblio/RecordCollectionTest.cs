// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using ManagedIrbis.Biblio;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace UnitTests.ManagedIrbis.Biblio;

[TestClass]
public sealed class RecordCollectionTest
{
    [TestMethod]
    [Description ("Конструктор")]
    public void RecordCollection_Construction_1()
    {
        var collection = new RecordCollection();
        Assert.AreEqual (0, collection.Count);
    }

    [TestMethod]
    [Description ("Верификация")]
    public void RecordCollection_Verify_1()
    {
        var collection = new RecordCollection();
        Assert.IsTrue (collection.Verify (false));
    }

}
