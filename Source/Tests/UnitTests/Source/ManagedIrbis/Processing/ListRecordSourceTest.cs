// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Processing;

#nullable enable

namespace UnitTests.ManagedIrbis.Gbl
{
    [TestClass]
    public sealed class ListRecordSourceTest
    {
        private Record[] _GetRecords()
        {
            return new Record[]
            {
                new () { Mfn = 1 },
                new () { Mfn = 2 },
                new () { Mfn = 3 },
            };
        }

        [TestMethod]
        [Description ("Конструктор")]
        public void ListRecordSource_Construction_1()
        {
            var records = _GetRecords();
            using var source = new ListRecordSource (records);
            Assert.AreSame (records, source.RecordList);
            Assert.AreEqual (-1, source.Index);
        }

        [TestMethod]
        [Description ("Перебор записей")]
        public void ListRecordSource_GetNextRecord_1()
        {
            var records = _GetRecords();
            using var source = new ListRecordSource (records);

            var record = source.GetNextRecord();
            Assert.IsNotNull (record);
            Assert.AreEqual (1, record.Mfn);

            record = source.GetNextRecord();
            Assert.IsNotNull (record);
            Assert.AreEqual (2, record.Mfn);

            record = source.GetNextRecord();
            Assert.IsNotNull (record);
            Assert.AreEqual (3, record.Mfn);

            record = source.GetNextRecord();
            Assert.IsNull (record);
        }

        [TestMethod]
        [Description ("Определение количества записей")]
        public void ListRecordSource_GetRecordCount_1()
        {
            var records = _GetRecords();
            using var source = new ListRecordSource (records);
            Assert.AreEqual (records.Length, source.GetRecordCount());
        }

        [TestMethod]
        [Description ("Асинхронный перебор записей")]
        public async Task ListRecordSource_GetNextRecordAsync_1()
        {
            var records = _GetRecords();
            await using var source = new ListRecordSource (records);

            var record = await source.GetNextRecordAsync();
            Assert.IsNotNull (record);
            Assert.AreEqual (1, record.Mfn);

            record = await source.GetNextRecordAsync();
            Assert.IsNotNull (record);
            Assert.AreEqual (2, record.Mfn);

            record = await source.GetNextRecordAsync();
            Assert.IsNotNull (record);
            Assert.AreEqual (3, record.Mfn);

            record = await source.GetNextRecordAsync();
            Assert.IsNull (record);
        }

        [TestMethod]
        [Description ("Асинхронное определение количества записей")]
        public async Task ListRecordSource_GetRecordCountAsync_1()
        {
            var records = _GetRecords();
            await using var source = new ListRecordSource (records);
            Assert.AreEqual (records.Length, await source.GetRecordCountAsync());
        }
    }
}
