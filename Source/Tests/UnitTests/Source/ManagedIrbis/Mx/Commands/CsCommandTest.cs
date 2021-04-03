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
    public class CsCommandTest
        : CommonMxCommandTest
    {
        [TestMethod]
        public void CsCommand_Construction_1()
        {
            var command = new CsCommand();
            Assert.AreEqual("CS", command.Name);
        }

        [TestMethod]
        public void CsCommand_Execute_1()
        {
            using (var executive = GetExecutive())
            {
                using (var command = new CsCommand())
                {
                    command.Initialize(executive);

                    var arguments = new MxArgument[0];
                    command.Execute(executive, arguments);
                }
            }
        }

        [TestMethod]
        public void CsCommand_ToString_1()
        {
            var command = new CsCommand();
            Assert.AreEqual("CS", command.ToString());
        }
    }
}
