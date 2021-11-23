// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using ManagedIrbis.Batch;
using ManagedIrbis.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Batch
{
    [TestClass]
    public sealed class ParallelRecordFormatterTest
    {
        [TestMethod]
        [Description ("Конструктор")]
        public void ParallelRecordFormatter_Construction_1()
        {
            const string connectionString = "none";
            var formatter = new ParallelRecordFormatter
                (
                    -1,
                    connectionString,
                    Array.Empty<int>(),
                    IrbisFormat.Brief
                );
            Assert.AreEqual (connectionString, formatter.ConnectionString);
            Assert.IsTrue (formatter.Parallelism > 0);
            Assert.AreEqual (IrbisFormat.Brief, formatter.Format);
            // Assert.IsFalse (formatter.IsStop);
        }

    }
}
