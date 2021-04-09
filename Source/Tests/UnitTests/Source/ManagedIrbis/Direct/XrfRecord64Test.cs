// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis;
using ManagedIrbis.Direct;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Direct
{
    [TestClass]
    public class XrfRecord64Test
    {
        [TestMethod]
        public void XrfRecord64_Construction_1()
        {
            var record = new XrfRecord64();
            Assert.AreEqual(0L, record.Offset);
            Assert.AreEqual(0, (int)record.Status);
            Assert.IsFalse(record.Deleted);
            Assert.IsFalse(record.Locked);
        }

        [TestMethod]
        public void XrfRecord64_Properties_1()
        {
            var record = new XrfRecord64();
            record.Offset = 23456;
            Assert.AreEqual(23456L, record.Offset);
            record.Status = RecordStatus.LogicallyDeleted;
            Assert.AreEqual(RecordStatus.LogicallyDeleted, record.Status);
            Assert.IsTrue(record.Deleted);
            Assert.IsFalse(record.Locked);
            record.Status = RecordStatus.Locked;
            Assert.IsTrue(record.Locked);
            Assert.IsFalse(record.Deleted);
        }

        [TestMethod]
        public void XrfRecord64_Locked_1()
        {
            var record = new XrfRecord64();
            record.Status = RecordStatus.Last;
            Assert.IsFalse(record.Locked);
            record.Locked = true;
            Assert.IsTrue(record.Locked);
            Assert.AreEqual(RecordStatus.Last|RecordStatus.Locked, record.Status);
            record.Locked = false;
            Assert.IsFalse(record.Locked);
            Assert.AreEqual(RecordStatus.Last, record.Status);
        }

        [TestMethod]
        public void XrfRecord64_ToString_1()
        {
            var record = new XrfRecord64();
            Assert.AreEqual("Offset: 0, Status: None", record.ToString());

            record = new XrfRecord64
            {
                Offset = 2345,
                Status = RecordStatus.Last
            };
            Assert.AreEqual("Offset: 2345, Status: Last", record.ToString());
        }
    }
}
