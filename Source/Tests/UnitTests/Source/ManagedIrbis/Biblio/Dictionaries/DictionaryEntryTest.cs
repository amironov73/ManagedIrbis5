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
    public sealed class DictionaryEntryTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void DictionaryEntry_Construction_1()
        {
            var dictionary = new DictionaryEntry();
            Assert.IsNull (dictionary.Title);
            Assert.IsNotNull (dictionary.References);
            Assert.AreEqual (0, dictionary.References.Count);
        }
    }
}
