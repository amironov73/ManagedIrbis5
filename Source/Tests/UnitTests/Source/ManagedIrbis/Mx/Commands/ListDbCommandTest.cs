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
    public sealed class ListDbCommandTest
        : CommonMxCommandTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void ListDbCommand_Construction_1()
        {
            var command = new ListDbCommand();
            Assert.AreEqual ("listdb", command.Name);
        }

        [TestMethod]
        [Description ("Выполнение команды")]
        public void ListDbCommand_Execute_1()
        {
            using var executive = GetExecutive();
            using var command = new ListDbCommand();
            command.Initialize (executive);

            var arguments = Array.Empty<MxArgument>();
            command.Execute (executive, arguments);
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void ListDbCommand_ToString_1()
        {
            var command = new ListDbCommand();
            Assert.AreEqual ("listdb", command.ToString());
        }
    }
}
