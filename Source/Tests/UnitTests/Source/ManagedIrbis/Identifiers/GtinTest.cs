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
    public sealed class GtinTest
    {
        private Gtin _GetGtin()
        {
            return new Gtin() { Identifier = "1234567" };
        }

        private void _Compare
            (
                Gtin first,
                Gtin second
            )
        {
            Assert.AreEqual (first.Identifier, second.Identifier);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Gtin_Construction_1()
        {
            var gtin = new Gtin();
            Assert.IsNull (gtin.Identifier);
            Assert.IsNull (gtin.UserData);
        }

        [TestMethod]
        [Description ("Разбор текста")]
        public void Gtin_ParseText_1()
        {
            const string identifier = "1234567";
            var gtin = new Gtin();
            gtin.ParseText (identifier);
            Assert.AreEqual (identifier, gtin.Identifier);
        }

        private void _TestSerialization
            (
                Gtin first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<Gtin>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void Gtin_Serialization_1()
        {
            var gtin = new Gtin();
            _TestSerialization (gtin);

            gtin = _GetGtin();
            gtin.UserData = "User data";
            _TestSerialization (gtin);

        }

        [TestMethod]
        [Description ("Верификация")]
        public void Gtin_Verification_1()
        {
            var gtin = new Gtin();
            Assert.IsFalse (gtin.Verify (false));

            gtin = _GetGtin();
            Assert.IsTrue (gtin.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void Gtin_ToXml_1()
        {
            var gtin = new Gtin();
            Assert.AreEqual ("<gtin />", XmlUtility.SerializeShort (gtin));

            gtin = _GetGtin();
            Assert.AreEqual ("<gtin><identifier>1234567</identifier></gtin>",
                XmlUtility.SerializeShort (gtin));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void Gtin_ToJson_1()
        {
            var gtin = new Gtin();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (gtin));

            gtin = _GetGtin();
            Assert.AreEqual ("{\"identifier\":\"1234567\"}",
                JsonUtility.SerializeShort (gtin));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void Gtin_ToString_1()
        {
            var gtin = new Gtin();
            Assert.AreEqual ("(null)", gtin.ToString());

            gtin = _GetGtin();
            Assert.AreEqual ("1234567", gtin.ToString());
        }

    }
}
