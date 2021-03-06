﻿// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

using System.Text;

using AM.IO;

using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Client
{
    [TestClass]
    public class ClientLMTest
    {
        [TestMethod]
        public void ClientLM_Construction_1()
        {
            var manager = new ClientLM();
            Assert.AreEqual(ClientLM.DefaultSalt, manager.Salt);
            Assert.AreSame(IrbisEncoding.Ansi, manager.Encoding);
        }

        [TestMethod]
        public void ClientLM_Construction_2()
        {
            var salt = "Salt";
            var encoding = Encoding.ASCII;
            var manager = new ClientLM(encoding, salt);
            Assert.AreSame(salt, manager.Salt);
            Assert.AreSame(encoding, manager.Encoding);
        }

        [TestMethod]
        public void ClientLM_ComputeHash_1()
        {
            var manager = new ClientLM();
            var actual = manager
                .ComputeHash("Иркутский государственный технический университет");
            var expected = "\x040E\x00A0\x00A0\x040E\x045B";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ClientLM_ComputeHash_2()
        {
            var user = "Иркутский государственный технический университет";
            var iniFile = new IniFile();
            var section = iniFile.CreateSection("Main");
            section["User"] = user;

            var manager = new ClientLM();
            section["Common"] = manager.ComputeHash(user);

            Assert.IsTrue(manager.CheckHash(iniFile));
        }

        [TestMethod]
        public void ClientLM_CheckHash_1()
        {
            var iniFile = new IniFile();
            var section = iniFile.CreateSection("Main");
            section["User"] = "Иркутский государственный технический университет";
            section["Common"] = "\x040E\x00A0\x00A0\x040E\x045B";

            var manager = new ClientLM();
            Assert.IsTrue(manager.CheckHash(iniFile));
        }

        [TestMethod]
        public void ClientLM_CheckHash_2()
        {
            var iniFile = new IniFile();
            var section = iniFile.CreateSection("Main");
            section["User"] = "Иркутский государственный технический университет";
            section["Common"] = "\x040E\x00A0\x00A0\x040E\x045C";

            var manager = new ClientLM();
            Assert.IsFalse(manager.CheckHash(iniFile));
        }

        [TestMethod]
        public void ClientLM_CheckHash_3()
        {
            var iniFile = new IniFile();
            var section = iniFile.CreateSection("Main");
            section["User"] = "Иркутский государственный технический университет";

            var manager = new ClientLM();
            Assert.IsFalse(manager.CheckHash(iniFile));
        }
    }
}
