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
    public sealed class BiblioDictionaryTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void BiblioDictionary_Construction_1()
        {
            var dictionary = new BiblioDictionary();
            Assert.AreEqual (0, dictionary.Count);
        }
    }
}
