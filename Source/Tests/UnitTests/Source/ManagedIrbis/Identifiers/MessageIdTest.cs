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
    public sealed class MessageIdTest
    {
        private MessageId _GetMessageId()
        {
            return new MessageId() { Identifier = "1234567" };
        }

        private void _Compare
            (
                MessageId first,
                MessageId second
            )
        {
            Assert.AreEqual (first.Identifier, second.Identifier);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void MessageId_Construction_1()
        {
            var message = new MessageId();
            Assert.IsNull (message.Identifier);
            Assert.IsNull (message.UserData);
        }

        [TestMethod]
        [Description ("Разбор текста")]
        public void MessageId_ParseText_1()
        {
            const string identifier = "1234567";
            var message = new MessageId();
            message.ParseText (identifier);
            Assert.AreEqual (identifier, message.Identifier);
        }

        private void _TestSerialization
            (
                MessageId first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<MessageId>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void MessageId_Serialization_1()
        {
            var message = new MessageId();
            _TestSerialization (message);

            message = _GetMessageId();
            message.UserData = "User data";
            _TestSerialization (message);

        }

        [TestMethod]
        [Description ("Верификация")]
        public void MessageId_Verification_1()
        {
            var message = new MessageId();
            Assert.IsFalse (message.Verify (false));

            message = _GetMessageId();
            Assert.IsTrue (message.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void MessageId_ToXml_1()
        {
            var message = new MessageId();
            Assert.AreEqual ("<message />", XmlUtility.SerializeShort (message));

            message = _GetMessageId();
            Assert.AreEqual ("<message><identifier>1234567</identifier></message>",
                XmlUtility.SerializeShort (message));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void MessageId_ToJson_1()
        {
            var message = new MessageId();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (message));

            message = _GetMessageId();
            Assert.AreEqual ("{\"identifier\":\"1234567\"}",
                JsonUtility.SerializeShort (message));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void MessageId_ToString_1()
        {
            var message = new MessageId();
            Assert.AreEqual ("(null)", message.ToString());

            message = _GetMessageId();
            Assert.AreEqual ("1234567", message.ToString());
        }

    }
}
