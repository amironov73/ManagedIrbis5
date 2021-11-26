// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using ManagedIrbis;
using ManagedIrbis.Server;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Server
{
    [TestClass]
    public sealed class ClientRequestTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void ClientRequest_Construction_1()
        {
            var request = new ClientRequest();
            Assert.AreEqual (0, request.RequestLength);
            Assert.IsNull (request.CommandCode1);
            Assert.IsNull (request.Workstation);
            Assert.IsNull (request.CommandCode2);
            Assert.IsNull (request.ClientId);
            Assert.IsNull (request.CommandNumber);
            Assert.IsNull (request.Password);
            Assert.IsNull (request.Login);
            Assert.IsNotNull (request.Memory);
            Assert.IsTrue (request.IsEot());
        }

        [TestMethod]
        [Description ("Конструктор")]
        [ExpectedException (typeof (ArgumentException))]
        public void ClientRequest_Construction_2()
        {
            var data = new WorkData();
            var request = new ClientRequest (data);
            Assert.IsNull (request.CommandCode1);
        }

        [TestMethod]
        [Description ("Присваивание")]
        public void ClientRequest_Construction_3()
        {
            var request = new ClientRequest()
            {
                CommandCode1 = "A",
                Workstation = "C",
                CommandCode2 = "A",
                ClientId = "123456",
                CommandNumber = "1",
                Password = "secret",
                Login = "librarian"
            };
            Assert.AreEqual ("A", request.CommandCode1);
            Assert.AreEqual ("C", request.Workstation);
            Assert.AreEqual ("A", request.CommandCode2);
            Assert.AreEqual ("123456", request.ClientId);
            Assert.AreEqual ("1", request.CommandNumber);
            Assert.AreEqual ("secret", request.Password);
            Assert.AreEqual ("librarian", request.Login);
            Assert.IsTrue (request.IsEot());
        }

        [TestMethod]
        [Description ("Чтение строки в виде последовательности байтов")]
        public void ClientRequest_GetString_1()
        {
            var request = new ClientRequest();
            var line = request.GetString();
            Assert.IsNull (line);
        }

        [TestMethod]
        [Description ("Чтение строки с автоматическим определением кодировки")]
        public void ClientRequest_GetAutoString_1()
        {
            var request = new ClientRequest();
            var line = request.GetAutoString();
            Assert.IsNull (line);
        }

        [TestMethod]
        [Description ("Требование не пустой строки")]
        [ExpectedException (typeof (IrbisException))]
        public void ClientRequest_RequireAutoString_1()
        {
            var request = new ClientRequest();
            var line = request.RequireAutoString();
            Assert.IsNull (line);
        }

        [TestMethod]
        [Description ("Чтение строки в кодировке ANSI")]
        public void ClientRequest_GetAnsiString_1()
        {
            var request = new ClientRequest();
            var line = request.GetAnsiString();
            Assert.IsNull (line);
        }

        [TestMethod]
        [Description ("Требование не пустой ANSI-строки")]
        [ExpectedException (typeof (IrbisException))]
        public void ClientRequest_RequireAnsiString_1()
        {
            var request = new ClientRequest();
            var line = request.RequireAnsiString();
            Assert.IsNull (line);
        }

        [TestMethod]
        [Description ("Чтение массива строк в кодировке ANSI")]
        public void ClientRequest_RemainingAnsiStrings_1()
        {
            var request = new ClientRequest();
            var lines = request.RemainingAnsiStrings();
            Assert.IsNotNull (lines);
            Assert.AreEqual (0, lines.Length);
        }

        [TestMethod]
        [Description ("Чтение остатка текста в кодировке ANSI")]
        public void ClientRequest_RemainingAnsiText_1()
        {
            var request = new ClientRequest();
            var text = request.RemainingAnsiText();
            Assert.IsNotNull (text);
            Assert.AreEqual (0, text.Length);
        }

        [TestMethod]
        [Description ("Чтение строки в кодировке UTF-8")]
        public void ClientRequest_GetUtfString_1()
        {
            var request = new ClientRequest();
            var line = request.GetUtfString();
            Assert.IsNull (line);
        }

        [TestMethod]
        [Description ("Требование не пустой UTF-строки")]
        [ExpectedException (typeof (IrbisException))]
        public void ClientRequest_RequireUtfString_1()
        {
            var request = new ClientRequest();
            var line = request.RequireUtfString();
            Assert.IsNull (line);
        }

        [TestMethod]
        [Description ("Чтение массива строк в кодировке UTF-8")]
        public void ClientRequest_RemainingUtfStrings_1()
        {
            var request = new ClientRequest();
            var lines = request.RemainingUtfStrings();
            Assert.IsNotNull (lines);
            Assert.AreEqual (0, lines.Length);
        }

        [TestMethod]
        [Description ("Чтение остатка текста в кодировке UTF-8")]
        public void ClientRequest_RemainingUtfText_1()
        {
            var request = new ClientRequest();
            var text = request.RemainingUtfText();
            Assert.IsNotNull (text);
            Assert.AreEqual (0, text.Length);
        }

        [TestMethod]
        [Description ("Чтение 32-битного целого")]
        public void ClientRequest_GetInt32_1()
        {
            var request = new ClientRequest();
            var number = request.GetInt32();
            Assert.AreEqual (0, number);
        }
    }
}
