// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class SupplierIdTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void SupplierId_Construction_1()
        {
            var id = new SupplierId();
            Assert.IsNotNull (id);
        }
    }
}
