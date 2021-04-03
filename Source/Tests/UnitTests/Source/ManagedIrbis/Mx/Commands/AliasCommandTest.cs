// ReSharper disable CheckNamespace
// ReSharper disable EqualExpressionComparison
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Mx;
using ManagedIrbis.Mx.Commands;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Mx.Commands
{
    [TestClass]
    public class AliasCommandTest
        : CommonMxCommandTest
    {
        [TestMethod]
        public void AliasCommand_Construction_1()
        {
            var command = new AliasCommand();
            Assert.AreEqual("Alias", command.Name);
        }

        [TestMethod]
        public void AliasCommand_Execute_1()
        {
            using var executive = GetExecutive();
            using var command = new AliasCommand();
            command.Initialize(executive);

            var arguments = new MxArgument[0];
            command.Execute(executive, arguments);
        }

        [TestMethod]
        public void AliasCommand_ToString_1()
        {
            var command = new AliasCommand();
            Assert.AreEqual("Alias", command.ToString());
        }
    }
}
