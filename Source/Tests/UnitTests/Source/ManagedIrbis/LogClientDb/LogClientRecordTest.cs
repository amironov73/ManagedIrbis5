// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis;
using ManagedIrbis.LogClientDb;

using Microsoft.VisualStudio.TestTools.UnitTesting;


#nullable enable

namespace UnitTests.ManagedIrbis.LogClientDb
{
    [TestClass]
    public sealed class LogClientRecordTest
    {
        private Record _GetRecord()
        {
            return new Record()
                .Add (1, "202112111320")
                .Add (2, "miron")
                .Add (3, "127.0.0.1")
                .Add (4, "IBIS")
                .Add (5, EventCode.Connect)
                .Add (6, "some")
                .Add (6, "data");
        }

        private LogClientRecord _GetLog()
        {
            return new ()
            {
                Moment = "202112111320",
                Login = "miron",
                IpAddress = "127.0.0.1",
                Database = "IBIS",
                ActionCode = EventCode.Connect,
                ActionEssence = new [] { "some", "data" }
            };
        }

        private void _Compare
            (
                LogClientRecord first,
                LogClientRecord second
            )
        {
            Assert.AreEqual (first.Moment, second.Moment);
            Assert.AreEqual (first.Login, second.Login);
            Assert.AreEqual (first.IpAddress, second.IpAddress);
            Assert.AreEqual (first.Database, second.Database);
            Assert.AreEqual (first.ActionCode, second.ActionCode);
            CollectionAssert.AreEqual (first.ActionEssence, second.ActionEssence);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void LogClientRecord_Construction_1()
        {
            var log = new LogClientRecord();
            Assert.IsNull (log.Moment);
            Assert.IsNull (log.Login);
            Assert.IsNull (log.IpAddress);
            Assert.IsNull (log.Database);
            Assert.IsNull (log.ActionCode);
            Assert.IsNull (log.ActionEssence);
            Assert.IsNull (log.Record);
            Assert.IsNull (log.UserData);
        }

        [TestMethod]
        [Description ("Присвоение")]
        public void LogClientRecord_Construction_2()
        {
            var log = new LogClientRecord
            {
                Moment = "202112111320",
                Login = "miron",
                IpAddress = "127.0.0.1",
                Database = "IBIS",
                ActionCode = EventCode.Connect,
                ActionEssence = new [] { "some", "data" },
                Record = new Record() { Mfn = 123 },
                UserData = "User data"
            };
            Assert.AreEqual ("202112111320", log.Moment);
            Assert.AreEqual ("miron", log.Login);
            Assert.AreEqual ("127.0.0.1", log.IpAddress);
            Assert.AreEqual ("IBIS", log.Database);
            Assert.AreEqual (EventCode.Connect, log.ActionCode);
            Assert.IsNotNull (log.ActionEssence);
            Assert.AreEqual (2, log.ActionEssence.Length);
            Assert.IsNotNull (log.Record);
            Assert.AreEqual (123, log.Record.Mfn);
            Assert.AreEqual ("User data", log.UserData);
        }

        private void _TestSerialization
            (
                LogClientRecord first
            )
        {
            var memory = first.SaveToMemory();
            var second = memory.RestoreObjectFromMemory<LogClientRecord>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.Record);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Применение данных к записи из базы")]
        public void LogClientRecord_ApplyTo_1()
        {
            var log = _GetLog();
            var record = new Record();
            log.ApplyToRecord (record);
            Assert.AreEqual (log.Moment, record.FM (1));
            Assert.AreEqual (log.Login, record.FM (2));
            Assert.AreEqual (log.IpAddress, record.FM (3));
            Assert.AreEqual (log.Database, record.FM (4));
            Assert.AreEqual (log.ActionCode, record.FM (5));
            CollectionAssert.AreEqual (log.ActionEssence, record.FMA (6));
        }

        [TestMethod]
        [Description ("Разбор записи из базы данных")]
        public void LogClientRecord_ParseRecord_1()
        {
            var record = _GetRecord();
            var log = LogClientRecord.ParseRecord (record);
            Assert.AreEqual (record.FM (1), log.Moment);
            Assert.AreEqual (record.FM (2), log.Login);
            Assert.AreEqual (record.FM (3), log.IpAddress);
            Assert.AreEqual (record.FM (4), log.Database);
            Assert.AreEqual (record.FM (5), log.ActionCode);
            CollectionAssert.AreEqual (record.FMA (6), log.ActionEssence);
            Assert.AreSame (record, log.Record);
        }

        [TestMethod]
        [Description ("Формирование по данным записи для базы")]
        public void LogClientRecord_ToRecord_1()
        {
            var log = _GetLog();
            var record = log.ToRecord();
            Assert.AreEqual (log.Moment, record.FM (1));
            Assert.AreEqual (log.Login, record.FM (2));
            Assert.AreEqual (log.IpAddress, record.FM (3));
            Assert.AreEqual (log.Database, record.FM (4));
            Assert.AreEqual (log.ActionCode, record.FM (5));
            CollectionAssert.AreEqual (log.ActionEssence, record.FMA (6));
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void LogClientRecord_Serialization_1()
        {
            var log = new LogClientRecord();
            _TestSerialization (log);

            log = _GetLog();
            log.Record = new Record();
            log.UserData = "User data";
            _TestSerialization (log);
        }

        [TestMethod]
        [Description ("Верификация")]
        public void LogClientRecord_Verify_1()
        {
            var log = new LogClientRecord();
            Assert.IsFalse (log.Verify (false));

            log = _GetLog();
            Assert.IsTrue (log.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void LogClientRecord_ToXml_1()
        {
            var log = new LogClientRecord();
            Assert.AreEqual
                (
                    "<log />",
                    XmlUtility.SerializeShort (log)
                );

            log = _GetLog();
            Assert.AreEqual
                (
                    "<log moment=\"202112111320\" login=\"miron\" ip-address=\"127.0.0.1\" database=\"IBIS\" code=\"9\"><essence>some</essence><essence>data</essence></log>",
                    XmlUtility.SerializeShort (log)
                );
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void LogClientRecord_ToJson_1()
        {
            var log = new LogClientRecord();
            Assert.AreEqual
                (
                    "{}",
                    JsonUtility.SerializeShort (log)
                );

            log = _GetLog();
            Assert.AreEqual
                (
                    "{\"moment\":\"202112111320\",\"login\":\"miron\",\"ipAddress\":\"127.0.0.1\",\"database\":\"IBIS\",\"code\":\"9\",\"essence\":[\"some\",\"data\"]}",
                    JsonUtility.SerializeShort (log)
                );
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void LogClientRecord_ToString_1()
        {
            var log = new LogClientRecord();
            Assert.AreEqual
                (
                    ";;;;",
                    log.ToString()
                );

            log = _GetLog();
            Assert.AreEqual
                (
                    "202112111320;miron;127.0.0.1;IBIS;9",
                    log.ToString()
                );
        }
    }
}
