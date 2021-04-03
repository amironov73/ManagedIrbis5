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
    public class CsFileCommandTest
        : CommonMxCommandTest
    {
        [TestMethod]
        public void CsFileCommand_Construction_1()
        {
            var command = new CsFileCommand();
            Assert.AreEqual("CSFile", command.Name);
        }

        [TestMethod]
        public void CsFileCommand_Execute_1()
        {
            using (var executive = GetExecutive())
            {
                using (var command = new CsFileCommand())
                {
                    command.Initialize(executive);

                    var arguments = new MxArgument[0];
                    command.Execute(executive, arguments);
                }
            }
        }

        [TestMethod]
        public void CsFileCommand_ToString_1()
        {
            var command = new CsFileCommand();
            Assert.AreEqual("CSFile", command.ToString());
        }
    }
}
