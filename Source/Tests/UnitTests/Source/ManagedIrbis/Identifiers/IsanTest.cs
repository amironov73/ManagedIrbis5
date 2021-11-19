// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis.Identifiers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Identifiers
{
    [TestClass]
    public sealed class IsanTest
    {
        private Isan _GetIsan()
        {
            return new Isan()
            {
                Root = 0x1881_BBC7_3420UL,
                Episode = 0,
                Version = 0x9F3A_0245U
            };
        }

        private void _Compare
            (
                Isan first,
                Isan second
            )
        {
            Assert.AreEqual (first.Root, second.Root);
            Assert.AreEqual (first.Episode, second.Episode);
            Assert.AreEqual (first.Version, second.Version);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Isan_Construction_1()
        {
            var isan = new Isan();
            Assert.AreEqual (0UL, isan.Root);
            Assert.AreEqual (0U, isan.Episode);
            Assert.AreEqual (0U, isan.Version);
            Assert.IsNull (isan.UserData);
        }

        [TestMethod]
        [Description ("Разбор текста")]
        public void Isan_ParseText_1()
        {
            var isan = new Isan();
            isan.ParseText ("1881-BBC7-3420-0000-7-9F3A-0245-U");
            Assert.AreEqual (0x1881_BBC7_3420UL, isan.Root);
            Assert.AreEqual (0U, isan.Episode);
            Assert.AreEqual (0x9F3A_0245, isan.Version);
        }

        [TestMethod]
        [Description ("Разбор текста")]
        public void Isan_ParseText_2()
        {
            var isan = new Isan();
            isan.ParseText ("1881-BBC7-3420-0000-9F3A-0245");
            Assert.AreEqual (0x1881_BBC7_3420UL, isan.Root);
            Assert.AreEqual (0U, isan.Episode);
            Assert.AreEqual (0x9F3A_0245, isan.Version);
        }

        [TestMethod]
        [Description ("Разбор текста")]
        public void Isan_ParseText_3()
        {
            var isan = new Isan();
            isan.ParseText ("1881BBC7342000009F3A0245");
            Assert.AreEqual (0x1881_BBC7_3420UL, isan.Root);
            Assert.AreEqual (0U, isan.Episode);
            Assert.AreEqual (0x9F3A_0245, isan.Version);
        }

        [TestMethod]
        [Description ("Разбор текста: неверный формат")]
        [ExpectedException (typeof (FormatException))]
        public void Isan_ParseText_4()
        {
            var isan = new Isan();
            isan.ParseText ("1881-BBC-3420-0000-7-9F3A-0245-U");
        }

        [TestMethod]
        [Description ("Разбор текста: неверный формат")]
        [ExpectedException (typeof (FormatException))]
        public void Isan_ParseText_5()
        {
            var isan = new Isan();
            isan.ParseText ("1881-BBC7-3420-000-9F3A-0245");
        }

        [TestMethod]
        [Description ("Разбор текста: неверный формат")]
        [ExpectedException (typeof (FormatException))]
        public void Isan_ParseText_6()
        {
            var isan = new Isan();
            isan.ParseText ("1881-BBC7-3420-0000-9F3A-025");
        }

        private void _TestSerialization
            (
                Isan first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<Isan>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void Isan_Serialization_1()
        {
            var isan = new Isan();
            _TestSerialization (isan);

            isan = _GetIsan();
            isan.UserData = "User data";
            _TestSerialization (isan);

        }

        [TestMethod]
        [Description ("Верификация")]
        public void Isan_Verification_1()
        {
            var isan = new Isan();
            Assert.IsFalse (isan.Verify (false));

            isan = _GetIsan();
            Assert.IsTrue (isan.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void Isan_ToXml_1()
        {
            var isan = new Isan();
            Assert.AreEqual ("<isan><root>0</root><episode>0</episode><version>0</version></isan>", XmlUtility.SerializeShort (isan));

            isan = _GetIsan();
            Assert.AreEqual ("<isan><root>26945480242208</root><episode>0</episode><version>2671379013</version></isan>",
                XmlUtility.SerializeShort (isan));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void Isan_ToJson_1()
        {
            var isan = new Isan();
            Assert.AreEqual ("{\"root\":0,\"episode\":0,\"version\":0}", JsonUtility.SerializeShort (isan));

            isan = _GetIsan();
            Assert.AreEqual ("{\"root\":26945480242208,\"episode\":0,\"version\":2671379013}",
                JsonUtility.SerializeShort (isan));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void Isan_ToString_1()
        {
            var isan = new Isan();
            Assert.AreEqual ("0000-0000-0000-0000-0000-0000", isan.ToString());

            isan = _GetIsan();
            Assert.AreEqual ("1881-BBC7-3420-0000-9F3A-0245", isan.ToString());
        }

    }
}
