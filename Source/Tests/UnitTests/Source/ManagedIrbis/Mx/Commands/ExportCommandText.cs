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
    public sealed class ExportCommandText
        : CommonMxCommandTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void ExportCommand_Construction_1()
        {
            var command = new ExportCommand();
            Assert.AreEqual ("export", command.Name);
        }

        [TestMethod]
        [Description ("Выполнение команды")]
        public void ExportCommand_Execute_1()
        {
            using var executive = GetExecutive();
            using var command = new ExportCommand();
            command.Initialize (executive);

            var arguments = Array.Empty<MxArgument>();
            command.Execute (executive, arguments);
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void ExportCommand_ToString_1()
        {
            var command = new ExportCommand();
            Assert.AreEqual ("export", command.ToString());
        }
    }
}
