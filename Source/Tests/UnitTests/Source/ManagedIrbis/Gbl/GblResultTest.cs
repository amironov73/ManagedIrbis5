// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Gbl;

#nullable enable

namespace UnitTests.ManagedIrbis.Gbl
{
    [TestClass]
    public class GblResultTest
    {
        [TestMethod]
        public void GblResult_Construction_1()
        {
            var result = new GblResult();
            Assert.AreEqual(false, result.Canceled);
            Assert.AreEqual(null, result.Exception);
            Assert.AreEqual(0, result.RecordsSupposed);
            Assert.AreEqual(0, result.RecordsProcessed);
            Assert.AreEqual(0, result.RecordsSucceeded);
            Assert.AreEqual(0, result.RecordsFailed);
            Assert.AreEqual(null, result.Protocol);
        }

        [TestMethod]
        public void GblResult_GetEmptyResult_1()
        {
            var result = GblResult.GetEmptyResult();
            Assert.AreEqual(false, result.Canceled);
            Assert.AreEqual(null, result.Exception);
            Assert.AreEqual(0, result.RecordsSupposed);
            Assert.AreEqual(0, result.RecordsProcessed);
            Assert.AreEqual(0, result.RecordsSucceeded);
            Assert.AreEqual(0, result.RecordsFailed);
            Assert.AreEqual(null, result.Protocol);
        }

        [TestMethod]
        public void GblResult_ToString_1()
        {
            var result = new GblResult();
            Assert.AreEqual
                (
                    "Records processed: 0, Canceled: False",
                    result.ToString()
                );
        }
    }
}
