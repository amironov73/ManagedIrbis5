// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.IO;

using AM.Text;

using ManagedIrbis.Direct;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Direct
{
    [TestClass]
    public class MstControlRecord64Test
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void MstControlRecord64_Construction_1()
        {
            var control = new MstControlRecord64();
            Assert.AreEqual(0, control.Reserv1);
            Assert.AreEqual(0, control.NextMfn);
            Assert.AreEqual(0L, control.NextPosition);
            Assert.AreEqual(0, control.Version);
            Assert.AreEqual(0, control.Reserv3);
            Assert.AreEqual(0, control.Reserv4);
            Assert.AreEqual(0, control.Blocked);
        }

        [TestMethod]
        public void MstControlRecord64_Read_1()
        {
            byte[] bytes = { 0, 0, 0, 0, 0, 0, 0, 111, 0, 188, 97,
                78, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 1 };
            var stream = new MemoryStream(bytes);
            var control = MstControlRecord64.Read(stream);
            Assert.AreEqual(0, control.Reserv1);
            Assert.AreEqual(111, control.NextMfn);
            Assert.AreEqual(12345678L, control.NextPosition);
            Assert.AreEqual(0, control.Version);
            Assert.AreEqual(0, control.Reserv3);
            Assert.AreEqual(0, control.Reserv4);
            Assert.AreEqual(1, control.Blocked);
        }

        [TestMethod]
        public void MstControlRecord64_Read_2()
        {
            var fileName = Path.Combine(TestDataPath, "KZD.MST");
            using (var stream = File.OpenRead(fileName))
            {
                var control = MstControlRecord64.Read(stream);
                Assert.AreEqual(0, control.Reserv1);
                Assert.AreEqual(32, control.NextMfn);
                Assert.AreEqual(37308L, control.NextPosition);
                Assert.AreEqual(0, control.Version);
                Assert.AreEqual(0, control.Reserv3);
                Assert.AreEqual(0, control.Reserv1);
                Assert.AreEqual(0, control.Blocked);
            }
        }

        [TestMethod]
        public void MstControlRecord64_Read_3()
        {
            var fileName = Path.Combine(Irbis64RootPath, "Datai/IBIS/ibis.mst");
            using (var stream = File.OpenRead(fileName))
            {
                var control = MstControlRecord64.Read(stream);
                Assert.AreEqual(0, control.Version);
                Assert.AreEqual(333, control.NextMfn);
                Assert.AreEqual(45843589L, control.NextPosition);
                Assert.AreEqual(0, control.Version);
                Assert.AreEqual(0, control.Reserv3);
                Assert.AreEqual(0, control.Reserv1);
                Assert.AreEqual(0, control.Blocked);
            }
        }

        [TestMethod]
        public void MstControlRecord64_Write_1()
        {
            var control = new MstControlRecord64
            {
                NextMfn = 111,
                NextPosition = 12345678L,
                Blocked = 1
            };
            var stream = new MemoryStream();
            control.Write(stream);
            byte[] expected = { 0, 0, 0, 0, 0, 0, 0, 111, 0, 188, 97,
                78, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 1 };
            var actual = stream.ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MstControlRecord64_Dump_1()
        {
            var fileName = Path.Combine(TestDataPath, "empty.mst");
            var writer = new StringWriter();
            using (var stream = File.OpenRead(fileName))
            {
                var control = MstControlRecord64.Read(stream);
                control.Dump(writer);
            }

            var expected = "CTLMFN: 0\nNXTMFN: 1\nNXTPOS: 36\nMFTYPE: 0\nRECCNT: 0\nLOCKED: 0\n";
            var actual = writer.ToString().DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MstControlRecord64_Dump_2()
        {
            var fileName = Path.Combine(TestDataPath, "KZD.MST");
            var writer = new StringWriter();
            using (var stream = File.OpenRead(fileName))
            {
                var control = MstControlRecord64.Read(stream);
                control.Dump(writer);
            }

            var expected = "CTLMFN: 0\nNXTMFN: 32\nNXTPOS: 37308\nMFTYPE: 0\nRECCNT: 0\nLOCKED: 0\n";
            var actual = writer.ToString().DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MstControlRecord64_Dump_3()
        {
            var fileName = Path.Combine(Irbis64RootPath, "Datai/IBIS/ibis.mst");
            var writer = new StringWriter();
            using (var stream = File.OpenRead(fileName))
            {
                var control = MstControlRecord64.Read(stream);
                control.Dump(writer);
            }

            var expected = "CTLMFN: 0\nNXTMFN: 333\nNXTPOS: 45843589\nMFTYPE: 0\nRECCNT: 0\nLOCKED: 0\n";
            var actual = writer.ToString().DosToUnix();
            Assert.AreEqual(expected, actual);
        }

    }
}
