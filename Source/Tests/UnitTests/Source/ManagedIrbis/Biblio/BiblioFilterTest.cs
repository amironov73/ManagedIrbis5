// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM.Text.Output;

using ManagedIrbis.Biblio;
using ManagedIrbis.Providers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Biblio
{
    [TestClass]
    public sealed class BiblioFilterTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void BiblioFilter_Construction_1()
        {
            var filter = new BiblioFilter();
            Assert.IsNull(filter.FormatExpression);
            Assert.IsNull(filter.SelectExpression);
            Assert.IsNull(filter.SortExpression);
        }

        [TestMethod]
        [Description ("Верификация")]
        public void BiblioFilter_Verify_1()
        {
            var filter = new BiblioFilter();
            Assert.IsTrue (filter.Verify (false));
        }

    }
}
