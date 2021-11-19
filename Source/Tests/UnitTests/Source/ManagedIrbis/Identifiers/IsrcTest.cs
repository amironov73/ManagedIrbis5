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
    public sealed class IsrcTest
    {
        private Isrc _GetIsrc()
        {
            return new Isrc() { Country = "GB", Registrant = "EMI", Year = "03", Number="00013" };
        }

        private void _Compare
            (
                Isrc first,
                Isrc second
            )
        {
            Assert.AreEqual (first.Country, second.Country);
            Assert.AreEqual (first.Registrant, second.Registrant);
            Assert.AreEqual (first.Year, second.Year);
            Assert.AreEqual (first.Number, second.Number);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Isrc_Construction_1()
        {
            var isrc = new Isrc();
            Assert.IsNull (isrc.Country);
            Assert.IsNull (isrc.Registrant);
            Assert.IsNull (isrc.Year);
            Assert.IsNull (isrc.Number);
            Assert.IsNull (isrc.UserData);
        }

        [TestMethod]
        [Description ("Разбор текста: без дефисов")]
        public void Isrc_ParseText_1()
        {
            var isrc = new Isrc();
            isrc.ParseText ("GBEMI0300013");
            Assert.AreEqual ("GB", isrc.Country);
            Assert.AreEqual ("EMI", isrc.Registrant);
            Assert.AreEqual ("03", isrc.Year);
            Assert.AreEqual ("00013", isrc.Number);
        }

        [TestMethod]
        [Description ("Разбор текста: с дефисами")]
        public void Isrc_ParseText_2()
        {
            var isrc = new Isrc();
            isrc.ParseText ("GB-EMI-03-00013");
            Assert.AreEqual ("GB", isrc.Country);
            Assert.AreEqual ("EMI", isrc.Registrant);
            Assert.AreEqual ("03", isrc.Year);
            Assert.AreEqual ("00013", isrc.Number);
        }

        private void _TestSerialization
            (
                Isrc first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<Isrc>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void Isrc_Serialization_1()
        {
            var isrc = new Isrc();
            _TestSerialization (isrc);

            isrc = _GetIsrc();
            isrc.UserData = "User data";
            _TestSerialization (isrc);

        }

        [TestMethod]
        [Description ("Верификация")]
        public void Isrc_Verification_1()
        {
            var isrc = new Isrc();
            Assert.IsFalse (isrc.Verify (false));

            isrc = _GetIsrc();
            Assert.IsTrue (isrc.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void Isrc_ToXml_1()
        {
            var isrc = new Isrc();
            Assert.AreEqual ("<isrc />", XmlUtility.SerializeShort (isrc));

            isrc = _GetIsrc();
            Assert.AreEqual ("<isrc><country>GB</country><registrant>EMI</registrant><year>03</year><number>00013</number></isrc>",
                XmlUtility.SerializeShort (isrc));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void Isrc_ToJson_1()
        {
            var isrc = new Isrc();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (isrc));

            isrc = _GetIsrc();
            Assert.AreEqual ("{\"country\":\"GB\",\"registrant\":\"EMI\",\"year\":\"03\",\"number\":\"00013\"}",
                JsonUtility.SerializeShort (isrc));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void Isrc_ToString_1()
        {
            var isrc = new Isrc();
            Assert.AreEqual ("---", isrc.ToString());

            isrc = _GetIsrc();
            Assert.AreEqual ("GB-EMI-03-00013", isrc.ToString());
        }

    }
}
