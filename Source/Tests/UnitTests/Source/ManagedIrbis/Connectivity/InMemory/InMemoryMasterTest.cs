// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.IO;

using ManagedIrbis.InMemory;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Connectivity.InMemory
{
    [TestClass]
    public sealed class InMemoryMasterTest
    {
        [TestMethod]
        [Description ("Конструктор")]
        public void InMemoryMaster_Construction_1()
        {
            var master = new InMemoryMaster();
            Assert.AreEqual (0, master.Count);
        }

        [TestMethod]
        [Description ("Дамп файла документов")]
        public void InMemoryMaster_Dump_1()
        {
            var master = new InMemoryMaster();
            var output = new StringWriter();
            master.Dump (output);
            var dump = output.ToString();
            Assert.IsNotNull (dump);
        }

        [TestMethod]
        [Description ("Загрузка из потока")]
        public void InMemoryMaster_Read_1()
        {
            var master = new InMemoryMaster();
            using var reader = new BinaryReader (Stream.Null);
            master.Read (reader);
        }

        [TestMethod]
        [Description ("Сохранение в поток")]
        public void InMemoryInverted_Write_1()
        {
            var master = new InMemoryMaster();
            using var memory = new MemoryStream();
            using var writer = new BinaryWriter(memory);
            master.Save (writer);
        }

    }
}
