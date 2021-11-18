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
    public sealed class CodenTest
    {
        private Coden _GetCoden()
        {
            return new Coden() { Identifier = "1234567" };
        }

        private void _Compare
            (
                Coden first,
                Coden second
            )
        {
            Assert.AreEqual (first.Identifier, second.Identifier);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Coden_Construction_1()
        {
            var coden = new Coden();
            Assert.IsNull (coden.Identifier);
            Assert.IsNull (coden.UserData);
        }

        [TestMethod]
        [Description ("Разбор текста")]
        public void Coden_ParseText_1()
        {
            const string identifier = "1234567";
            var coden = new Coden();
            coden.ParseText (identifier);
            Assert.AreEqual (identifier, coden.Identifier);
        }

        private void _TestSerialization
            (
                Coden first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<Coden>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void Coden_Serialization_1()
        {
            var coden = new Coden();
            _TestSerialization (coden);

            coden = _GetCoden();
            coden.UserData = "User data";
            _TestSerialization (coden);

        }

        [TestMethod]
        [Description ("Верификация")]
        public void Coden_Verification_1()
        {
            var coden = new Coden();
            Assert.IsFalse (coden.Verify (false));

            coden = _GetCoden();
            Assert.IsTrue (coden.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void Coden_ToXml_1()
        {
            var coden = new Coden();
            Assert.AreEqual ("<coden />", XmlUtility.SerializeShort (coden));

            coden = _GetCoden();
            Assert.AreEqual ("<coden><identifier>1234567</identifier></coden>",
                XmlUtility.SerializeShort (coden));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void Coden_ToJson_1()
        {
            var coden = new Coden();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (coden));

            coden = _GetCoden();
            Assert.AreEqual ("{\"identifier\":\"1234567\"}",
                JsonUtility.SerializeShort (coden));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void Coden_ToString_1()
        {
            var coden = new Coden();
            Assert.AreEqual ("(null)", coden.ToString());

            coden = _GetCoden();
            Assert.AreEqual ("1234567", coden.ToString());
        }

    }
}
