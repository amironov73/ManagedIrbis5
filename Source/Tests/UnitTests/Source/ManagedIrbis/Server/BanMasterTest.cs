// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.IO;

using ManagedIrbis.Server;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Server
{
    [TestClass]
    public sealed class BanMasterTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void BanMaster_Construction_1()
        {
            var master = new BanMaster();
            Assert.AreEqual  (0, master.Count);
        }

        [TestMethod]
        [Description ("Забанивание")]
        public void BanMaster_BanTheAddress_1()
        {
            const string loopback = "127.0.0.1";
            var master = new BanMaster();
            master.BanTheAddress (loopback);
            Assert.AreEqual (1, master.Count);
            Assert.IsTrue (master.IsAddressBanned (loopback));
            Assert.IsFalse (master.IsAddressBanned ("172.0.0.1"));
        }

        [TestMethod]
        [Description ("Проверка, забанен ли адрес")]
        public void BanMaster_IsAddressBanned_1()
        {
            const string loopback = "127.0.0.1";
            var master = new BanMaster();
            Assert.IsFalse (master.IsAddressBanned (loopback));
            master.BanTheAddress (loopback);
            Assert.AreEqual (1, master.Count);
            Assert.IsTrue (master.IsAddressBanned (loopback));
            Assert.IsFalse (master.IsAddressBanned ("172.0.0.1"));
        }

        [TestMethod]
        [Description ("Загрузка из файла")]
        public void BanMaster_LoadFile_1()
        {
            var fileName = Path.Combine (TestDataPath, "banlist.txt");
            var master = new BanMaster();
            master.LoadFile (fileName);
            Assert.AreEqual (3, master.Count);
            Assert.IsTrue (master.IsAddressBanned ("1.1.1.1"));
            Assert.IsTrue (master.IsAddressBanned ("2.2.2.2"));
            Assert.IsTrue (master.IsAddressBanned ("3.3.3.3"));
            Assert.IsFalse (master.IsAddressBanned ("4.4.4.4"));
        }

        [TestMethod]
        [Description ("Сохранение бан-листа в файл")]
        public void BanMaster_SaveToFile_1()
        {
            var fileName = Path.GetTempFileName();
            var master = new BanMaster();
            master.BanTheAddress ("1.1.1.1");
            master.BanTheAddress ("2.2.2.2");
            master.BanTheAddress ("3.3.3.3");
            master.SaveToFile (fileName);
            Assert.IsTrue (File.Exists (fileName));
        }

        [TestMethod]
        [Description ("Разбанивание")]
        public void BanMaster_UnbanTheAddress_1()
        {
            const string loopback = "127.0.0.1";
            var master = new BanMaster();
            master.BanTheAddress (loopback);
            master.UnbanTheAddress (loopback);
            Assert.AreEqual (0, master.Count);
        }

        [TestMethod]
        [Description ("Разбанивание")]
        public void BanMaster_UnbanAll_1()
        {
            const string loopback = "127.0.0.1";
            var master = new BanMaster();
            master.BanTheAddress (loopback);
            master.UnbanAll ();
            Assert.IsFalse (master.IsAddressBanned (loopback));
            Assert.AreEqual (0, master.Count);
        }

    }
}
