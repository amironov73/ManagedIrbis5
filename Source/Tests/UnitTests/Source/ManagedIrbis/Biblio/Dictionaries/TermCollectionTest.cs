// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Biblio;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Biblio.Dictionaries
{
    [TestClass]
    public sealed class TermCollectionTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void TermCollection_Construction_1()
        {
            var collection = new TermCollection();
            Assert.AreEqual (0, collection.Count);
        }
    }
}
