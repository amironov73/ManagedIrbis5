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
    public sealed class IstcTest
    {
        private Istc _GetIstc()
        {
            return new Istc() { Identifier = "1234567" };
        }

        private void _Compare
            (
                Istc first,
                Istc second
            )
        {
            Assert.AreEqual (first.Identifier, second.Identifier);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Istc_Construction_1()
        {
            var istc = new Istc();
            Assert.IsNull (istc.Identifier);
            Assert.IsNull (istc.UserData);
        }

        [TestMethod]
        [Description ("Разбор текста")]
        public void Istc_ParseText_1()
        {
            const string identifier = "1234567";
            var istc = new Istc();
            istc.ParseText (identifier);
            Assert.AreEqual (identifier, istc.Identifier);
        }

        private void _TestSerialization
            (
                Istc first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<Istc>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void Istc_Serialization_1()
        {
            var istc = new Istc();
            _TestSerialization (istc);

            istc = _GetIstc();
            istc.UserData = "User data";
            _TestSerialization (istc);

        }

        [TestMethod]
        [Description ("Верификация")]
        public void Istc_Verification_1()
        {
            var istc = new Istc();
            Assert.IsFalse (istc.Verify (false));

            istc = _GetIstc();
            Assert.IsTrue (istc.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void Istc_ToXml_1()
        {
            var istc = new Istc();
            Assert.AreEqual ("<istc />", XmlUtility.SerializeShort (istc));

            istc = _GetIstc();
            Assert.AreEqual ("<istc><identifier>1234567</identifier></istc>",
                XmlUtility.SerializeShort (istc));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void Istc_ToJson_1()
        {
            var istc = new Istc();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (istc));

            istc = _GetIstc();
            Assert.AreEqual ("{\"identifier\":\"1234567\"}",
                JsonUtility.SerializeShort (istc));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void Istc_ToString_1()
        {
            var istc = new Istc();
            Assert.AreEqual ("(null)", istc.ToString());

            istc = _GetIstc();
            Assert.AreEqual ("1234567", istc.ToString());
        }

    }
}
