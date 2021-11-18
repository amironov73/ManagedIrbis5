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
    public sealed class XriTest
    {
        private Xri _GetXri()
        {
            return new Xri() { Identifier = "1234567" };
        }

        private void _Compare
            (
                Xri first,
                Xri second
            )
        {
            Assert.AreEqual (first.Identifier, second.Identifier);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Xri_Construction_1()
        {
            var xri = new Xri();
            Assert.IsNull (xri.Identifier);
            Assert.IsNull (xri.UserData);
        }

        [TestMethod]
        [Description ("Разбор текста")]
        public void Xri_ParseText_1()
        {
            const string identifier = "1234567";
            var xri = new Xri();
            xri.ParseText (identifier);
            Assert.AreEqual (identifier, xri.Identifier);
        }

        private void _TestSerialization
            (
                Xri first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<Xri>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void Xri_Serialization_1()
        {
            var xri = new Xri();
            _TestSerialization (xri);

            xri = _GetXri();
            xri.UserData = "User data";
            _TestSerialization (xri);

        }

        [TestMethod]
        [Description ("Верификация")]
        public void Xri_Verification_1()
        {
            var xri = new Xri();
            Assert.IsFalse (xri.Verify (false));

            xri = _GetXri();
            Assert.IsTrue (xri.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void Xri_ToXml_1()
        {
            var xri = new Xri();
            Assert.AreEqual ("<xri />", XmlUtility.SerializeShort (xri));

            xri = _GetXri();
            Assert.AreEqual ("<xri><identifier>1234567</identifier></xri>",
                XmlUtility.SerializeShort (xri));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void Xri_ToJson_1()
        {
            var xri = new Xri();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (xri));

            xri = _GetXri();
            Assert.AreEqual ("{\"identifier\":\"1234567\"}",
                JsonUtility.SerializeShort (xri));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void Xri_ToString_1()
        {
            var xri = new Xri();
            Assert.AreEqual ("(null)", xri.ToString());

            xri = _GetXri();
            Assert.AreEqual ("1234567", xri.ToString());
        }

    }
}
