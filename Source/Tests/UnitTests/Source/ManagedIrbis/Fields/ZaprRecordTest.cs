// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis;
using ManagedIrbis.Fields;

#nullable enable

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public sealed class ZaprRecordTest
    {
        private Record _GetRecord() => new Record()
            .Add (1, "A0123")
            .Add (2, "^A<.>U=5$<.>^C<.>U=5$<.>^D20170214");

        private ZaprRecord _GetZaprRecord() => new ()
        {
            Ticket = "A0123",
            Requests = new []
            {
                new ZaprInfo()
                {
                    NaturalLanguage = "<.>U=5$<.>",
                    SearchQuery = "<.>U=5$<.>",
                    Date = "20170214"
                }
            }
        };

        private void _Compare
            (
                ZaprInfo first,
                ZaprInfo second
            )
        {
            Assert.AreEqual (first.NaturalLanguage, second.NaturalLanguage);
            Assert.AreEqual (first.FullTextQuery, second.FullTextQuery);
            Assert.AreEqual (first.SearchQuery, second.SearchQuery);
            Assert.AreEqual (first.Date, second.Date);
        }

        private void _Compare
            (
                ZaprRecord first,
                ZaprRecord second
            )
        {
            Assert.AreEqual (first.Ticket, second.Ticket);
            Assert.AreEqual (first.Requests is not null, second.Requests is not null);
            if (first.Requests is not null && second.Requests is not null)
            {
                for (var i = 0; i < first.Requests.Length; i++)
                {
                    _Compare (first.Requests[i], second.Requests[i]);
                }
            }
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void ZaprRecord_Construction_1()
        {
            var zapr = new ZaprRecord();
            Assert.IsNull (zapr.Ticket);
            Assert.IsNull (zapr.Requests);
            Assert.IsNull (zapr.Record);
            Assert.IsNull (zapr.UserData);
        }

        [TestMethod]
        [Description ("Применение данных к указанному полю записи")]
        public void ZaprRecord_ApplyTo_1()
        {
            // var expected = _GetRecord();
            var actual = new Record();
            var zapr = _GetZaprRecord();
            Assert.AreSame (actual, zapr.ApplyTo (actual));
            Assert.AreEqual (2, actual.Fields.Count);
            // CompareRecords (expected, actual);
        }

        [TestMethod]
        [Description ("Разбор всей записи")]
        public void ZaprRecord_ParseRecord_1()
        {
            var record = _GetRecord();
            var actual = ZaprRecord.ParseRecord (record);
            var expected = _GetZaprRecord();
            _Compare (expected, actual);
            Assert.AreSame (record, actual.Record);
            Assert.IsNull (actual.UserData);
        }

        [TestMethod]
        [Description ("Преобразование в запись")]
        public void ZaprRecord_ToField_1()
        {
            var zapr = _GetZaprRecord();
            var actual = zapr.ToRecord();
            var expected = _GetRecord();
            Assert.IsNotNull (expected);
            Assert.AreEqual (expected.Fields.Count, actual.Fields.Count);
            // CompareRecords (expected, actual);
        }

        private void _TestSerialization
            (
                ZaprRecord first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<ZaprRecord>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.Record);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void ZaprRecord_Serialization_1()
        {
            var zapr = new ZaprRecord();
            _TestSerialization (zapr);

            zapr = _GetZaprRecord();
            zapr.Record = new Record();
            zapr.UserData = "User data";
            _TestSerialization (zapr);
        }

        [TestMethod]
        [Description ("Верифцикация")]
        public void ZaprRecord_Verify_1()
        {
            var zapr = new ZaprRecord();
            Assert.IsFalse (zapr.Verify (false));

            zapr = _GetZaprRecord();
            Assert.IsTrue (zapr.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void ZaprRecord_ToXml_1()
        {
            var zapr = new ZaprRecord();
            Assert.AreEqual ("<zapr />", XmlUtility.SerializeShort (zapr));

            zapr = _GetZaprRecord();
            Assert.AreEqual ("<zapr><ticket>A0123</ticket><request><natural>&lt;.&gt;U=5$&lt;.&gt;</natural><search>&lt;.&gt;U=5$&lt;.&gt;</search><date>20170214</date></request></zapr>",
                XmlUtility.SerializeShort (zapr));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void ZaprRecord_ToJson_1()
        {
            var zapr = new ZaprRecord();
            Assert.AreEqual ("{}",
                JsonUtility.SerializeShort (zapr));

            zapr = _GetZaprRecord();
            var expected = "{\"ticket\":\"A0123\",\"zapr\":[{\"natual\":\"\\u003C.\\u003EU=5$\\u003C.\\u003E\",\"search\":\"\\u003C.\\u003EU=5$\\u003C.\\u003E\",\"date\":\"20170214\"}]}";
            var actual = JsonUtility.SerializeShort (zapr);
            Assert.AreEqual (expected, actual);
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void ZaprRecord_ToString_1()
        {
            var zapr = new ZaprRecord();
            Assert.AreEqual ("(null)", zapr.ToString());

            zapr = _GetZaprRecord();
            Assert.AreEqual ("A0123",
                zapr.ToString());
        }

    }
}
