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
    public sealed class MediumTypeTest
        : Common.CommonUnitTest
    {
        private Field _GetField() => new Field (MediumType.Tag)
            .Add ('a', "непосредственное");

        private MediumType _GetMediumType() => new ()
        {
            MediumCode = "непосредственное"
        };

        private void _Compare
            (
                MediumType first,
                MediumType second
            )
        {
            Assert.AreEqual (first.MediumCode, second.MediumCode);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void MediumType_Construction_1()
        {
            var medium = new MediumType();
            Assert.IsNull (medium.MediumCode);
            Assert.IsNull (medium.UnknownSubFields);
            Assert.IsNull (medium.Field);
            Assert.IsNull (medium.UserData);
        }

        [TestMethod]
        [Description ("Применение данных к указанному полю записи")]
        public void MediumType_ApplyTo_1()
        {
            var expected = _GetField();
            var actual = new Field();
            var medium = _GetMediumType();
            Assert.AreSame (actual, medium.ApplyTo (actual));
            CompareFields (expected, actual);
        }

        [TestMethod]
        [Description ("Разбор поля записи")]
        public void MediumType_ParseField_1()
        {
            var field = _GetField();
            var actual = MediumType.ParseField (field);
            var expected = _GetMediumType();
            _Compare (expected, actual);
            Assert.AreSame (field, actual.Field);
            Assert.IsNull (actual.UserData);
        }

        [TestMethod]
        [Description ("Преобразование в поле записи")]
        public void MediumType_ToField_1()
        {
            var medium = _GetMediumType();
            var actual = medium.ToField();
            var expected = _GetField();
            CompareFields (expected, actual);
        }

        private void _TestSerialization
            (
                MediumType first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<MediumType>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.Field);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void MediumType_Serialization_1()
        {
            var medium = new MediumType();
            _TestSerialization (medium);

            medium = _GetMediumType();
            medium.Field = new Field();
            medium.UserData = "User data";
            _TestSerialization (medium);
        }

        [TestMethod]
        [Description ("Верифцикация")]
        public void MediumType_Verify_1()
        {
            var medium = new MediumType();
            Assert.IsFalse (medium.Verify (false));

            medium = _GetMediumType();
            Assert.IsTrue (medium.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void MediumType_ToXml_1()
        {
            var medium = new MediumType();
            Assert.AreEqual ("<medium />", XmlUtility.SerializeShort (medium));

            medium = _GetMediumType();
            Assert.AreEqual ("<medium>непосредственное</medium>",
                XmlUtility.SerializeShort (medium));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void MediumType_ToJson_1()
        {
            var medium = new MediumType();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (medium));

            medium = _GetMediumType();
            var expected = "{\"medium\":\"\\u043D\\u0435\\u043F\\u043E\\u0441\\u0440\\u0435\\u0434\\u0441\\u0442\\u0432\\u0435\\u043D\\u043D\\u043E\\u0435\"}";
            var actual = JsonUtility.SerializeShort (medium);
            Assert.AreEqual (expected, actual);
        }

        [TestMethod]
        [Description ("Текстовое представление")]
        public void MediumType_ToString_1()
        {
            var medium = new MediumType();
            Assert.AreEqual ("(null)", medium.ToString());

            medium = _GetMediumType();
            Assert.AreEqual ("непосредственное", medium.ToString());
        }
    }
}
