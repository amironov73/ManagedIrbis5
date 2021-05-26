﻿// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis;
using ManagedIrbis.Readers;

#nullable enable

namespace UnitTests.ManagedIrbis.Readers
{
    [TestClass]
    public class ArchiveReaderInfoTest
    {
        [TestMethod]
        public void ArchiveReaderInfo_Construction_1()
        {
            var reader = new ArchiveReaderInfo();
            Assert.AreEqual(0, reader.Id);
            Assert.AreEqual(0, reader.Mfn);
            Assert.IsNull(reader.Ticket);
            Assert.IsNull(reader.Visits);
            Assert.IsNull(reader.UserData);
            Assert.AreEqual(DateTime.MinValue, reader.FirstVisitDate);
            Assert.AreEqual(DateTime.MinValue, reader.LastVisitDate);
            Assert.IsNull(reader.LastVisitPlace);
            Assert.IsNull(reader.LastVisitResponsible);
            Assert.IsFalse(reader.Marked);
        }

        private void _TestSerialization
            (
                ArchiveReaderInfo first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<ArchiveReaderInfo>();

            Assert.IsNotNull(second);
            Assert.AreEqual(first.Id, second!.Id);
            Assert.AreEqual(first.Mfn, second.Mfn);
            Assert.AreEqual(first.Ticket, second.Ticket);
            Assert.IsNull(second.UserData);
            Assert.IsFalse(second.Marked);
            if (ReferenceEquals(first.Visits, null))
            {
                Assert.IsNull(second.Visits);
            }
            else
            {
                Assert.IsNotNull(second.Visits);
                Assert.AreEqual(first.Visits.Length, second.Visits!.Length);
            }
        }

        [TestMethod]
        public void ArchiveReaderInfo_Serialization_1()
        {
            var reader = new ArchiveReaderInfo();
            _TestSerialization(reader);

            reader.Id = 1;
            reader.Mfn = 123;
            reader.Ticket = "Q123W456";
            reader.Visits = new[]
            {
                new VisitInfo{Barcode = "123", Database = "IBIS"},
                new VisitInfo{Barcode = "321", Database = "IBIS"}
            };
            reader.UserData = "USER_DATA";
            reader.Marked = true;
            _TestSerialization(reader);
        }

        [TestMethod]
        public void ArchiveReaderInfo_Parse_1()
        {
            var mfn = 123;
            var ticket = "TICKET";
            var record = new Record { Mfn = mfn };
            record.Add(30, ticket);
            record.Add(40, "^A84(2=Рус)5/Ц 27-422726^B1539325^CЦветаева М. И. Стихотворения^KФ302^V*^D20170930^1172914^E20171014^F20171028^GIBIS^HE00401004C75DB57^Isamo");
            var reader = ArchiveReaderInfo.Parse(record);
            Assert.IsNotNull(reader);
            Assert.AreEqual(0, reader.Id);
            Assert.AreEqual(mfn, reader.Mfn);
            Assert.AreEqual(ticket, reader.Ticket);
            Assert.IsFalse(reader.Marked);
            Assert.IsNull(reader.UserData);
            Assert.IsNotNull(reader.Visits);
            Assert.AreEqual(1, reader.Visits!.Length);
            Assert.AreEqual("84(2=Рус)5/Ц 27-422726", reader.Visits[0].Index);
        }

        private ArchiveReaderInfo _GetReaderInfo()
        {
            var result = new ArchiveReaderInfo
            {
                Id = 1,
                Mfn = 123,
                Ticket = "Q123W456",
                Visits = new []
                {
                    new VisitInfo
                    {
                        Index = "84(2=Рус)5/Ц 27-422726",
                        Description = "Цветаева М. И. Стихотворения",
                        Sigla = "Ф302",
                        Barcode = "E00401004C75DB57",
                        Database = "IBIS",
                        Department = "*",
                        Responsible = "samo",
                        DateGivenString = "20170930",
                        DateExpectedString = "20171014",
                        DateReturnedString = "20171028"
                    }
                }
            };

            return result;
        }

        [TestMethod]
        public void ArchiveReaderInfo_ToRecord_1()
        {
            var reader = _GetReaderInfo();
            var record = reader.ToRecord();
            Assert.IsNotNull(reader);
            Assert.AreEqual(reader.Mfn, record.Mfn);
            Assert.AreEqual(reader.Ticket, record.FM(30));
            Assert.IsNotNull(reader.Visits);
            Assert.AreEqual(reader.Visits!.Length, record.Fields.GetFieldCount(40));
        }

        [TestMethod]
        public void ArchiveReaderInfo_SaveToFile_1()
        {
            var fileName = Path.GetTempFileName();
            ArchiveReaderInfo[] first = { _GetReaderInfo() };
            ArchiveReaderInfo.SaveToFile(fileName, first);
            var second = ArchiveReaderInfo.ReadFromFile(fileName);
            Assert.IsNotNull(second);
            Assert.AreEqual(first.Length, second.Length);
            Assert.AreEqual(first[0].Id, second[0].Id);
            Assert.AreEqual(first[0].Mfn, second[0].Mfn);
            Assert.AreEqual(first[0].Ticket, second[0].Ticket);
            Assert.IsNotNull(first[0].Visits);
            Assert.IsNotNull(second[0].Visits);
            Assert.AreEqual(first[0].Visits!.Length, second[0].Visits!.Length);
        }

        [TestMethod]
        public void ArchiveReaderInfo_ToString_1()
        {
            var reader = new ArchiveReaderInfo();
            Assert.AreEqual("(null)", reader.ToString());

            reader = _GetReaderInfo();
            Assert.AreEqual(reader.Ticket, reader.ToString());
        }

        [TestMethod]
        public void ArchiveReaderInfo_FirstVisitDate_1()
        {
            var reader = _GetReaderInfo();
            Assert.AreEqual(new DateTime(2017, 9, 30), reader.FirstVisitDate);
        }

        [TestMethod]
        public void ArchiveReaderInfo_LastVisitDate_1()
        {
            var reader = _GetReaderInfo();
            Assert.AreEqual(new DateTime(2017, 9, 30), reader.LastVisitDate);
        }

        [TestMethod]
        public void ArchiveReaderInfo_LastVisitPlace_1()
        {
            var reader = _GetReaderInfo();
            Assert.AreEqual("*", reader.LastVisitPlace);
        }

        [TestMethod]
        public void ArchiveReaderInfo_LastVisitResponsible_1()
        {
            var reader = _GetReaderInfo();
            Assert.AreEqual("samo", reader.LastVisitResponsible);
        }
    }
}
