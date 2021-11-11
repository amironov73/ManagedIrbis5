// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis.Identifiers;

#nullable enable

namespace UnitTests.ManagedIrbis.Identifiers
{
    [TestClass]
    public sealed class NgroeTest
    {
        private Ngroe _GetNgroe() => new ()
        {
            Type = "КН",
            Form = "П",
            Year = "19",
            Number = "123456"
        };

        private void _Compare
            (
                Ngroe first,
                Ngroe second
            )
        {
            Assert.AreEqual (first.Type, second.Type);
            Assert.AreEqual (first.Form, second.Form);
            Assert.AreEqual (first.Year, second.Year);
            Assert.AreEqual (first.Number, second.Number);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Ngroe_Construction_1()
        {
            var ngroe = new Ngroe();
            Assert.IsNull (ngroe.Type);
            Assert.IsNull (ngroe.Form);
            Assert.IsNull (ngroe.Year);
            Assert.IsNull (ngroe.Number);
            Assert.IsNull (ngroe.UserData);
        }

        [TestMethod]
        [Description ("Разбор строки: пустая строка")]
        public void Ngroe_Parse_1()
        {
            var ngroe = Ngroe.Parse (ReadOnlySpan<char>.Empty);
            Assert.IsNull (ngroe.Type);
            Assert.IsNull (ngroe.Form);
            Assert.IsNull (ngroe.Year);
            Assert.IsNull (ngroe.Number);
            Assert.IsNull (ngroe.UserData);
        }

        [TestMethod]
        [Description ("Разбор строки: только тип")]
        public void Ngroe_Parse_2()
        {
            var ngroe = Ngroe.Parse ("КН");
            Assert.AreEqual ("КН", ngroe.Type);
            Assert.IsNull (ngroe.Form);
            Assert.IsNull (ngroe.Year);
            Assert.IsNull (ngroe.Number);
            Assert.IsNull (ngroe.UserData);
        }

        [TestMethod]
        [Description ("Разбор строки: только тип и форма")]
        public void Ngroe_Parse_3()
        {
            var ngroe = Ngroe.Parse ("КН-П");
            Assert.AreEqual ("КН", ngroe.Type);
            Assert.AreEqual ("П", ngroe.Form);
            Assert.IsNull (ngroe.Year);
            Assert.IsNull (ngroe.Number);
            Assert.IsNull (ngroe.UserData);
        }

        [TestMethod]
        [Description ("Разбор строки: только тип, форма и год")]
        public void Ngroe_Parse_4()
        {
            var ngroe = Ngroe.Parse ("КН-П-21");
            Assert.AreEqual ("КН", ngroe.Type);
            Assert.AreEqual ("П", ngroe.Form);
            Assert.AreEqual ("21", ngroe.Year);
            Assert.IsNull (ngroe.Number);
            Assert.IsNull (ngroe.UserData);
        }

        [TestMethod]
        [Description ("Разбор строки: есть все компоненты")]
        public void Ngroe_Parse_5()
        {
            var ngroe = Ngroe.Parse ("КН-П-21-123456");
            Assert.AreEqual ("КН", ngroe.Type);
            Assert.AreEqual ("П", ngroe.Form);
            Assert.AreEqual ("21", ngroe.Year);
            Assert.AreEqual ("123456", ngroe.Number);
            Assert.IsNull (ngroe.UserData);
        }

        [TestMethod]
        [Description ("Разбор строки: лишние пробелы")]
        public void Ngroe_Parse_6()
        {
            var ngroe = Ngroe.Parse (" КН - П - 21 - 123456 ");
            Assert.AreEqual ("КН", ngroe.Type);
            Assert.AreEqual ("П", ngroe.Form);
            Assert.AreEqual ("21", ngroe.Year);
            Assert.AreEqual ("123456", ngroe.Number);
            Assert.IsNull (ngroe.UserData);
        }

        [TestMethod]
        [Description ("Произвольные пользовательские данные")]
        public void Ngroe_UserData_1()
        {
            const string userData = "User data";
            var ngroe = new Ngroe();
            Assert.IsNull (ngroe.UserData);
            ngroe.UserData = userData;
            Assert.AreEqual (userData, ngroe.UserData);
        }

        private void _TestSerialization
            (
                Ngroe first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<Ngroe>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void Ngroe_Serialization_1()
        {
            var ngroe = new Ngroe();
            _TestSerialization (ngroe);

            ngroe = _GetNgroe();
            ngroe.UserData = "User data";
            _TestSerialization (ngroe);
        }

        [TestMethod]
        [Description ("Верифцикация")]
        public void Ngroe_Verify_1()
        {
            var ngroe = new Ngroe();
            Assert.IsFalse (ngroe.Verify (false));

            ngroe = _GetNgroe();
            Assert.IsTrue (ngroe.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void Ngroe_ToXml_1()
        {
            var ngroe = new Ngroe();
            Assert.AreEqual ("<ngroe />", XmlUtility.SerializeShort (ngroe));

            ngroe = _GetNgroe();
            Assert.AreEqual ("<ngroe><type>КН</type><form>П</form><year>19</year><number>123456</number></ngroe>",
                XmlUtility.SerializeShort (ngroe));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void Ngroe_ToJson_1()
        {
            var ngroe = new Ngroe();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (ngroe));

            ngroe = _GetNgroe();
            var expected = "{\"type\":\"\\u041A\\u041D\",\"form\":\"\\u041F\",\"year\":\"19\",\"number\":\"123456\"}";
            var actual = JsonUtility.SerializeShort (ngroe);
            Assert.AreEqual (expected, actual);
        }

        [TestMethod]
        [Description ("Текстовое представление")]
        public void Ngroe_ToString_1()
        {
            var ngroe = new Ngroe();
            Assert.AreEqual ("(null)-(null)-(null)-(null)", ngroe.ToString());

            ngroe = _GetNgroe();
            Assert.AreEqual ("КН-П-19-123456", ngroe.ToString());
        }

    }
}
