// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class ProductTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Product_Construction_1()
        {
            var product = new Product();
            Assert.IsNull (product.Id);
            Assert.IsNull (product.DeletionText);
            Assert.IsNull (product.NotificationType);
            Assert.IsNull (product.RecordReference);
            Assert.IsNull (product.RecordSourceIdentifier);
            Assert.IsNull (product.RecordSourceName);
            Assert.IsNull (product.RecordSourceType);
        }
    }
}
