// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Processing;

#nullable enable

namespace UnitTests.ManagedIrbis.Gbl
{
    [TestClass]
    public sealed class NullRecordSourceTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void NullRecordSource_Construction_1()
        {
            using var source = new NullRecordSource();
            Assert.IsNotNull (source);
        }

        [TestMethod]
        [Description ("Перебор записей")]
        public void NullRecordSource_GetNextRecord_1()
        {
            using var source = new NullRecordSource();
            Assert.IsNull (source.GetNextRecord());
        }

        [TestMethod]
        [Description ("Определение количества записей")]
        public void NullRecordSource_GetRecordCount_1()
        {
            using var source = new NullRecordSource();
            Assert.AreEqual (0, source.GetRecordCount());
        }

        [TestMethod]
        [Description ("Асинхронный перебор записей")]
        public async Task NullRecordSource_GetNextRecordAsync_1()
        {
            await using var source = new NullRecordSource();
            Assert.IsNull (await source.GetNextRecordAsync());
        }

        [TestMethod]
        [Description ("Асинхронное определение количества записей")]
        public async Task NullRecordSource_GetRecordCountAsync_1()
        {
            await using var source = new NullRecordSource();
            Assert.AreEqual (0, await source.GetRecordCountAsync());
        }
    }
}
