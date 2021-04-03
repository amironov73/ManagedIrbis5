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
    public class SearchCommandTest
        : CommonMxCommandTest
    {
        [TestMethod]
        public void SearchCommand_Construction_1()
        {
            var command = new SearchCommand();
            Assert.AreEqual("Search", command.Name);
        }

        [TestMethod]
        public void SearchCommand_Execute_1()
        {
            using (var executive = GetExecutive())
            {
                using (var command = new SearchCommand())
                {
                    command.Initialize(executive);

                    var arguments = new MxArgument[0];
                    command.Execute(executive, arguments);
                }
            }
        }

        [TestMethod]
        public void SearchCommand_ToString_1()
        {
            var command = new SearchCommand();
            Assert.AreEqual("Search", command.ToString());
        }
    }
}
