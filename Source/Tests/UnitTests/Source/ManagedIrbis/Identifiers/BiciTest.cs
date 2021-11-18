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
    public sealed class BiciTest
    {
        private Bici _GetBici()
        {
            return new Bici() { Identifier = "0521416205(1993)(10;EAAWL;234-261)2.2.TX;1-1" };
        }

        private void _Compare
            (
                Bici first,
                Bici second
            )
        {
            Assert.AreEqual (first.Identifier, second.Identifier);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Bici_Construction_1()
        {
            var bici = new Bici();
            Assert.IsNull (bici.Identifier);
            Assert.IsNull (bici.UserData);
        }

        [TestMethod]
        [Description ("Разбор текста")]
        public void Bici_ParseText_1()
        {
            const string identifier = "0521416205(1993)(10;EAAWL;234-261)2.2.TX;1-1";
            var bici = new Bici();
            bici.ParseText (identifier);
            Assert.AreEqual (identifier, bici.Identifier);
        }

        private void _TestSerialization
            (
                Bici first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<Bici>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void Bici_Serialization_1()
        {
            var bici = new Bici();
            _TestSerialization (bici);

            bici = _GetBici();
            bici.UserData = "User data";
            _TestSerialization (bici);

        }

        [TestMethod]
        [Description ("Верификация")]
        public void Bici_Verification_1()
        {
            var bici = new Bici();
            Assert.IsFalse (bici.Verify (false));

            bici = _GetBici();
            Assert.IsTrue (bici.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void Bici_ToXml_1()
        {
            var bici = new Bici();
            Assert.AreEqual ("<bici />", XmlUtility.SerializeShort (bici));

            bici = _GetBici();
            Assert.AreEqual ("<bici><identifier>0521416205(1993)(10;EAAWL;234-261)2.2.TX;1-1</identifier></bici>",
                XmlUtility.SerializeShort (bici));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void Bici_ToJson_1()
        {
            var bici = new Bici();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (bici));

            bici = _GetBici();
            Assert.AreEqual ("{\"identifier\":\"0521416205(1993)(10;EAAWL;234-261)2.2.TX;1-1\"}",
                JsonUtility.SerializeShort (bici));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void Bici_ToString_1()
        {
            var bici = new Bici();
            Assert.AreEqual ("(null)", bici.ToString());

            bici = _GetBici();
            Assert.AreEqual ("0521416205(1993)(10;EAAWL;234-261)2.2.TX;1-1", bici.ToString());
        }

    }
}
