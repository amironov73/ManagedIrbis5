// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Biblio;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Biblio.Chapters
{
    [TestClass]
    public sealed class ChapterCollectionTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void ChapterCollection_Construction_1()
        {
            var collection = new ChapterCollection();
            Assert.IsNull (collection.Parent);
            Assert.AreEqual (0, collection.Count);
        }
    }
}
