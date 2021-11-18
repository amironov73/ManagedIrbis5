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
    public sealed class IswcTest
    {
        private Iswc _GetIswc()
        {
            return new Iswc() { Identifier = "1234567" };
        }

        private void _Compare
            (
                Iswc first,
                Iswc second
            )
        {
            Assert.AreEqual (first.Identifier, second.Identifier);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Iswc_Construction_1()
        {
            var iswc = new Iswc();
            Assert.IsNull (iswc.Identifier);
            Assert.IsNull (iswc.UserData);
        }

        [TestMethod]
        [Description ("Разбор текста")]
        public void Iswc_ParseText_1()
        {
            const string identifier = "1234567";
            var iswc = new Iswc();
            iswc.ParseText (identifier);
            Assert.AreEqual (identifier, iswc.Identifier);
        }

        private void _TestSerialization
            (
                Iswc first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<Iswc>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void Iswc_Serialization_1()
        {
            var iswc = new Iswc();
            _TestSerialization (iswc);

            iswc = _GetIswc();
            iswc.UserData = "User data";
            _TestSerialization (iswc);

        }

        [TestMethod]
        [Description ("Верификация")]
        public void Iswc_Verification_1()
        {
            var iswc = new Iswc();
            Assert.IsFalse (iswc.Verify (false));

            iswc = _GetIswc();
            Assert.IsTrue (iswc.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void Iswc_ToXml_1()
        {
            var iswc = new Iswc();
            Assert.AreEqual ("<iswc />", XmlUtility.SerializeShort (iswc));

            iswc = _GetIswc();
            Assert.AreEqual ("<iswc><identifier>1234567</identifier></iswc>",
                XmlUtility.SerializeShort (iswc));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void Iswc_ToJson_1()
        {
            var iswc = new Iswc();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (iswc));

            iswc = _GetIswc();
            Assert.AreEqual ("{\"identifier\":\"1234567\"}",
                JsonUtility.SerializeShort (iswc));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void Iswc_ToString_1()
        {
            var iswc = new Iswc();
            Assert.AreEqual ("(null)", iswc.ToString());

            iswc = _GetIswc();
            Assert.AreEqual ("1234567", iswc.ToString());
        }

    }
}
