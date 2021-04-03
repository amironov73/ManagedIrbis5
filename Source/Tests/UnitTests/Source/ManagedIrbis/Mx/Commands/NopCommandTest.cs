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
    public class NopCommandTest
        : CommonMxCommandTest
    {
        [TestMethod]
        public void NopCommand_Construction_1()
        {
            var command = new NopCommand();
            Assert.AreEqual("Nop", command.Name);
        }

        [TestMethod]
        public void NopCommand_Execute_1()
        {
            using (var executive = GetExecutive())
            {
                using (var command = new NopCommand())
                {
                    command.Initialize(executive);

                    var arguments = new MxArgument[0];
                    command.Execute(executive, arguments);
                }
            }
        }

        [TestMethod]
        public void NopCommand_ToString_1()
        {
            var command = new NopCommand();
            Assert.AreEqual("Nop", command.ToString());
        }
    }
}
