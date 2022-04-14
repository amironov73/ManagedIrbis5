// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM.Text.Output;

using ManagedIrbis.Biblio;
using ManagedIrbis.Providers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

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