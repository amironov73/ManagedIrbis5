// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using System.IO;

using AM;

using ManagedIrbis;
using ManagedIrbis.Direct;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Direct
{
    [TestClass]
    public class MstFile64Test
        : Common.CommonUnitTest
    {
        private MstRecord64 _GetRecord()
        {
            var record = new Record();

            var field = new Field(700)
            {
                {'a', "Иванов"},
                {'b', "И. И."}
            };
            record.Fields.Add(field);

            field = new Field(701)
            {
                {'a', "Петров"},
                {'b', "П. П."}
            };
            record.Fields.Add(field);

            field = new Field(200)
            {
                {'a', "Заглавие"},
                {'e', "подзаголовочное"},
                {'f', "И. И. Иванов, П. П. Петров"}
            };
            record.Fields.Add(field);

            field = new Field(300, "Первое примечание");
            record.Fields.Add(field);
            field = new Field(300, "Второе примечание");
            record.Fields.Add(field);
            field = new Field(300, "Третье примечание");
            record.Fields.Add(field);

            var result = MstRecord64.EncodeRecord(record);

            return result;
        }

        private string _GetFileName()
        {
            return Path.Combine
                (
                    Irbis64RootPath,
                    "Datai/IBIS/ibis.mst"
                );
        }

        private string _CreateDatabase()
        {
            var random = new Random();
            var directory = Path.Combine
                (
                    Path.GetTempPath(),
                    random.Next().ToInvariantString()
                );
            Directory.CreateDirectory(directory);
            var path = Path.Combine(directory, "database");
            DirectUtility.CreateDatabase64(path);
            var result = path + ".mst";

            return result;
        }

        [TestMethod]
        public void MstFile64_ReadRecord_1()
        {
            var fileName = _GetFileName();
            using (var file = new MstFile64(fileName, DirectAccessMode.ReadOnly))
            {
                Assert.AreSame(fileName, file.FileName);
                Assert.AreEqual(333, file.ControlRecord.NextMfn);
                var record = file.ReadRecord(22951100L);
                Assert.AreEqual(100, record.Dictionary.Count);
                var expected = "Tag: 200, Position: 2652, Length: 173, Text: ^AКуда пойти учиться?^EИнформ. - реклам. справ^FЗ. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]";
                Assert.AreEqual(expected, record.Dictionary[87].ToString());
            }
        }

        [Ignore]
        [TestMethod]
        public void MstFile64_LockDatabase_1()
        {
            var fileName = _CreateDatabase();
            using (var file = new MstFile64(fileName, DirectAccessMode.Exclusive))
            {
                file.LockDatabase(true);
                Assert.IsTrue(file.ReadDatabaseLockedFlag());
                file.LockDatabase(false);
                Assert.IsFalse(file.ReadDatabaseLockedFlag());
            }
        }

        [Ignore]
        [TestMethod]
        public void MstFile64_ReopenFile_1()
        {
            var fileName = _CreateDatabase();
            using (var file = new MstFile64(fileName, DirectAccessMode.ReadOnly))
            {
                Assert.AreEqual(DirectAccessMode.ReadOnly, file.Mode);
                file.ReopenFile(DirectAccessMode.Exclusive);
                Assert.AreEqual(DirectAccessMode.Exclusive, file.Mode);
            }
        }

        [Ignore]
        [TestMethod]
        public void MstFile64_WriteRecord_1()
        {
            var fileName = _CreateDatabase();
            using (var file = new MstFile64(fileName, DirectAccessMode.Exclusive))
            {
                var record = _GetRecord();
                var leader1 = record.Leader;
                leader1.Mfn = 1;
                leader1.Version = 1;
                record.Leader = leader1;
                record.Prepare();
                var offset = file.WriteRecord(record);
                file.UpdateLeader(leader1, offset);
                Assert.AreNotEqual(0L, offset);
                file.UpdateControlRecord(true);
                var leader2 = file.ReadLeader(offset);
                Assert.AreEqual(leader1.Mfn, leader2.Mfn);
                Assert.AreEqual(leader1.Length, leader2.Length);
            }
        }
    }
}
