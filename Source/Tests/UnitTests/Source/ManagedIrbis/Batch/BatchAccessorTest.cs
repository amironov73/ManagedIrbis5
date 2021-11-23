// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using ManagedIrbis.Batch;
using ManagedIrbis.Providers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Batch
{
    [TestClass]
    public sealed class BatchAccessorTest
    {
        [TestMethod]
        [Description ("Конструктор")]
        public void BatchAccessor_Construction_1()
        {
            var connection = new NullProvider();
            var accessor = new BatchAccessor (connection);
            Assert.AreSame (connection, accessor.Connection);
        }

        [TestMethod]
        [Description ("Чтение множества записей в параллель")]
        public void BatchAccessor_ReadRecords_1()
        {
            var connection = new NullProvider();
            var accessor = new BatchAccessor (connection);
            var records = accessor.ReadRecords
                (
                    "IBIS",
                    Array.Empty<int>()
                );
            Assert.IsNotNull (records);
            Assert.AreEqual (0, records.Length);
        }

        [TestMethod]
        [Description ("Чтение множества записей в параллель")]
        public void BatchAccessor_ReadRecords_2()
        {
            var connection = new NullProvider();
            var accessor = new BatchAccessor (connection);
            var records = accessor.ReadRecords
                (
                    "IBIS",
                    Array.Empty<int>(),
                    record => record.Mfn
                );
            Assert.IsNotNull (records);
            Assert.AreEqual (0, records.Length);
        }
    }
}
