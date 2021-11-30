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
    public sealed class NullRecordProcessorTest
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
        [Description ("Конструктор по умолчанию")]
        public void NullRecordProcessor_Construction_1()
        {
            using var processor = new NullRecordProcessor();
            Assert.IsNotNull (processor);
        }

        [TestMethod]
        [Description ("Обработка одной записи")]
        public void NullRecordProcessor_ProcessOneRecord_1()
        {
            var records = _GetRecords();
            using var processor = new NullRecordProcessor();
            foreach (var record in records)
            {
                var line = processor.ProcessOneRecord (record);
                Assert.IsNotNull (line);
            }
        }

        [TestMethod]
        [Description ("Обработка нескольких записей")]
        public void NullRecordProcessor_ProcessRecords_1()
        {
            var records = _GetRecords();
            using var source = new ListRecordSource (records);
            using var sink = new NullRecordSink();
            using var processor = new NullRecordProcessor();
            var result = processor.ProcessRecords (source, sink);
            Assert.IsNotNull (result);
        }

        [TestMethod]
        [Description ("Обработка одной записи")]
        public async Task NullRecordProcessor_ProcessOneRecordAsync_1()
        {
            var records = _GetRecords();
            await using var processor = new NullRecordProcessor();
            foreach (var record in records)
            {
                var line = await processor.ProcessOneRecordAsync (record);
                Assert.IsNotNull (line);
            }
        }

        [TestMethod]
        [Description ("Обработка нескольких записей")]
        public async Task NullRecordProcessor_ProcessRecordsAsync_1()
        {
            var records = _GetRecords();
            await using var source = new ListRecordSource (records);
            await using var sink = new NullRecordSink();
            await using var processor = new NullRecordProcessor();
            var result = await processor.ProcessRecordsAsync (source, sink);
            Assert.IsNotNull (result);
        }

    }
}
