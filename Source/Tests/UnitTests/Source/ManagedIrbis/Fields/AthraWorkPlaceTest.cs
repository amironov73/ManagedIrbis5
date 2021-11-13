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
    public sealed class AthraWorkPlaceTest
        : Common.CommonUnitTest
    {
        private Field _GetField() => new Field (AthraWorkPlace.Tag)
            .Add ('y', "да")
            .Add ('p', "ИОГУНБ");

        private AthraWorkPlace _GetAthraWorkPlace() => new ()
        {
            WorksHere = "да",
            WorkPlace = "ИОГУНБ"
        };

        private void _Compare
            (
                AthraWorkPlace first,
                AthraWorkPlace second
            )
        {
            Assert.AreEqual (first.WorksHere, second.WorksHere);
            Assert.AreEqual (first.WorkPlace, second.WorkPlace);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void AthraWorkPlace_Construction_1()
        {
            var athra = new AthraWorkPlace();
            Assert.IsNull (athra.WorksHere);
            Assert.IsNull (athra.WorkPlace);
            Assert.IsNull (athra.UnknownSubFields);
            Assert.IsNull (athra.Field);
            Assert.IsNull (athra.UserData);
        }

        [TestMethod]
        [Description ("Применение данных к указанному полю записи")]
        public void AthraWorkPlace_ApplyTo_1()
        {
            var expected = _GetField();
            var actual = new Field();
            var athra = _GetAthraWorkPlace();
            Assert.AreSame (actual, athra.ApplyTo (actual));
            CompareFields (expected, actual);
        }

        [TestMethod]
        [Description ("Разбор поля записи")]
        public void AthraWorkPlace_ParseField_1()
        {
            var field = _GetField();
            var actual = AthraWorkPlace.ParseField (field);
            var expected = _GetAthraWorkPlace();
            _Compare (expected, actual);
            Assert.AreSame (field, actual.Field);
            Assert.IsNull (actual.UserData);
        }

        [TestMethod]
        [Description ("Преобразование в поле записи")]
        public void Atrha_ToField_1()
        {
            var athra = _GetAthraWorkPlace();
            var actual = athra.ToField();
            var expected = _GetField();
            CompareFields (expected, actual);
        }

        private void _TestSerialization
            (
                AthraWorkPlace first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<AthraWorkPlace>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.Field);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void AthraWorkPlace_Serialization_1()
        {
            var athra = new AthraWorkPlace();
            _TestSerialization (athra);

            athra = _GetAthraWorkPlace();
            athra.Field = new Field();
            athra.UserData = "User data";
            _TestSerialization (athra);
        }

        [TestMethod]
        [Description ("Верифцикация")]
        public void AthraWorkPlace_Verify_1()
        {
            var athra = new AthraWorkPlace();
            Assert.IsFalse (athra.Verify (false));

            athra = _GetAthraWorkPlace();
            Assert.IsTrue (athra.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void AthraWorkPlace_ToXml_1()
        {
            var athra = new AthraWorkPlace();
            Assert.AreEqual ("<workplace />", XmlUtility.SerializeShort (athra));

            athra = _GetAthraWorkPlace();
            Assert.AreEqual ("<workplace><here>да</here><place>ИОГУНБ</place></workplace>",
                XmlUtility.SerializeShort (athra));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void AthraWorkPlace_ToJson_1()
        {
            var athra = new AthraWorkPlace();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (athra));

            athra = _GetAthraWorkPlace();
            var expected = "{\"here\":\"\\u0434\\u0430\",\"place\":\"\\u0418\\u041E\\u0413\\u0423\\u041D\\u0411\"}";
            var actual = JsonUtility.SerializeShort (athra);
            Assert.AreEqual (expected, actual);
        }

        [TestMethod]
        [Description ("Текстовое представление")]
        public void AthraWorkPlace_ToString_1()
        {
            var athra = new AthraWorkPlace();
            Assert.AreEqual ("(null)", athra.ToString());

            athra = _GetAthraWorkPlace();
            Assert.AreEqual ("ИОГУНБ", athra.ToString());
        }

    }
}
