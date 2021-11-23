// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using System.Linq;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Providers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Batch
{
    [TestClass]
    public sealed class BatchRecordReaderTest
    {
        [TestMethod]
        [Description ("Конструктор")]
        public void BatchRecordReader_Construction_1()
        {
            const int batchSize = 5000;
            var connection = new NullProvider();
            var reader = new BatchRecordReader
                (
                    connection,
                    Array.Empty<int>(),
                    batchSize,
                    Constants.Ibis,
                    true
                );
            Assert.AreSame (connection, reader.Connection);
            Assert.AreEqual (batchSize, reader.BatchSize);
            Assert.AreEqual (Constants.Ibis, reader.Database);
            Assert.IsTrue (reader.OmitDeletedRecords);
            Assert.AreEqual (0, reader.RecordsRead);
            Assert.AreEqual (0, reader.TotalRecords);
        }

        [TestMethod]
        [Description ("Чтение всех записей")]
        public void BatchRecordReader_ReadAll_1()
        {
            const int batchSize = 5000;
            var connection = new NullProvider();
            var reader = new BatchRecordReader
                (
                    connection,
                    Array.Empty<int>(),
                    database: Constants.Ibis
                );
            var records = reader.ReadAll();
            Assert.IsNotNull (records);
            Assert.AreEqual (0, records.Count);
        }

        [TestMethod]
        [Description ("Чтение интервала записей")]
        public void BatchRecordReader_Interval_1()
        {
            var connection = new NullProvider();
            var records = BatchRecordReader.Interval
                (
                    connection,
                    database: Constants.Ibis
                )
                .ToArray();
            Assert.IsNotNull (records);
            Assert.AreEqual (0, records.Length);
        }

        [TestMethod]
        [Description ("Чтение найденных записей")]
        public void BatchRecordReader_Search_1()
        {
            var connection = new NullProvider();
            var records = BatchRecordReader.Search
                (
                    connection,
                    "no such record",
                    database: Constants.Ibis
                )
                .ToArray();
            Assert.IsNotNull (records);
            Assert.AreEqual (0, records.Length);
        }

        [TestMethod]
        [Description ("Чтение всех записей")]
        [ExpectedException (typeof (ArgumentOutOfRangeException))]
        public void BatchRecordReader_WholeDatabase_1()
        {
            var connection = new NullProvider();
            var records = BatchRecordReader.WholeDatabase
                (
                    connection,
                    database: Constants.Ibis
                )
                .ToArray();
            Assert.IsNotNull (records);
            Assert.AreEqual (0, records.Length);
        }

    }
}
