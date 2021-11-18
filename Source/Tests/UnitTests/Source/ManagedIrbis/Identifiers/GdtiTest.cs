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
    public sealed class GdtiTest
    {
        private Gdti _GetGdti()
        {
            return new Gdti() { Identifier = "1234567" };
        }

        private void _Compare
            (
                Gdti first,
                Gdti second
            )
        {
            Assert.AreEqual (first.Identifier, second.Identifier);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Gdti_Construction_1()
        {
            var gdti = new Gdti();
            Assert.IsNull (gdti.Identifier);
            Assert.IsNull (gdti.UserData);
        }

        [TestMethod]
        [Description ("Разбор текста")]
        public void Gdti_ParseText_1()
        {
            const string identifier = "1234567";
            var gdti = new Gdti();
            gdti.ParseText (identifier);
            Assert.AreEqual (identifier, gdti.Identifier);
        }

        private void _TestSerialization
            (
                Gdti first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<Gdti>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void Gdti_Serialization_1()
        {
            var gdti = new Gdti();
            _TestSerialization (gdti);

            gdti = _GetGdti();
            gdti.UserData = "User data";
            _TestSerialization (gdti);

        }

        [TestMethod]
        [Description ("Верификация")]
        public void Gdti_Verification_1()
        {
            var gdti = new Gdti();
            Assert.IsFalse (gdti.Verify (false));

            gdti = _GetGdti();
            Assert.IsTrue (gdti.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void Gdti_ToXml_1()
        {
            var gdti = new Gdti();
            Assert.AreEqual ("<gdti />", XmlUtility.SerializeShort (gdti));

            gdti = _GetGdti();
            Assert.AreEqual ("<gdti><identifier>1234567</identifier></gdti>",
                XmlUtility.SerializeShort (gdti));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void Gdti_ToJson_1()
        {
            var gdti = new Gdti();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (gdti));

            gdti = _GetGdti();
            Assert.AreEqual ("{\"identifier\":\"1234567\"}",
                JsonUtility.SerializeShort (gdti));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void Gdti_ToString_1()
        {
            var gdti = new Gdti();
            Assert.AreEqual ("(null)", gdti.ToString());

            gdti = _GetGdti();
            Assert.AreEqual ("1234567", gdti.ToString());
        }
    }
}
