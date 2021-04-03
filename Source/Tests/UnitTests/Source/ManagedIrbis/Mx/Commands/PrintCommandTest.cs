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
    public class PrintCommandTest
        : CommonMxCommandTest
    {
        [TestMethod]
        public void PrintCommand_Construction_1()
        {
            var command = new PrintCommand();
            Assert.AreEqual("Print", command.Name);
        }

        [TestMethod]
        public void PrintCommand_Execute_1()
        {
            using (var executive = GetExecutive())
            {
                using (var command = new PrintCommand())
                {
                    command.Initialize(executive);

                    var arguments = new MxArgument[0];
                    command.Execute(executive, arguments);
                }
            }
        }

        [TestMethod]
        public void PrintCommand_ToString_1()
        {
            var command = new PrintCommand();
            Assert.AreEqual("Print", command.ToString());
        }
    }
}
