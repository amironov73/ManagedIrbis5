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
    public class SortCommandTest
        : CommonMxCommandTest
    {
        [TestMethod]
        public void SortCommand_Construction_1()
        {
            var command = new SortCommand();
            Assert.AreEqual("Sort", command.Name);
        }

        [TestMethod]
        public void SortCommand_Execute_1()
        {
            using (var executive = GetExecutive())
            {
                using (var command = new SortCommand())
                {
                    command.Initialize(executive);

                    var arguments = new MxArgument[0];
                    command.Execute(executive, arguments);
                }
            }
        }

        [TestMethod]
        public void SortCommand_ToString_1()
        {
            var command = new SortCommand();
            Assert.AreEqual("Sort", command.ToString());
        }
    }
}
