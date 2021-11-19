// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class SupplierTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Supplier_Construction_1()
        {
            var supplier = new Supplier();
            Assert.IsNotNull (supplier);
        }
    }
}
