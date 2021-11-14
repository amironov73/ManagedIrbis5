// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable StringLiteralTypo

using AM.Json;
using AM.Runtime;
using AM.Xml;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Fields;

#nullable enable

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public sealed class TechnologyTest
        : Common.CommonUnitTest
    {
        public Technology _GetTechnology() => new ()
        {
            Phase = "КР",
            Date = "20211114",
            Responsible = "МироновАВ"
        };

        public Field _GetField() => new Field (Technology.Tag)
        {
            { 'a', "20211114" },
            { 'b', "МироновАВ" },
            { 'c', "КР" }
        };

        public Record _GetRecord() => new ()
        {
            { 907, "^cПК^a20010101^bМироновАВ" },
            { 907, "^cКТ^a20101010^bМироновАВ" },
            { 907, "^cКР^a20121212^bМироновАВ" },
            { 907, "^cКР^a20211114^bМироновАВ" }
        };

        public void _Compare
            (
                Technology first,
                Technology second
            )
        {
            Assert.AreEqual (first.Phase, second.Phase);
            Assert.AreEqual (first.Date, second.Date);
            Assert.AreEqual (first.Responsible, second.Responsible);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Technology_Constructor_1()
        {
            var technology = new Technology();
            Assert.IsNull (technology.Phase);
            Assert.IsNull (technology.Date);
            Assert.IsNull (technology.Responsible);
            Assert.IsNull (technology.UnknownSubFields);
            Assert.IsNull (technology.Field);
            Assert.IsNull (technology.UserData);
        }

        [TestMethod]
        [Description ("Применение данных к полю библиографической записи")]
        public void Technology_ApplyTo_1()
        {
            var actual = new Field()
            {
                { 'a', "11111" },
                { 'b', "22222" },
                { 'c', "33333" },
            };
            var technology = _GetTechnology();
            technology.ApplyToField (actual);
            var expected = _GetField();
            CompareFields (expected, actual);
        }

        [TestMethod]
        [Description ("Разбор поля библиографической записи")]
        public void Technology_ParseField_1()
        {
            var field = _GetField();
            var actual = Technology.ParseField (field);
            var expected = _GetTechnology();
            _Compare (expected, actual);
        }

        [TestMethod]
        [Description ("Разбор библиографической записи")]
        public void Technology_ParseRecord_1()
        {
            var expected = _GetTechnology();
            var record = _GetRecord();
            var actual = Technology.ParseRecord (record);
            Assert.IsNotNull (actual);
            Assert.AreEqual (4, actual.Length);
            _Compare (expected, actual[3]);
        }

        [TestMethod]
        [Description ("Преобразование в поле библиографической записи")]
        public void Technology_ToField_1()
        {
            var technology = _GetTechnology();
            var actual = technology.ToField();
            var expected = _GetField();
            CompareFields (expected, actual);
        }

        private void _TestSerialization
            (
                Technology first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<Technology>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.Field);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void Technology_Serialization_1()
        {
            var technology = new Technology();
            _TestSerialization (technology);

            technology = _GetTechnology();
            technology.Field = new Field();
            technology.UserData = "User data";
            _TestSerialization (technology);

        }

        [TestMethod]
        [Description ("Верификация")]
        public void Technology_Verification_1()
        {
            var technology = new Technology();
            Assert.IsFalse (technology.Verify (false));

            technology = _GetTechnology();
            Assert.IsTrue (technology.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void Technology_ToXml_1()
        {
            var technology = new Technology();
            Assert.AreEqual ("<technology />", XmlUtility.SerializeShort (technology));

            technology = _GetTechnology();
            Assert.AreEqual ("<technology><phase>КР</phase><date>20211114</date><responsible>МироновАВ</responsible></technology>",
                XmlUtility.SerializeShort (technology));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void Technology_ToJson_1()
        {
            var technology = new Technology();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (technology));

            technology = _GetTechnology();
            Assert.AreEqual ("{\"phase\":\"\\u041A\\u0420\",\"date\":\"20211114\",\"responsible\":\"\\u041C\\u0438\\u0440\\u043E\\u043D\\u043E\\u0432\\u0410\\u0412\"}",
                JsonUtility.SerializeShort (technology));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void Technology_ToString_1()
        {
            var technology = new Technology();
            Assert.AreEqual ("(null): (null): (null)",
                technology.ToString());

            technology = _GetTechnology();
            Assert.AreEqual ("КР: 20211114: МироновАВ",
                technology.ToString());
        }

        [TestMethod]
        [Description ("Получение даты первой модификации (создания) записи")]
        public void Technology_GetFirststDate_1()
        {
            const string expected = "20010101";
            var record = _GetRecord();
            var actual = Technology.GetFirstDate (record);
            Assert.AreEqual (expected, actual);
        }

        [TestMethod]
        [Description ("Получение даты первой модификации (создания) записи")]
        public void Technology_GetFirststDate_2()
        {
            var record = new Record();
            var actual = Technology.GetFirstDate (record);
            Assert.IsNull (actual);
        }

        [TestMethod]
        [Description ("Получение даты последней модификации записи")]
        public void Technology_GetLatestDate_1()
        {
            var record = new Record();
            var actual = Technology.GetLatestDate (record);
            Assert.IsNull (actual);
        }

        [TestMethod]
        [Description ("Получение даты последней модификации записи")]
        public void Technology_GetLatestDate_2()
        {
            const string expected = "20211114";
            var record = _GetRecord();
            var actual = Technology.GetLatestDate (record);
            Assert.AreEqual (expected, actual);
        }
    }
}
