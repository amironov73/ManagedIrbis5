// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.IO;

using ManagedIrbis;
using ManagedIrbis.Direct;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Direct
{
    [TestClass]
    public class MappedAccess64Test
        : Common.CommonUnitTest
    {
        private string _GetMasterPath()
        {
            return Path.Combine
            (
                TestDataPath,
                "Irbis64/Datai/IBIS/ibis.mst"
            );
        }

        private MappedAccess64 _GetAccess()
        {
            return new MappedAccess64(_GetMasterPath());
        }

        [TestMethod]
        public void MappedAccess64_Construction_1()
        {
            var fileName = _GetMasterPath();
            var database = Path.GetFileNameWithoutExtension(fileName);
            var access = new MappedAccess64(fileName);
            Assert.AreEqual(database, access.Database);
            Assert.IsNotNull(access.Mst);
            Assert.IsNotNull(access.Xrf);
            Assert.IsNotNull(access.InvertedFile);
            access.Dispose();
        }

        [TestMethod]
        public void MappedAccess64_GetMaxMfn_1()
        {
            using var access = _GetAccess();
            var actual = access.GetMaxMfn();
            Assert.AreEqual(332, actual);
        }

        [TestMethod]
        public void MappedAccess64_ReadRawRecord_1()
        {
            using var access = _GetAccess();
            var record = access.ReadRawRecord(1);

            Assert.IsNotNull(record);
            Assert.IsNotNull(record!.Dictionary);
            Assert.AreEqual(100, record.Dictionary.Count);
            Assert.IsNotNull(record.Leader);
            Assert.AreEqual(0L, record.Offset);
            Assert.IsFalse(record.Deleted);
        }

        [TestMethod]
        public void MappedAccess64_ReadRawRecord_2()
        {
            using var access = _GetAccess();
            var record = access.ReadRawRecord(1001);
            Assert.IsNull(record);
        }

        [TestMethod]
        public void MappedAccess64_ReadRecord_1()
        {
            using var access = _GetAccess();
            var record = access.ReadRecord(1);
            Assert.IsNotNull(record);
            Assert.AreEqual(100, record!.Fields.Count);
            Assert.AreEqual(2, record.Version);
            Assert.IsFalse(record.Deleted);
        }

        [TestMethod]
        public void MappedAccess64_ReadRecord_2()
        {
            using var access = _GetAccess();
            var record = access.ReadRecord(1001);
            Assert.IsNull(record);
        }

        [TestMethod]
        public void MappedAccess64_ReadAllRecordVersions_1()
        {
            using var access = _GetAccess();
            var versions = access.ReadAllRecordVersions(1);
            Assert.IsNotNull(versions);

            // TODO: пока считывается только последняя версия
            Assert.AreEqual(1, versions.Length);
        }

        [TestMethod]
        public void MappedAccess64_ReadLinks_1()
        {
            using var access = _GetAccess();
            var links = access.ReadLinks("K=CASE");
            Assert.IsNotNull(links);
            Assert.AreEqual(2, links.Length);
        }

        [TestMethod]
        public void MappedAccess64_ReadLinks_2()
        {
            using var access = _GetAccess();
            var links = access.ReadLinks("K=CAS0");
            Assert.IsNotNull(links);
            Assert.AreEqual(0, links.Length);
        }

        [TestMethod]
        public void MappedAccess64_ReadTerms_1()
        {
            using var access = _GetAccess();
            var parameters = new TermParameters
            {
                StartTerm = "K=",
                NumberOfTerms = 10
            };
            var terms = access.ReadTerms(parameters);
            Assert.IsNotNull(terms);
            Assert.AreEqual(10, terms.Length);
        }

        [TestMethod]
        public void MappedAccess64_SearchSimple_1()
        {
            using var access = _GetAccess();
            var found = access.SearchSimple("K=CASE");
            Assert.IsNotNull(found);
            Assert.AreEqual(2, found.Length);
        }

        [TestMethod]
        public void MappedAccess64_SearchSimple_2()
        {
            using var access = _GetAccess();
            var found = access.SearchSimple("K=CAS0");
            Assert.IsNotNull(found);
            Assert.AreEqual(0, found.Length);
        }

        [TestMethod]
        public void MappedAccess64_SearchSimple_3()
        {
            using var access = _GetAccess();
            var found = access.SearchSimple("K=C$");
            Assert.IsNotNull(found);
            Assert.AreEqual(19, found.Length);
        }

        [TestMethod]
        public void MappedAccess64_SearchReadSimple_1()
        {
            using var access = _GetAccess();
            var found = access.SearchReadSimple("K=C$");
            Assert.IsNotNull(found);
            Assert.AreEqual(19, found.Length);
        }
    }
}
