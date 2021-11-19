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
    public sealed class IsilTest
    {
        private Isil _GetIsil()
        {
            return new Isil() { Prefix = "RU", Identifier = "10010033" };
        }

        private void _Compare
            (
                Isil first,
                Isil second
            )
        {
            Assert.AreEqual (first.Prefix, second.Prefix);
            Assert.AreEqual (first.Identifier, second.Identifier);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Isil_Construction_1()
        {
            var isil = new Isil();
            Assert.IsNull (isil.Prefix);
            Assert.IsNull (isil.Identifier);
            Assert.IsNull (isil.UserData);
        }

        [TestMethod]
        [Description ("Разбор текста")]
        public void Isil_ParseText_1()
        {
            var isil = new Isil();
            isil.ParseText ("RU-10010033");
            Assert.AreEqual ("RU", isil.Prefix);
            Assert.AreEqual ("10010033", isil.Identifier);
        }

        private void _TestSerialization
            (
                Isil first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<Isil>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void Isil_Serialization_1()
        {
            var isil = new Isil();
            _TestSerialization (isil);

            isil = _GetIsil();
            isil.UserData = "User data";
            _TestSerialization (isil);

        }

        [TestMethod]
        [Description ("Верификация")]
        public void Isil_Verification_1()
        {
            var isil = new Isil();
            Assert.IsFalse (isil.Verify (false));

            isil = _GetIsil();
            Assert.IsTrue (isil.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void Isil_ToXml_1()
        {
            var isil = new Isil();
            Assert.AreEqual ("<isil />", XmlUtility.SerializeShort (isil));

            isil = _GetIsil();
            Assert.AreEqual ("<isil><prefix>RU</prefix><identifier>10010033</identifier></isil>",
                XmlUtility.SerializeShort (isil));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void Isil_ToJson_1()
        {
            var isil = new Isil();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (isil));

            isil = _GetIsil();
            Assert.AreEqual ("{\"prefix\":\"RU\",\"identifier\":\"10010033\"}",
                JsonUtility.SerializeShort (isil));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void Isil_ToString_1()
        {
            var isil = new Isil();
            Assert.AreEqual ("-", isil.ToString());

            isil = _GetIsil();
            Assert.AreEqual ("RU-10010033", isil.ToString());
        }

    }
}
