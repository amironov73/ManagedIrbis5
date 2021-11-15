// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InvokeAsExtensionMethod
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
    public sealed class FullTextInfoTest
        : Common.CommonUnitTest
    {
        private Field _GetField() => new (FullTextInfo.Tag, "^AПример PDF-файла.PDF^TПример внешнего объекта в виде PDF-файла - с постраничным просмотром^N14^BAR0123");

        private FullTextInfo _GetFullTextInfo() => new ()
        {
            FileName = "Пример PDF-файла.PDF",
            DisplayText = "Пример внешнего объекта в виде PDF-файла - с постраничным просмотром",
            PageCount = "14",
            AccessRights = "AR0123"
        };

        private void _Compare
            (
                FullTextInfo first,
                FullTextInfo second
            )
        {
            Assert.AreEqual (first.FileName, second.FileName);
            Assert.AreEqual (first.DisplayText, second.DisplayText);
            Assert.AreEqual (first.PageCount, second.PageCount);
            Assert.AreEqual (first.AccessRights, second.AccessRights);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void FullTextInfo_Construction_1()
        {
            var info = new FullTextInfo();
            Assert.IsNull (info.FileName);
            Assert.IsNull (info.DisplayText);
            Assert.IsNull (info.PageCount);
            Assert.IsNull (info.AccessRights);
            Assert.IsNull (info.Field);
            Assert.IsNull (info.UnknownSubFields);
            Assert.IsNull (info.UserData);
        }

        [TestMethod]
        [Description ("Применение данных к указанному полю записи")]
        public void FullTextInfo_ApplyTo_1()
        {
            var expected = _GetField();
            var actual = new Field();
            var info = _GetFullTextInfo();
            info.ApplyTo (actual);
            CompareFields (expected, actual);
        }

        [TestMethod]
        [Description ("Разбор поля записи")]
        public void FullTextInfo_ParseField_1()
        {
            var field = _GetField();
            var actual = FullTextInfo.ParseField (field);
            var expected = _GetFullTextInfo();
            _Compare (expected, actual);
            Assert.AreSame (field, actual.Field);
            Assert.IsNull (actual.UserData);
        }

        [TestMethod]
        [Description ("Разбор записи")]
        public void FullTextInfo_ParseRecord_1()
        {
            var record = new Record();
            var field = _GetField();
            record.Add (field);
            var actual = FullTextInfo.ParseRecord (record);
            var expected = _GetFullTextInfo();
            Assert.IsNotNull (actual);
            Assert.AreEqual (1, actual.Length);
            _Compare (expected, actual[0]);
            Assert.AreSame (field, actual[0].Field);
            Assert.IsNull (actual[0].UserData);
        }

        private void _TestSerialization
            (
                FullTextInfo first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<FullTextInfo>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.Field);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void FullTextInfo_Serialization_1()
        {
            var info = new FullTextInfo();
            _TestSerialization (info);

            info = _GetFullTextInfo();
            info.Field = new Field();
            info.UserData = "User data";
            _TestSerialization (info);
        }

        [TestMethod]
        [Description ("Преобразование в поле записи")]
        public void FullTextInfo_ToField_1()
        {
            var info = _GetFullTextInfo();
            var expected = _GetField();
            var actual = info.ToField();
            CompareFields (expected, actual);
        }

        [TestMethod]
        [Description ("Верифцикация")]
        public void FullTextInfo_Verify_1()
        {
            var info = new FullTextInfo();
            Assert.IsFalse (info.Verify (false));

            info = _GetFullTextInfo();
            Assert.IsTrue (info.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void FullTextInfo_ToXml_1()
        {
            var info = new FullTextInfo();
            Assert.AreEqual ("<fulltext />", XmlUtility.SerializeShort (info));

            info = _GetFullTextInfo();
            Assert.AreEqual ("<fulltext display-text=\"Пример внешнего объекта в виде PDF-файла - с постраничным просмотром\" filename=\"Пример PDF-файла.PDF\" page-count=\"14\" access-rights=\"AR0123\" />",
                XmlUtility.SerializeShort (info));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void FullTextInfo_ToJson_1()
        {
            var info = new FullTextInfo();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (info));

            info = _GetFullTextInfo();
            var expected = "{\"displayText\":\"\\u041F\\u0440\\u0438\\u043C\\u0435\\u0440 \\u0432\\u043D\\u0435\\u0448\\u043D\\u0435\\u0433\\u043E \\u043E\\u0431\\u044A\\u0435\\u043A\\u0442\\u0430 \\u0432 \\u0432\\u0438\\u0434\\u0435 PDF-\\u0444\\u0430\\u0439\\u043B\\u0430 - \\u0441 \\u043F\\u043E\\u0441\\u0442\\u0440\\u0430\\u043D\\u0438\\u0447\\u043D\\u044B\\u043C \\u043F\\u0440\\u043E\\u0441\\u043C\\u043E\\u0442\\u0440\\u043E\\u043C\",\"filename\":\"\\u041F\\u0440\\u0438\\u043C\\u0435\\u0440 PDF-\\u0444\\u0430\\u0439\\u043B\\u0430.PDF\",\"pageCount\":\"14\",\"accessRights\":\"AR0123\"}";
            var actual = JsonUtility.SerializeShort (info);
            Assert.AreEqual (expected, actual);
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void FullTextInfo_ToString_1()
        {
            var info = new FullTextInfo();
            Assert.AreEqual ("(null)", info.ToString());

            info = _GetFullTextInfo();
            Assert.AreEqual ("Пример PDF-файла.PDF", info.ToString());
        }

    }
}
