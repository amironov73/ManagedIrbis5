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
    public sealed class GsrnTest
    {
        private Gsrn _GetGsrn()
        {
            return new Gsrn() { Identifier = "1234567" };
        }

        private void _Compare
            (
                Gsrn first,
                Gsrn second
            )
        {
            Assert.AreEqual (first.Identifier, second.Identifier);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Gsrn_Construction_1()
        {
            var gsrn = new Gsrn();
            Assert.IsNull (gsrn.Identifier);
            Assert.IsNull (gsrn.UserData);
        }

        [TestMethod]
        [Description ("Разбор текста")]
        public void Gsrn_ParseText_1()
        {
            const string identifier = "1234567";
            var gsrn = new Gsrn();
            gsrn.ParseText (identifier);
            Assert.AreEqual (identifier, gsrn.Identifier);
        }

        private void _TestSerialization
            (
                Gsrn first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<Gsrn>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void Gsrn_Serialization_1()
        {
            var gsrn = new Gsrn();
            _TestSerialization (gsrn);

            gsrn = _GetGsrn();
            gsrn.UserData = "User data";
            _TestSerialization (gsrn);

        }

        [TestMethod]
        [Description ("Верификация")]
        public void Gsrn_Verification_1()
        {
            var gsrn = new Gsrn();
            Assert.IsFalse (gsrn.Verify (false));

            gsrn = _GetGsrn();
            Assert.IsTrue (gsrn.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void Gsrn_ToXml_1()
        {
            var gsrn = new Gsrn();
            Assert.AreEqual ("<gsrn />", XmlUtility.SerializeShort (gsrn));

            gsrn = _GetGsrn();
            Assert.AreEqual ("<gsrn><identifier>1234567</identifier></gsrn>",
                XmlUtility.SerializeShort (gsrn));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void Gsrn_ToJson_1()
        {
            var gsrn = new Gsrn();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (gsrn));

            gsrn = _GetGsrn();
            Assert.AreEqual ("{\"identifier\":\"1234567\"}",
                JsonUtility.SerializeShort (gsrn));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void Gsrn_ToString_1()
        {
            var gsrn = new Gsrn();
            Assert.AreEqual ("(null)", gsrn.ToString());

            gsrn = _GetGsrn();
            Assert.AreEqual ("1234567", gsrn.ToString());
        }

    }
}
