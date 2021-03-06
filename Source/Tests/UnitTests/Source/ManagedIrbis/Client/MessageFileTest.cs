﻿// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System.IO;

using AM.Runtime;

using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Client
{
    [TestClass]
    public class MessageFileTest
        : Common.CommonUnitTest
    {
        private string _GetFileName() => Path.Combine (Irbis64RootPath, MessageFile.DefaultName);

        [TestMethod]
        public void MessageFile_Construction_1()
        {
            var file = new MessageFile();
            Assert.IsNull(file.Name);
            Assert.AreEqual(0, file.LineCount);
        }

        [TestMethod]
        public void MessageFile_ReadLocalFile_1()
        {
            var fileName = _GetFileName();
            var encoding = IrbisEncoding.Ansi;
            var file = MessageFile.ReadLocalFile(fileName, encoding);
            Assert.AreEqual(1422, file.LineCount);
            Assert.AreSame(fileName, file.Name);
            Assert.AreEqual("Ассоциация ЭБНИТ", file.GetMessage(1));
            Assert.AreEqual("MISSING @11111", file.GetMessage(11111));
        }

        private void _TestSerialization
            (
                MessageFile first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<MessageFile>();
            Assert.IsNotNull(second);
            Assert.AreEqual(first.Name, second!.Name);
            Assert.AreEqual(first.LineCount, second.LineCount);
        }

        [TestMethod]
        public void MessageFile_Serialization_1()
        {
            var file = new MessageFile();
            _TestSerialization(file);

            var fileName = _GetFileName();
            var encoding = IrbisEncoding.Ansi;
            file = MessageFile.ReadLocalFile(fileName, encoding);
            _TestSerialization(file);
        }
    }
}
