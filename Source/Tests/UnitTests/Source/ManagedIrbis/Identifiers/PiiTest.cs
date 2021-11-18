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
    public sealed class PiiTest
    {
        private Pii _GetPii()
        {
            return new Pii() { Identifier = "1234567" };
        }

        private void _Compare
            (
                Pii first,
                Pii second
            )
        {
            Assert.AreEqual (first.Identifier, second.Identifier);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Pii_Construction_1()
        {
            var pii = new Pii();
            Assert.IsNull (pii.Identifier);
            Assert.IsNull (pii.UserData);
        }

        [TestMethod]
        [Description ("Разбор текста")]
        public void Pii_ParseText_1()
        {
            const string identifier = "1234567";
            var pii = new Pii();
            pii.ParseText (identifier);
            Assert.AreEqual (identifier, pii.Identifier);
        }

        private void _TestSerialization
            (
                Pii first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<Pii>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void Pii_Serialization_1()
        {
            var pii = new Pii();
            _TestSerialization (pii);

            pii = _GetPii();
            pii.UserData = "User data";
            _TestSerialization (pii);

        }

        [TestMethod]
        [Description ("Верификация")]
        public void Pii_Verification_1()
        {
            var pii = new Pii();
            Assert.IsFalse (pii.Verify (false));

            pii = _GetPii();
            Assert.IsTrue (pii.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void Pii_ToXml_1()
        {
            var pii = new Pii();
            Assert.AreEqual ("<pii />", XmlUtility.SerializeShort (pii));

            pii = _GetPii();
            Assert.AreEqual ("<pii><identifier>1234567</identifier></pii>",
                XmlUtility.SerializeShort (pii));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void Pii_ToJson_1()
        {
            var pii = new Pii();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (pii));

            pii = _GetPii();
            Assert.AreEqual ("{\"identifier\":\"1234567\"}",
                JsonUtility.SerializeShort (pii));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void Pii_ToString_1()
        {
            var pii = new Pii();
            Assert.AreEqual ("(null)", pii.ToString());

            pii = _GetPii();
            Assert.AreEqual ("1234567", pii.ToString());
        }

    }
}
