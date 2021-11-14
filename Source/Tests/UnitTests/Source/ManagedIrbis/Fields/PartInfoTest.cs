// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable StringLiteralTypo

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis;
using ManagedIrbis.Fields;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public class PartInfoTest
        : Common.CommonUnitTest
    {
        private PartInfo _GetPart() => new ()
        {
            SecondLevelNumber = "Ч. 2",
            SecondLevelTitle = "Отрочество"
        };

        private Field _GetField() => new Field (PartInfo.Tag)
            .Add ('h', "Ч. 2")
            .Add ('i', "Отрочество");

        private void _Compare
            (
                PartInfo first,
                PartInfo second
            )
        {
            Assert.AreEqual (first.SecondLevelNumber, second.SecondLevelNumber);
            Assert.AreEqual (first.SecondLevelTitle, second.SecondLevelTitle);
            Assert.AreEqual (first.ThirdLevelNumber, second.ThirdLevelNumber);
            Assert.AreEqual (first.ThirdLevelTitle, second.ThirdLevelTitle);
            Assert.AreEqual (first.Role, second.Role);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void PartInfo_Construction_1()
        {
            var part = new PartInfo();
            Assert.IsNull (part.SecondLevelNumber);
            Assert.IsNull (part.SecondLevelTitle);
            Assert.IsNull (part.ThirdLevelNumber);
            Assert.IsNull (part.ThirdLevelTitle);
            Assert.IsNull (part.Role);
            Assert.IsNull (part.UnknownSubFields);
            Assert.IsNull (part.Field);
            Assert.IsNull (part.UserData);
        }

        private void _TestSerialization
            (
                PartInfo first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<PartInfo>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.AreSame (first.UnknownSubFields, second.UnknownSubFields);
            Assert.IsNull (second.Field);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void PartInfo_Serialization_1()
        {
            var part = new PartInfo();
            _TestSerialization (part);

            part.Field = new Field();
            part.UserData = "User data";
            _TestSerialization (part);

            part = _GetPart();
            _TestSerialization (part);
        }

        [TestMethod]
        [Description ("Разбор поля библиографической записи")]
        public void PartInfo_ParseField_1()
        {
            var field = _GetField();
            var expected = _GetPart();
            var actual = PartInfo.ParseField (field);
            Assert.AreSame (field, actual.Field);
            _Compare (expected, actual);
            Assert.IsNotNull (actual.UnknownSubFields);
            Assert.AreEqual (0, actual.UnknownSubFields.Length);
            Assert.IsNull (actual.UserData);
        }

        [TestMethod]
        [Description ("Разбор библиографической записи")]
        public void PartInfo_ParseRecord_1()
        {
            var expected = _GetPart();
            var record = new Record();
            var field = _GetField();
            record.Fields.Add (field);
            var actual = PartInfo.ParseRecord (record);
            Assert.AreEqual (1, actual.Length);
            Assert.AreSame (field, actual[0].Field);
            _Compare (expected, actual[0]);
            Assert.IsNotNull (actual[0].UnknownSubFields);
            Assert.AreEqual (0, actual[0].UnknownSubFields!.Length);
            Assert.IsNull (actual[0].UserData);
        }

        [TestMethod]
        [Description ("Преобразование информации в поле библиографической записи")]
        public void PartInfo_ToField_1()
        {
            var expected = _GetField();
            var part = _GetPart();
            var actual = part.ToField();
            Assert.AreEqual (expected.Tag, actual.Tag);
            CompareFields (expected, actual);
        }

        [TestMethod]
        [Description ("Применение информации к полю библиографической записи")]
        public void PartInfo_ApplyToField_1()
        {
            var expected = _GetField();
            var actual = new Field (PartInfo.Tag)
                .Add ('h', "???")
                .Add ('i', "???");
            var part = _GetPart();
            part.ApplyToField (actual);
            Assert.AreEqual (expected.Tag, actual.Tag);
            CompareFields (expected, actual);
        }

        [TestMethod]
        [Description ("Верификация")]
        public void PartInfo_Verify_1()
        {
            var part = new PartInfo();
            Assert.IsFalse (part.Verify (false));

            part = _GetPart();
            Assert.IsTrue (part.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void PartInfo_ToXml_1()
        {
            var part = new PartInfo();
            Assert.AreEqual ("<part />", XmlUtility.SerializeShort (part));

            part = _GetPart();
            Assert.AreEqual ("<part><secondLevelNumber>Ч. 2</secondLevelNumber><secondLevelTitle>Отрочество</secondLevelTitle></part>",
                XmlUtility.SerializeShort (part));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void PartInfo_ToJson_1()
        {
            var part = new PartInfo();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (part));

            part = _GetPart();
            Assert.AreEqual ("{\"secondLevelNumber\":\"\\u0427. 2\",\"secondLevelTitle\":\"\\u041E\\u0442\\u0440\\u043E\\u0447\\u0435\\u0441\\u0442\\u0432\\u043E\"}",
                JsonUtility.SerializeShort (part));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void PartInfo_ToString_1()
        {
            var part = new PartInfo();
            Assert.AreEqual ("(null)", part.ToString());

            part = _GetPart();
            Assert.AreEqual ("Ч. 2 -- Отрочество", part.ToString());
        }
    }
}
