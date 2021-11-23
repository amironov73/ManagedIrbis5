// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using ManagedIrbis.Batch;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Batch
{
    public class ParallelRecordReaderTest
    {
        [TestMethod]
        [Description ("Конструктор")]
        public void ParallelRecordReader_Construction_1()
        {
            const string connectionString = "none";
            var reader = new ParallelRecordReader
                (
                    -1,
                    connectionString
                );
            Assert.AreEqual (connectionString, reader.ConnectionString);
            Assert.IsTrue (reader.Parallelism > 0);
            Assert.IsTrue (reader.IsStop);
        }

        [TestMethod]
        [Description ("Конструктор")]
        public void ParallelRecordReader_Construction_2()
        {
            const string connectionString = "none";
            var reader = new ParallelRecordReader
                (
                    -1,
                    connectionString,
                    Array.Empty<int>()
                );
            Assert.AreEqual (connectionString, reader.ConnectionString);
            Assert.IsTrue (reader.Parallelism > 0);
            Assert.IsTrue (reader.IsStop);
        }

        [TestMethod]
        [Description ("Чтение всех записей")]
        public void ParallelRecordReader_ReadAll_1()
        {
            const string connectionString = "none";
            var reader = new ParallelRecordReader
                (
                    -1,
                    connectionString,
                    Array.Empty<int>()
                );
            var records = reader.ReadAll();
            Assert.IsNotNull (records);
            Assert.AreEqual (0, records.Length);
        }

    }
}
