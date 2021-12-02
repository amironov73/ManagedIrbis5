// ReSharper disable CheckNamespace
// ReSharper disable EqualExpressionComparison
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using System.IO;

using ManagedIrbis.Mx;
using ManagedIrbis.Mx.Commands;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Mx.Commands
{
    [TestClass]
    public sealed class ClsCommandTest
        : CommonMxCommandTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void ClsCommand_Construction_1()
        {
            var command = new ClsCommand();
            Assert.AreEqual ("cls", command.Name);
        }

        [Ignore]
        [TestMethod]
        [Description ("Выполнение команды")]
        [ExpectedException (typeof(IOException))]
        public void ClsCommand_Execute_1()
        {
            // TODO: разобраться, что делать, когда выполняется
            // из-под Rider, а когда на настоящей консоли

            using var executive = GetExecutive();
            using var command = new ClsCommand();
            command.Initialize (executive);

            var arguments = Array.Empty<MxArgument>();
            command.Execute (executive, arguments);
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void ClsCommand_ToString_1()
        {
            var command = new ClsCommand();
            Assert.AreEqual ("cls", command.ToString());
        }
    }
}
