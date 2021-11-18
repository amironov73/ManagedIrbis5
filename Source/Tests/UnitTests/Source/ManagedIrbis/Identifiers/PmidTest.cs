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
    public sealed class PmidTest
    {
        private Pmid _GetPmid()
        {
            return new Pmid() { Identifier = "1234567" };
        }

        private void _Compare
            (
                Pmid first,
                Pmid second
            )
        {
            Assert.AreEqual (first.Identifier, second.Identifier);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Pmid_Construction_1()
        {
            var pmid = new Pmid();
            Assert.IsNull (pmid.Identifier);
            Assert.IsNull (pmid.UserData);
        }

        [TestMethod]
        [Description ("Разбор текста")]
        public void Pmid_ParseText_1()
        {
            const string identifier = "1234567";
            var pmid = new Pmid();
            pmid.ParseText (identifier);
            Assert.AreEqual (identifier, pmid.Identifier);
        }

        private void _TestSerialization
            (
                Pmid first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<Pmid>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void Pmid_Serialization_1()
        {
            var pmid = new Pmid();
            _TestSerialization (pmid);

            pmid = _GetPmid();
            pmid.UserData = "User data";
            _TestSerialization (pmid);

        }

        [TestMethod]
        [Description ("Верификация")]
        public void Pmid_Verification_1()
        {
            var pmid = new Pmid();
            Assert.IsFalse (pmid.Verify (false));

            pmid = _GetPmid();
            Assert.IsTrue (pmid.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void Pmid_ToXml_1()
        {
            var pmid = new Pmid();
            Assert.AreEqual ("<pmid />", XmlUtility.SerializeShort (pmid));

            pmid = _GetPmid();
            Assert.AreEqual ("<pmid><identifier>1234567</identifier></pmid>",
                XmlUtility.SerializeShort (pmid));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void Pmid_ToJson_1()
        {
            var pmid = new Pmid();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (pmid));

            pmid = _GetPmid();
            Assert.AreEqual ("{\"identifier\":\"1234567\"}",
                JsonUtility.SerializeShort (pmid));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void Pmid_ToString_1()
        {
            var pmid = new Pmid();
            Assert.AreEqual ("(null)", pmid.ToString());

            pmid = _GetPmid();
            Assert.AreEqual ("1234567", pmid.ToString());
        }

    }
}
