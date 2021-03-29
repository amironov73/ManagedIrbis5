// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis.Fields;

#nullable enable

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public class ExemplarInfoTest
    {
        private void _TestSerialization
            (
                ExemplarInfo first
            )
        {
            var bytes = first.SaveToMemory();

            var second = bytes
                .RestoreObjectFromMemory<ExemplarInfo>();
            Assert.AreEqual(first.Number, second.Number);
            Assert.AreEqual(first.Barcode, second.Barcode);
        }

        [TestMethod]
        public void TestExemplarInfoSerialization()
        {
            var exemplarInfo = new ExemplarInfo();
            _TestSerialization(exemplarInfo);

            exemplarInfo.Number = "1";
            exemplarInfo.Barcode = "2";
            _TestSerialization(exemplarInfo);
        }
    }
}
