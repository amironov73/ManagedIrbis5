// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM.Text;

using ManagedIrbis.Biblio;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Biblio
{
    [TestClass]
    public sealed class BiblioItemTest
    {
        [TestMethod]
        [Description ("Конструктор")]
        public void BiblioItem_Construction_1()
        {
            var item = new BiblioItem();
            Assert.IsNull (item.Chapter);
            Assert.AreEqual (0, item.Number);
            Assert.IsNull (item.Record);
            Assert.IsNull (item.Description);
            Assert.IsNull (item.Order);
            Assert.IsNotNull (item.Terms);
            Assert.AreEqual (0, item.Terms.Count);
            Assert.IsNull (item.UserData);
        }

        [TestMethod]
        [Description ("Верификация")]
        public void BiblioItem_Verify_1()
        {
            var item = new BiblioItem();
            Assert.IsTrue (item.Verify (false));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void BiblioItem_ToString_1()
        {
            var item = new BiblioItem();
            Assert.AreEqual ("(null)\n(null)", item.ToString().DosToUnix());
        }

    }
}
