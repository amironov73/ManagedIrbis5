// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class PriceTypeCodeTest
    {
        [TestMethod]
        [Description ("Получение массива значений констант")]
        public void PriceTypeCode_ListValues_1()
        {
            var values = PriceTypeCode.ListValues();
            Assert.AreEqual (4, values.Length);
        }
    }
}
