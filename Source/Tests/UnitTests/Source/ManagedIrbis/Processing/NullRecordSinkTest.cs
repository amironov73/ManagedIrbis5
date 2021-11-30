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
    public sealed class NullRecordSinkTest
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
        public void NullRecordSink_Construction_1()
        {
            using var sink = new NullRecordSink();
            Assert.IsNotNull (sink);
        }

        [TestMethod]
        [Description ("Прием одной записи")]
        public void NullRecordSink_PostRecord_1()
        {
            var records = _GetRecords();
            using var sink = new NullRecordSink();
            foreach (var record in records)
            {
                sink.PostRecord (record, "Some message");
            }

            sink.Complete();
        }

        [TestMethod]
        [Description ("Завершение приема записей")]
        public void NullRecordSink_Complete_1()
        {
            using var sink = new NullRecordSink();
            sink.Complete();
        }

        [TestMethod]
        [Description ("Получение протокола")]
        public void NullRecordSink_GetProtocol_1()
        {
            using var sink = new NullRecordSink();
            var protocol = sink.GetProtocol();
            Assert.IsNotNull (protocol);
        }

        [TestMethod]
        [Description ("Получение протокола")]
        public void NullRecordSink_GetProtocol_2()
        {
            using var sink = new NullRecordSink();
            var protocol = ((ISyncRecordSink) sink).GetProtocol();
            Assert.IsNotNull (protocol);
        }

        [TestMethod]
        [Description ("Асинхронный прием одной записи")]
        public async Task NullRecordSink_PostRecordAsync_1()
        {
            var records = _GetRecords();
            await using var sink = new NullRecordSink();
            foreach (var record in records)
            {
                await sink.PostRecordAsync (record, "Some message");
            }

            await sink.CompleteAsync();
        }

        [TestMethod]
        [Description ("Асинхронное завершение приема записей")]
        public async Task NullRecordSink_CompleteAsync_1()
        {
            await using var sink = new NullRecordSink();
            await sink.CompleteAsync();
        }

        [TestMethod]
        [Description ("Асинхронное получение протокола")]
        public async Task NullRecordSink_GetProtocolAsync_1()
        {
            await using var sink = new NullRecordSink();
            var protocol = await sink.GetProtocolAsync();
            Assert.IsNotNull (protocol);
        }
    }
}
