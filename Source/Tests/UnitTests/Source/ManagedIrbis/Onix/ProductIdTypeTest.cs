// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class ProductIdTypeTest
    {
        [TestMethod]
        [Description ("Получение массива значений констант")]
        public void ProductIdType_ListValues_1()
        {
            var values = ProductIdType.ListValues();
            Assert.AreEqual (11, values.Length);
        }
    }
}
