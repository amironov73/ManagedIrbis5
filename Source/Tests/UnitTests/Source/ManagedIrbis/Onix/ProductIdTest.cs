// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class ProductIdTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void ProductId_Construction_1()
        {
            var id = new ProductId();
            Assert.IsNull (id.IdValue);
            Assert.AreEqual (default, id.ProductIdType);
        }
    }
}
