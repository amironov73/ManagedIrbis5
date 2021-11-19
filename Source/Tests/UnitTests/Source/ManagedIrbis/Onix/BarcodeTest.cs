// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class BarcodeTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Barcode_Construction_1()
        {
            var barcode = new Barcode();
            Assert.IsNull (barcode.Position);
            Assert.IsNull (barcode.Type);
        }
    }
}
