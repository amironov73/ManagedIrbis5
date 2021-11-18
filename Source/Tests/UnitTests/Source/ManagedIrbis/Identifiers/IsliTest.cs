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
    public sealed class IsliTest
    {
        private Isli _GetIsli()
        {
            return new Isli() { Identifier = "1234567" };
        }

        private void _Compare
            (
                Isli first,
                Isli second
            )
        {
            Assert.AreEqual (first.Identifier, second.Identifier);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Isli_Construction_1()
        {
            var isli = new Isli();
            Assert.IsNull (isli.Identifier);
            Assert.IsNull (isli.UserData);
        }

        [TestMethod]
        [Description ("Разбор текста")]
        public void Isli_ParseText_1()
        {
            const string identifier = "1234567";
            var isli = new Isli();
            isli.ParseText (identifier);
            Assert.AreEqual (identifier, isli.Identifier);
        }

        private void _TestSerialization
            (
                Isli first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<Isli>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void Isli_Serialization_1()
        {
            var isli = new Isli();
            _TestSerialization (isli);

            isli = _GetIsli();
            isli.UserData = "User data";
            _TestSerialization (isli);

        }

        [TestMethod]
        [Description ("Верификация")]
        public void Isli_Verification_1()
        {
            var isli = new Isli();
            Assert.IsFalse (isli.Verify (false));

            isli = _GetIsli();
            Assert.IsTrue (isli.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void Isli_ToXml_1()
        {
            var isli = new Isli();
            Assert.AreEqual ("<isli />", XmlUtility.SerializeShort (isli));

            isli = _GetIsli();
            Assert.AreEqual ("<isli><identifier>1234567</identifier></isli>",
                XmlUtility.SerializeShort (isli));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void Isli_ToJson_1()
        {
            var isli = new Isli();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (isli));

            isli = _GetIsli();
            Assert.AreEqual ("{\"identifier\":\"1234567\"}",
                JsonUtility.SerializeShort (isli));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void Isli_ToString_1()
        {
            var isli = new Isli();
            Assert.AreEqual ("(null)", isli.ToString());

            isli = _GetIsli();
            Assert.AreEqual ("1234567", isli.ToString());
        }

    }
}
