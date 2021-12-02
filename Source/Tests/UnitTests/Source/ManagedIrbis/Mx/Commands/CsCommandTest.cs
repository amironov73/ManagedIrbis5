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
    public sealed class CsCommandTest
        : CommonMxCommandTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void CsCommand_Construction_1()
        {
            var command = new CsCommand();
            Assert.AreEqual ("CS", command.Name);
        }

        [TestMethod]
        [Description ("Выполнение команлы")]
        public void CsCommand_Execute_1()
        {
            using var executive = GetExecutive();
            using var command = new CsCommand();
            command.Initialize (executive);

            var arguments = Array.Empty<MxArgument>();
            command.Execute (executive, arguments);
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void CsCommand_ToString_1()
        {
            var command = new CsCommand();
            Assert.AreEqual ("CS", command.ToString());
        }
    }
}
