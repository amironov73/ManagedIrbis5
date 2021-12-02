// ReSharper disable CheckNamespace
// ReSharper disable EqualExpressionComparison
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using ManagedIrbis.Mx;
using ManagedIrbis.Mx.Commands;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Mx.Commands
{
    [TestClass]
    public sealed class PingCommandTest
        : CommonMxCommandTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void PingCommand_Construction_1()
        {
            var command = new PingCommand();
            Assert.AreEqual ("ping", command.Name);
        }

        [TestMethod]
        [Description ("Выполнение команды")]
        public void PingCommand_Execute_1()
        {
            using var executive = GetExecutive();
            using var command = new PingCommand();
            command.Initialize (executive);

            var arguments = Array.Empty<MxArgument>();
            command.Execute (executive, arguments);
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void PingCommand_ToString_1()
        {
            var command = new PingCommand();
            Assert.AreEqual ("ping", command.ToString());
        }
    }
}
