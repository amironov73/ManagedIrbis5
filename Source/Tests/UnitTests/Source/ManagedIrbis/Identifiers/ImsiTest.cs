// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis.Identifiers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Identifiers
{
    [TestClass]
    public sealed class ImsiTest
    {
        private Imsi _GetImsi()
        {
            return new Imsi() { Identifier = "1234567" };
        }

        private void _Compare
            (
                Imsi first,
                Imsi second
            )
        {
            Assert.AreEqual (first.Identifier, second.Identifier);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Imsi_Construction_1()
        {
            var imsi = new Imsi();
            Assert.IsNull (imsi.Identifier);
            Assert.IsNull (imsi.UserData);
        }

        [TestMethod]
        [Description ("Разбор текста")]
        public void Imsi_ParseText_1()
        {
            const string identifier = "1234567";
            var imsi = new Imsi();
            imsi.ParseText (identifier);
            Assert.AreEqual (identifier, imsi.Identifier);
        }

        private void _TestSerialization
            (
                Imsi first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<Imsi>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void Imsi_Serialization_1()
        {
            var imsi = new Imsi();
            _TestSerialization (imsi);

            imsi = _GetImsi();
            imsi.UserData = "User data";
            _TestSerialization (imsi);

        }

        [TestMethod]
        [Description ("Верификация")]
        public void Imsi_Verification_1()
        {
            var imsi = new Imsi();
            Assert.IsFalse (imsi.Verify (false));

            imsi = _GetImsi();
            Assert.IsTrue (imsi.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void Imsi_ToXml_1()
        {
            var imsi = new Imsi();
            Assert.AreEqual ("<imsi />", XmlUtility.SerializeShort (imsi));

            imsi = _GetImsi();
            Assert.AreEqual ("<imsi><identifier>1234567</identifier></imsi>",
                XmlUtility.SerializeShort (imsi));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void Imsi_ToJson_1()
        {
            var imsi = new Imsi();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (imsi));

            imsi = _GetImsi();
            Assert.AreEqual ("{\"identifier\":\"1234567\"}",
                JsonUtility.SerializeShort (imsi));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void Imsi_ToString_1()
        {
            var imsi = new Imsi();
            Assert.AreEqual ("(null)", imsi.ToString());

            imsi = _GetImsi();
            Assert.AreEqual ("1234567", imsi.ToString());
        }

    }
}
