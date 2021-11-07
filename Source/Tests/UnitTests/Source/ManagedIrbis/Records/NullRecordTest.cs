// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo
// ReSharper disable UseObjectOrCollectionInitializer

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Direct;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Records;

#nullable enable

namespace UnitTests.Source.ManagedIrbis.Records
{
    [TestClass]
    public sealed class NullRecordTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void NullRecord_Constructor_1()
        {
            var record = new NullRecord();
            Assert.IsNull (record.Database);
            Assert.AreEqual (0, record.Mfn);
            Assert.AreEqual (0, record.Version);
            Assert.AreEqual (RecordStatus.None, record.Status);
        }

        [TestMethod]
        [Description ("Разбор ответа сервера ИРБИС64")]
        public void NullRecord_Decode_1()
        {
            using var connection = new SyncConnection();
            using var response = new Response (connection);
            var record = new NullRecord();
            record.Decode (response);
            Assert.IsNull (record.Database);
            Assert.AreEqual (0, record.Mfn);
            Assert.AreEqual (0, record.Version);
            Assert.AreEqual (RecordStatus.None, record.Status);
        }

        [TestMethod]
        [Description ("Разбор записи, прочитанной из мастер-файла")]
        public void NullRecord_Decode_2()
        {
            var mstRecord = new MstRecord64();
            var record = new NullRecord();
            record.Decode (mstRecord);
            Assert.IsNull (record.Database);
            Assert.AreEqual (0, record.Mfn);
            Assert.AreEqual (0, record.Version);
            Assert.AreEqual (RecordStatus.None, record.Status);
        }

        [TestMethod]
        [Description ("Получение текстового представления записи")]
        public void NullRecord_Encode_1()
        {
            var record = new NullRecord();
            Assert.AreEqual (string.Empty, record.Encode ());
        }

        [TestMethod]
        [Description ("Кодирование записи для сохранения в мастер-файл")]
        public void NullRecord_Encode_2()
        {
            var mstRecord = new MstRecord64();
            var record = new NullRecord();
            record.Encode (mstRecord);
            Assert.AreEqual (0L, mstRecord.Offset);
        }

        [TestMethod]
        [Description ("Доступ к полям записи")]
        public void NullRecord_FM_1()
        {
            var record = new NullRecord();
            Assert.IsNull (record.FM (100));
            Assert.IsNull (record.FM (1000));
        }

        [TestMethod]
        [Description ("Свойство Database")]
        public void NullRecord_Database_1()
        {
            const string database = "IBIS";
            var record = new NullRecord();
            record.Database = database;
            Assert.AreEqual (database, record.Database);
        }

        [TestMethod]
        [Description ("Свойство Mfn")]
        public void NullRecord_Mfn_1()
        {
            const int mfn = 123;
            var record = new NullRecord();
            record.Mfn = mfn;
            Assert.AreEqual (mfn, record.Mfn);
        }

        [TestMethod]
        [Description ("Свойство Version")]
        public void NullRecord_Version_1()
        {
            const int version = 123;
            var record = new NullRecord();
            record.Version = version;
            Assert.AreEqual (version, record.Version);
        }

        [TestMethod]
        [Description ("Свойство Status")]
        public void NullRecord_Status_1()
        {
            const RecordStatus status = RecordStatus.AutoinError;
            var record = new NullRecord();
            record.Status = status;
            Assert.AreEqual (status, record.Status);
        }
    }
}
