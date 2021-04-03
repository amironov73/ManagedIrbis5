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
    public class StoreCommandTest
        : CommonMxCommandTest
    {
        [TestMethod]
        public void StoreCommand_Construction_1()
        {
            var command = new StoreCommand();
            Assert.AreEqual("Store", command.Name);
        }

        [TestMethod]
        public void StoreCommand_Execute_1()
        {
            using (var executive = GetExecutive())
            {
                using (var command = new StoreCommand())
                {
                    command.Initialize(executive);

                    var arguments = new MxArgument[0];
                    command.Execute(executive, arguments);
                }
            }
        }

        [TestMethod]
        public void StoreCommand_ToString_1()
        {
            var command = new StoreCommand();
            Assert.AreEqual("Store", command.ToString());
        }
    }
}
