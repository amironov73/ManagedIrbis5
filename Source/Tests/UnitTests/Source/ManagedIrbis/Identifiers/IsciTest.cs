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
    public sealed class IsciTest
    {
        private Isci _GetIsci()
        {
            return new Isci() { Identifier = "1234567" };
        }

        private void _Compare
            (
                Isci first,
                Isci second
            )
        {
            Assert.AreEqual (first.Identifier, second.Identifier);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Isci_Construction_1()
        {
            var isci = new Isci();
            Assert.IsNull (isci.Identifier);
            Assert.IsNull (isci.UserData);
        }

        [TestMethod]
        [Description ("Разбор текста")]
        public void Isci_ParseText_1()
        {
            const string identifier = "1234567";
            var isci = new Isci();
            isci.ParseText (identifier);
            Assert.AreEqual (identifier, isci.Identifier);
        }

        private void _TestSerialization
            (
                Isci first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<Isci>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void Isci_Serialization_1()
        {
            var isci = new Isci();
            _TestSerialization (isci);

            isci = _GetIsci();
            isci.UserData = "User data";
            _TestSerialization (isci);

        }

        [TestMethod]
        [Description ("Верификация")]
        public void Isci_Verification_1()
        {
            var isci = new Isci();
            Assert.IsFalse (isci.Verify (false));

            isci = _GetIsci();
            Assert.IsTrue (isci.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void Isci_ToXml_1()
        {
            var isci = new Isci();
            Assert.AreEqual ("<isci />", XmlUtility.SerializeShort (isci));

            isci = _GetIsci();
            Assert.AreEqual ("<isci><identifier>1234567</identifier></isci>",
                XmlUtility.SerializeShort (isci));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void Isci_ToJson_1()
        {
            var isci = new Isci();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (isci));

            isci = _GetIsci();
            Assert.AreEqual ("{\"identifier\":\"1234567\"}",
                JsonUtility.SerializeShort (isci));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void Isci_ToString_1()
        {
            var isci = new Isci();
            Assert.AreEqual ("(null)", isci.ToString());

            isci = _GetIsci();
            Assert.AreEqual ("1234567", isci.ToString());
        }

    }
}
