// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Batch
{
    [TestClass]
    public sealed class BatchFactoryTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void BatchFactory_Construction_1()
        {
            var factory = new BatchFactory();
            Assert.IsNotNull (factory);
        }

        [TestMethod]
        [Description ("Чтение множества записей в параллель")]
        [ExpectedException (typeof (NotImplementedException))]
        public void BatchFactory_GetBatchReader_1()
        {
            var factory = new BatchFactory();
            var reader = factory.GetBatchReader
                (
                    BatchFactory.Simple,
                    "none",
                    Constants.Ibis,
                    Array.Empty<int>()
                );
            Assert.IsNotNull (reader);
        }

        [TestMethod]
        [Description ("Чтение множества записей в параллель")]
        [ExpectedException (typeof (ArgumentException))]
        public void BatchFactory_GetBatchFormatter_1()
        {
            var factory = new BatchFactory();
            var formatter = factory.GetFormatter
                (
                    BatchFactory.Simple,
                    "none",
                    Constants.Ibis,
                    IrbisFormat.Brief,
                    Array.Empty<int>()
                );
            Assert.IsNotNull (formatter);
        }
    }
}
