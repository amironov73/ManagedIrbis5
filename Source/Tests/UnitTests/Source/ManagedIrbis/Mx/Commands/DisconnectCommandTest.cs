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
    public class DisconnectCommandTest
        : CommonMxCommandTest
    {
        [TestMethod]
        public void DisconnectCommand_Construction_1()
        {
            var command = new DisconnectCommand();
            Assert.AreEqual("Disconnect", command.Name);
        }

        [TestMethod]
        public void DisconnectCommand_Execute_1()
        {
            using (var executive = GetExecutive())
            {
                using (var command = new DisconnectCommand())
                {
                    command.Initialize(executive);

                    var arguments = new MxArgument[0];
                    command.Execute(executive, arguments);
                }
            }
        }

        [TestMethod]
        public void DisconnectCommand_ToString_1()
        {
            var command = new DisconnectCommand();
            Assert.AreEqual("Disconnect", command.ToString());
        }
    }
}
