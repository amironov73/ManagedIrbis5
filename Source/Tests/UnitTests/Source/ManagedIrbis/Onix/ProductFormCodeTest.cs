// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class ProductFormCodeTest
    {
        [TestMethod]
        [Description ("Получение массива значений констант")]
        public void ProductFormCode_ListValues_1()
        {
            var values = ProductFormCode.ListValues();
            Assert.AreEqual (80, values.Length);
        }
    }
}
