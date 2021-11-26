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
    public sealed class FlushServerCommandTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        [Description ("Конструктор")]
        public void FlushServerCommand_Construction_1()
        {
            var workData = new WorkData();
            var command = new FlushServerCommand (workData);
            Assert.AreSame (workData, command.Data);
            Assert.IsFalse (command.SendVersion);
        }

        [TestMethod]
        [Description ("Исполнение команды")]
        [ExpectedException (typeof (ArgumentException))]
        public void FlushServerCommand_Execute_1()
        {
            var workData = new WorkData();
            var command = new FlushServerCommand (workData);
            command.Execute();
        }
    }
}
