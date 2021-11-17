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
    public sealed class DirectorInfoTest
        : Common.CommonUnitTest
    {
        private Field _GetField() => new Field (DirectorInfo.Tag)
            .Add ('a', "Директор Иркутской областной государственной универсальной научной библиотеки")
            .Add ('d', "Директор ИОГУНБ")
            .Add ('b', "Сулейманова Л. А.");

        private DirectorInfo _GetDirectorInfo() => new ()
        {
            Title = "Директор Иркутской областной государственной универсальной научной библиотеки",
            Abbreviation = "Директор ИОГУНБ",
            DirectorName = "Сулейманова Л. А."
        };

        private void _Compare
            (
                DirectorInfo first,
                DirectorInfo second
            )
        {
            Assert.AreEqual (first.Title, second.Title);
            Assert.AreEqual (first.Abbreviation, second.Abbreviation);
            Assert.AreEqual (first.DirectorName, second.DirectorName);
            Assert.AreEqual (first.AccountantName, second.AccountantName);
            Assert.AreEqual (first.ContactName, second.ContactName);
            Assert.AreEqual (first.ContactPhone, second.ContactPhone);
            Assert.AreEqual (first.HeadOfStructuralUnit, second.HeadOfStructuralUnit);
            Assert.AreEqual (first.Registry1, second.Registry1);
            Assert.AreEqual (first.Registry2, second.Registry2);
            Assert.AreEqual (first.Registry3, second.Registry3);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void DirectorInfo_Construction_1()
        {
            var director = new DirectorInfo();
            Assert.IsNull (director.Title);
            Assert.IsNull (director.Abbreviation);
            Assert.IsNull (director.DirectorName);
            Assert.IsNull (director.AccountantName);
            Assert.IsNull (director.ContactName);
            Assert.IsNull (director.ContactPhone);
            Assert.IsNull (director.HeadOfStructuralUnit);
            Assert.IsNull (director.Registry1);
            Assert.IsNull (director.Registry2);
            Assert.IsNull (director.Registry3);
            Assert.IsNull (director.UnknownSubFields);
            Assert.IsNull (director.Field);
            Assert.IsNull (director.UserData);
        }

        [TestMethod]
        [Description ("Применение данных к указанному полю записи")]
        public void DirectorInfo_ApplyTo_1()
        {
            var expected = _GetField();
            var actual = new Field();
            var director = _GetDirectorInfo();
            Assert.AreSame (actual, director.ApplyTo (actual));
            CompareFields (expected, actual);
        }

        [TestMethod]
        [Description ("Разбор поля записи")]
        public void DirectorInfo_ParseField_1()
        {
            var field = _GetField();
            var actual = DirectorInfo.ParseField (field);
            var expected = _GetDirectorInfo();
            _Compare (expected, actual);
            Assert.AreSame (field, actual.Field);
            Assert.IsNull (actual.UserData);
        }

        [TestMethod]
        [Description ("Преобразование в поле записи")]
        public void DirectorInfo_ToField_1()
        {
            var director = _GetDirectorInfo();
            var actual = director.ToField();
            var expected = _GetField();
            CompareFields (expected, actual);
        }

        private void _TestSerialization
            (
                DirectorInfo first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<DirectorInfo>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.Field);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void DirectorInfo_Serialization_1()
        {
            var director = new DirectorInfo();
            _TestSerialization (director);

            director = _GetDirectorInfo();
            director.Field = new Field();
            director.UserData = "User data";
            _TestSerialization (director);
        }

        [TestMethod]
        [Description ("Верифцикация")]
        public void DirectorInfo_Verify_1()
        {
            var director = new DirectorInfo();
            Assert.IsFalse (director.Verify (false));

            director = _GetDirectorInfo();
            Assert.IsTrue (director.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void DirectorInfo_ToXml_1()
        {
            var director = new DirectorInfo();
            Assert.AreEqual ("<director />", XmlUtility.SerializeShort (director));

            director = _GetDirectorInfo();
            Assert.AreEqual ("<director><title>Директор Иркутской областной государственной универсальной научной библиотеки</title><abbreviation>Директор ИОГУНБ</abbreviation><director-name>Сулейманова Л. А.</director-name></director>",
                XmlUtility.SerializeShort (director));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void DirectorInfo_ToJson_1()
        {
            var director = new DirectorInfo();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (director));

            director = _GetDirectorInfo();
            var expected = "{\"title\":\"\\u0414\\u0438\\u0440\\u0435\\u043A\\u0442\\u043E\\u0440 \\u0418\\u0440\\u043A\\u0443\\u0442\\u0441\\u043A\\u043E\\u0439 \\u043E\\u0431\\u043B\\u0430\\u0441\\u0442\\u043D\\u043E\\u0439 \\u0433\\u043E\\u0441\\u0443\\u0434\\u0430\\u0440\\u0441\\u0442\\u0432\\u0435\\u043D\\u043D\\u043E\\u0439 \\u0443\\u043D\\u0438\\u0432\\u0435\\u0440\\u0441\\u0430\\u043B\\u044C\\u043D\\u043E\\u0439 \\u043D\\u0430\\u0443\\u0447\\u043D\\u043E\\u0439 \\u0431\\u0438\\u0431\\u043B\\u0438\\u043E\\u0442\\u0435\\u043A\\u0438\",\"abbreviation\":\"\\u0414\\u0438\\u0440\\u0435\\u043A\\u0442\\u043E\\u0440 \\u0418\\u041E\\u0413\\u0423\\u041D\\u0411\",\"directorName\":\"\\u0421\\u0443\\u043B\\u0435\\u0439\\u043C\\u0430\\u043D\\u043E\\u0432\\u0430 \\u041B. \\u0410.\"}";
            var actual = JsonUtility.SerializeShort (director);
            Assert.AreEqual (expected, actual);
        }

        [TestMethod]
        [Description ("Текстовое представление")]
        public void DirectorInfo_ToString_1()
        {
            var director = new DirectorInfo();
            Assert.AreEqual ("Title: (null), DirectorName: (null)", director.ToString());

            director = _GetDirectorInfo();
            Assert.AreEqual ("Title: Директор Иркутской областной государственной универсальной научной библиотеки, DirectorName: Сулейманова Л. А.", director.ToString());
        }
    }
}
