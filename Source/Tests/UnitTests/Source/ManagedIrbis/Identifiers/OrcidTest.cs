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
    public sealed class OrcidTest
    {
        private Orcid _GetOrcid()
        {
            return new Orcid() { Identifier = "1234567" };
        }

        private void _Compare
            (
                Orcid first,
                Orcid second
            )
        {
            Assert.AreEqual (first.Identifier, second.Identifier);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Orcid_Construction_1()
        {
            var orcid = new Orcid();
            Assert.IsNull (orcid.Identifier);
            Assert.IsNull (orcid.UserData);
        }

        [TestMethod]
        [Description ("Разбор текста")]
        public void Orcid_ParseText_1()
        {
            const string identifier = "1234567";
            var orcid = new Orcid();
            orcid.ParseText (identifier);
            Assert.AreEqual (identifier, orcid.Identifier);
        }

        private void _TestSerialization
            (
                Orcid first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<Orcid>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void Orcid_Serialization_1()
        {
            var orcid = new Orcid();
            _TestSerialization (orcid);

            orcid = _GetOrcid();
            orcid.UserData = "User data";
            _TestSerialization (orcid);

        }

        [TestMethod]
        [Description ("Верификация")]
        public void Orcid_Verification_1()
        {
            var orcid = new Orcid();
            Assert.IsFalse (orcid.Verify (false));

            orcid = _GetOrcid();
            Assert.IsTrue (orcid.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void Orcid_ToXml_1()
        {
            var orcid = new Orcid();
            Assert.AreEqual ("<orcid />", XmlUtility.SerializeShort (orcid));

            orcid = _GetOrcid();
            Assert.AreEqual ("<orcid><identifier>1234567</identifier></orcid>",
                XmlUtility.SerializeShort (orcid));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void Orcid_ToJson_1()
        {
            var orcid = new Orcid();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (orcid));

            orcid = _GetOrcid();
            Assert.AreEqual ("{\"identifier\":\"1234567\"}",
                JsonUtility.SerializeShort (orcid));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void Orcid_ToString_1()
        {
            var orcid = new Orcid();
            Assert.AreEqual ("(null)", orcid.ToString());

            orcid = _GetOrcid();
            Assert.AreEqual ("1234567", orcid.ToString());
        }

    }
}
