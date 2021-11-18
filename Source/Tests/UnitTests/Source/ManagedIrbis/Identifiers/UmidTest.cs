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
    public sealed class UmidTest
    {
        private Umid _GetUmid()
        {
            return new Umid() { Identifier = "1234567" };
        }

        private void _Compare
            (
                Umid first,
                Umid second
            )
        {
            Assert.AreEqual (first.Identifier, second.Identifier);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Umid_Construction_1()
        {
            var umid = new Umid();
            Assert.IsNull (umid.Identifier);
            Assert.IsNull (umid.UserData);
        }

        [TestMethod]
        [Description ("Разбор текста")]
        public void Umid_ParseText_1()
        {
            const string identifier = "1234567";
            var umid = new Umid();
            umid.ParseText (identifier);
            Assert.AreEqual (identifier, umid.Identifier);
        }

        private void _TestSerialization
            (
                Umid first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<Umid>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void Umid_Serialization_1()
        {
            var umid = new Umid();
            _TestSerialization (umid);

            umid = _GetUmid();
            umid.UserData = "User data";
            _TestSerialization (umid);

        }

        [TestMethod]
        [Description ("Верификация")]
        public void Umid_Verification_1()
        {
            var umid = new Umid();
            Assert.IsFalse (umid.Verify (false));

            umid = _GetUmid();
            Assert.IsTrue (umid.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void Umid_ToXml_1()
        {
            var umid = new Umid();
            Assert.AreEqual ("<umid />", XmlUtility.SerializeShort (umid));

            umid = _GetUmid();
            Assert.AreEqual ("<umid><identifier>1234567</identifier></umid>",
                XmlUtility.SerializeShort (umid));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void Umid_ToJson_1()
        {
            var umid = new Umid();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (umid));

            umid = _GetUmid();
            Assert.AreEqual ("{\"identifier\":\"1234567\"}",
                JsonUtility.SerializeShort (umid));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void Umid_ToString_1()
        {
            var umid = new Umid();
            Assert.AreEqual ("(null)", umid.ToString());

            umid = _GetUmid();
            Assert.AreEqual ("1234567", umid.ToString());
        }

    }
}
