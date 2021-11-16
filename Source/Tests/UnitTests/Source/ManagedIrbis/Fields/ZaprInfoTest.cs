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
    public sealed class ZaprInfoTest
        : Common.CommonUnitTest
    {
        private Field _GetField() => new Field (ZaprInfo.Tag)
            .Add ('a', "<.>U=5$<.>")
            .Add ('c', "<.>U=5$<.>")
            .Add ('d', "20170214");

        private ZaprInfo _GetZaprInfo() => new ()
        {
            NaturalLanguage = "<.>U=5$<.>",
            SearchQuery = "<.>U=5$<.>",
            Date = "20170214"
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

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void ZaprInfo_Construction_1()
        {
            var zapr = new ZaprInfo();
            Assert.IsNull (zapr.NaturalLanguage);
            Assert.IsNull (zapr.FullTextQuery);
            Assert.IsNull (zapr.SearchQuery);
            Assert.IsNotNull (zapr.Date);
            Assert.IsNull (zapr.UnknownSubFields);
            Assert.IsNull (zapr.Field);
            Assert.IsNull (zapr.UserData);
        }

        [TestMethod]
        [Description ("Применение данных к указанному полю записи")]
        public void ZaprInfo_ApplyTo_1()
        {
            var expected = _GetField();
            var actual = new Field();
            var zapr = _GetZaprInfo();
            Assert.AreSame (actual, zapr.ApplyTo (actual));
            CompareFields (expected, actual);
        }

        [TestMethod]
        [Description ("Разбор поля записи")]
        public void ZaprInfo_ParseField_1()
        {
            var field = _GetField();
            var actual = ZaprInfo.ParseField (field);
            var expected = _GetZaprInfo();
            _Compare (expected, actual);
            Assert.AreSame (field, actual.Field);
            Assert.IsNull (actual.UserData);
        }

        [TestMethod]
        [Description ("Разбор всей записи")]
        public void ZaprInfo_ParseRecord_1()
        {
            var field = _GetField();
            var record = new Record().Add (field);
            var actual = ZaprInfo.ParseRecord (record);
            var expected = _GetZaprInfo();
            Assert.IsNotNull (actual);
            Assert.AreEqual (1, actual.Length);
            _Compare (expected, actual[0]);
            Assert.AreSame (field, actual[0].Field);
            Assert.IsNull (actual[0].UserData);
        }

        [TestMethod]
        [Description ("Преобразование в поле записи")]
        public void ZaprInfo_ToField_1()
        {
            var zapr = _GetZaprInfo();
            var actual = zapr.ToField();
            var expected = _GetField();
            CompareFields (expected, actual);
        }

        private void _TestSerialization
            (
                ZaprInfo first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<ZaprInfo>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.Field);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void ZaprInfo_Serialization_1()
        {
            var zapr = new ZaprInfo();
            _TestSerialization (zapr);

            zapr = _GetZaprInfo();
            zapr.Field = new Field();
            zapr.UserData = "User data";
            _TestSerialization (zapr);
        }

        [TestMethod]
        [Description ("Верифцикация")]
        public void ZaprInfo_Verify_1()
        {
            var zapr = new ZaprInfo();
            Assert.IsFalse (zapr.Verify (false));

            zapr = _GetZaprInfo();
            Assert.IsTrue (zapr.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void ZaprInfo_ToXml_1()
        {
            var zapr = new ZaprInfo();
            Assert.AreEqual ("<zapr><date>20211116</date></zapr>", XmlUtility.SerializeShort (zapr));

            zapr = _GetZaprInfo();
            Assert.AreEqual ("<zapr><natural>&lt;.&gt;U=5$&lt;.&gt;</natural><search>&lt;.&gt;U=5$&lt;.&gt;</search><date>20170214</date></zapr>",
                XmlUtility.SerializeShort (zapr));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void ZaprInfo_ToJson_1()
        {
            var zapr = new ZaprInfo();
            Assert.AreEqual ("{\"date\":\"20211116\"}", JsonUtility.SerializeShort (zapr));

            zapr = _GetZaprInfo();
            var expected = "{\"natual\":\"\\u003C.\\u003EU=5$\\u003C.\\u003E\",\"search\":\"\\u003C.\\u003EU=5$\\u003C.\\u003E\",\"date\":\"20170214\"}";
            var actual = JsonUtility.SerializeShort (zapr);
            Assert.AreEqual (expected, actual);
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void ZaprInfo_ToString_1()
        {
            var zapr = new ZaprInfo();
            Assert.AreEqual ("20211116", zapr.ToString());

            zapr = _GetZaprInfo();
            Assert.AreEqual ("<.>U=5$<.> -- <.>U=5$<.> -- 20170214", zapr.ToString());
        }
    }
}
