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
    public sealed class IsadnTest
    {
        private Isadn _GetIsadn()
        {
            return new Isadn() { Identifier = "1234567" };
        }

        private void _Compare
            (
                Isadn first,
                Isadn second
            )
        {
            Assert.AreEqual (first.Identifier, second.Identifier);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Isadn_Construction_1()
        {
            var isadn = new Isadn();
            Assert.IsNull (isadn.Identifier);
            Assert.IsNull (isadn.UserData);
        }

        [TestMethod]
        [Description ("Разбор текста")]
        public void Isadn_ParseText_1()
        {
            const string identifier = "1234567";
            var isadn = new Isadn();
            isadn.ParseText (identifier);
            Assert.AreEqual (identifier, isadn.Identifier);
        }

        private void _TestSerialization
            (
                Isadn first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<Isadn>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void Isadn_Serialization_1()
        {
            var isadn = new Isadn();
            _TestSerialization (isadn);

            isadn = _GetIsadn();
            isadn.UserData = "User data";
            _TestSerialization (isadn);

        }

        [TestMethod]
        [Description ("Верификация")]
        public void Isadn_Verification_1()
        {
            var isadn = new Isadn();
            Assert.IsFalse (isadn.Verify (false));

            isadn = _GetIsadn();
            Assert.IsTrue (isadn.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void Isadn_ToXml_1()
        {
            var isadn = new Isadn();
            Assert.AreEqual ("<isadn />", XmlUtility.SerializeShort (isadn));

            isadn = _GetIsadn();
            Assert.AreEqual ("<isadn><identifier>1234567</identifier></isadn>",
                XmlUtility.SerializeShort (isadn));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void Isadn_ToJson_1()
        {
            var isadn = new Isadn();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (isadn));

            isadn = _GetIsadn();
            Assert.AreEqual ("{\"identifier\":\"1234567\"}",
                JsonUtility.SerializeShort (isadn));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void Isadn_ToString_1()
        {
            var isadn = new Isadn();
            Assert.AreEqual ("(null)", isadn.ToString());

            isadn = _GetIsadn();
            Assert.AreEqual ("1234567", isadn.ToString());
        }

    }
}
