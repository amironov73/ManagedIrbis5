// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis.Tables;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Tables
{
    [TestClass]
    public sealed class KeyDefinitionTest
    {
        private KeyDefinition _GetDefinition()
        {
            return new ()
            {
                Length = 100,
                Multiple = true,
                Format = "@brief",
            };
        }

        private void _Compare
            (
                KeyDefinition first,
                KeyDefinition second
            )
        {
            Assert.AreEqual (first.Length, second.Length);
            Assert.AreEqual (first.Multiple, second.Multiple);
            Assert.AreEqual (first.Format, second.Format);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void KeyDefinition_Construction_1()
        {
            var definition = new KeyDefinition();
            Assert.AreEqual (0, definition.Length);
            Assert.AreEqual (false, definition.Multiple);
            Assert.IsNull (definition.Format);
        }

        [TestMethod]
        [Description ("Присвоение")]
        public void KeyDefinition_Construction_2()
        {
            var definition = new KeyDefinition
            {
                Length = 100,
                Multiple = true,
                Format = "@brief",
            };
            Assert.AreEqual (100, definition.Length);
            Assert.AreEqual (true, definition.Multiple);
            Assert.AreEqual ("@brief", definition.Format);
        }

        private void _TestSerialization
            (
                KeyDefinition first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<KeyDefinition>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void KeyDefinition_Serialization_1()
        {
            var definition = new KeyDefinition();
            _TestSerialization (definition);

            definition = _GetDefinition();
            definition.UserData = "User data";
            _TestSerialization (definition);
        }

        [TestMethod]
        [Description ("Верификация")]
        public void KeyDefinition_Verify_1()
        {
            var definition = new KeyDefinition();
            Assert.IsFalse (definition.Verify (false));

            definition = _GetDefinition();
            Assert.IsTrue (definition.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void KeyDefinition_ToXml_1()
        {
            var definition = new KeyDefinition();
            Assert.AreEqual
                (
                    "<key length=\"0\" multiple=\"false\" />",
                    XmlUtility.SerializeShort (definition)
                );

            definition = _GetDefinition();
            Assert.AreEqual
                (
                    "<key length=\"100\" multiple=\"true\"><format>@brief</format></key>",
                    XmlUtility.SerializeShort (definition)
                );
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void KeyDefinition_ToJson_1()
        {
            var definition = new KeyDefinition();
            Assert.AreEqual
                (
                    "{\"length\":0,\"multiple\":false}",
                    JsonUtility.SerializeShort (definition)
                );

            definition = _GetDefinition();
            var expected = "{\"length\":100,\"multiple\":true,\"format\":\"@brief\"}";
            var actual = JsonUtility.SerializeShort (definition);
            Assert.AreEqual (expected, actual);
        }

        [TestMethod]
        [Description ("Текстовое представление")]
        public void KeyDefinition_ToString_1()
        {
            var definition = new KeyDefinition();
            Assert.AreEqual
                (
                    "(null)",
                    definition.ToString()
                );

            definition = _GetDefinition();
            Assert.AreEqual
                (
                    "@brief",
                    definition.ToString()
                );
        }
    }
}
