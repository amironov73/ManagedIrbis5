// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using AM;
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
    public class Field203Test
        : Common.CommonUnitTest
    {
        private Field203 _Get203() => new ()
        {
            ContentType = new [] { "Текст" },
            Access = new [] { "непосредственный" },
            ContentDescription = new [] { "визуальный" }
        };

        private Field _GetField() => new (Field203.Tag, "^aТекст^cнепосредственный^oвизуальный");

        private void _Compare
            (
                Field203 first,
                Field203 second
            )
        {
            CollectionAssert.AreEqual (first.ContentType, second.ContentType);
            CollectionAssert.AreEqual (first.Access, second.Access);
            CollectionAssert.AreEqual (first.ContentDescription, second.ContentDescription);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Field203_Construction_1()
        {
            var field = new Field203();
            Assert.IsNull (field.Access);
            Assert.IsNull (field.ContentDescription);
            Assert.IsNull (field.ContentType);
            Assert.IsNull (field.Field);
            Assert.IsNull (field.UnknownSubFields);
            Assert.IsNull (field.UserData);
        }

        [TestMethod]
        [Description ("Конструктор со значениями")]
        public void Field203_Construction_2()
        {
            var field = new Field203 ("Музыка", "исполнительская", "знаковая");
            Assert.AreEqual ("Музыка", Utility.NonEmptyOrDefault (field.ContentType));
            Assert.AreEqual ("исполнительская", Utility.NonEmptyOrDefault (field.Access));
            Assert.AreEqual ("знаковая", Utility.NonEmptyOrDefault (field.ContentDescription));
            Assert.IsNull (field.Field);
            Assert.IsNull (field.UnknownSubFields);
            Assert.IsNull (field.UserData);
        }

        [TestMethod]
        [Description ("Применение данных к полю библиографической записи")]
        public void Field203_ApplyTo_1()
        {
            var actual = new Field()
                .Add ('a', "o")
                .Add ('c', "03")
                .Add ('o', "91");
            var f203 = _Get203();
            f203.ApplyToField (actual);
            var expected = _GetField();
            CompareFields (expected, actual);
        }

        [TestMethod]
        [Description ("Разбор поля библиографической записи")]
        public void Field203_ParseField_1()
        {
            var field = new Field (Field203.Tag);
            var parsed = Field203.ParseField (field);
            Assert.IsNotNull (parsed.Access);
            Assert.AreEqual (0, parsed.Access.Length);
            Assert.IsNotNull (parsed.ContentDescription);
            Assert.AreEqual (0, parsed.ContentDescription.Length);
            Assert.IsNotNull (parsed.ContentType);
            Assert.AreEqual (0, parsed.ContentType.Length);
            Assert.AreSame (field, parsed.Field);
            Assert.IsNotNull (parsed.UnknownSubFields);
            Assert.IsNull (parsed.UserData);
        }

        [TestMethod]
        [Description ("Разбор поля библиографической записи")]
        public void Field203_ParseField_2()
        {
            var field = _GetField();
            var parsed = Field203.ParseField (field);
            Assert.IsNotNull (parsed.Access);
            Assert.AreEqual (1, parsed.Access.Length);
            Assert.AreEqual ("непосредственный", parsed.Access[0]);
            Assert.IsNotNull (parsed.ContentDescription);
            Assert.AreEqual (1, parsed.ContentDescription.Length);
            Assert.AreEqual ("визуальный", parsed.ContentDescription[0]);
            Assert.IsNotNull (parsed.ContentType);
            Assert.AreEqual (1, parsed.ContentType.Length);
            Assert.AreEqual ("Текст", parsed.ContentType[0]);
            Assert.AreSame (field, parsed.Field);
            Assert.IsNotNull (parsed.UnknownSubFields);
            Assert.IsNull (parsed.UserData);
        }

        [TestMethod]
        [Description ("Разбор поля библиографической записи")]
        public void Field203_ParseRecord_1()
        {
            var field = _GetField();
            var record = new Record() { field };
            var parsed = Field203.ParseRecord (record);
            Assert.IsNotNull (parsed);
            Assert.AreEqual (1, parsed.Length);
            Assert.IsNotNull (parsed[0].Access);
            Assert.AreEqual (1, parsed[0].Access!.Length);
            Assert.AreEqual ("непосредственный", parsed[0].Access![0]);
            Assert.IsNotNull (parsed[0].ContentDescription);
            Assert.AreEqual (1, parsed[0].ContentDescription!.Length);
            Assert.AreEqual ("визуальный", parsed[0].ContentDescription![0]);
            Assert.IsNotNull (parsed[0].ContentType);
            Assert.AreEqual (1, parsed[0].ContentType!.Length);
            Assert.AreEqual ("Текст", parsed[0].ContentType![0]);
            Assert.AreSame (field, parsed[0].Field);
            Assert.IsNotNull (parsed[0].UnknownSubFields);
            Assert.IsNull (parsed[0].UserData);
        }

        [TestMethod]
        [Description ("Преобразование в поле библиографической записи")]
        public void Field203_ToField_1()
        {
            var field203 = _Get203();
            var actual = field203.ToField();
            var expected = _GetField();
            CompareFields (expected, actual);
        }

        private void _TestSerialization
            (
                Field203 first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<Field203>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.Field);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void Field203_Serialization_1()
        {
            var field203 = new Field203();
            _TestSerialization (field203);

            field203 = _Get203();
            field203.Field = new Field();
            field203.UserData = "User data";
            _TestSerialization (field203);

        }

        [TestMethod]
        [Description ("Верификация")]
        public void Field203_Verification_1()
        {
            var field203 = new Field203();
            Assert.IsFalse (field203.Verify (false));

            field203 = _Get203();
            Assert.IsTrue (field203.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void Field203_ToXml_1()
        {
            var field203 = new Field203();
            Assert.AreEqual ("<field-203 />", XmlUtility.SerializeShort (field203));

            field203 = _Get203();
            Assert.AreEqual ("<field-203 content-type=\"Текст\" access=\"непосредственный\" content-description=\"визуальный\" />",
                XmlUtility.SerializeShort (field203));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void Field203_ToJson_1()
        {
            var field203 = new Field203();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (field203));

            field203 = _Get203();
            Assert.AreEqual ("{\"contentType\":[\"\\u0422\\u0435\\u043A\\u0441\\u0442\"],\"access\":[\"\\u043D\\u0435\\u043F\\u043E\\u0441\\u0440\\u0435\\u0434\\u0441\\u0442\\u0432\\u0435\\u043D\\u043D\\u044B\\u0439\"],\"content-description\":[\"\\u0432\\u0438\\u0437\\u0443\\u0430\\u043B\\u044C\\u043D\\u044B\\u0439\"]}",
                JsonUtility.SerializeShort (field203));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void Field203_ToString_1()
        {
            var field203 = new Field203();
            Assert.AreEqual ("(none): (none)",
                field203.ToString());

            field203 = _Get203();
            Assert.AreEqual ("Текст: непосредственный",
                field203.ToString());
        }
    }
}
