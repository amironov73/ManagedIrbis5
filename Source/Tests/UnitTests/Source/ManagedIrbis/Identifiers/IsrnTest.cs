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
    public sealed class IsrnTest
    {
        private Isrn _GetIsrn()
        {
            return new Isrn() { Identifier = "1234567" };
        }

        private void _Compare
            (
                Isrn first,
                Isrn second
            )
        {
            Assert.AreEqual (first.Identifier, second.Identifier);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Isrn_Construction_1()
        {
            var isrn = new Isrn();
            Assert.IsNull (isrn.Identifier);
            Assert.IsNull (isrn.UserData);
        }

        [TestMethod]
        [Description ("Разбор текста")]
        public void Isrn_ParseText_1()
        {
            const string identifier = "1234567";
            var isrn = new Isrn();
            isrn.ParseText (identifier);
            Assert.AreEqual (identifier, isrn.Identifier);
        }

        private void _TestSerialization
            (
                Isrn first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<Isrn>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void Isrn_Serialization_1()
        {
            var isrn = new Isrn();
            _TestSerialization (isrn);

            isrn = _GetIsrn();
            isrn.UserData = "User data";
            _TestSerialization (isrn);

        }

        [TestMethod]
        [Description ("Верификация")]
        public void Isrn_Verification_1()
        {
            var isrn = new Isrn();
            Assert.IsFalse (isrn.Verify (false));

            isrn = _GetIsrn();
            Assert.IsTrue (isrn.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void Isrn_ToXml_1()
        {
            var isrn = new Isrn();
            Assert.AreEqual ("<isrn />", XmlUtility.SerializeShort (isrn));

            isrn = _GetIsrn();
            Assert.AreEqual ("<isrn><identifier>1234567</identifier></isrn>",
                XmlUtility.SerializeShort (isrn));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void Isrn_ToJson_1()
        {
            var isrn = new Isrn();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (isrn));

            isrn = _GetIsrn();
            Assert.AreEqual ("{\"identifier\":\"1234567\"}",
                JsonUtility.SerializeShort (isrn));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void Isrn_ToString_1()
        {
            var isrn = new Isrn();
            Assert.AreEqual ("(null)", isrn.ToString());

            isrn = _GetIsrn();
            Assert.AreEqual ("1234567", isrn.ToString());
        }

    }
}
