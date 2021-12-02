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
    public sealed class AliasCommandTest
        : CommonMxCommandTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void AliasCommand_Construction_1()
        {
            var command = new AliasCommand();
            Assert.AreEqual ("Alias", command.Name);
        }

        [TestMethod]
        [Description ("Выполнение команды")]
        public void AliasCommand_Execute_1()
        {
            using var executive = GetExecutive();
            using var command = new AliasCommand();
            command.Initialize (executive);

            var arguments = Array.Empty<MxArgument>();
            command.Execute (executive, arguments);
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void AliasCommand_ToString_1()
        {
            var command = new AliasCommand();
            Assert.AreEqual ("Alias", command.ToString());
        }
    }
}
