// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using ManagedIrbis.Server;
using ManagedIrbis.Server.Commands;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Server.Commands
{
    [TestClass]
    public sealed class ConnectCommandTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        [Description ("Конструктор")]
        public void ConnectCommand_Construction_1()
        {
            var workData = new WorkData();
            var command = new ConnectCommand (workData);
            Assert.AreSame (workData, command.Data);
            Assert.IsTrue (command.SendVersion);
        }

        [TestMethod]
        [Description ("Исполнение команды")]
        [ExpectedException (typeof (ArgumentException))]
        public void ConnectCommand_Execute_1()
        {
            var workData = new WorkData();
            var command = new ConnectCommand (workData);
            command.Execute();
        }
    }
}
