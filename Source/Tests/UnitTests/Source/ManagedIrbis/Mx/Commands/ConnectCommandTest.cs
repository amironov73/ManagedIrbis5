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
    public class ConnectCommandTest
        : CommonMxCommandTest
    {
        [TestMethod]
        public void ConnectCommand_Construction_1()
        {
            var command = new ConnectCommand();
            Assert.AreEqual("Connect", command.Name);
        }

        [TestMethod]
        public void ConnectCommand_Execute_1()
        {
            using (var executive = GetExecutive())
            {
                using (var command = new ConnectCommand())
                {
                    command.Initialize(executive);

                    var arguments = new MxArgument[0];
                    command.Execute(executive, arguments);
                }
            }
        }

        [TestMethod]
        public void ConnectCommand_ToString_1()
        {
            var command = new ConnectCommand();
            Assert.AreEqual("Connect", command.ToString());
        }
    }
}
