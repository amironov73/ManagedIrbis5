using System.IO;
using ManagedIrbis.Direct;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

namespace UnitTests.ManagedIrbis.Direct
{
    [TestClass]
    public class XrfFile64Test
        : Common.CommonUnitTest
    {
        private string _GetFileName()
        {
            return Path.Combine
                (
                    Irbis64RootPath,
                    "Datai/IBIS/ibis.xrf"
                );
        }

        [TestMethod]
        public void XrfFile64_ReadRecord_1()
        {
            var fileName = _GetFileName();
            var mode = DirectAccessMode.ReadOnly;
            using XrfFile64 file = new XrfFile64(fileName, mode);
            var record = file.ReadRecord(1);
            Assert.AreEqual(22951100L, record.Offset);
            Assert.AreEqual(0, (int)record.Status);
        }
    }
}
