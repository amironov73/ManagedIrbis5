// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Biblio;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Biblio
{
    [TestClass]
    public sealed class BiblioTermTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void BiblioTerm_Construction_1()
        {
            var term = new BiblioTerm();
            Assert.IsNull (term.Dictionary);
            Assert.IsNull (term.Title);
            Assert.IsNull (term.Extended);
            Assert.IsNull (term.Order);
            Assert.IsNull (term.Item);
        }

        [TestMethod]
        [Description ("Верификация")]
        public void BiblioTerm_Verify_1()
        {
            var term = new BiblioTerm();
            Assert.IsTrue (term.Verify (false));
        }

    }
}
